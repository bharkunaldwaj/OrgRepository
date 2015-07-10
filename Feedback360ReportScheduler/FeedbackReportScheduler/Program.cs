using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FeedbackReportScheduler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            int? programID = null;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
                programID = (int?)int.Parse(args[0]);
            else
                programID = null;

            Application.Run(new frmMain(programID));
        }
    }
}
