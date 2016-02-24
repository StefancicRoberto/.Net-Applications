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

namespace ControlCenter.Applications.Defragmenter
{
    /// <summary>
    /// Interaction logic for Defragmenter.xaml
    /// </summary>
    public partial class Defragmenter : Window
    {
        public delegate void DefragWindowClosing();
        public static event DefragWindowClosing DefragClosed;

        private const string DEFRAG_DISK_CMD = "defrag.exe";
       // private const string CHECK_DISK_PARAMS = "/c echo y|chkdsk.exe ";
        private const string DEFRAG_DISK_ARG = " /A /U /V";

        public Defragmenter()
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
        private void DefragmenterWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProcessManagement.OverwriteLine -= new ProcessManagement.OverwriteMessage(OverwriteListBox);
            ProcessManagement.AddLine -= new ProcessManagement.NewMessage(AddLineListBox);
            DefragClosed();
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
            if (lbDrives.SelectedIndex != -1)
            {
                ClearList();
                string drive_letter = lbDrives.SelectedItem.ToString().Replace(@"\", "");
                if (!ProcessManagement.LaunchCommandLineApp(DEFRAG_DISK_CMD, drive_letter))
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

        private void btnAnalyze_Click(object sender, RoutedEventArgs e)
        {
            ClearList();
            string drive_letter = lbDrives.SelectedItem.ToString().Replace(@"\", "");
            if (!ProcessManagement.LaunchCommandLineApp(DEFRAG_DISK_CMD, drive_letter + DEFRAG_DISK_ARG))
                System.Windows.MessageBox.Show("Couldn't start the process!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            btnAbort.IsEnabled = true;
        }
    }
}

