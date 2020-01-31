using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeamsPlusPlus
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

            var userId = Prompt.ShowDialog("Login", "Enter username: ", LoginModel.Instance.MyUserId);
            LoginModel.Instance.LoginAs(userId);

            var mainForm = new MainForm();
            mainForm.FormClosed += (dummy1, dummy2) => Application.Exit();
            ChatRoomsModel.Instance.SubscribeOnDataChanged(() =>
            {
                if (!mainForm.IsDisposed) mainForm.Invoke((MethodInvoker)(async () =>
                {
                    if (!mainForm.IsDisposed) await mainForm.RedrawAsync();
                }));
            });

            Application.Run(mainForm);
        }
        
    }
}
