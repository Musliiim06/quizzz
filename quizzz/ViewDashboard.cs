using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using quizzz.Services;
using quizzz.Models;

namespace quizzz
{
    public partial class ViewDashboard : UserControl
    {
        private readonly AdminService _service;
        private readonly int _userId; // set via ctor

        public ViewDashboard() : this(0) { }

        public ViewDashboard(int userId)
        {
            _service = new AdminService();
            // if userId == 0, pick first non-admin user if exists
            if (userId == 0)
            {
                var first = _service.GetAllUsers().FirstOrDefault(u => u.Role != "Admin");
                _userId = first != null ? first.Id : 1;
            }
            else
            {
                _userId = userId;
            }

            this.BackColor = Color.FromArgb(16, 23, 42);
            this.Dock = DockStyle.Fill;
            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            // Fetch real data
            int quizzesCompleted = _service.GetQuizzesCompletedCount(_userId);
            double avgScore = Math.Round(_service.GetAverageScore(_userId), 1);
            var recent = _service.GetRecentResults(_userId, 10);

            // Main container
            var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4 };
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            main.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Header
            var header = new Panel { Dock = DockStyle.Top, Height = 72, BackColor = Color.Transparent };
            var lblWelcome = new Label
            {
                Text = "Welcome back, Engineer",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(18, 12)
            };
            var lblSession = new Label
            {
                Text = $"Session started {DateTime.Now:hh:mm tt} • Last active: Today",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(170, 180, 200),
                AutoSize = true,
                Location = new Point(18, 48)
            };
            header.Controls.Add(lblWelcome);
            header.Controls.Add(lblSession);
            main.Controls.Add(header, 0, 0);

            // Stats row
            var statsRow = new TableLayoutPanel { Dock = DockStyle.Top, Height = 140, ColumnCount = 4, Margin = new Padding(18, 6, 18, 6) };
            for (int i = 0; i < 4; i++) statsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            void AddStat(string title, string value, string sub, int col)
            {
                var card = new ModernPanel { Dock = DockStyle.Fill, Margin = new Padding(8), FillColor = Color.FromArgb(22, 30, 46), BorderRadius = 8 };
                var lblVal = new Label { Text = value, Font = new Font("Segoe UI", 22, FontStyle.Bold), ForeColor = Color.White, Location = new Point(18, 16), AutoSize = true, BackColor = Color.Transparent };
                var lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 10), ForeColor = Color.LightGray, Location = new Point(18, 62), AutoSize = true, BackColor = Color.Transparent };
                var lblSub = new Label { Text = sub, Font = new Font("Segoe UI", 8), ForeColor = Color.FromArgb(150, 150, 160), Location = new Point(18, 86), AutoSize = true, BackColor = Color.Transparent };
                card.Controls.Add(lblVal); card.Controls.Add(lblTitle); card.Controls.Add(lblSub);
                statsRow.Controls.Add(card, col, 0);
            }

            AddStat("Quizzes Completed", quizzesCompleted.ToString(), "+3 this week", 0);
            AddStat("Average Score", avgScore > 0 ? avgScore + "%" : "0%", "+2.1% vs last week", 1);
            AddStat("Global Rank", "#142", "Top 5%", 2);
            AddStat("Current Streak", "12 days", "Personal best!", 3);

            main.Controls.Add(statsRow, 0, 1);

            // Launch card + right quick access (matches mockup spacing/colors)
            var launchRow = new TableLayoutPanel { Dock = DockStyle.Top, Height = 170, ColumnCount = 2, Margin = new Padding(18, 6, 18, 6) };
            launchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            launchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

            var launchCard = new ModernPanel { Dock = DockStyle.Fill, FillColor = Color.FromArgb(28, 38, 58), BorderRadius = 8 };
            var launchInner = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            var playIcon = new Panel { Size = new Size(56, 56), BackColor = Color.FromArgb(52, 120, 255), Location = new Point(18, 28) };
            var lblLaunchTitle = new Label { Text = "Launch New Quiz Session", ForeColor = Color.White, Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(88, 28), AutoSize = true, BackColor = Color.Transparent };
            var lblLaunchDesc = new Label { Text = "Start a new technical assessment. Choose from 50+ topics across programming, architecture, and design patterns.", ForeColor = Color.LightGray, Font = new Font("Segoe UI", 10), Location = new Point(88, 60), Size = new Size(480, 36), BackColor = Color.Transparent };
            launchInner.Controls.Add(playIcon); launchInner.Controls.Add(lblLaunchTitle); launchInner.Controls.Add(lblLaunchDesc);

            // Info chips below
            var chips = new FlowLayoutPanel { Location = new Point(88, 106), Size = new Size(520, 40), BackColor = Color.Transparent, FlowDirection = FlowDirection.LeftToRight };
            string[] chipTexts = { "10-50 questions", "15-60 minutes", "Ctrl+N" };
            foreach (var t in chipTexts)
            {
                var chip = new ModernPanel { Size = new Size(140, 30), FillColor = Color.FromArgb(22, 30, 46), BorderRadius = 6, Margin = new Padding(6) };
                var l = new Label { Text = t, ForeColor = Color.FromArgb(160, 180, 220), Font = new Font("Segoe UI", 9), Location = new Point(10, 6), AutoSize = true, BackColor = Color.Transparent };
                chip.Controls.Add(l);
                chips.Controls.Add(chip);
            }
            launchInner.Controls.Add(chips);
            launchCard.Controls.Add(launchInner);

            // Right quick access panel
            var rightPanel = new ModernPanel { Dock = DockStyle.Fill, FillColor = Color.FromArgb(22, 30, 46), BorderRadius = 8 };
            var lblQuick = new Label { Text = "Quick Access", ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(12, 12), AutoSize = true, BackColor = Color.Transparent };
            rightPanel.Controls.Add(lblQuick);
            var qaFlow = new FlowLayoutPanel { Location = new Point(12, 44), Size = new Size(240, 100), BackColor = Color.Transparent, FlowDirection = FlowDirection.TopDown };
            string[] quicks = { "C# Fundamentals", "Design Patterns", "SQL Queries", "System Design" };
            foreach (var q in quicks)
            {
                var b = new Button { Text = q, Width = 220, Height = 36, BackColor = Color.FromArgb(13, 22, 36), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Margin = new Padding(0, 6, 0, 0) };
                b.FlatAppearance.BorderColor = Color.FromArgb(40, 50, 70);
                qaFlow.Controls.Add(b);
            }
            rightPanel.Controls.Add(qaFlow);

            launchRow.Controls.Add(launchCard, 0, 0);
            launchRow.Controls.Add(rightPanel, 1, 0);
            main.Controls.Add(launchRow, 0, 2);

            // Recent Activity
            var recentCard = new ModernPanel { Dock = DockStyle.Fill, FillColor = Color.FromArgb(22, 30, 46), BorderRadius = 8, Margin = new Padding(18, 6, 18, 18) };
            var recentLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            recentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            recentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

            var lblRecent = new Label { Text = "Recent Activity", ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(12, 12), AutoSize = true, BackColor = Color.Transparent };
            recentCard.Controls.Add(lblRecent);

            var itemsFlow = new FlowLayoutPanel { Dock = DockStyle.Left, Width = 760, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, Location = new Point(12, 44), BackColor = Color.Transparent };
            foreach (var r in recent)
            {
                var item = new ModernPanel { Width = 740, Height = 64, Margin = new Padding(6), FillColor = Color.FromArgb(28, 36, 52), BorderRadius = 6 };
                var name = new Label { Text = r.QuizName, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(12, 10), AutoSize = true, BackColor = Color.Transparent };
                var meta = new Label { Text = r.CompletedAt.ToLocalTime().ToString("g"), ForeColor = Color.LightGray, Font = new Font("Segoe UI", 8), Location = new Point(12, 34), AutoSize = true, BackColor = Color.Transparent };
                var score = new Label { Text = r.Score.ToString(), ForeColor = Color.FromArgb(84, 152, 255), Font = new Font("Segoe UI", 12, FontStyle.Bold), Location = new Point(660, 20), AutoSize = true, BackColor = Color.Transparent };
                item.Controls.Add(name); item.Controls.Add(meta); item.Controls.Add(score);
                itemsFlow.Controls.Add(item);
            }

            recentCard.Controls.Add(itemsFlow);
            main.Controls.Add(recentCard, 0, 3);

            this.Controls.Add(main);
        }
    }
}
