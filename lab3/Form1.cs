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

        int i;
        string ruleString;
        private Dictionary<(bool, bool, bool), bool> ruleDictionary = new Dictionary<(bool, bool, bool), bool>();

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private bool GetRule(int i)
        {
            if (Convert.ToBoolean(ruleString[i].Equals('0')))
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
            ruleDictionary.Add((false, false, false), GetRule(7));
            ruleDictionary.Add((false, false, true), GetRule(6));
            ruleDictionary.Add((false, true, false), GetRule(5));
            ruleDictionary.Add((false, true, true), GetRule(4));
            ruleDictionary.Add((true, false, false), GetRule(3));
            ruleDictionary.Add((true, false, true), GetRule(2));
            ruleDictionary.Add((true, true, false), GetRule(1));
            ruleDictionary.Add((true, true, true), GetRule(0));
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
            Label RowNumber = CreateLabel();
            RowNumber.Text = Convert.ToString(i);
            tableLayoutPanel1.Controls.Add(RowNumber, 0, i);
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
                Label l = tableLayoutPanel1.GetControlFromPosition(k, i) as Label;
                if (PredictCellCondition(k, i))
                    l.BackColor = Color.Black;
                else
                    l.BackColor = Color.White;
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
            if (i < 15)
                i++;
            else
                timer1.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillTable();
            i = 1;
        }
    }
}
