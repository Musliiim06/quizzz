using System.Windows.Forms;

namespace quizzz
{
    partial class ViewQuiz
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ViewQuiz
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "ViewQuiz";
            this.Size = new System.Drawing.Size(1200, 800);
            this.ResumeLayout(false);
        }
    }
}
