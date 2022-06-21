using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace JudicialTest
{
    public partial class App : Application 
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr handleWindow, int nCmdShow);
        private const int ShowMaximized = 3;
        //private const int Show = 5;

        protected override void OnStartup(StartupEventArgs e)
        {            
            Process currentProc = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProc.ProcessName);
            if (processes.Length > 1)
            {
                var runningProc = processes.Where(proc => proc.Id != currentProc.Id).FirstOrDefault();
                ShowWindow(runningProc.MainWindowHandle, ShowMaximized);
                Environment.Exit(0);
                return;
            }
            base.OnStartup(e);
        }
    }
}
