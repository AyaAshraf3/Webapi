namespace OMS
{
    partial class submitForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            btnSubmit = new Button();
            errorProvider1 = new ErrorProvider(components);
            label1 = new Label();
            txtQuantity = new TextBox();
            txtPrice = new TextBox();
            txtDir = new TextBox();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(83, 88);
            label2.Name = "label2";
            label2.Size = new Size(65, 20);
            label2.TabIndex = 1;
            label2.Text = "Quantity";
            
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(83, 148);
            label3.Name = "label3";
            label3.Size = new Size(41, 20);
            label3.TabIndex = 2;
            label3.Text = "Price";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(83, 205);
            label4.Name = "label4";
            label4.Size = new Size(70, 20);
            label4.TabIndex = 3;
            label4.Text = "Direction";
            // 
            // btnSubmit
            // 
            btnSubmit.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnSubmit.AutoSize = true;
            btnSubmit.Location = new Point(558, 268);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(120, 49);
            btnSubmit.TabIndex = 10;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += button1_Click;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
            label1.Location = new Point(30, 28);
            label1.Name = "label1";
            label1.Size = new Size(313, 28);
            label1.TabIndex = 11;
            label1.Text = "Enter the following information";
            // 
            // txtQuantity
            // 
            txtQuantity.Location = new Point(212, 88);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(330, 27);
            txtQuantity.TabIndex = 12;
            // 
            // txtPrice
            // 
            txtPrice.Location = new Point(212, 145);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(330, 27);
            txtPrice.TabIndex = 13;
            // 
            // txtDir
            // 
            txtDir.Location = new Point(212, 205);
            txtDir.Name = "txtDir";
            txtDir.Size = new Size(330, 27);
            txtDir.TabIndex = 14;
            // 
            // submitForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(749, 353);
            Controls.Add(txtDir);
            Controls.Add(txtPrice);
            Controls.Add(txtQuantity);
            Controls.Add(label1);
            Controls.Add(btnSubmit);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Name = "submitForm";
            Text = "Order Form";
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Label label3;
        private Label label4;
        private Button btnSubmit;
        private ErrorProvider errorProvider1;
        private Label label1;
        private TextBox txtDir;
        private TextBox txtPrice;
        private TextBox txtQuantity;
    }
}
