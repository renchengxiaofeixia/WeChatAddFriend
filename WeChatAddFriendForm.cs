using AdvancedSharpAdbClient;
using System.Threading;

namespace WeChatAddFriend
{
    public partial class WeChatAddFriendForm : Form
    {
        public static AdbClient adbClient;
        public static AdbServer srv;
        public static readonly string ADBPath = "tools\\platform-tools\\adb.exe";

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


            var t = new Thread(() => {
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
            var devices = adbClient.GetDevices();

            var phoneNos = tbPhoneNos.Text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            var avgPhoneNoCnt = phoneNos.Count() / adbClient.GetDevices().Count();

            var idx = 0;
            while (idx < devices.Count)
            {
                CreateAddFriendTask(devices[idx],phoneNos.Skip(idx * avgPhoneNoCnt).Take(avgPhoneNoCnt).ToList());
                idx++;
            }
        }


        public async Task CreateAddFriendTask(DeviceData d,List<string> phoneNos)
        {
            var idx = 0;

            while(idx < phoneNos.Count)
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
                    await Task.Delay(1000);
                }

                USERNOTEXIST:
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/cd7']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    adbClient.ClearInput(d, 15);
                }

                adbClient.SendText(d, phoneNos[idx]);

                await Task.Delay(1000);

                //点击搜索
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/kms']", TimeSpan.FromSeconds(2));
                adbClient.Click(d, add.cords);

                await Task.Delay(1000);

                var userNotExist = adbClient.FindElement(d, "//node[@text='该用户不存在']", TimeSpan.FromSeconds(2));
                if (userNotExist != null)
                {
                    idx++;
                    goto USERNOTEXIST;
                }

                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/khj']", TimeSpan.FromSeconds(2));

                adbClient.Click(d, add.cords);

                await Task.Delay(1000);


                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/e9q']", TimeSpan.FromSeconds(2));

                adbClient.Click(d, add.cords);

                await Task.Delay(1000);

                adbClient.BackBtn(d);

                await Task.Delay(1000);

                adbClient.BackBtn(d);


                idx++;

            }
        }
    }

}