using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChatAddFriend.Version;

namespace WeChatAddFriend
{
    public partial class LoginForm : Form
    {
        public static string url = "http://112.74.19.214:31005";

        public static string LoginUserName = string.Empty;

        private static LoginForm inst;
        public static LoginForm Inst
        {
            get
            {
                if (inst == null) inst = new LoginForm();
                return inst;
            }
        }

        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var latestVer = InstalledVersionManager.GetNewestVersion();
            var client = new HttpClient();
            using StringContent loginContent = new(
                JsonSerializer.Serialize(new
                {
                    userName = txtUserName.Text.Trim(),
                    password = txtPassword.Text.Trim(),
                    appVersion = latestVer == null ? 0 : latestVer.Version
                }),
                Encoding.UTF8,
                "application/json");

            var res = await client.PostAsync($"{url}/signin", loginContent);
            var rt = await res.Content.ReadAsStringAsync();
            var loginDto = JsonSerializer.Deserialize<LoginDto>(rt);
            if (loginDto != null && !string.IsNullOrEmpty(loginDto.UserName))
            {
                this.Visible = false;
                LoginUserName = loginDto.UserName;
                if (loginDto.Patch != null)
                {
#if !DEBUG
                    ClientUpdater.UpdateForTip(loginDto.Patch);
#endif
                }
                new WeChatAddFriendForm().ShowDialog();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(AppContext.BaseDirectory, "mj.txt")))
            {
                txtUserName.Text = "youmanju";
                txtPassword.Text = "123";
                btnLogin_Click(null, null);
            }
        }
    }


}
