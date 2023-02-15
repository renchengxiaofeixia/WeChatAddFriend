using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatAddFriend
{
    public static class ControlExtensions
    {
        public static void InvokeOnUiThreadIfRequired(this Control control, Action action)
        {
            if (control == null)
            {
                return;
            }
            if (control.Disposing || control.IsDisposed || !control.IsHandleCreated)
            {
                return;
            }
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
                return;
            }
            action();
        }
    }
}
