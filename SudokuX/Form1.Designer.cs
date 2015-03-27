namespace SudokuX
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
            this.sudokuGrid1 = new SudokuX.Controls.SudokuGrid();
            this.SuspendLayout();
            // 
            // sudokuGrid1
            // 
            this.sudokuGrid1.ColumnCount = 9;
            this.sudokuGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.sudokuGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.sudokuGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.sudokuGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.sudokuGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.sudokuGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.sudokuGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.sudokuGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.sudokuGrid1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.sudokuGrid1.Location = new System.Drawing.Point(13, 13);
            this.sudokuGrid1.Name = "sudokuGrid1";
            this.sudokuGrid1.RowCount = 9;
            this.sudokuGrid1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sudokuGrid1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sudokuGrid1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sudokuGrid1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sudokuGrid1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sudokuGrid1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sudokuGrid1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sudokuGrid1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sudokuGrid1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sudokuGrid1.Size = new System.Drawing.Size(381, 382);
            this.sudokuGrid1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(412, 410);
            this.Controls.Add(this.sudokuGrid1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SudokuGrid sudokuGrid1;


    }
}

