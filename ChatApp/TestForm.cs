using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            
            this.Click += (x,xx) => {
                //var l = new Label();
                //l.Text = "SASSA";
                //listView1.Controls.Add(l);

                var l = new Label();
                l.Text = "SSS";
                listBox1.Controls.Add(l);
                listBox1.Update();
                listBox1.DataSource = new List<string> { "a", "a", "a", "bb" };
            };
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
