using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace ControlCenter
{
    public static class ProcessManagement
    {
        
        private static System.Diagnostics.Process process1;

        public delegate void OverwriteMessage(string message);
        public static event OverwriteMessage OverwriteLine;

        public delegate void NewMessage(string message);
        public static event NewMessage AddLine;

        public delegate void ProcessFinished();
        public static event ProcessFinished IsFinished;

        public static bool LaunchCommandLineApp(string command, string arguments, string arguments2="")
        {

            try
            {
                process1 = new Process();
                process1.StartInfo.CreateNoWindow = true;
                process1.StartInfo.UseShellExecute = false;
                process1.StartInfo.RedirectStandardOutput = true;
                process1.StartInfo.FileName = command;
                if (arguments2 != "")
                    process1.StartInfo.Arguments = string.Format("{0} {1}", arguments2, arguments);
                else
                    process1.StartInfo.Arguments = arguments;

                process1.Start();

                ThreadPool.QueueUserWorkItem(delegate { ReadCmdOutput(process1); });

                return true;
            }
            catch (Exception ex)
            {
                return false;
                // Log error.
            }
        }

        public static bool StopProcess()
        {
            try
            {
                process1.Close();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private static void ReadCmdOutput(Process p)
        {
            char[] buff = new char[256];
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            byte rec_r = 0;
            byte rec_n = 0;
            bool overwriteCurrentrtLine = false;
            bool leaveprocess = false;

            try
            {
                do
                {
                    int char_received = p.StandardOutput.Read(buff, 0, buff.Length);
                    if (char_received > 0)
                    {


                        int i;
                        for (i = 0; i < char_received; i++)
                        {
                            if (buff[i] == '\r')
                            {
                                rec_r = 1;
                            }
                            else if (buff[i] == '\n')
                            {
                                rec_n = 1;
                            }
                            else
                            {
                                if (rec_n != 0)
                                {
                                    if (overwriteCurrentrtLine)
                                        OverwriteLine(sb.ToString());
                                    else
                                        AddLine(sb.ToString());

                                    overwriteCurrentrtLine = false;
                                    sb.Remove(0, sb.Length);
                                    sb.Append(buff[i]);
                                }
                                else if (rec_r != 0)
                                {
                                    if (overwriteCurrentrtLine)
                                        OverwriteLine(sb.ToString());
                                    else
                                        AddLine(sb.ToString());

                                    sb.Remove(0, sb.Length);
                                    sb.Append(buff[i]);
                                    overwriteCurrentrtLine = true;
                                }
                                else
                                {
                                    sb.Append(buff[i]);
                                }

                                rec_r = 0;
                                rec_n = 0;
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }

                    if ((p.HasExited) && (sb.Length == 0))
                    {
                        leaveprocess = true;
                        IsFinished();
                    }
                    else if((p.HasExited)&&(sb.Length>0))
                    {
                        sb.Remove(0, sb.Length);
                    }

                } while (!leaveprocess);
            }
            catch(Exception ex)
            {

            }
        }
    }
}