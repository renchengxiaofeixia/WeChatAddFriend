using System.Diagnostics;
using System.Text.Json.Serialization;
using WeChatAddFriend.Version;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace WeChatAddFriend
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ApplicationExit += Application_ApplicationExit;
            ApplicationConfiguration.Initialize();
            try
            {
                bool createdNew;
                using (new Mutex(true, "WeChatAddFriend", out createdNew))
                {
                    if (createdNew)
                    {
                        KillProcess();
                        Log.Initiate("运行日志.txt");
                        Application.Run(LoginForm.Inst);
                    }
                    else
                    {
                        MessageBox.Show("软件已经运行了！","提示");
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("WeChatAddFriend:" + ex.Message, "提示");
                Log.Exception(ex);
            }
        }

        private static void KillProcess()
        {
            var processes = Process.GetProcessesByName("WeChatAddFriend");
            var curProcess = Process.GetCurrentProcess();
            foreach (var p in processes)
            {
                if (p.Id != curProcess.Id)
                {
                    p.Kill();
                }
            }
        }


        private static void Application_ApplicationExit(object? sender, EventArgs e)
        {
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.Exception(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //Log.Info(e.ExceptionObject as Exception);
        }
    }
}