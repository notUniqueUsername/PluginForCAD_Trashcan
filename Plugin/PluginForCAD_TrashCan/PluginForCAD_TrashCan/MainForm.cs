using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginForCAD_TrashCan
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            comboBox1.Items.Insert(0, "Паралелепипидная");
            comboBox1.Items.Insert(1, "Цилиндрическая");
            comboBox1.SelectedIndex = 0;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                var TopRadiusLabel = new Label();
                var BottomRadiusLabel = new Label();
                TopRadiusLabel.Name = "TopRadiusLabel";
                BottomRadiusLabel.Name = "BottomRadiusLabel";
                TopRadiusLabel.Location = TopLengthLabel.Location;
                BottomRadiusLabel.Location = BottomLengthLabel.Location;
                TopWidthLabel.Visible = false;
                TopLengthLabel.Visible = false;
                BottomLengthLabel.Visible = false;
                BottomWidthLabel.Visible = false;
                BottomWidthTextBox.Visible = false;
                TopWidthTextBox.Visible = false;
                TopRadiusLabel.Enabled = true;
                BottomRadiusLabel.Enabled = true;
                TopRadiusLabel.Text = "Радиус";
                BottomRadiusLabel.Text = "Радиус";
                TopRadiusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F); ;
                BottomRadiusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F); ;
                TopRadiusLabel.Visible = true;
                BottomRadiusLabel.Visible = true;
                Controls.Add(BottomRadiusLabel);
                Controls.Add(TopRadiusLabel);
            }
        }
    }
}
