using Microsoft.Win32;
using System;
using System.Collections;
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

namespace ControlCenter.Applications.Cleaner
{
    /// <summary>
    /// Interaction logic for Cleaner.xaml
    /// </summary>
    public partial class Cleaner : Window
    {
        public delegate void CleanerWindowClosing();
        public static event CleanerWindowClosing CleanDskClosed;

        private const string CLEANER_DISK_CMD = "Cleanmgr.exe";
        private const string CLEANER_DISK_PARAMS = "/d ";

        private int crtIndex = -1;

        public Cleaner()
        {
            InitializeComponent();
            GetDrives();
            GetBrowsers();
            GetEnvironmentVariables();
        }

        private void GetDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                lbDrives.Items.Add(drive.ToString());
            }
            int nr = Int32.Parse(lbDrives.Items.Count.ToString());
            if (nr > 4)
                nr = 4;
            lbDrives.Height = lbDrives.Height * nr;
        }

        private void CleanerWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CleanDskClosed();
        }

        private void GetBrowsers()
        {
            try
            {
                RegistryKey browserKeys;
                browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");
                if (browserKeys == null)
                    browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
                string[] browserNames = browserKeys.GetSubKeyNames();

                foreach (string s in browserNames)
                {
                    lbBrowsers.Items.Add(s.Replace(".EXE", ""));
                }

                int nr = Int32.Parse(lbBrowsers.Items.Count.ToString());
                if (nr > 3)
                    nr = 3;
                lbBrowsers.Height = lbBrowsers.Height * nr;
            }
            catch
            {
                lbBrowsers.Items.Add("Error fetching browsers");
                lbBrowsers.Height *= 2;
                lbBrowsers.Width *= 2;
            }
        }

        #region EnvironmentVariables
        private void GetEnvironmentVariables()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            string[] pathArray = environmentVariables["Path"].ToString().Split(';');

            foreach(string path in pathArray)
            {
                lbEnvVariables.Items.Add(path);
            }
        }

        private void btnGet_Click(object sender, RoutedEventArgs e)
        {
            crtIndex = lbEnvVariables.SelectedIndex;
            txtModifyEnv.Text = lbEnvVariables.SelectedValue.ToString();
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            lbEnvVariables.Items[crtIndex] = txtModifyEnv.Text;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            lbEnvVariables.Items.Add(txtModifyEnv.Text);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            lbEnvVariables.Items.RemoveAt(lbEnvVariables.SelectedIndex);
        }

        private void btnSetChanges_Click(object sender, RoutedEventArgs e)
        {
           if(MessageBox.Show("Are you sure you want to set these changes?\nThis action cannot be undone!", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                SetEnvironmentVariables();
            }
        }

        private void SetEnvironmentVariables()
        {
            string path = String.Empty;
            for (int i = 0; i < lbEnvVariables.Items.Count; i++)
            {
                path += lbEnvVariables.Items[i] + ";";
            }
            Environment.SetEnvironmentVariable("Path", path);
        }
        #endregion

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (lbDrives.SelectedIndex != -1)
            {
                string drive_letter = lbDrives.SelectedItem.ToString().Replace(@"\", "");
                if (!ProcessManagement.LaunchCommandLineApp(CLEANER_DISK_CMD, CLEANER_DISK_PARAMS + drive_letter))
                    System.Windows.MessageBox.Show("Couldn't start the process!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (chkDelete.IsChecked == true)
                for (int i = 0; i < lbBrowsers.Items.Count; i++)
                {
                    DeleteBrowsersCache(GetBrowserPath(i));
                }
        }

        private string GetBrowserPath(int index)
        {
            string browser = lbBrowsers.Items[index].ToString();
            browser = browser.ToUpper();
            if (browser.Contains("FIREFOX"))
            {
                browser = "Local\\Mozilla\\Firefox\\Profiles";
            }
            else if (browser.Contains("CHROME"))
            {
                browser = "Local\\Google\\Chrome\\User Data\\Default\\Cache";
            }
            else if (browser.Contains("EXPLORE"))
            {
                browser = "Local\\Microsoft\\Internet Explorer";
            }
            return browser;
        }

        private void DeleteBrowsersCache(string cachePath)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString().Replace("Roaming", "");
                MessageBox.Show(path + cachePath);
                //Directory.Delete(path + cachePath);
            }
            catch
            { }

        }
    }
}
