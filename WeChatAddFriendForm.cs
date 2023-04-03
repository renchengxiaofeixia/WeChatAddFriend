using AdvancedSharpAdbClient;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO;
using System.Threading;
using WeChatAddFriend.Tools;
using static System.Net.WebRequestMethods;

namespace WeChatAddFriend
{
    public partial class WeChatAddFriendForm : MaterialForm
    {
        public static AdbClient adbClient;
        public static AdbServer srv;
        public static readonly string ADBPath = "platform-tools\\adb.exe";
        public static int addFriendTotalCount = 0;

        public List<string> importPhoneNos { get; set; }

        public WeChatAddFriendForm()
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

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

        private async void btnSaveMoments_Click(object sender, EventArgs e)
        {
            var d = adbClient.GetDevices()[0];

            ////首页
            //var add = adbClient.FindElement(d, "//node[@resource-id='android:id/text1']", TimeSpan.FromSeconds(2));
            //if (add != null)
            //{
            //    //点击要转发朋友圈的好友，进入到聊天界面
            //    add = adbClient.FindElement(d, "//node[@text='One']");
            //    if (add != null)
            //    {
            //        add.Click();
            //        await Task.Delay(1000);
            //    }
            //}

            //////右上角的三个点，进入聊天记录界面
            ////add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/eo']", TimeSpan.FromSeconds(2));
            ////if (add != null)
            ////{
            ////    adbClient.Click(d, add.cords);
            ////    await Task.Delay(1000);
            ////}

            //////点击好友头像
            ////add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/iw8']", TimeSpan.FromSeconds(2));
            ////if (add != null)
            ////{
            ////    adbClient.Click(d, add.cords);
            ////    await Task.Delay(1000);
            ////}

            //////点击朋友圈
            ////add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/o8' and @text='朋友圈']", TimeSpan.FromSeconds(2));
            ////if (add != null)
            ////{
            ////    adbClient.Click(d, add.cords);
            ////    await Task.Delay(1000);
            ////}

            //////点击进入朋友圈详情
            ////add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/br8']", TimeSpan.FromSeconds(2));
            ////if (add != null)
            ////{
            ////    adbClient.Click(d, add.cords);
            ////    await Task.Delay(1000);
            ////}

            ////长按朋友圈文字，准备复制
            //add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/c2h']", TimeSpan.FromSeconds(2));
            //if (add != null)
            //{
            //    adbClient.Swipe(d, add, add, 2000);
            //    await Task.Delay(1000);
            //}

            ////复制朋友圈内容
            //add = adbClient.FindElement(d, "//node[@text='复制']", TimeSpan.FromSeconds(2));
            //if (add != null)
            //{
            //    adbClient.Click(d, add.cords);
            //    await Task.Delay(1000);
            //}

            //保存朋友圈图片
            var pics = adbClient.FindElements(d, "//node[@content-desc='图片']", TimeSpan.FromSeconds(2));
            var idx = 0;
            while (idx < pics.Count())
            {
                var p = pics[idx];
                if (idx == 0)
                {
                    adbClient.Click(d, p.cords);
                    await Task.Delay(1000);
                }

                //长按图片弹出  保存按钮
                adbClient.Swipe(d, p, p, 2000);
                await Task.Delay(1000);

                //点击保存图片
                var savePicBtn = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/knx' and @text='保存图片']", TimeSpan.FromSeconds(2));
                if (savePicBtn != null)
                {
                    adbClient.Click(d, savePicBtn.cords);
                    await Task.Delay(2000);
                }

                //左滑
                adbClient.Swipe(d, p.cords.x, p.cords.y, 0, p.cords.y, 150);
                await Task.Delay(2000);

                idx++;
            }

        }

        private async void btnSendMoments_Click(object sender, EventArgs e)
        {
            var d = adbClient.GetDevices()[0];
            var add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/f2s' and @text='发现']/..", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }

            add = adbClient.FindElement(d, "//node[@resource-id='android:id/title' and @text='朋友圈']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }

            add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/eo' and @content-desc='拍照分享']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }

            add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/knx' and @text='从相册选择']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }



            //获取相册里的图片
            var picChks = adbClient.FindElements(d, "//node[@resource-id='com.tencent.mm:id/gpy']", TimeSpan.FromSeconds(2));

            var idx = 0;
            var picCount = 5;
            while (idx < picCount)
            {
                var chk = picChks[idx];
                adbClient.Click(d, chk.startCords);
                await Task.Delay(300);
                idx++;
            }

            add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/en' and @text='完成']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }

            //完成图片选择
            add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/en']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }

            //粘贴文字
            add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/jsy']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
                //KEYCODE_PASTE
                adbClient.SendKeyEvent(d, "279");
                await Task.Delay(1000);

                //退掉键盘
                adbClient.BackBtn(d);
            }

            //发圈
            add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/en' and @text='发表']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {

            var d = adbClient.GetDevices()[0];

            var picChks = adbClient.FindElements(d, "//node[@resource-id='com.tencent.mm:id/gpy']", TimeSpan.FromSeconds(2));

            picChks[3].Click();

            var p = picChks[3];


            p.cords = new Cords { x= 666,y=163 };

            adbClient.Click(d,p.cords);

            //var idx = 0;
            //var picCount = 5;
            //while (idx < picCount)
            //{
            //    picChks[idx].Click();
            //    await Task.Delay(300);
            //    idx++;
            //}
        }



    }

}