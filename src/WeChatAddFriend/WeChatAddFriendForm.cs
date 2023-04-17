using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.DeviceCommands;
using AdvancedSharpAdbClient.Receivers;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using WeChatAddFriend.Tools;

namespace WeChatAddFriend
{
    public partial class WeChatAddFriendForm : Form
    {
        public static AdbClient adbClient;
        public static AdbServer srv;
        public static readonly string ADBPath = "platform-tools\\adb.exe";
        public static int addFriendTotalCount = 0;
        public List<string> importPhoneNos { get; set; }
        Params appParams;

        CancellationTokenSource addFriendTokenSource = new CancellationTokenSource();
        CancellationTokenSource forwardMomentsTokenSource = new CancellationTokenSource();
        public WeChatAddFriendForm()
        {
            InitializeComponent();
            Load += WeChatAddFriendForm_Load;

            dgPhones.AutoGenerateColumns = false;
        }

        private async void WeChatAddFriendForm_Load(object? sender, EventArgs e)
        {
            var paramFn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appParams");
            appParams = File.Exists(paramFn) ? JsonSerializer.Deserialize<Params>(File.ReadAllText(paramFn)) : new Params();

            txtWeChatNo.Text = appParams.ForwardMomentsWeChatNo;
            txtHour.Text = appParams.ForwardMomentsWaitHour.ToString();
            txtMint.Text = appParams.AddFriendWaitMinute.ToString();

            await Task.Run(() =>
            {
                //KillProcess("adb");
                StopProcess("5037");
                StopProcess("5555");
                StopProcess("62001");
                StopProcess("21503");
                StopProcess("5554");

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
                        try
                        {
                            UpdateDevices(adbClient.GetDevices());
                        }
                        catch (Exception ex) { }

                        Thread.Sleep(10000);
                    }
                });
                t.IsBackground = true;
                t.Start();

            });
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            btnStopAddFriend.Visible = true;
            addFriendTokenSource = new CancellationTokenSource();
            await Task.Run(async () =>
            {
                addFriendTotalCount = 0;
                if (adbClient == null) return;
                var devices = adbClient.GetDevices();
                if (devices == null || devices.Count < 1) return;
                devices = devices.Where(k => k.State == DeviceState.Online).ToList();
                var avgPhoneNoCnt = importPhoneNos.Count() / devices.Count();

                var idx = 0;
                var tasks = new List<Task>();
                while (idx < devices.Count)
                {
                    var task = AddFriendTask(devices[idx], importPhoneNos.Skip(idx * avgPhoneNoCnt).Take(avgPhoneNoCnt).ToList(), addFriendTokenSource);
                    tasks.Add(task);
                    idx++;
                }

                await Task.WhenAll(tasks);
            }, addFriendTokenSource.Token);
        }

        private async Task UploadPhoneNos(List<string> importPhoneNos)
        {
            var client = new HttpClient();
            using StringContent phoneNoContent = new(
                JsonSerializer.Serialize(
                    importPhoneNos.Select(phoneNo=>
                    new WeChatPhoneDto
                    {
                        PhoneNo = phoneNo,
                        Creator = LoginForm.LoginUserName,
                    })),
                Encoding.UTF8,
            "application/json");

            var res = await client.PostAsync($"{LoginForm.url}/addwechat", phoneNoContent);
            var rt = await res.Content.ReadAsStringAsync();
        }

        private async Task StartWeChat(DeviceData d)
        {
            //启动微信
            adbClient.StartApp(d, "com.tencent.mm");
            //回到首页
            await BackWeChatHome(d);
        }

        public Task AddFriendTask(DeviceData d, List<string> phoneNos, CancellationTokenSource cancellationTokenSource)
        {
            return Task.Run(async () =>
            {
                await StartWeChat(d);

                var idx = 0;
                var homeTabBar = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/f2s' and @text='微信']/..", TimeSpan.FromSeconds(2));
                if (homeTabBar != null)
                {
                    homeTabBar.Click();
                    await Task.Delay(1000);
                }

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

                    PrintLog($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}--{d.Name} 添加手机号:-{phoneNos[idx]}");

                    //返回
                    adbClient.BackBtn(d);

                    int.TryParse(txtMint.Text.Trim(), out var min);
                    //await Task.Delay(min * 60 * 1000);

                    if (addFriendTokenSource.Token.IsCancellationRequested)
                    {
                        addFriendTokenSource.Token.ThrowIfCancellationRequested();
                    }

                    SleepWithDoEvent(min * 60 * 1000);

                    adbClient.BackBtn(d);

                    addFriendTotalCount++;

                    idx++;
                }
            }, addFriendTokenSource.Token);
        }

        private async void btnImportData_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "文本文件(*.txt)|*.txt";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                importPhoneNos = System.IO.File.ReadAllLines(dlg.FileName).ToList();
                btnImportData.Text = $"导入数据({importPhoneNos.Count()}条)";
            }
            await UploadPhoneNos(importPhoneNos);
        }

        void PrintLog(string logtxt)
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                txtLog.Text += string.IsNullOrEmpty(txtLog.Text.Trim()) ? logtxt : "\n" + logtxt;
            });
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private async Task BackWeChatHome(DeviceData d)
        {
            while (true)
            {
                var radarAddFriend = adbClient.FindElement(d, "//node[@resource-id='android:id/title' and @text='雷达加朋友']", TimeSpan.FromSeconds(2));

                var disCover = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/f2s' and @text='发现']", TimeSpan.FromSeconds(2));
                if (disCover != null && radarAddFriend == null)
                {
                    break;
                }

                adbClient.BackBtn(d);
                await Task.Delay(1000);
            }
        }

        /// <summary>
        /// 转发朋友圈
        /// </summary>
        /// <returns></returns>
        private async void btnForwardMoments_Click(object sender, EventArgs e)
        {
            btnStopForwardMoments.Visible = true;
            forwardMomentsTokenSource = new CancellationTokenSource();
            await Task.Run(async () =>
            {
                if (adbClient == null) return;
                var devices = adbClient.GetDevices();
                if (devices == null) return;
                devices = devices.Where(k => k.State == DeviceState.Online).ToList();
                var idx = 0;
                var tasks = new List<Task>();
                while (idx < devices.Count())
                {
                    tasks.Add(ForwardMoments(devices[idx], txtWeChatNo.Text.Trim()));
                    idx++;
                }

                await Task.WhenAll(tasks);
            }, forwardMomentsTokenSource.Token);
        }

        /// <summary>
        /// 转发朋友圈
        /// </summary>
        /// <param name="d"></param>
        /// <param name="wxNo"></param>
        /// <returns></returns>
        private Task ForwardMoments(DeviceData d, string wxNo)
        {
            return Task.Run(async () =>
            {
                await StartWeChat(d);
                var savePicCount = 0;
                var hasVideo = false;
                await BackWeChatHome(d);

                var friendsTabBar = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/f2s' and @text='通讯录']/..", TimeSpan.FromSeconds(2));
                if (friendsTabBar != null)
                {
                    friendsTabBar.Click();
                    await Task.Delay(1000);
                }

                var searchFriendBtn = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/gsl']", TimeSpan.FromSeconds(2));
                if (searchFriendBtn != null)
                {
                    searchFriendBtn.Click();
                    await Task.Delay(1000);
                }

                var searchEdt = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/cd7' and @text='搜索']/..", TimeSpan.FromSeconds(2));
                if (searchEdt != null)
                {
                    searchEdt.Click();
                    await Task.Delay(1000);
                    adbClient.ClearInput(d, 15);
                }

                if (forwardMomentsTokenSource.Token.IsCancellationRequested)
                {
                    forwardMomentsTokenSource.Token.ThrowIfCancellationRequested();
                }
                //wangyaqing1991    

                //设置搜索要复制转发的好友
                adbClient.SendText(d, wxNo);
                await Task.Delay(3000);

                var searchFriendResultTextView = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/kpm']", TimeSpan.FromSeconds(2));
                if (searchFriendResultTextView != null)
                {
                    searchFriendResultTextView.Click();
                    await Task.Delay(1000);
                }

                //右上角的三个点，进入聊天记录界面
                var add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/eo']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }

                //点击好友头像
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/iw8']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }

                //点击朋友圈
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/o8' and @text='朋友圈']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }

                //点击进入朋友圈详情
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/br8']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }

                //长按朋友圈文字，准备复制
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/c2h']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Swipe(d, add, add, 2000);
                    await Task.Delay(1000);
                }

                //复制朋友圈内容
                add = adbClient.FindElement(d, "//node[@text='复制']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }
                if (forwardMomentsTokenSource.Token.IsCancellationRequested)
                {
                    forwardMomentsTokenSource.Token.ThrowIfCancellationRequested();
                }
                //保存朋友圈图片
                var pics = adbClient.FindElements(d, "//node[@content-desc='图片']", TimeSpan.FromSeconds(2));
                if (pics != null && pics.Count() > 0)
                {
                    savePicCount = pics.Count();
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

                var video = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/b47' and @content-desc='视频']", TimeSpan.FromSeconds(2));
                if (video != null)
                {
                    hasVideo = true;
                    adbClient.Click(d, video.cords);
                    await Task.Delay(5000);

                    //长按图片弹出  保存按钮
                    adbClient.Swipe(d, video, video, 2000);
                    await Task.Delay(2000);

                    //点击保存视频
                    var saveVideoBtn = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/knx' and @text='保存视频']", TimeSpan.FromSeconds(2));
                    if (saveVideoBtn != null)
                    {
                        adbClient.Click(d, saveVideoBtn.cords);
                        await Task.Delay(5000);
                    }
                }

                await BackWeChatHome(d);

                //开始发朋友圈
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/f2s' and @text='发现']/..", TimeSpan.FromSeconds(2));
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

                if (savePicCount > 0)
                {
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
                    while (idx < savePicCount)
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
                }
                else if (hasVideo)
                {
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

                    //获取相册里的图片 CheckBox
                    var videoChks = adbClient.FindElements(d, "//node[@resource-id='com.tencent.mm:id/gpy']", TimeSpan.FromSeconds(2));
                    var chk = videoChks[0];
                    adbClient.Click(d, chk.startCords);
                    await Task.Delay(300);

                    add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/en' and @text='完成']", TimeSpan.FromSeconds(2));
                    if (add != null)
                    {
                        adbClient.Click(d, add.cords);
                        await Task.Delay(1000);
                    }

                    //视频第二次点完成
                    add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/cco' and @text='完成']", TimeSpan.FromSeconds(2));
                    if (add != null)
                    {
                        adbClient.Click(d, add.cords);
                        await Task.Delay(1000);
                    }
                }
                else
                {
                    //长按拍照分享按钮,发纯文字朋友圈
                    adbClient.Swipe(d, add, add, 2000);
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


                int.TryParse(txtHour.Text.Trim(), out var hour);
                //await Task.Delay(hour * 60  * 60 * 1000);

                SleepWithDoEvent(hour * 60 * 60 * 1000);
            }, forwardMomentsTokenSource.Token);
        }

        private async void btnSaveMoments_Click(object sender, EventArgs e)
        {
            var d = adbClient.GetDevices()[0];

            //首页
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

            await BackWeChatHome(d);

            var friendsTabBar = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/f2s' and @text='通讯录']/..", TimeSpan.FromSeconds(2));
            if (friendsTabBar != null)
            {
                friendsTabBar.Click();
                await Task.Delay(1000);
            }

            var searchFriendBtn = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/gsl']", TimeSpan.FromSeconds(2));
            if (searchFriendBtn != null)
            {
                searchFriendBtn.Click();
                await Task.Delay(1000);
            }

            var searchEdt = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/cd7' and @text='搜索']/..", TimeSpan.FromSeconds(2));
            if (searchEdt != null)
            {
                searchEdt.Click();
                await Task.Delay(1000);
                adbClient.ClearInput(d, 15);
            }

            //wangyaqin1991    

            //设置搜索要复制转发的好友
            adbClient.SendText(d, "wangyaqin1991");
            await Task.Delay(3000);

            var searchFriendResultTextView = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/kpm']", TimeSpan.FromSeconds(2));
            if (searchFriendResultTextView != null)
            {
                searchFriendResultTextView.Click();
                await Task.Delay(1000);
            }

            //右上角的三个点，进入聊天记录界面
            var add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/eo']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }

            //点击好友头像
            add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/iw8']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }

            //点击朋友圈
            add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/o8' and @text='朋友圈']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }

            //点击进入朋友圈详情
            add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/br8']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }

            //长按朋友圈文字，准备复制
            add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/c2h']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Swipe(d, add, add, 2000);
                await Task.Delay(1000);
            }

            //复制朋友圈内容
            add = adbClient.FindElement(d, "//node[@text='复制']", TimeSpan.FromSeconds(2));
            if (add != null)
            {
                adbClient.Click(d, add.cords);
                await Task.Delay(1000);
            }

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


            await BackWeChatHome(d);
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

        private void 转为WIFI连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = dgPhones.CurrentRow.DataBoundItem as DeviceData;
            var ip = adbClient.GetDeviceIp(d);
            adbClient.StartTcpIp(d);
            adbClient.Connect(ip);
        }

        private void 断开连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = dgPhones.CurrentRow.DataBoundItem as DeviceData;
            var ip = adbClient.GetDeviceIp(d);
            adbClient.Disconnect(new DnsEndPoint(ip, AdbClient.DefaultPort));
        }

        public void KillProcess(string processName)
        {
            try
            {
                foreach (Process process in Process.GetProcessesByName(processName))
                {
                    if (!process.CloseMainWindow())
                    {
                        process.Kill();
                    }
                }
            }
            catch
            {
            }
        }

        private async void UpdateDevices(List<DeviceData> devices)
        {
            //var idx = 0;
            //while (idx < devices.Count)
            //{
            //    var device = devices[idx++];
            //    var ip = string.Empty;
            //    if (!device.Serial.EndsWith(":5555") && device.State == DeviceState.Online)
            //    {
            //        ip = adbClient.GetDeviceIp(device);
            //        if (!devices.Any(d => d.Serial.StartsWith(ip)))
            //        {
            //            device.Name = ip;
            //            await Task.Run(() =>
            //            {
            //                if (!string.IsNullOrEmpty(ip) && !devices.Any(d => d.Serial.StartsWith(ip)))
            //                {
            //                    adbClient.StartTcpIp(device);
            //                    adbClient.Connect(ip);
            //                }
            //            });
            //        }
            //        if (!string.IsNullOrEmpty(ip))
            //        {
            //            device.Name = ip;
            //        }
            //    }
            //}

            var dvs = dgPhones.DataSource as List<DeviceData>;
            dvs = dvs ?? new List<DeviceData>();
            devices = devices.Where(k => k.State == DeviceState.Online).ToList();
            var diffCount = devices.ExceptBy(dvs.Select(k => k.Serial), d => d.Serial).Count();
            if (dvs == null || diffCount > 0 || devices.Count != dvs.Count)
            {
                this.InvokeOnUiThreadIfRequired(() =>
                {
                    dgPhones.DataSource = devices;
                });
            }

        }

        public static void StopProcess(string port)
        {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.StandardInput.WriteLine("netstat -ano");
            process.StandardInput.WriteLine("exit");
            Regex regex = new Regex("\\s ", RegexOptions.Compiled);
            string text;
            while ((text = process.StandardOutput.ReadLine()) != null)
            {
                text = text.Trim();
                if (text.StartsWith("TCP", StringComparison.OrdinalIgnoreCase))
                {
                    text = regex.Replace(text, ",");
                    string[] array = text.Split(',', StringSplitOptions.None);
                    int processId;
                    if (array[2].EndsWith(":" + port) && array.Length >= 13 && int.TryParse(array[13], out processId))
                    {
                        KillProcess(processId);
                    }
                }
            }
        }

        public static void KillProcess(int processId)
        {
            try
            {
                var process = Process.GetProcessById(processId);
                if (!process.CloseMainWindow())
                {
                    process.Kill();
                }
            }
            catch
            {
            }
        }

        public static void SleepWithDoEvent(int ms)
        {
            int millisecondsTimeout = Math.Min(30, ms);
            DateTime now = DateTime.Now;
            do
            {
                Thread.Sleep(millisecondsTimeout);
                Application.DoEvents();
            }
            while ((DateTime.Now - now).TotalMilliseconds < (double)ms);
        }

        private void txtWeChatNo_TextChanged(object sender, EventArgs e)
        {
            //PersistentParams.TrySaveParam<string>("ForwardMomentsWeChatNo",txtWeChatNo.Text.Trim());
            appParams.ForwardMomentsWeChatNo = txtWeChatNo.Text.Trim();
            WriteParams();
        }

        private void txtMint_TextChanged(object sender, EventArgs e)
        {
            //PersistentParams.TrySaveParam<string>("AddFriendWaitMinute", txtWeChatNo.Text.Trim());

            int.TryParse(txtMint.Text.Trim(), out int mint);
            appParams.AddFriendWaitMinute = mint;
            WriteParams();
        }

        private void txtHour_TextChanged(object sender, EventArgs e)
        {
            //PersistentParams.TrySaveParam<string>("ForwardMomentsWaitHour", txtWeChatNo.Text.Trim());
            int.TryParse(txtHour.Text.Trim(), out int hour);
            appParams.ForwardMomentsWaitHour = hour;
            WriteParams();
        }

        void WriteParams()
        {
            var paramFn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appParams");
            File.WriteAllText(paramFn, JsonSerializer.Serialize(appParams));
        }

        private void btnStopForwardMoments_VisibleChanged(object sender, EventArgs e)
        {
            btnForwardMoments.Visible = !btnStopForwardMoments.Visible;
        }

        private void btnStopAddFriend_VisibleChanged(object sender, EventArgs e)
        {
            btnAdd.Visible = !btnStopAddFriend.Visible;
        }

        private void btnStopForwardMoments_Click(object sender, EventArgs e)
        {
            btnStopForwardMoments.Visible = false;
            forwardMomentsTokenSource.Cancel();
        }

        private void btnStopAddFriend_Click(object sender, EventArgs e)
        {
            btnStopAddFriend.Visible = false;
            addFriendTokenSource.Cancel();
        }
    }

    public class Params
    {
        public string ForwardMomentsWeChatNo { get; set; }
        public int AddFriendWaitMinute { get; set; }
        public int ForwardMomentsWaitHour { get; set; }
    }

}