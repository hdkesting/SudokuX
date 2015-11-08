namespace SudokuX.Solver.Visualizers
{
    partial class GridVisualizerForm
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
            this.GridLabel = new System.Windows.Forms.Label();
            this.radioSolution = new System.Windows.Forms.RadioButton();
            this.radioChallenge = new System.Windows.Forms.RadioButton();
            this.radioComplexity = new System.Windows.Forms.RadioButton();
            this.radioClues = new System.Windows.Forms.RadioButton();
            this.radioAvailable = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // GridLabel
            // 
            this.GridLabel.AutoSize = true;
            this.GridLabel.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GridLabel.Location = new System.Drawing.Point(12, 68);
            this.GridLabel.Name = "GridLabel";
            this.GridLabel.Size = new System.Drawing.Size(90, 19);
            this.GridLabel.TabIndex = 0;
            this.GridLabel.Text = "GridLabel";
            // 
            // radioSolution
            // 
            this.radioSolution.AutoSize = true;
            this.radioSolution.Location = new System.Drawing.Point(13, 13);
            this.radioSolution.Name = "radioSolution";
            this.radioSolution.Size = new System.Drawing.Size(63, 17);
            this.radioSolution.TabIndex = 1;
            this.radioSolution.TabStop = true;
            this.radioSolution.Text = "Solution";
            this.radioSolution.UseVisualStyleBackColor = true;
            this.radioSolution.Click += new System.EventHandler(this.Radio_Click);
            this.radioSolution.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Radio_KeyDown);
            // 
            // radioChallenge
            // 
            this.radioChallenge.AutoSize = true;
            this.radioChallenge.Location = new System.Drawing.Point(83, 13);
            this.radioChallenge.Name = "radioChallenge";
            this.radioChallenge.Size = new System.Drawing.Size(72, 17);
            this.radioChallenge.TabIndex = 2;
            this.radioChallenge.TabStop = true;
            this.radioChallenge.Text = "Challenge";
            this.radioChallenge.UseVisualStyleBackColor = true;
            this.radioChallenge.Click += new System.EventHandler(this.Radio_Click);
            this.radioChallenge.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Radio_KeyDown);
            // 
            // radioComplexity
            // 
            this.radioComplexity.AutoSize = true;
            this.radioComplexity.Location = new System.Drawing.Point(162, 13);
            this.radioComplexity.Name = "radioComplexity";
            this.radioComplexity.Size = new System.Drawing.Size(75, 17);
            this.radioComplexity.TabIndex = 3;
            this.radioComplexity.TabStop = true;
            this.radioComplexity.Text = "Complexity";
            this.radioComplexity.UseVisualStyleBackColor = true;
            this.radioComplexity.Click += new System.EventHandler(this.Radio_Click);
            this.radioComplexity.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Radio_KeyDown);
            // 
            // radioClues
            // 
            this.radioClues.AutoSize = true;
            this.radioClues.Location = new System.Drawing.Point(244, 13);
            this.radioClues.Name = "radioClues";
            this.radioClues.Size = new System.Drawing.Size(58, 17);
            this.radioClues.TabIndex = 4;
            this.radioClues.TabStop = true;
            this.radioClues.Text = "#Clues";
            this.radioClues.UseVisualStyleBackColor = true;
            this.radioClues.Click += new System.EventHandler(this.Radio_Click);
            this.radioClues.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Radio_KeyDown);
            // 
            // radioAvailable
            // 
            this.radioAvailable.AutoSize = true;
            this.radioAvailable.Location = new System.Drawing.Point(13, 37);
            this.radioAvailable.Name = "radioAvailable";
            this.radioAvailable.Size = new System.Drawing.Size(68, 17);
            this.radioAvailable.TabIndex = 5;
            this.radioAvailable.TabStop = true;
            this.radioAvailable.Text = "Available";
            this.radioAvailable.UseVisualStyleBackColor = true;
            this.radioAvailable.Click += new System.EventHandler(this.Radio_Click);
            this.radioAvailable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Radio_KeyDown);
            // 
            // GridVisualizerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(353, 387);
            this.Controls.Add(this.radioAvailable);
            this.Controls.Add(this.radioClues);
            this.Controls.Add(this.radioComplexity);
            this.Controls.Add(this.radioChallenge);
            this.Controls.Add(this.radioSolution);
            this.Controls.Add(this.GridLabel);
            this.MinimizeBox = false;
            this.Name = "GridVisualizerForm";
            this.Text = "GridVisualizerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label GridLabel;
        private System.Windows.Forms.RadioButton radioSolution;
        private System.Windows.Forms.RadioButton radioChallenge;
        private System.Windows.Forms.RadioButton radioComplexity;
        private System.Windows.Forms.RadioButton radioClues;
        private System.Windows.Forms.RadioButton radioAvailable;
    }
}