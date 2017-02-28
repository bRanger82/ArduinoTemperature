using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino_Temperature_Retrofit
{
    static class Program
    {
        private static string appGuid = "c0a76b5a-12ab-45c5-b9d9-d693faa6e7b9";

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("Programm bereits aktiv!", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                Application.Run(new frmMain());
            }
            
        }
    }
}
