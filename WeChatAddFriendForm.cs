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

        public WeChatAddFriendForm()
        {
            InitializeComponent();

            Load += WeChatAddFriendForm_Load;
        }

        private void WeChatAddFriendForm_Load(object? sender, EventArgs e)
        {
            Log.Initiate(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"������־.txt"), false, (int)Math.Pow(2.0, 23.0));
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
                            lblDeviceNum.Text = $"������:��{adbClient.GetDevices().Count()}��";
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

            var phoneNos = txtLog.Text.Split("\n", StringSplitOptions.RemoveEmptyEntries);

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

            IShellOutputReceiver rcvr = null;
            //����΢��
            adbClient.ExecuteRemoteCommand("am start com.tencent.mm/com.tencent.mm.ui.LauncherUI", d,rcvr);

            while (idx < phoneNos.Count)
            {
                var add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/hy6']", TimeSpan.FromSeconds(2));

                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }

                add = adbClient.FindElement(d, "//node[@text='�������']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }

                // ����������������
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

                //�����ı��ֻ���
                adbClient.SendText(d, phoneNos[idx]);
                await Task.Delay(3000);

                //�������
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/kms']", TimeSpan.FromSeconds(2));

                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(2000);
                }

                var userNotExist = adbClient.FindElement(d, "//node[@text='���û�������']", TimeSpan.FromSeconds(2));
                if (userNotExist != null)
                {
                    Log.Info($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}--{d.Name} add phone ---{phoneNos[idx]}--�����û�������");
                    idx++;
                    goto ADD_NEXT_PHONE;
                }

                //�Ѿ��Ǻ��ѵ� ��ť text Ϊ����Ϣ
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/khj' and @text='����Ϣ']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    await Task.Delay(2000);
                    adbClient.BackBtn(d);
                    Log.Info($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}--{d.Name} add phone ---{phoneNos[idx]}--�����û��Ѿ��Ǻ���");
                    idx++;
                    goto ADD_NEXT_PHONE;

                }
                //��Ӻ��ѵ�ͨѶ¼
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/khj' and @text='��ӵ�ͨѶ¼']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(2000);
                }

                //������Ӻ��Ѱ�ť
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/e9q']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }

                //�ٴε�����ͼӺ�������
                add = adbClient.FindElement(d, "//node[@resource-id='com.tencent.mm:id/e9q']", TimeSpan.FromSeconds(2));
                if (add != null)
                {
                    adbClient.Click(d, add.cords);
                    await Task.Delay(1000);
                }
                //����
                adbClient.BackBtn(d);
                var rt = int.TryParse(txtSecd.Text.Trim(),out var secd);

                await Task.Delay(secd * 1000);
                adbClient.BackBtn(d);

                addFriendTotalCount++;

                Log.Info($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}--{d.Name} add phone ---{phoneNos[idx]}");
                idx++;
            }
        }

        private void btnImportData_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "�ı��ļ�(*.txt)|*.txt";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var phoneNos = System.IO.File.ReadAllLines(dlg.FileName);
                btnImportData.Text = $"��������({phoneNos.Count()}��)";
            }
        }

        void PrintLog(string logtxt)
        {
            txtLog.Text += "\n" + logtxt;
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
        }
    }

}