using System;
using System.Drawing;
using System.Windows.Forms;

namespace quizzz
{
    public partial class ViewQuiz : UserControl
    {
        public ViewQuiz()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(16, 23, 42);
            InitQuiz();
        }

        private void InitQuiz()
        {
            Label lblQuest = new Label
            {
                Text = "Explain the difference between Abstract Class and Interface in C#",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 50),
                Size = new Size(800, 100),
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(lblQuest);

            string[] options = { "Abstract classes can have implementation", "Interfaces only define contracts", "A class can implement multiple interfaces", "All of the above" };
            for (int i = 0; i < options.Length; i++)
            {
                Button btn = new Button
                {
                    Text = options[i],
                    Location = new Point(50, 180 + (i * 70)),
                    Size = new Size(600, 55),
                    BackColor = Color.FromArgb(30, 41, 59),
                    ForeColor = Color.LightSlateGray,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 12)
                };
                btn.FlatAppearance.BorderColor = Color.FromArgb(51, 65, 85);
                this.Controls.Add(btn);
            }
        }
    }
}