using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kompas6API5;
using PluginForCAD_TrashcanLibrary;

namespace PluginForCAD_TrashCanUI
{
    public partial class MainForm : Form
    {
        private Parameters _parameters;
        private UrnForms _urnForms;
        private KompasConnector _kompasObject = new KompasConnector();
        private Label _topRadiusLabel = new Label();
        private Label _bottomRadiusLabel = new Label();

        public MainForm()
        {
            InitializeComponent();
#if !DEBUG
            TestButton.Visible = false;
#endif
            UrnFormComboBox.Items.Insert(0, "Паралелепипидная");
            UrnFormComboBox.Items.Insert(1, "Цилиндрическая");
            UrnFormComboBox.SelectedIndex = 0;

        }

        private double StringTODouble(string text)
        {
            if (double.TryParse(text, out double result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Введено не число");
            }
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
                        _topRadiusLabel.Font = new Font("Microsoft Sans Serif", 14F); ;
                        _bottomRadiusLabel.Font = new Font("Microsoft Sans Serif", 14F); ;
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
            var parametersList = new List<double>();
            parametersList.Add(StringTODouble(BottomThicknessTextBox.Text));
            parametersList.Add(StringTODouble(WallThicknessTextBox.Text));
            parametersList.Add(StringTODouble(UrnHeightTextBox.Text));
            switch (UrnFormComboBox.SelectedIndex)
            {
                case 0:
                    parametersList.Add(StringTODouble(BottomWidthTextBox.Text));
                    parametersList.Add(StringTODouble(TopWidthTextBox.Text));
                    parametersList.Add(StringTODouble(BottomLengthORRadiusTextBox.Text));
                    parametersList.Add(StringTODouble(TopLengthORRadiusTextBox.Text));
                    _urnForms = UrnForms.Rectangle;
                    break;
                case 1:
                    parametersList.Add(StringTODouble(TopLengthORRadiusTextBox.Text));
                    parametersList.Add(StringTODouble(BottomLengthORRadiusTextBox.Text));
                    _urnForms = UrnForms.Circle;
                    break;
                default:
                    break;
            }
            if (StandCheckBox.Checked)
            {
                parametersList.Add(StringTODouble(StandHeightTextBox.Text));
            }
            try
            {
                _parameters = new Parameters(parametersList,_urnForms,StandCheckBox.Checked);
                var builder = new CircleUrnBuilder(_kompasObject.KompasObject);
                builder.Build(_parameters);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }



        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            _kompasObject.StartKompas();
        }

        private void CloseKompasButton_Click(object sender, EventArgs e)
        {
            _kompasObject.CloseKompas();
        }

        private void StandCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (StandCheckBox.Checked)
            {
                StandHeightTextBox.Visible = true;
                StandHeightLabel.Visible = true;
            }
            else
            {
                StandHeightTextBox.Visible = false;
                StandHeightLabel.Visible = false;
            }
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            BottomThicknessTextBox.Text = "2";
            WallThicknessTextBox.Text = "1";
            UrnHeightTextBox.Text = "30";
            BottomWidthTextBox.Text = "20";
            TopWidthTextBox.Text = "20";
            BottomLengthORRadiusTextBox.Text = "20";
            TopLengthORRadiusTextBox.Text = "20";
            StandHeightTextBox.Text = "40";
        }
    }
}
