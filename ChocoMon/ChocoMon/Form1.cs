using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.Win32;
using System.IO;


namespace ChocoMon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            notifyIcon1.Visible = false;
            this.notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
            this.Resize += new System.EventHandler(this.Form1_Resize);

            //Read period and * 1000 to seconds
            string UpdatePeriod = ConfigurationManager.AppSettings["UpdatePeriod"];
            textBox1.Text = UpdatePeriod;
            int UpdatePeriodNum = Convert.ToInt32(UpdatePeriod);
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = UpdatePeriodNum * 1000;

            // Enable timer.
            timer1.Enabled = true;
            button1.Text = "Stop";

            //Checked checkbox Tray an startup
            string MinimizeToTray = ConfigurationManager.AppSettings["MinimizeToTray"];
            if (MinimizeToTray == "1")
            {
                checkTray.Checked = true;
            }
            else
            {
                checkTray.Checked = false;
            }
            //checked autostart
            //Checked checkbox Tray an startup
            string Autostart = ConfigurationManager.AppSettings["Autostart"];
            if (Autostart == "1")
            {
                checkStartUp.Checked = true;
            }
            else
            {
                checkStartUp.Checked = false;
            }
            //check update
            string Update = ConfigurationManager.AppSettings["Update"];
            if (Update == "1")
            {
                checkBox1.Checked = true;
                CheckUpdate();
            }

        }
        string WorkDir = System.Windows.Forms.Application.StartupPath;
        //For grouping dynamic elements
        GroupBox new_groupBox = new GroupBox();
        int A = 3;
        int B = 3;
        int C = 3;

        private void Check_Click(object sender, EventArgs e)

        {
            //время выполнения
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            //save timer to config file
            SaveTimer();

            //очищаем форму
            new_groupBox.Controls.Clear();

            new_groupBox.Height = 1000;
            new_groupBox.Width = 500;
            this.Controls.Add(new_groupBox);


            string URL;
            // Read the file and display it line by line.
            //string WorkDir = System.Windows.Forms.Application.StartupPath;
            System.IO.StreamReader file = new System.IO.StreamReader(WorkDir + @"\\UrlList.txt");
            while ((URL = file.ReadLine()) != null)
            {
                AddNewTextBox(URL);
                AddNewTextBox2(URL);

            }

            file.Close();
            A = 3;
            B = 3;
            C = 3;

            //time stop and add to textbox 
            sw.Stop();
            //в секундах
            textBox2.Text = (sw.ElapsedMilliseconds / 1000.0).ToString();

        }

        public System.Windows.Forms.TextBox AddNewTextBox(string URL)
        {
            TextBox txt = new TextBox();
            this.Controls.Add(txt);
            txt.Top = A * 10;
            txt.Left = 45;
            //set string from app.config
            txt.Text = URL;
            txt.Width = 300;
            txt.ReadOnly = true;
            A = A + 3;
            new_groupBox.Controls.Add(txt);
            return txt;
        }

        //Download stats
        public System.Windows.Forms.TextBox AddNewTextBox2(string URL)
        {

            //string URL = ConfigurationManager.AppSettings[ii];

            // Создаём экземпляр класса
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            // Присваиваем текстовой переменной k html-код

            WebClient webClient = new WebClient();
            string PageIn = webClient.DownloadString(URL);
            //string PageIn = webClient.DownloadString("2.html");
            // Загружаем в класс (парсер) наш html
            doc.LoadHtml(PageIn);
            //получить нужны код для ноды можно через среду разработки хрома - копируется необходимы кусок как 
            //Copy as XPath
            //HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//div[@class='stat']/stat-number");

            HtmlNode FullStatNum = doc.DocumentNode.SelectSingleNode("//*[@id='stats']/div[1]/p[1]");
            HtmlNode PackageStatNum = doc.DocumentNode.SelectSingleNode("//*[@id='stats']/div[2]/p[1]");
            // Выводим на экран результат работы парсера
            //MessageBox.Show(bodyNode.InnerText);
            //MessageBox.Show(bodyNode.Attributes["stat-number"].Value);

            //Get image status and sent to AddNewPictureBox
            //work
            //HtmlNode ImageStatus = doc.DocumentNode.SelectSingleNode("//*[@id='mainColumn']/div[1]/div[1]/h2[2]/a/img");
            HtmlNode ImageStatus;
            HtmlNode GreenRedBall = doc.DocumentNode.SelectSingleNode("//*[@id='mainColumn']/div[1]/div[1]/h2[2]/a/img");
            HtmlNode GreyBall = doc.DocumentNode.SelectSingleNode("//*[@id='mainColumn']/div[1]/div[1]/h2[2]/img");
            HtmlNode YellowBall = doc.DocumentNode.SelectSingleNode("//*[@id='mainColumn']/div[1]/div[1]/h2[3]/img");

            if (GreenRedBall !=null)
            {
                 ImageStatus = GreenRedBall;
                 AddNewPictureBox(ImageStatus.Attributes["src"].Value, ImageStatus.Attributes["alt"].Value, URL);
            }
            else if (GreyBall != null)
            {
                 ImageStatus = GreyBall;
                 AddNewPictureBox(ImageStatus.Attributes["src"].Value, ImageStatus.Attributes["alt"].Value, URL);
            }
            else if (YellowBall != null)
            {
                 ImageStatus = YellowBall;
                 AddNewPictureBox(ImageStatus.Attributes["src"].Value, ImageStatus.Attributes["alt"].Value, URL);
            }
           
            
            TextBox txt2 = new TextBox();
            this.Controls.Add(txt2);
            txt2.Top = B * 10;
            txt2.Left = 370;
            txt2.Width = 63;
            txt2.ReadOnly = true;
            txt2.Text = PackageStatNum.InnerText;
            new ToolTip().SetToolTip(txt2, FullStatNum.InnerText + " All downloads");
            B = B + 3;

            new_groupBox.Controls.Add(txt2);
            return txt2;
        }

        //image Status
        public System.Windows.Forms.PictureBox AddNewPictureBox(string ImageStatus, string StatusComment, string URL)
        {
            /*
            string URL = ConfigurationManager.AppSettings[i];
            WebClient webClient = new WebClient();

            string PageIn = webClient.DownloadString(URL);
            var ball = Regex.Match(PageIn, @"\/content.*_ball_...png", RegexOptions.IgnoreCase).Value;

            PictureBox pict = new PictureBox();
            //string StatusImageURL = URL + ball;
            //pict.Load(StatusImageURL);
            //MessageBox.Show(ImageStatus);
            */

            PictureBox pict = new PictureBox();
            if (ImageStatus == "/content/images/green_ball_48.png")
            {
                pict.Load(WorkDir + "\\Images/green_ball_48.png");
            }
            else if (ImageStatus == "/content/images/grey_ball_48.png")
            {
                pict.Load(WorkDir + "\\Images/grey_ball_48.png");
            }
            else if (ImageStatus == "/content/images/yellow_ball_48.png")
            {
                pict.Load(WorkDir + "\\Images/yellow_ball_48.png");
            }
            else if (ImageStatus == "/content/images/red_ball_48.png")
            {
                pict.Load(WorkDir + "\\Images/red_ball_48.png");
            }
            else
            {
                pict.Load("https://chocolatey.org" + ImageStatus);
            }

            this.Controls.Add(pict);
            pict.Top = C * 10;
            pict.Left = 10;
            pict.Width = 20;
            pict.Height = 20;
            pict.SizeMode = PictureBoxSizeMode.StretchImage;

            new ToolTip().SetToolTip(pict, StatusComment + "Please double click to visit web site for more information.");
            pict.Click += (sender, e) => { System.Diagnostics.Process.Start(URL); };

            //pict.Text = "pictBox" + this.C.ToString();
            C = C + 3;
            new_groupBox.Controls.Add(pict);
            return pict;
        }

        //run check when timer tic
        private void timer1_Tick(object Sender, EventArgs e)
        {
            Check_Click(Check, null);
        }
        private void SaveTimer()
        {
            //save timer to config file
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings["UpdatePeriod"].Value = textBox1.Text;
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("UpdatePeriod");
            Properties.Settings.Default.Reload();

            int UpdatePeriodNum = Convert.ToInt32(textBox1.Text);
            timer1.Interval = UpdatePeriodNum * 1000;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Stop")
            {
                SaveTimer();
                button1.Text = "Start";
                timer1.Enabled = false;
            }
            else
            {
                SaveTimer();
                button1.Text = "Stop";
                timer1.Enabled = true;
            }
        }
        //minimize to tray
        private void Form1_Resize(object sender, EventArgs e)
        {
            // проверяем наше окно, и если оно было свернуто, делаем событие        
            if (WindowState == FormWindowState.Minimized && checkTray.Checked)
            {
                // прячем наше окно из панели
                this.ShowInTaskbar = false;
                // делаем нашу иконку в трее активной
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.BalloonTipTitle = "ChocoMon";
                notifyIcon1.BalloonTipText = "Now ChocoMon in tray";
                notifyIcon1.ShowBalloonTip(5);

            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // делаем нашу иконку скрытой
            notifyIcon1.Visible = false;
            // возвращаем отображение окна в панели
            this.ShowInTaskbar = true;
            //разворачиваем окно
            WindowState = FormWindowState.Normal;
        }

        private void checkTray_CheckedChanged(object sender, EventArgs e)
        {
            if (checkTray.Checked)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                config.AppSettings.Settings["MinimizeToTray"].Value = "1";
                config.Save(ConfigurationSaveMode.Modified);
            }
            else
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                config.AppSettings.Settings["MinimizeToTray"].Value = "0";
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        //Open UrlList.txt 
        private void Edit_Click(object sender, EventArgs e)
        {
            //string WorkDir = System.Windows.Forms.Application.StartupPath;
            //MessageBox.Show(WorkDir);
            System.Diagnostics.Process.Start (WorkDir+"\\UrlList.txt");
        }

        
        //Set startup when windows start
        private void checkStartUp_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (checkStartUp.Checked)
            {
                rk.SetValue("ChocoMon", Application.ExecutablePath.ToString());
            }
            else {
                rk.DeleteValue("ChocoMon", false);
            }
        }
        //Check Updates
        private void CheckUpdate()
        {
            //First check internet connection
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    //If available -> Check update
                    string TempDir = Path.GetTempPath();
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile("https://raw.githubusercontent.com/zersh01/ChocoMon/master/InstallChocoMon/latest.txt", TempDir + "\\latest.txt");

                    string LatestVersion = TempDir + "\\latest.txt";

                    using (StreamReader sr = new StreamReader(LatestVersion))
                    {
                        string CurVersion = textBox3.Text;
                        string NewVersion = sr.ReadToEnd();

                        if (CurVersion != NewVersion)
                        {

                            textBox3.ForeColor = Color.DarkRed;
                            textBox3.BackColor = Color.Yellow;
                            textBox3.Text = NewVersion;

                            ToolTip updtooltip = new ToolTip();
                            updtooltip.SetToolTip(textBox3, "Avalible new version. Press double click to go websuite.");

                            //Close and Delete file
                            sr.Close();
                            System.IO.File.Delete(LatestVersion);
                        }
                        else
                        {
                            //Close and Delete file latest
                            sr.Close();
                            System.IO.File.Delete(LatestVersion);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Don't check updates. No Internet Connection. You may disable auto check in Help menu.");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                config.AppSettings.Settings["Update"].Value = "1";
                config.Save(ConfigurationSaveMode.Modified);
            }
            else
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                config.AppSettings.Settings["Update"].Value = "0";
                config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}
