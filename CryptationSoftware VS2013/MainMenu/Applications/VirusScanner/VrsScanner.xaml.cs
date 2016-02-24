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
using Microsoft.Win32.TaskScheduler;

namespace ControlCenter.Applications.VirusScanner
{
    /// <summary>
    /// Interaction logic for VrsScanner.xaml
    /// </summary>
    public partial class VrsScanner : Window
    {
        public delegate void VrsWindowClosing();
        public static event VrsWindowClosing VrsScanClosed;

        private string pathScanFile = String.Empty;

        public VrsScanner()
        {
            InitializeComponent();

            ProcessManagement.OverwriteLine += new ProcessManagement.OverwriteMessage(OverwriteStatusBox);
            ProcessManagement.AddLine += new ProcessManagement.NewMessage(AddLineStatusBox);
            ProcessManagement.IsFinished += () => { ButtonsState(true); };
        }

        private void VirusScannerWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProcessManagement.OverwriteLine -= new ProcessManagement.OverwriteMessage(OverwriteStatusBox);
            ProcessManagement.AddLine -= new ProcessManagement.NewMessage(AddLineStatusBox);
            ProcessManagement.IsFinished -= () => { ButtonsState(true); };
            VrsScanClosed();
        }

        #region Scheduele
        private void cbMonth_Loaded(object sender, RoutedEventArgs e)
        {
            cbMonth.Items.Add("01");
            cbMonth.Items.Add("02");
            cbMonth.Items.Add("03");
            cbMonth.Items.Add("04");
            cbMonth.Items.Add("05");
            cbMonth.Items.Add("06");
            cbMonth.Items.Add("07");
            cbMonth.Items.Add("08");
            cbMonth.Items.Add("09");
            cbMonth.Items.Add("10");
            cbMonth.Items.Add("11");
            cbMonth.Items.Add("12");
        }

        private void cbDay_Loaded(object sender, RoutedEventArgs e)
        {
            cbDay.Items.Add("01");
            cbDay.Items.Add("02");
            cbDay.Items.Add("03");
            cbDay.Items.Add("04");
            cbDay.Items.Add("05");
            cbDay.Items.Add("06");
            cbDay.Items.Add("07");
            cbDay.Items.Add("08");
            cbDay.Items.Add("09");
            cbDay.Items.Add("10");
            cbDay.Items.Add("11");
            cbDay.Items.Add("12");
            cbDay.Items.Add("13");
            cbDay.Items.Add("14");
            cbDay.Items.Add("15");
            cbDay.Items.Add("16");
            cbDay.Items.Add("17");
            cbDay.Items.Add("18");
            cbDay.Items.Add("19");
            cbDay.Items.Add("20");
            cbDay.Items.Add("21");
            cbDay.Items.Add("22");
            cbDay.Items.Add("23");
            cbDay.Items.Add("24");
            cbDay.Items.Add("25");
            cbDay.Items.Add("26");
            cbDay.Items.Add("27");
            cbDay.Items.Add("28");
            cbDay.Items.Add("29");
            cbDay.Items.Add("30");
        }

        private void cbHour_Loaded(object sender, RoutedEventArgs e)
        {
            cbHour.Items.Add("00:00");
            cbHour.Items.Add("01:00");
            cbHour.Items.Add("02:00");
            cbHour.Items.Add("03:00");
            cbHour.Items.Add("04:00");
            cbHour.Items.Add("05:00");
            cbHour.Items.Add("06:00");
            cbHour.Items.Add("07:00");
            cbHour.Items.Add("08:00");
            cbHour.Items.Add("09:00");
            cbHour.Items.Add("10:00");
            cbHour.Items.Add("11:00");
            cbHour.Items.Add("12:00");
            cbHour.Items.Add("13:00");
            cbHour.Items.Add("14:00");
            cbHour.Items.Add("15:00");
            cbHour.Items.Add("16:00");
            cbHour.Items.Add("17:00");
            cbHour.Items.Add("18:00");
            cbHour.Items.Add("19:00");
            cbHour.Items.Add("20:00");
            cbHour.Items.Add("21:00");
            cbHour.Items.Add("22:00");
            cbHour.Items.Add("23:00");
        }
        #endregion

        private void OverwriteStatusBox(string sText)
        {
            System.Action action = () =>
            {
                if (lbStatus.Items.Count > 0) lbStatus.Items.RemoveAt(lbStatus.Items.Count - 1);
                if (!String.IsNullOrEmpty(sText))
                {
                    lbStatus.Items.Add(sText);
                    lbStatus.SelectedIndex = lbStatus.Items.Count - 1;
                }
                lbStatus.Items.Add(sText);
            };
            lbStatus.Dispatcher.BeginInvoke(action);
        }

        private void AddLineStatusBox(string sText)
        {
            System.Action action = () =>
            {
                if (!String.IsNullOrEmpty(sText))
                {
                    lbStatus.Items.Add(sText);
                    lbStatus.SelectedIndex = lbStatus.Items.Count - 1;
                }
            };
            lbStatus.Dispatcher.BeginInvoke(action);
        }

        private void ButtonsState(bool state)
        {
            System.Action action = () =>
            {
                btnStartScan.IsEnabled = state;
                btnQuickScan.IsEnabled = state;
                btnFullScan.IsEnabled = state;
            };
            Dispatcher.BeginInvoke(action);
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                pathScanFile = dlg.FileName;
            }
        }

        private void btnStartScan_Click(object sender, RoutedEventArgs e)
        {
            if (pathScanFile != String.Empty)
            {
                if (File.Exists(pathScanFile))
                {
                    ProcessManagement.LaunchCommandLineApp("Utils/ssDownload.bat", pathScanFile, arguments2: "3");
                    ButtonsState(false);
                }
                else
                    MessageBox.Show("The selected file doesn't exist!", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("No file has been selected to be scanned.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void btnQuickScan_Click(object sender, RoutedEventArgs e)
        {
            ProcessManagement.LaunchCommandLineApp("Utils/ssDownload.bat", pathScanFile, arguments2: "1");
            ButtonsState(false);
        }

        private void btnFullScan_Click(object sender, RoutedEventArgs e)
        {
            ProcessManagement.LaunchCommandLineApp("Utils/ssDownload.bat", pathScanFile, arguments2: "2");
            ButtonsState(false);
        }

        private void btnSchedueleScan_Click(object sender, RoutedEventArgs e)
        {
            if ((cbMonth.SelectedIndex != -1) || (cbDay.SelectedIndex != -1) || (cbHour.SelectedIndex != -1))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("2015");
                sb.Append(".");
                sb.Append(cbMonth.SelectedValue);
                sb.Append(".");
                sb.Append(cbDay.SelectedValue);
                sb.Append(" ");
                sb.Append(cbHour.SelectedValue);

                DateTime dt = new DateTime();
                dt = DateTime.Parse(sb.ToString());

                SchedueleTask(dt);
            }
            else
                MessageBox.Show("Please select a date to begin scan.", "Scheduele", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        
        private void SchedueleTask(DateTime date)
        {
            using(TaskService ts=new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "WinDef System Scan";
                td.Triggers.Add(new TimeTrigger() {StartBoundary=date, Enabled=false});
                td.Actions.Add(new ExecAction("cmd.exe", "C:\\Program Files\\Windows Defender\\MpCmdRun.exe -Scan -ScanType 2", null));
                ts.RootFolder.RegisterTaskDefinition(@"WDscan", td);
                ts.RootFolder.DeleteTask("WDscan");
            }
        }

    }
}
