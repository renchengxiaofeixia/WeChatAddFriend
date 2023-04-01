using AdvancedSharpAdbClient;
using System.IO;
using System.Threading;
using WeChatAddFriend.Tools;
using static System.Net.WebRequestMethods;

namespace WeChatAddFriend
{
    public partial class WeChatAddFriendForm : Form
    {
        public static AdbClient adbClient;
        public static AdbServer srv;
        public static readonly string ADBPath = "platform-tools\\adb.exe";
        public static int addFriendTotalCount = 0;

        public List<string> importPhoneNos { get; set; }

        public WeChatAddFriendForm()
        {
            InitializeComponent();

            Load += WeChatAddFriendForm_Load;
        }

        private void WeChatAddFriendForm_Load(object? sender, EventArgs e)
        {
            srv = new AdbServer();
            if (!srv.GetStatus().IsRunning)
            {
                srv.StartServer(ADBPath, true);
            }
            adbClient = new AdbClient();


            var t = new Thread(() =>
            {
                while (true)
                {
                    if (adbClient != null)
                    {
                        this.InvokeOnUiThreadIfRequired(() =>
                        {
                            lblDeviceNum.Text = $"在线数:（{adbClient.GetDevices().Count()}）";
                        });
                    }
                    Thread.Sleep(2000);
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            addFriendTotalCount = 0;

            var devices = adbClient.GetDevices();
            var avgPhoneNoCnt = importPhoneNos.Count() / adbClient.GetDevices().Count();

            var idx = 0;
            var tasks = new List<Task>();
            while (idx < devices.Count)
            {
                var task = AddFriendTask(devices[idx], importPhoneNos.Skip(idx * avgPhoneNoCnt).Take(avgPhoneNoCnt).ToList());
                tasks.Add(task);
                idx++;
            }

            await Task.WhenAll(tasks);
        }


        public async Task AddFriendTask(DeviceData d, List<string> phoneNos)
        {
            var idx = 0;
            IShellOutputReceiver rcvr = null;
            //启动微信
            adbClient.ExecuteRemoteCommand("am start com.tencent.mm/com.tencent.mm.ui.LauncherUI", d, rcvr);

            while (idx < phoneNos.Count)
            {
                var add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/hy6']", TimeSpan.FromSeconds(2));

                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }

                add = adbClient.FindElement(d, "//node[@text='添加朋友']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }

                // 点击添加朋友搜索框
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/jcd']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(2000);
                }

            ADD_NEXT_PHONE:
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/cd7']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                    adbClient.ClearInput(d, 15);
                }

                if (idx >= phoneNos.Count) break;

                //设置文本手机号
                adbClient.SendText(d, phoneNos[idx]);
                await Task.Delay(3000);

                //点击搜索
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/kms']", TimeSpan.FromSeconds(2));

                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(2000);
                }

                var userNotExist = adbClient.FindElement(d, "//node[@text='该用户不存在']", TimeSpan.FromSeconds(2));
                if (userNotExist != null)
                {
                    PrintLog($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}--{d.Name} add phone ---{phoneNos[idx]}--跳过用户不存在");
                    idx++;
                    goto ADD_NEXT_PHONE;
                }

                //已经是好友的 按钮 text 为发消息
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/khj' and @text='发消息']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    await Task.Delay(2000);
                    adbClient.BackBtn(d);
                    PrintLog($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}--{d.Name} add phone ---{phoneNos[idx]}--跳过用户已经是好友");
                    idx++;
                    goto ADD_NEXT_PHONE;

                }
                //添加好友到通讯录
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/khj' and @text='添加到通讯录']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(2000);
                }

                //发送添加好友按钮
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/e9q']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }

                //再次点击发送加好友请求
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/e9q']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }
                //返回
                adbClient.BackBtn(d);

                int.TryParse(txtSecd.Text.Trim(), out var secd);
                await Task.Delay(secd * 1000);

                adbClient.BackBtn(d);

                addFriendTotalCount++;

                PrintLog($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}--{d.Name} add phone ---{phoneNos[idx]}");
                
                idx++;
            }
        }

        private void btnImportData_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "文本文件(*.txt)|*.txt";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                importPhoneNos = System.IO.File.ReadAllLines(dlg.FileName).ToList();
                btnImportData.Text = $"导入数据({importPhoneNos.Count()}条)";
            }
        }

        void PrintLog(string logtxt)
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                txtLog.Text += "\n" + logtxt;
            });
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }
    }

}