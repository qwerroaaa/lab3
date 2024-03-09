using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int currentRow;
        string ruleString;
        private Dictionary<(bool, bool, bool), bool> ruleDictionary = new Dictionary<(bool, bool, bool), bool>();

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private bool GetRuleFromIndex(int index)
        {
            if (Convert.ToBoolean(ruleString[index].Equals('0')))
                return false;
            else
                return true;
        }



        private void SetRules()
        {
            ruleString = Convert.ToString(Convert.ToInt32(inputRule.Value), 2);
            while (ruleString.Length < 8)
                ruleString = "0" + ruleString;
            ruleDictionary.Clear();
            ruleDictionary.Add((false, false, false), GetRuleFromIndex(7));
            ruleDictionary.Add((false, false, true), GetRuleFromIndex(6));
            ruleDictionary.Add((false, true, false), GetRuleFromIndex(5));
            ruleDictionary.Add((false, true, true), GetRuleFromIndex(4));
            ruleDictionary.Add((true, false, false), GetRuleFromIndex(3));
            ruleDictionary.Add((true, false, true), GetRuleFromIndex(2));
            ruleDictionary.Add((true, true, false), GetRuleFromIndex(1));
            ruleDictionary.Add((true, true, true), GetRuleFromIndex(0));
        }

        private Label CreateLabel()
        {
            Label label = new Label();
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Dock = DockStyle.Fill;
            label.Margin = new System.Windows.Forms.Padding(0);
            return label;
        }

        private void CreateRowNumberLabel(int i)
        {
            Label RowNumberLabel = CreateLabel();
            RowNumberLabel.Text = Convert.ToString(i);
            tableLayoutPanel1.Controls.Add(RowNumberLabel, 0, i);
        }



        private void FillTable()
        {
            for (int k = 0; k < 16; k++)
            {
                CreateRowNumberLabel(k);
                for (int n = 1; n < 16; n++)
                {
                    Label TableLabel = CreateLabel();
                    TableLabel.BackColor = Color.White;
                    tableLayoutPanel1.Controls.Add(TableLabel, n, k);
                }
            }
            for (int k = 1; k < 16; k++)
                tableLayoutPanel1.GetControlFromPosition(k, 0).Click += new EventHandler(CellClick);
        }

        private bool GetCellCondition(int x, int y)
        {
            if (x == -1)
                x = 15;
            if (x == 16)
                x = 0;
            Label l = tableLayoutPanel1.GetControlFromPosition(x, y) as Label;
            if (l.BackColor == Color.Black)
                return true;
            else
                return false;
        }

        private void ChangeCellColor(Label cell)
        {
            if (cell.BackColor == Color.White)
                cell.BackColor = Color.Black;
            else
                cell.BackColor = Color.White;
        }

        private void CellClick(object sender, EventArgs e)
        {
            Label cell = sender as Label;
            ChangeCellColor(cell);
        }
        private bool PredictCellCondition(int x, int y)
        {
            bool outValue = false;
            ruleDictionary.TryGetValue((GetCellCondition(x - 1, y - 1), GetCellCondition(x, y - 1), GetCellCondition(x + 1, y - 1)), out outValue);
            return outValue;
        }

        private void NewIteration()
        {
            for (int k = 1; k < 16; k++)
            {
                Label cellLabel = tableLayoutPanel1.GetControlFromPosition(k, currentRow) as Label;
                if (PredictCellCondition(k, currentRow))
                    cellLabel.BackColor = Color.Black;
                else
                    cellLabel.BackColor = Color.White;
            }
        }

        private void StopStartButton_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                SetRules();
                timer1.Start();
            }
            else
                timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NewIteration();
            if (currentRow < 15)
                currentRow++;
            else
                timer1.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillTable();
            currentRow = 1;
        }
    }
}
