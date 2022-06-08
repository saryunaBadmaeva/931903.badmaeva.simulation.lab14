using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAb14
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int busyOps = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            label4.Text = "";
            Model.Run((double)edLambdaArrival.Value, (double)edLambdaOper.Value, (int)edOperatorsAmount.Value);
            chart1.Series[0].Points.AddXY(0, 0);
            busyOps = Model.getBusyOperatorsSize();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        { 
            timer1.Enabled = Model.Iter();
            if (busyOps < Model.getBusyOperatorsSize())
            {
                chart1.Series[0].Points.AddXY(Model.Time, busyOps);
                busyOps++;
                chart1.Series[0].Points.AddXY(Model.Time, busyOps);
            }
            else
            {
                chart1.Series[0].Points.AddXY(Model.Time, busyOps);
                busyOps--;
                chart1.Series[0].Points.AddXY(Model.Time, busyOps);
            }
            label1.Text = Model.queueSize().ToString();
            label4.Text = Model.CAmount.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
