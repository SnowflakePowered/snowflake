namespace Snowflake.Tests.ManualIntegrationTests
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
            this.romFileTestButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tgdbScrapeTest = new System.Windows.Forms.Button();
            this.retroarchcfgbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // romFileTestButton
            // 
            this.romFileTestButton.Location = new System.Drawing.Point(0, 0);
            this.romFileTestButton.Name = "romFileTestButton";
            this.romFileTestButton.Size = new System.Drawing.Size(280, 23);
            this.romFileTestButton.TabIndex = 0;
            this.romFileTestButton.Text = "Rom File Identification Tests";
            this.romFileTestButton.UseVisualStyleBackColor = true;
            this.romFileTestButton.Click += new System.EventHandler(this.romFileTestButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tgdbScrapeTest
            // 
            this.tgdbScrapeTest.Location = new System.Drawing.Point(0, 29);
            this.tgdbScrapeTest.Name = "tgdbScrapeTest";
            this.tgdbScrapeTest.Size = new System.Drawing.Size(280, 23);
            this.tgdbScrapeTest.TabIndex = 1;
            this.tgdbScrapeTest.Text = "Scrape with The GamesDB";
            this.tgdbScrapeTest.UseVisualStyleBackColor = true;
            this.tgdbScrapeTest.Click += new System.EventHandler(this.tgdbScrapeTest_Click);
            // 
            // retroarchcfgbtn
            // 
            this.retroarchcfgbtn.Location = new System.Drawing.Point(0, 68);
            this.retroarchcfgbtn.Name = "retroarchcfgbtn";
            this.retroarchcfgbtn.Size = new System.Drawing.Size(280, 21);
            this.retroarchcfgbtn.TabIndex = 2;
            this.retroarchcfgbtn.Text = "Generate Retroarch CFG";
            this.retroarchcfgbtn.UseVisualStyleBackColor = true;
            this.retroarchcfgbtn.Click += new System.EventHandler(this.retroarchcfgbtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.retroarchcfgbtn);
            this.Controls.Add(this.tgdbScrapeTest);
            this.Controls.Add(this.romFileTestButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button romFileTestButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button tgdbScrapeTest;
        private System.Windows.Forms.Button retroarchcfgbtn;
    }
}

