namespace PluginForCAD_TrashCanUI
{
    partial class PluginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConnectButton = new System.Windows.Forms.Button();
            this.CloseKompasButton = new System.Windows.Forms.Button();
            this.UrnFormComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.userControlForRectangleParameters1 = new PluginForCAD_TrashCanUI.UserControlForRectangleParameters();
            this.userControlForUrnCircleParameters1 = new PluginForCAD_TrashCanUI.UserControlForCircleParameters();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(9, 42);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(180, 23);
            this.ConnectButton.TabIndex = 29;
            this.ConnectButton.Text = "Подключить компас";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // CloseKompasButton
            // 
            this.CloseKompasButton.Location = new System.Drawing.Point(9, 71);
            this.CloseKompasButton.Name = "CloseKompasButton";
            this.CloseKompasButton.Size = new System.Drawing.Size(179, 23);
            this.CloseKompasButton.TabIndex = 28;
            this.CloseKompasButton.Text = "Отключить компас";
            this.CloseKompasButton.UseVisualStyleBackColor = true;
            this.CloseKompasButton.Click += new System.EventHandler(this.CloseKompasButton_Click);
            // 
            // UrnFormComboBox
            // 
            this.UrnFormComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UrnFormComboBox.FormattingEnabled = true;
            this.UrnFormComboBox.Location = new System.Drawing.Point(201, 105);
            this.UrnFormComboBox.Name = "UrnFormComboBox";
            this.UrnFormComboBox.Size = new System.Drawing.Size(166, 21);
            this.UrnFormComboBox.TabIndex = 27;
            this.UrnFormComboBox.SelectedIndexChanged += new System.EventHandler(this.UrnFormComboBox_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label8.Location = new System.Drawing.Point(5, 100);
            this.label8.Margin = new System.Windows.Forms.Padding(3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 24);
            this.label8.TabIndex = 26;
            this.label8.Text = "Форма урны";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Location = new System.Drawing.Point(5, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 24);
            this.label1.TabIndex = 25;
            this.label1.Text = "Компас";
            // 
            // userControlForRectangleParameters1
            // 
            this.userControlForRectangleParameters1.KompasConnector = null;
            this.userControlForRectangleParameters1.Location = new System.Drawing.Point(-1, 130);
            this.userControlForRectangleParameters1.Name = "userControlForRectangleParameters1";
            this.userControlForRectangleParameters1.Size = new System.Drawing.Size(380, 408);
            this.userControlForRectangleParameters1.TabIndex = 30;
            // 
            // userControlForUrnCircleParameters1
            // 
            this.userControlForUrnCircleParameters1.KompasConnector = null;
            this.userControlForUrnCircleParameters1.Location = new System.Drawing.Point(-1, 130);
            this.userControlForUrnCircleParameters1.Name = "userControlForUrnCircleParameters1";
            this.userControlForUrnCircleParameters1.Size = new System.Drawing.Size(380, 408);
            this.userControlForUrnCircleParameters1.TabIndex = 31;
            // 
            // PluginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 538);
            this.Controls.Add(this.userControlForUrnCircleParameters1);
            this.Controls.Add(this.userControlForRectangleParameters1);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.CloseKompasButton);
            this.Controls.Add(this.UrnFormComboBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(2048, 577);
            this.MinimumSize = new System.Drawing.Size(407, 577);
            this.Name = "PluginForm";
            this.Text = "PluginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button CloseKompasButton;
        private System.Windows.Forms.ComboBox UrnFormComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private UserControlForRectangleParameters userControlForRectangleParameters1;
        private UserControlForCircleParameters userControlForUrnCircleParameters1;
    }
}