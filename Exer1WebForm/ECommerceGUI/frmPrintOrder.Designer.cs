namespace ECommerceGUI
{
    partial class frmPrintOrder
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
            this.rtbInvoice = new System.Windows.Forms.RichTextBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbInvoice
            // 
            this.rtbInvoice.BackColor = System.Drawing.SystemColors.Info;
            this.rtbInvoice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbInvoice.Location = new System.Drawing.Point(30, 31);
            this.rtbInvoice.Name = "rtbInvoice";
            this.rtbInvoice.ReadOnly = true;
            this.rtbInvoice.Size = new System.Drawing.Size(779, 344);
            this.rtbInvoice.TabIndex = 0;
            this.rtbInvoice.Text = "";
            this.rtbInvoice.TextChanged += new System.EventHandler(this.rtbInvoice_TextChanged);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Location = new System.Drawing.Point(199, 411);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(404, 36);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmPrintOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(900, 478);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.rtbInvoice);
            this.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmPrintOrder";
            this.Text = "frmPrintOrder";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbInvoice;
        private System.Windows.Forms.Button btnPrint;
    }
}