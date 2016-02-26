using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ControlCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region cmds
        private const string CHECK_DISK_CMD = "cmd.exe";
        private const string CHECK_DISK_PARAMS = "/c echo y|chkdsk.exe ";
        private const string CHECK_DISK_ARG = " /F /X";
        private const string DEFRAG_DISK_CMD = "defrag.exe";
        private const string DEFRAG_DISK_ARG = " /A /U /V";
        private const string CLEANER_DISK_CMD = "Cleanmgr.exe";
        private const string CLEANER_DISK_PARAMS = "/d ";
        #endregion

        bool isRunning = false;
        private System.Windows.Forms.NotifyIcon MyNotifyIcon;
        private IDictionary<int, string> commands;


        public delegate void isMainWindowClosing(bool result);
        public static event isMainWindowClosing isClosing;

        public MainWindow()
        {
            InitializeComponent();
            btnAbortShutdown.Visibility = Visibility.Hidden;

            MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
            MyNotifyIcon.Icon = ControlCenter.Properties.Resources.icon;
            MyNotifyIcon.MouseDoubleClick +=new System.Windows.Forms.MouseEventHandler(MyNotifyIcon_MouseDoubleClick);

            ControlCenter.Applications.VirusScanner.VrsScanner.VrsScanClosed += () => { this.WindowState = WindowState.Normal; };
            ControlCenter.Applications.CheckDisk.Chkdsk.ChkDskClosed += () => { this.WindowState = WindowState.Normal; };
            ControlCenter.Applications.Defragmenter.Defragmenter.DefragClosed += () => { this.WindowState = WindowState.Normal; };
            ControlCenter.Applications.SecureDownload.Downloader.DwnldClosed += () => { this.WindowState = WindowState.Normal; };
            ControlCenter.Applications.Cleaner.Cleaner.CleanDskClosed += () => { this.WindowState = WindowState.Normal; };
            EncryptionSoftware.MainWindow.CryptClosed += () => { this.WindowState = WindowState.Normal; };

            ProcessManagement.OverwriteLine += new ProcessManagement.OverwriteMessage(OverwriteStatusBox);
            ProcessManagement.AddLine += new ProcessManagement.NewMessage(AddLineStatusBox);
            ProcessManagement.IsFinished +=()=> {isRunning=false;};
        }

        #region Target Events
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isRunning)
                if (MessageBox.Show("Are you sure you want to close the application, while running?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                }

            if (isClosing != null)
                isClosing(true);
        }

        private void OverwriteStatusBox(string sText)
        {
            System.Action action = () =>
            {
                if (lbOutput.Items.Count > 0) lbOutput.Items.RemoveAt(lbOutput.Items.Count - 1);
                if (!String.IsNullOrEmpty(sText))
                {
                    lbOutput.Items.Add(sText);
                    lbOutput.SelectedIndex = lbOutput.Items.Count - 1;
                }
                lbOutput.Items.Add(sText);
            };
            lbOutput.Dispatcher.BeginInvoke(action);
        }

        private void AddLineStatusBox(string sText)
        {
            System.Action action = () =>
            {
                if (!String.IsNullOrEmpty(sText))
                {
                    lbOutput.Items.Add(sText);
                    lbOutput.SelectedIndex = lbOutput.Items.Count - 1;
                }
            };
            lbOutput.Dispatcher.BeginInvoke(action);
        }
        #endregion

        #region Minimize to Tray
        private void MyNotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                MyNotifyIcon.BalloonTipTitle = "Minimize to system tray.";
                MyNotifyIcon.BalloonTipText = "Click to open Control Center. ";
                MyNotifyIcon.ShowBalloonTip(400);
                MyNotifyIcon.Visible = true;

                ProcessManagement.OverwriteLine -= new ProcessManagement.OverwriteMessage(OverwriteStatusBox);
                ProcessManagement.AddLine -= new ProcessManagement.NewMessage(AddLineStatusBox);
                ProcessManagement.IsFinished -= () => { };
            }
            else if (this.WindowState == WindowState.Normal)
            {
                MyNotifyIcon.Visible = false;
                this.ShowInTaskbar = true;

                ProcessManagement.OverwriteLine += new ProcessManagement.OverwriteMessage(OverwriteStatusBox);
                ProcessManagement.AddLine += new ProcessManagement.NewMessage(AddLineStatusBox);
                ProcessManagement.IsFinished += () => { };
            }
        }
        #endregion

        #region buttons UI
        private void btnDefrag_MouseEnter(object sender, MouseEventArgs e)
        {
            btnDefrag.Content = "Defragment";
        }

        private void btnDefrag_MouseLeave(object sender, MouseEventArgs e)
        {
            btnDefrag.Content = "";
        }

        private void btnChkDsk_MouseEnter(object sender, MouseEventArgs e)
        {
            btnChkDsk.Content = "Check Disk";
        }

        private void btnChkDsk_MouseLeave(object sender, MouseEventArgs e)
        {
            btnChkDsk.Content = "";
        }

        private void btnCleaner_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCleaner.Content = "Cleaner\n  Tools";
        }

        private void btnCleaner_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCleaner.Content = "";
        }

        private void btnScan_MouseEnter(object sender, MouseEventArgs e)
        {
            btnScan.Content = "  Virus\nScanner";

        }

        private void btnScan_MouseLeave(object sender, MouseEventArgs e)
        {
            btnScan.Content = "";
        }

        private void btnSecureDwnld_MouseEnter(object sender, MouseEventArgs e)
        {
            btnSecureDwnld.Content = "   Secure\nDownload";
        }

        private void btnSecureDwnld_MouseLeave(object sender, MouseEventArgs e)
        {
            btnSecureDwnld.Content = "";
        }

        private void btnEncryption_MouseEnter(object sender, MouseEventArgs e)
        {
            btnEncryption.Content = "Encryption\n Software";
        }

        private void btnEncryption_MouseLeave(object sender, MouseEventArgs e)
        {
            btnEncryption.Content = "";
        }
        #endregion

        #region Tools Start
        private void btnEncryption_Click(object sender, RoutedEventArgs e)
        {
            EncryptionSoftware.MainWindow crypt = new EncryptionSoftware.MainWindow();
            crypt.Show();
            this.WindowState = WindowState.Minimized;
        }

        private void btnChkDsk_Click(object sender, RoutedEventArgs e)
        {
            ControlCenter.Applications.CheckDisk.Chkdsk chdsdk = new ControlCenter.Applications.CheckDisk.Chkdsk();
            chdsdk.Show();
            this.WindowState = WindowState.Minimized;
           
        }

        private void btnDefrag_Click(object sender, RoutedEventArgs e)
        {
            ControlCenter.Applications.Defragmenter.Defragmenter defrag = new ControlCenter.Applications.Defragmenter.Defragmenter();
            defrag.Show();
            this.WindowState = WindowState.Minimized;
        }

        private void btnSecureDwnld_Click(object sender, RoutedEventArgs e)
        {
            ControlCenter.Applications.SecureDownload.Downloader dwnld = new ControlCenter.Applications.SecureDownload.Downloader();
            dwnld.Show();
            this.WindowState = WindowState.Minimized;
        }

        private void btnScan_Click(object sender, RoutedEventArgs e)
        {
            ControlCenter.Applications.VirusScanner.VrsScanner vrsScan = new ControlCenter.Applications.VirusScanner.VrsScanner();
            vrsScan.Show();
            this.WindowState = WindowState.Minimized;
        }

        private void btnCleaner_Click(object sender, RoutedEventArgs e)
        {
            ControlCenter.Applications.Cleaner.Cleaner cleaner = new Applications.Cleaner.Cleaner();
            cleaner.Show();
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            MessageBox.Show("Version: " + version.ToString(), "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void miConverter_Click(object sender, RoutedEventArgs e)
        {
            ControlCenter.Applications.Converter.Converter Converter = new ControlCenter.Applications.Converter.Converter();
            Converter.Show();
        }

        private void Collapse_Collapsed(object sender, RoutedEventArgs e)
        {
            txtFastCmd.Text = String.Empty;
            lbOutput.Items.Clear();
            this.Width = 500;
        }

        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            lbOutput.Items.Clear();
            if(txtFastCmd.Text!=String.Empty)
            {
                if (ParseCommand(txtFastCmd.Text))
                    this.Width = 855;
                else
                    MessageBox.Show("Wrong input command!", "Not found");
            }
        }

        //Same as btnProceed_Click, just it handles the Enter key when pressed inside the textBox.
        private void txtFastCmd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                lbOutput.Items.Clear();
                if (txtFastCmd.Text != String.Empty)
                {
                    if (ParseCommand(txtFastCmd.Text))
                        this.Width = 855;
                    else
                        MessageBox.Show("Wrong input command!", "Not found");
                }
            }
        }

        private bool ParseCommand(string cmd)
        {
            if (cmd=="abort")
            {
                if (ProcessManagement.StopProcess())
                {
                    isRunning = false;
                    return true;
                }
                return false;
            }
            else
            {
                if (commands == null)
                {
                    commands = LoadCmds.load("/commands/cmd");
                }
                string[] cmdParameters = cmd.Split('-');
                int cmdNr = -1;
                for (int i = 0; i < commands.Count; i++)
                {
                    if (cmdParameters[0] == commands[i])
                    {
                        cmdNr = i;
                        break;
                    }
                }

                if(!isRunning)
                switch (cmdNr)
                {
                    case 0:
                        {
                            if (!ProcessManagement.LaunchCommandLineApp(DEFRAG_DISK_CMD, cmdParameters[1]))
                                return false;
                            else isRunning = true;
                        }
                        break;
                    case 1:
                        {
                            string arg = CHECK_DISK_PARAMS + cmdParameters[1] + CHECK_DISK_ARG;
                            if (!ProcessManagement.LaunchCommandLineApp(CHECK_DISK_CMD, arg))
                                return false;
                            else isRunning = true;
                        }
                        break;
                    case 2:
                        if (!ProcessManagement.LaunchCommandLineApp(CLEANER_DISK_CMD, CLEANER_DISK_PARAMS + cmdParameters[1]))
                            return false;
                        break;
                    case 3:
                        if (!ProcessManagement.LaunchCommandLineApp("ssDownload.bat", "", arguments2: "1"))
                            return false;
                        break;
                    case 4:
                        if (SendMail(cmdParameters[1], cmdParameters[2], cmdParameters[3]))
                        {
                            lbOutput.Items.Add("Mail Send to: " + cmdParameters[1]);
                            lbOutput.Items.Add("Subject: " + cmdParameters[2]);
                            lbOutput.Items.Add("Body: " + cmdParameters[3]);
                        }
                        else
                            return false;
                        break;
                    case 5:
                         int seconds=0;
                         if (int.TryParse(cmdParameters[1], out seconds))
                         {
                             shutdown(cmdParameters[1]);
                            return true;
                         }
                        return false;
                    case 6:
                        this.WindowState = WindowState.Minimized;
                        break;
                    case 7:
                        string[] about = LoadCmds.load("/commands/about")[0].Split(';');
                        foreach (string s in about)
                            lbOutput.Items.Add(s);
                        break;
                    case 8:
                        string[] help = LoadCmds.load("/commands/help")[0].Split(';');
                        foreach (string s in help)
                            lbOutput.Items.Add(s);
                        break;
                    case 9:
                        this.Close();
                        break;
                    default:
                        return false;

                }

                return true;
            }
        }

        private void shutdown(string seconds)
        {
            ProcessManagement.LaunchCommandLineApp("cmd.exe", "/c shutdown -s -t " + seconds);
            lbOutput.Items.Add("Shutdown will occur in: " + seconds + " seconds.");
            btnAbortShutdown.Visibility = Visibility.Visible;
        }

        private void btnAbortShutdown_Click(object sender, RoutedEventArgs e)
        {
            ProcessManagement.LaunchCommandLineApp("cmd.exe", "/c shutdown -a");
            lbOutput.Items.Add("Shutdown aborted!");
            btnAbortShutdown.Visibility = Visibility.Hidden;
        }

        private bool SendMail(string emailTo, string subject, string body)
        {
            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;

            string emailFrom = ""; //set your mail address
            string password = ""; //password for the mail address

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    // Can set to false, if you are sending pure text.

                    SmtpClient smtp = new SmtpClient(smtpAddress, portNumber);
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }  
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
