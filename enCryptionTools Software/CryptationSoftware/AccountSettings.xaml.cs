using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace CryptationSoftware
{
    /// <summary>
    /// Interaction logic for AccountSettings.xaml
    /// </summary>
    public partial class AccountSettings : Window
    {
        public AccountSettings()
        {
            InitializeComponent();
            LoadSettings();
            lblStatus.Visibility = Visibility.Hidden;
        }

        private void LoadSettings()
        {
            if(File.Exists("Utils/AccountSettings.xml"))
            {
                XDocument doc = XDocument.Load("Utils/AccountSettings.xml");

                txtAccount.Text = doc.Root.Element("account").Value;
                txtPassword.Text = Algorithms.Decrypt(doc.Root.Element("password").Value, "!@#$%", "^&*()");
                txtAddress.Text = doc.Root.Element("address").Value;
                txtPort.Text = doc.Root.Element("port").Value;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if ((txtAccount.Text != String.Empty) && (txtPassword.Text != String.Empty) && (txtAddress.Text != String.Empty) && (txtPort.Text != String.Empty))
            {
                try
                {
                    XDocument doc = new XDocument(new XElement("credentials",
                                                   new XElement("account", txtAccount.Text),
                                                   new XElement("password", Algorithms.Encrypt(txtPassword.Text, "!@#$%", "^&*()")),
                                                   new XElement("address", txtAddress.Text),
                                                   new XElement("port", txtPort.Text)));

                    doc.Save("Utils/AccountSettings.xml");

                    lblStatus.Visibility = Visibility.Visible;
                }
                catch
                {
                    lblStatus.Foreground = Brushes.Red;
                    lblStatus.Content = "Save Failed";
                    lblStatus.Visibility = Visibility.Visible;
                }
            }
            else
                MessageBox.Show("Please fill all fields!", "Fields empty");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
