namespace LogoFX.Tools.Templates.Wizard
{
    partial class UserInputForm
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
            this.checkBoxFake = new System.Windows.Forms.CheckBox();
            this.checkBoxTests = new System.Windows.Forms.CheckBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBoxFake
            // 
            this.checkBoxFake.AutoSize = true;
            this.checkBoxFake.Location = new System.Drawing.Point(12, 12);
            this.checkBoxFake.Name = "checkBoxFake";
            this.checkBoxFake.Size = new System.Drawing.Size(112, 17);
            this.checkBoxFake.TabIndex = 0;
            this.checkBoxFake.Text = "Fake data support";
            this.checkBoxFake.UseVisualStyleBackColor = true;
            // 
            // checkBoxTests
            // 
            this.checkBoxTests.AutoSize = true;
            this.checkBoxTests.Location = new System.Drawing.Point(12, 49);
            this.checkBoxTests.Name = "checkBoxTests";
            this.checkBoxTests.Size = new System.Drawing.Size(52, 17);
            this.checkBoxTests.TabIndex = 1;
            this.checkBoxTests.Text = "Tests";
            this.checkBoxTests.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(178, 122);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // UserInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 157);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.checkBoxTests);
            this.Controls.Add(this.checkBoxFake);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserInputForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UserInputForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxFake;
        private System.Windows.Forms.CheckBox checkBoxTests;
        private System.Windows.Forms.Button buttonOk;
    }
}