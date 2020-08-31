namespace FRM_SerialSet
{
    partial class Form1
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
            this.NameInput = new System.Windows.Forms.TextBox();
            this.OK = new System.Windows.Forms.Button();
            this.readLocationLabel = new System.Windows.Forms.Label();
            this.readDataRangeLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NameInput
            // 
            this.NameInput.Location = new System.Drawing.Point(141, 39);
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(149, 20);
            this.NameInput.TabIndex = 0;
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(615, 302);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(94, 27);
            this.OK.TabIndex = 1;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // readLocationLabel
            // 
            this.readLocationLabel.AutoSize = true;
            this.readLocationLabel.Location = new System.Drawing.Point(41, 42);
            this.readLocationLabel.Name = "readLocationLabel";
            this.readLocationLabel.Size = new System.Drawing.Size(77, 13);
            this.readLocationLabel.TabIndex = 2;
            this.readLocationLabel.Text = "Read Location";
            this.readLocationLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // readDataRangeLabel
            // 
            this.readDataRangeLabel.AutoSize = true;
            this.readDataRangeLabel.Location = new System.Drawing.Point(41, 83);
            this.readDataRangeLabel.Name = "readDataRangeLabel";
            this.readDataRangeLabel.Size = new System.Drawing.Size(94, 13);
            this.readDataRangeLabel.TabIndex = 3;
            this.readDataRangeLabel.Text = "Read Data Range";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(141, 79);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(149, 20);
            this.textBox1.TabIndex = 4;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(567, 33);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(128, 31);
            this.connectButton.TabIndex = 5;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.readDataRangeLabel);
            this.Controls.Add(this.readLocationLabel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.NameInput);
            this.Name = "Form1";
            this.Text = "Serial Connection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NameInput;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label readLocationLabel;
        private System.Windows.Forms.Label readDataRangeLabel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button connectButton;
    }
}

