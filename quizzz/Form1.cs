using System;
using System.Drawing;
using System.Windows.Forms;

namespace quizzz
{
    public partial class Form1 : Form
    {
        private Panel sidebar;
        private Panel contentPanel;
        private Label lblActiveTitle;
        private Services.AdminService _adminService;

        public Form1()
        {
            InitializeComponent();
            _adminService = new Services.AdminService();
            SetupMainUI();
            // По умолчанию открываем Dashboard
            ShowView(new ViewDashboard());
        }

        private void SetupMainUI()
        {
            this.Text = "QuizForge v2.4.1";
            this.Size = new Size(1280, 800);
            this.BackColor = Color.FromArgb(16, 23, 42);

            // Сайдбар
            sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                BackColor = Color.FromArgb(10, 15, 28)
            };

            // Логотип сверху
            Label lblLogo = new Label
            {
                Text = "Q QuizForge v2.4.1",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            sidebar.Controls.Add(lblLogo);

            // Кнопки меню
            AddMenuButton("Dashboard", 80, (s, e) => ShowView(new ViewDashboard()));
            AddMenuButton("Quiz Library", 140, (s, e) => ShowView(new ViewQuiz()));
            AddMenuButton("High Scores", 200, (s, e) => ShowView(new ViewLeaderboard()));
            AddMenuButton("Settings", 260, (s, e) => { /* Settings code */ });

            // Список игроков (динамически из БД)
            Label lblPlayers = new Label
            {
                Text = "Players",
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 330),
                AutoSize = true
            };
            sidebar.Controls.Add(lblPlayers);

            var playersPanel = new FlowLayoutPanel
            {
                Location = new Point(10, 360),
                Size = new Size(240, 300),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown
            };

            var players = _adminService.GetAllUsers().FindAll(u => u.Role != "Admin");
            foreach (var p in players)
            {
                var pb = new Button
                {
                    Text = p.Username,
                    Width = 220,
                    Height = 34,
                    BackColor = Color.FromArgb(13, 22, 36),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Tag = p.Id,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                pb.FlatAppearance.BorderSize = 0;
                pb.Click += (s, e) => ShowView(new ViewDashboard((int)((Button)s).Tag));
                playersPanel.Controls.Add(pb);
            }

            sidebar.Controls.Add(playersPanel);

            // Профиль пользователя снизу
            Panel userPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Color.FromArgb(15, 23, 42)
            };
            Label lblUser = new Label
            {
                Text = "Engineer\nLevel 12 • 2,450 XP",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 9),
                Location = new Point(60, 20),
                AutoSize = true
            };
            userPanel.Controls.Add(lblUser);
            sidebar.Controls.Add(userPanel);

            // Основная панель для контента
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 23, 42),
                Padding = new Padding(30)
            };

            this.Controls.Add(contentPanel);
            this.Controls.Add(sidebar);
        }

        private void AddMenuButton(string text, int top, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = "    " + text,
                TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(148, 163, 184),
                Font = new Font("Segoe UI", 10),
                Size = new Size(240, 50),
                Location = new Point(10, top),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
            btn.Click += onClick;
            sidebar.Controls.Add(btn);
        }

        public void ShowView(UserControl view)
        {
            contentPanel.Controls.Clear();
            view.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(view);
        }
    }
}