using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Feedback360Scheduler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //SchedulerApp SchedulerApp = new Feedback360Scheduler.SchedulerApp();
            //SchedulerApp.SendReminder1Email();
            //SchedulerApp.SendReminder2Email();
            //SchedulerApp.SendReminder3Email();
            //SchedulerApp.SendParticipantReminder1();
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SchedulerApp());
        }
    }
}
