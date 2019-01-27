using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PluginForCAD_TrashcanLibrary;

namespace PluginForCAD_TrashCanUI
{
    public partial class UserControlForRectangleParameters : UserControl
    {
        private RectangleParameters _parameters;
        public KompasConnector KompasConnector { get; set; }

        public UserControlForRectangleParameters()
        {
            InitializeComponent();
#if !DEBUG
            TestButton.Visible = false;
#endif
        }

        /// <summary>
        /// Парсер строки в число
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Обработчик нажатия на конпку построить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildButton_Click(object sender, EventArgs e)
        {
            try
            {
                var parametersList = new List<double>();
                parametersList.Add(StringTODouble(BottomThicknessTextBox.Text));
                parametersList.Add(StringTODouble(WallThicknessTextBox.Text));
                parametersList.Add(StringTODouble(UrnHeightTextBox.Text));
                parametersList.Add(StringTODouble(TopWidthTextBox.Text));
                parametersList.Add(StringTODouble(BottomWidthTextBox.Text));
                parametersList.Add(StringTODouble(TopLengthORRadiusTextBox.Text));
                parametersList.Add(StringTODouble(BottomLengthORRadiusTextBox.Text));
                if (StandCheckBox.Checked)
                {
                    parametersList.Add(StringTODouble(StandHeightTextBox.Text));
                }
                _parameters = new RectangleParameters(parametersList, StandCheckBox.Checked, AshtrayCheckBox.Checked);
                var rectangleBuilder = new RectangleUrnBuilder(KompasConnector.KompasObject);
                rectangleBuilder.Build(_parameters);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Сперва нужно запустить компас", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                KompasConnector.CloseKompas();
                MessageBox.Show("Компас был закрыт не через приложение, откройте компас заново", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        /// <summary>
        /// Обработка нажатия на кнопку тест
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Обработка изменения состояния чекбокса стойки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Обработка кнопки закрыть
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.ParentForm.Close();
        }
    }
}
