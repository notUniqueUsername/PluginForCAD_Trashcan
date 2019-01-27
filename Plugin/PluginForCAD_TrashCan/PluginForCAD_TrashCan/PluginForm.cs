using PluginForCAD_TrashcanLibrary;
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
    public partial class PluginForm : Form
    {
        private KompasConnector _kompasObject = new KompasConnector();
        public PluginForm()
        {
            InitializeComponent();
            UrnFormComboBox.Items.Insert(0, "Паралелепипидная");
            UrnFormComboBox.Items.Insert(1, "Цилиндрическая");
            UrnFormComboBox.SelectedIndex = 0;
        }
        
        /// <summary>
        /// Обработка сены индекса в ComboBox для выбора формы урны
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UrnFormComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (UrnFormComboBox.SelectedIndex)
            {
                case 0:
                    {
                        userControlForRectangleParameters1.KompasConnector = _kompasObject;
                        userControlForUrnCircleParameters1.Visible = false;
                        userControlForRectangleParameters1.Visible = true;
                        break;
                    }
                case 1:
                    {
                        userControlForUrnCircleParameters1.KompasConnector = _kompasObject;
                        userControlForUrnCircleParameters1.Visible = true;
                        userControlForRectangleParameters1.Visible = false;
                        break;
                    }
                default:
                    break;
            }

        }

        /// <summary>
        /// Обработка кнопки подключить компас
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            _kompasObject.StartKompas();
        }

        /// <summary>
        /// Обработка кнопки отключить компас
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseKompasButton_Click(object sender, EventArgs e)
        {
            _kompasObject.CloseKompas();
        }
    }
}
