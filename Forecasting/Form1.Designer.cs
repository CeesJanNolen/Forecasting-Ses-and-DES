namespace Forecasting
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
	  this.components = new System.ComponentModel.Container();
	  this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
	  this.checkBox1 = new System.Windows.Forms.CheckBox();
	  this.SuspendLayout();
	  // 
	  // zedGraphControl1
	  // 
	  this.zedGraphControl1.IsShowPointValues = true;
	  this.zedGraphControl1.Location = new System.Drawing.Point(50, 50);
	  this.zedGraphControl1.Name = "zedGraphControl1";
	  this.zedGraphControl1.Size = new System.Drawing.Size(10, 71);
	  this.zedGraphControl1.TabIndex = 0;
	  this.zedGraphControl1.UseExtendedPrintDialog = true;
	  // 
	  // checkBox1
	  // 
	  this.checkBox1.AutoSize = true;
	  this.checkBox1.Location = new System.Drawing.Point(168, 121);
	  this.checkBox1.Name = "checkBox1";
	  this.checkBox1.Size = new System.Drawing.Size(98, 21);
	  this.checkBox1.TabIndex = 0;
	  this.checkBox1.Text = "checkBox1";
	  this.checkBox1.UseVisualStyleBackColor = true;
	  // 
	  // Form1
	  // 
	  this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
	  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	  this.ClientSize = new System.Drawing.Size(662, 374);
	  this.Controls.Add(this.checkBox1);
	  this.Name = "Form1";
	  this.Text = "Form1";
	  this.Load += new System.EventHandler(this.Form1_Load);
	  this.ResumeLayout(false);
	  this.PerformLayout();

	}

	private ZedGraph.ZedGraphControl zedGraphControl1;
	#endregion

	private System.Windows.Forms.CheckBox checkBox1;
  }
}

