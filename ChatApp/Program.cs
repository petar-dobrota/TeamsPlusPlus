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
                try {
                    mainForm.Invoke((MethodInvoker)(async () =>
                    {
                        try
                        {
                            await mainForm.RedrawAsync();
                        }
                        catch (ObjectDisposedException)
                        {
                            // app is shuting down, this is ok
                        }
                    }));
                }
                catch (ObjectDisposedException)
                {
                    // app is shuting down, this is ok
                }
            });

            Application.Run(mainForm);
        }
        
    }
}
