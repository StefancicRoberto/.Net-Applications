using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Xml.Linq;

namespace EncryptionSoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void CryptWindowClosing();
        public static event CryptWindowClosing CryptClosed;
        int encryptType = 1;

        public MainWindow()
        {
            InitializeComponent();
            txtMail.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CryptClosed();
        }

        private void txtInput_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (txtInput.Text == "Enter the text for conversion...")
            {
                txtInput.Text = String.Empty;
            }
        }

        private void txtMail_GotMouseCapture(object sender, MouseEventArgs e)
        {
            txtMail.Text = String.Empty;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (chkMail.IsChecked == true)
            {
                txtMail.IsEnabled = true;
                btnConvertSend.Content = "Send";
            }
            else
            {
                txtMail.IsEnabled = false;
                btnConvertSend.Content = "Convert";
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                ParseTextFromFile(dlg.FileName);
            }
        }

        private void ParseTextFromFile(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    txtInput.Text = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error at reading file!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnConvertSend_Click(object sender, RoutedEventArgs e)
        {
            ProcessCommand();
        }

        private void ProcessCommand()
        {
            if (btnConvertSend.Content.ToString().Contains("Send"))
            {
                if (txtMail.Text != String.Empty)
                    SendMail();
                else
                    MessageBox.Show("Please complete the e-mail filed.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (txtKeys.Text != String.Empty)
                {
                    string[] keys = txtKeys.Text.Split(' ');
                    switch (encryptType)
                    {
                        case 1:
                            if (keys.Length >= 2)
                                txtOutput.Text = Algorithms.Encrypt(txtInput.Text, keys[0], keys[1]);
                            else
                                txtOutput.Text = Algorithms.Encrypt(txtInput.Text, keys[0], "");
                            break;
                        case 2:
                            if (keys.Length >= 2)
                                txtOutput.Text = Algorithms.Decrypt(txtInput.Text, keys[0], keys[1]);
                            else
                                txtOutput.Text = Algorithms.Decrypt(txtInput.Text, keys[0], "");
                            break;
                        default: break;

                    }
                }
                else MessageBox.Show("Please provide a valid key!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SendMail()
        {
           
            if (File.Exists("Utils/AccountSettings.xml"))
            {
                SmtpClient mail = new SmtpClient();
                mail.EnableSsl = true;

                XDocument doc = XDocument.Load("Utils/AccountSettings.xml");
                mail.Credentials = new System.Net.NetworkCredential(doc.Root.Element("account").Value, Algorithms.Decrypt(doc.Root.Element("password").Value, "!@#$%", "^&*()"));
                mail.Host = doc.Root.Element("address").Value;
                mail.Port = Int32.Parse(doc.Root.Element("port").Value);


                using (MailMessage msg = new MailMessage(doc.Root.Element("account").Value, txtMail.Text))
                {
                    msg.Subject = "To " + txtMail.Text;
                    msg.Body = txtOutput.Text;

                    mail.Send(msg);
                }
            }
            else 
                if(MessageBox.Show("Credentials not found!\n Would you like to complete your credentials now?", "Credentials Error", MessageBoxButton.YesNo, MessageBoxImage.Error)==MessageBoxResult.Yes)
                {
                    AccountSettings settings = new AccountSettings();
                    settings.ShowDialog();
                }
        }


        #region Menu Items

        private void miNew_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("This operation will erase all your settings.\nAre you sure you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                txtInput.Clear();
                txtOutput.Clear();
                txtKeys.Clear();
                txtMail.Clear();

                if (File.Exists("AccountSettings.xml"))
                    try
                    {
                        File.Delete("AccountSettings.xml");
                    }
                    catch
                    {
                    }
            }

        }

        private void miSaveAs_Click(object sender, RoutedEventArgs e)
        {
            string path = SelectPath();
            if(path!=String.Empty)
            {
                byte[] buffer = new byte[txtOutput.Text.Length * sizeof(char)];
                System.Buffer.BlockCopy(txtOutput.Text.ToCharArray(), 0, buffer, 0, buffer.Length);

                using(FileStream fs= new FileStream(path+@"\Output.txt", FileMode.Create))
                {
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
        }

        private string SelectPath()
        {
            using (System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    return dlg.SelectedPath;
                }
            }
            return String.Empty;
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void miEncrypt_Click(object sender, RoutedEventArgs e)
        {
            encryptType = 1;
            MessageBox.Show("Encrypt Mode", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void miDecrypt_Click(object sender, RoutedEventArgs e)
        {
            encryptType = 2;
            MessageBox.Show("Decrypt Mode", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void settMailAcc_Click(object sender, RoutedEventArgs e)
        {
            AccountSettings settings = new AccountSettings();
            settings.ShowDialog();
        }

        private void miZoomIn_Click(object sender, RoutedEventArgs e)
        {
            txtInput.FontSize += (txtInput.FontSize * 10) / 100;
            txtOutput.FontSize += (txtOutput.FontSize * 10) / 100;
        }

        private void miZoomOut_Click(object sender, RoutedEventArgs e)
        {
            txtInput.FontSize -= (txtInput.FontSize * 10) / 100;
            txtOutput.FontSize -= (txtOutput.FontSize * 10) / 100;
        }

        private void miHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }
}
