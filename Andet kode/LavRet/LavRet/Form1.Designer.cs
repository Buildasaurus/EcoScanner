namespace LavRet
{
	partial class Form1
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
			this.button1 = new System.Windows.Forms.Button();
			this.NavnText = new System.Windows.Forms.TextBox();
			this.IDtext = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.VareList = new System.Windows.Forms.ListBox();
			this.AmountList = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(470, 400);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(173, 38);
			this.button1.TabIndex = 0;
			this.button1.Text = "Opload";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// NavnText
			// 
			this.NavnText.Location = new System.Drawing.Point(193, 93);
			this.NavnText.Name = "NavnText";
			this.NavnText.Size = new System.Drawing.Size(268, 31);
			this.NavnText.TabIndex = 2;
			this.NavnText.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// IDtext
			// 
			this.IDtext.Location = new System.Drawing.Point(193, 56);
			this.IDtext.Name = "IDtext";
			this.IDtext.Size = new System.Drawing.Size(268, 31);
			this.IDtext.TabIndex = 3;
			this.IDtext.TextChanged += new System.EventHandler(this.IDtext_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(157, 59);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(30, 25);
			this.label1.TabIndex = 4;
			this.label1.Text = "ID";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(134, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 25);
			this.label2.TabIndex = 5;
			this.label2.Text = "Navn";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(141, 141);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(46, 25);
			this.label3.TabIndex = 6;
			this.label3.Text = "Vare";
			// 
			// VareList
			// 
			this.VareList.FormattingEnabled = true;
			this.VareList.ItemHeight = 25;
			this.VareList.Items.AddRange(new object[] {
            "Vand, postevand, vejl. Værdier",
            "Ris, parboiled, rå",
            "Æblemost, uspec.",
            "Bouillon, hønsekød, spiseklar",
            "Olivenolie",
            "Broccoli, rå",
            "Asparges, grønne, rå",
            "Forårsløg, rå",
            "Citron, rå",
            "Spinat, rå",
            "Parmesan ost, 32+",
            "Hasselnød, tørret"});
			this.VareList.Location = new System.Drawing.Point(184, 141);
			this.VareList.Name = "VareList";
			this.VareList.Size = new System.Drawing.Size(268, 179);
			this.VareList.TabIndex = 7;
			// 
			// AmountList
			// 
			this.AmountList.FormattingEnabled = true;
			this.AmountList.ItemHeight = 25;
			this.AmountList.Items.AddRange(new object[] {
            "0.1",
            "0.2",
            "0.2",
            "0.015",
            "0.028",
            "0.06",
            "0.09",
            "0.038",
            "0.0425",
            "0.025",
            "0.12",
            "0.007"});
			this.AmountList.Location = new System.Drawing.Point(470, 141);
			this.AmountList.Name = "AmountList";
			this.AmountList.Size = new System.Drawing.Size(268, 179);
			this.AmountList.TabIndex = 8;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.AmountList);
			this.Controls.Add(this.VareList);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.IDtext);
			this.Controls.Add(this.NavnText);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Button button1;
		private TextBox NavnText;
		private TextBox IDtext;
		private Label label1;
		private Label label2;
		private Label label3;
		private ListBox VareList;
		private ListBox AmountList;
	}
}