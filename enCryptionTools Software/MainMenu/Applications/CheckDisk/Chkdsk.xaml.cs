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
using System.Windows.Forms;

namespace ControlCenter.Applications.CheckDisk
{
    /// <summary>
    /// Interaction logic for Chkdsk.xaml
    /// </summary>
    public partial class Chkdsk : Window
    {
        public delegate void ChkWindowClosing();
        public static event ChkWindowClosing ChkDskClosed;

        private const string CHECK_DISK_CMD = "cmd.exe";
        private const string CHECK_DISK_PARAMS = "/c echo y|chkdsk.exe ";
        private const string CHECK_DISK_ARG = " /F /X";

        public Chkdsk()
        {
            InitializeComponent();
            btnAbort.IsEnabled = false;
            ProcessManagement.OverwriteLine += new ProcessManagement.OverwriteMessage(OverwriteListBox);
            ProcessManagement.AddLine += new ProcessManagement.NewMessage(AddLineListBox);
            GetDrives();

        }

        public void GetDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                lbDrives.Items.Add(drive.ToString());
            }
            int nr = Int32.Parse(lbDrives.Items.Count.ToString());
            lbDrives.Height = lbDrives.Height * nr;
        }

        #region Target Events
        private void ChkdskWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProcessManagement.OverwriteLine -= new ProcessManagement.OverwriteMessage(OverwriteListBox);
            ProcessManagement.AddLine -= new ProcessManagement.NewMessage(AddLineListBox);
            ChkDskClosed();
        }

        private void OverwriteListBox(string sText)
        {
            Action action = () =>
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

        private void AddLineListBox(string sText)
        {
            Action action = () =>
            {
                if (!String.IsNullOrEmpty(sText))
                {
                    lbOutput.Items.Add(sText);
                    lbOutput.SelectedIndex = lbOutput.Items.Count - 1;
                }
            };
            lbOutput.Dispatcher.BeginInvoke(action);
        }

        private void ClearList()
        {
            if (CheckAccess())
            {
                Action action = () =>
                {
                    lbOutput.Items.Clear();
                };
                lbOutput.Dispatcher.BeginInvoke(action);
            }
            else
            {
                lbOutput.Items.Clear();
            }
        }
        #endregion

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ClearList();
            if (lbDrives.SelectedIndex != -1)
            {
                string drive_letter = lbDrives.SelectedItem.ToString().Replace(@"\", "");
                string arg = CHECK_DISK_PARAMS + drive_letter + CHECK_DISK_ARG;
                if (!ProcessManagement.LaunchCommandLineApp(CHECK_DISK_CMD, arg))
                    System.Windows.MessageBox.Show("Couldn't start the process!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                btnAbort.IsEnabled = true;
            }

        }

        private void btnAbort_Click(object sender, RoutedEventArgs e)
        {
            if (!ProcessManagement.StopProcess())
                System.Windows.MessageBox.Show("Couldn't abort the process!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                btnAbort.IsEnabled = false;
            }
        }
    }
}
