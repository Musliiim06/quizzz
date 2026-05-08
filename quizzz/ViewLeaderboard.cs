using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using quizzz.Services;

namespace quizzz
{
    public partial class ViewLeaderboard : UserControl
    {
        private readonly AdminService _service = new AdminService();

        public ViewLeaderboard()
        {
            this.BackColor = Color.FromArgb(16, 23, 42);
            InitializeLeaderboard();
        }

        private void InitializeLeaderboard()
        {
            Label lblTitle = new Label
            {
                Text = "High Scores",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(18, 12),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            ListView lv = new ListView
            {
                Location = new Point(18, 64),
                Size = new Size(900, 600),
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                View = View.Details,
                FullRowSelect = true
            };
            lv.Columns.Add("RANK", 80);
            lv.Columns.Add("NAME", 300);
            lv.Columns.Add("SCORE", 120);
            lv.Columns.Add("COMPLETION TIME", 160);

            // Load users and their best scores (simple demo)
            var users = _service.GetAllUsers().Where(u => u.Role != "Admin").ToList();
            int rank = 1;
            foreach (var u in users)
            {
                // For demo we will use sum of user's quiz results as score
                var results = _service.GetRecentResults(u.Id, 100);
                int total = results.Any() ? results.Sum(r => r.Score) : 0;
                string latest = results.Any() ? results.OrderByDescending(x => x.CompletedAt).First().CompletedAt.ToLocalTime().ToString("HH:mm") : "-";
                lv.Items.Add(new ListViewItem(new[] { rank.ToString(), u.Username, total.ToString(), latest }));
                rank++;
            }

            this.Controls.Add(lv);
        }
    }
}
