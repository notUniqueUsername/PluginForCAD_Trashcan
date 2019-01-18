using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginForCAD_TrashCanUI
{
    public partial class MainForm : Form
    {
        private Label _topRadiusLabel = new Label();
        private Label _bottomRadiusLabel = new Label();

        public MainForm()
        {
            InitializeComponent();
            UrnFormComboBox.Items.Insert(0, "Паралелепипидная");
            UrnFormComboBox.Items.Insert(1, "Цилиндрическая");
            UrnFormComboBox.SelectedIndex = 0;

        }

        private void UrnFormComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (UrnFormComboBox.SelectedIndex)
            {
                case 0:
                    {
                        TopWidthLabel.Visible = true;
                        TopLengthLabel.Visible = true;
                        BottomLengthLabel.Visible = true;
                        BottomWidthLabel.Visible = true;
                        BottomWidthTextBox.Visible = true;
                        TopWidthTextBox.Visible = true;
                        _topRadiusLabel.Visible = false;
                        _bottomRadiusLabel.Visible = false;
                        break;
                    }
                case 1:
                    {

                        _topRadiusLabel.Name = "TopRadiusLabel";
                        _bottomRadiusLabel.Name = "BottomRadiusLabel";
                        _topRadiusLabel.Location = TopLengthLabel.Location;
                        _bottomRadiusLabel.Location = BottomLengthLabel.Location;
                        TopWidthLabel.Visible = false;
                        TopLengthLabel.Visible = false;
                        BottomLengthLabel.Visible = false;
                        BottomWidthLabel.Visible = false;
                        BottomWidthTextBox.Visible = false;
                        TopWidthTextBox.Visible = false;
                        _topRadiusLabel.Enabled = true;
                        _bottomRadiusLabel.Enabled = true;
                        _topRadiusLabel.Text = "Радиус";
                        _bottomRadiusLabel.Text = "Радиус";
                        _topRadiusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F); ;
                        _bottomRadiusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F); ;
                        _topRadiusLabel.Visible = true;
                        _bottomRadiusLabel.Visible = true;
                        Controls.Add(_bottomRadiusLabel);
                        Controls.Add(_topRadiusLabel);
                        break;
                    } 
                default:
                    break;
            }

        }

        private void BuildButton_Click(object sender, EventArgs e)
        {

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {

        }

        private void CloseKompasButton_Click(object sender, EventArgs e)
        {

        }
    }
}
