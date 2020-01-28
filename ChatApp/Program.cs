using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new TestForm());
            //return;

            var mainForm = new MainForm();
            mainForm.FormClosed += (dummy1, dummy2) => Application.Exit();
            ChatRoomsModel.Instance.SubscribeOnMessage(newMessage =>
            {
                if (!mainForm.IsDisposed) mainForm.Invoke((MethodInvoker)(async () => await mainForm.RedrawAsync()));
            });

            Application.Run(mainForm);
            
        }
    }
}
