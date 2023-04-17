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
            //ClientUpdater.Update();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ApplicationExit += Application_ApplicationExit;
            ApplicationConfiguration.Initialize();
            Log.Initiate("运行日志.txt");
            Application.Run(LoginForm.Inst);
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