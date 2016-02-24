using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace ControlCenter.Applications.SecureDownload
{
    /// <summary>
    /// Interaction logic for Downloader.xaml
    /// </summary>
    public partial class Downloader : Window
    {
        public delegate void DwnldWindowClosing();
        public static event DwnldWindowClosing DwnldClosed;

        private WebClient wc;
        private WebClient wcSecure;
        private bool isProcessFinished = true;

        public Downloader()
        {
            InitializeComponent();
            progressDownload.IsEnabled = false;
            btnCancel.IsEnabled = false;

            progressDownloadSecure.IsEnabled = false;
            btnCancelSecure.IsEnabled = false;

            ProcessManagement.AddLine += new ProcessManagement.NewMessage(GetResult);
            ProcessManagement.IsFinished += () => { isProcessFinished = true; };
        }

        private void DownloaderWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProcessManagement.AddLine -= new ProcessManagement.NewMessage(GetResult);
            ProcessManagement.IsFinished -= () => { isProcessFinished = true; };
            DwnldClosed();
        }

        #region Download     
        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtURL.Text != String.Empty)
                {
                    Uri url = new Uri(ParseURL(txtURL.Text));
                    string fileName = System.IO.Path.GetFileName(url.AbsolutePath);
                    string dwnldPath = SelectDownloadLocation();
                    using (wc = new WebClient())
                    {
                        wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                        if (dwnldPath != String.Empty)
                        {
                            wc.DownloadFileAsync(url, dwnldPath + fileName);
                            btnDownload.IsEnabled = false;
                            lblProgress.Content = "";
                            progressDownload.IsEnabled = true;
                            btnCancel.IsEnabled = true;
                            txtURL.IsEnabled = false;
                            lblProgress.Content = "";
                        }
                    }
                }
                else
                    MessageBox.Show("Please provide a valid URL!", "Invalid URL", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch
            { }
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressDownload.Value = e.ProgressPercentage;
            if(progressDownload.Value==100)
            {
                lblProgress.Content = "Completed";
                txtURL.IsEnabled = true;
                progressDownload.Value = 0;
                progressDownload.IsEnabled = false;
                btnCancel.IsEnabled = false;
                btnDownload.IsEnabled = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wc.CancelAsync();
                wc.Dispose();
            }
            catch
            {

            }
            txtURL.IsEnabled = true;
            progressDownload.Value = 0;
            progressDownload.IsEnabled = false;
            lblProgress.Content = "Cancelled";
            btnCancel.IsEnabled = false;
        }
        #endregion

        #region SecureDownload
        private string fileNameSecure = String.Empty;
        private string dwnldPathSecure = String.Empty;

        private void btnDownloadSecure_Click(object sender, RoutedEventArgs e)
        {
            if (isProcessFinished)
            {
                try
                {
                    if (txtURLSecure.Text != String.Empty)
                    {
                        Uri url = new Uri(ParseURL(txtURLSecure.Text));
                        fileNameSecure = System.IO.Path.GetFileName(url.AbsolutePath);
                        dwnldPathSecure = SelectDownloadLocation();
                        using (wcSecure = new WebClient())
                        {
                            wcSecure.DownloadProgressChanged += wcSecure_DownloadProgressChanged;
                            if (dwnldPathSecure != String.Empty)
                            {
                                wcSecure.DownloadFileAsync(url, dwnldPathSecure + fileNameSecure);
                                btnDownloadSecure.IsEnabled = false;
                                txtProgressSecure.Text = "";
                                progressDownloadSecure.IsEnabled = true;
                                btnCancelSecure.IsEnabled = true;
                                txtURLSecure.IsEnabled = false;
                                txtProgressSecure.Text = "";
                            }
                        }
                    }
                    else
                        MessageBox.Show("Please provide a valid URL!", "Invalid URL", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                catch
                { }
            }
            else
                MessageBox.Show("Please wait for the scan to finish!", "Interrupting process", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void wcSecure_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressDownloadSecure.Value = e.ProgressPercentage;
            if (progressDownloadSecure.Value == 100)
            {
                string path = dwnldPathSecure + fileNameSecure;
                txtProgressSecure.Text = "Scanning File";
                ProcessManagement.LaunchCommandLineApp("Utils/ssDownload.bat", path, arguments2: "3");
                isProcessFinished = false;
                txtURLSecure.IsEnabled = true;
                progressDownloadSecure.Value = 0;
                progressDownloadSecure.IsEnabled = false;
                btnCancelSecure.IsEnabled = false;
                btnDownloadSecure.IsEnabled = true;
            }
        }

        private void GetResult(string message)
        {
            Action action = () =>
            {
                txtProgressSecure.Text = message;
            };
            Dispatcher.BeginInvoke(action);
        }

        private void btnCancelSecure_Click(object sender, RoutedEventArgs e)
        {
            {
                try
                {
                    wcSecure.CancelAsync();
                    wcSecure.Dispose();
                    if (progressDownloadSecure.Value == 100)
                    {
                        ProcessManagement.StopProcess();
                    }
                }
                catch
                {

                }
                txtURLSecure.IsEnabled = true;
                progressDownloadSecure.Value = 0;
                progressDownloadSecure.IsEnabled = false;
                txtProgressSecure.Text = "Cancelled";
                btnCancelSecure.IsEnabled = false;
            }
        }
        #endregion

        private string ParseURL(string url)
        {
            if (url.StartsWith("http://"))
                return url;
            else
                return "http://" + url;
        }

        private string SelectDownloadLocation()
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
    }
}
