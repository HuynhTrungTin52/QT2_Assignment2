namespace ECommerceGUI
{
    partial class frmFilter
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
            this.cboFilterType = new System.Windows.Forms.ComboBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.dgvFilterResult = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterResult)).BeginInit();
            this.SuspendLayout();
            // 
            // cboFilterType
            // 
            this.cboFilterType.FormattingEnabled = true;
            this.cboFilterType.Items.AddRange(new object[] {
            "Best Sellers",
            "Purchased Items",
            "Revenue"});
            this.cboFilterType.Location = new System.Drawing.Point(25, 33);
            this.cboFilterType.Name = "cboFilterType";
            this.cboFilterType.Size = new System.Drawing.Size(198, 24);
            this.cboFilterType.TabIndex = 0;
            // 
            // btnExecute
            // 
            this.btnExecute.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnExecute.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecute.Location = new System.Drawing.Point(250, 33);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(107, 42);
            this.btnExecute.TabIndex = 1;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // dgvFilterResult
            // 
            this.dgvFilterResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterResult.Location = new System.Drawing.Point(7, 81);
            this.dgvFilterResult.Name = "dgvFilterResult";
            this.dgvFilterResult.RowHeadersWidth = 51;
            this.dgvFilterResult.RowTemplate.Height = 24;
            this.dgvFilterResult.Size = new System.Drawing.Size(723, 357);
            this.dgvFilterResult.TabIndex = 2;
            this.dgvFilterResult.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilterResult_CellContentClick);
            // 
            // frmFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvFilterResult);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.cboFilterType);
            this.Name = "frmFilter";
            this.Text = "frmFilter";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboFilterType;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.DataGridView dgvFilterResult;
    }
}