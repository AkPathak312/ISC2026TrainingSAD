namespace ISCTraining
{
    public partial class Form1 : Form
    {
        int invalidLogin = 0;
        int lockSeconds = 10;
        System.Windows.Forms.Timer loginTimer;
        System.Windows.Forms.Timer countdownTimer;
        DateTime competitionDate;
        public Form1()
        {
            InitializeComponent();
            loginTimer = new System.Windows.Forms.Timer();
            countdownTimer = new System.Windows.Forms.Timer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin" && textBox2.Text == "pass")
            {
                Dashboard form = new Dashboard(textBox1.Text);
                form.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Failure!");
                invalidLogin++;
                if (invalidLogin >= 3)
                {
                    MessageBox.Show("Too many unsuccessful attempts. Please login in sometime", "Error");
                    button1.Enabled = false;
                    loginTimer.Start();
                    label1.Text = $"Try again in {lockSeconds} seconds";
                    invalidLogin = 0;
                }
            }
        }

        private void loginTimerFuntion(object? sender, EventArgs e)
        {
            lockSeconds--;
            label1.Text = $"Try again in {lockSeconds} seconds";
            if (lockSeconds <= 0)
            {
                loginTimer.Stop();
                button1.Enabled = true;
                label1.Text = "";
                lockSeconds = 10;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loginTimer.Interval = 1000;
            loginTimer.Tick += loginTimerFuntion;
            label1.Text = "";

            countdownTimer.Interval = 1000;
            countdownTimer.Tick += countdownTimerFunction;
            countdownTimer.Start();
            competitionDate = new DateTime(2026, 02, 25);
            //competitionDate = new DateTime(5 * 60 * 1000);
        }

        private void countdownTimerFunction(object? sender, EventArgs e)
        {
            TimeSpan timeSpan = competitionDate - DateTime.Now;
            label2.Text = $"Competition starts in {timeSpan.Days} Days, {timeSpan.Hours} Hours, {timeSpan.Minutes} minutes and {timeSpan.Seconds} Seconds";
            if (timeSpan.TotalSeconds <= 0)
            {
                countdownTimer.Stop();
                label2.Text = "";
            }
        }
    }
}
