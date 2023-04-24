using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Json;
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
        //public static string url = "http://127.0.0.1:31005";
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

        private LoginUser loginUser;

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
                LoadUserPhones(LoginUserName);
                new WeChatAddFriendForm().ShowDialog();
            }
        }

        async void LoadUserPhones(string userName)
        { 
            var res = await new HttpClient().GetAsync($"{LoginForm.url}/getuserphones/{userName}");
            var userPhoneJson = await res.Content.ReadAsStringAsync();
            var paramFn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "userPhones");
            File.WriteAllText(paramFn, userPhoneJson);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            loginUser = new LoginUser();
            if (File.Exists(Path.Combine(AppContext.BaseDirectory, "mj.txt")))
            {
                txtUserName.Text = "youmanju";
                txtPassword.Text = "123";
                btnLogin_Click(null, null);
            }
            else
            {
                loginUser = GetUser();
                txtUserName.Text = loginUser.UserName;
                txtPassword.Text = loginUser.Password;
                if (loginUser.IsAutoLogin)
                {
                    btnLogin_Click(null, null);
                }
            }
        }


        private LoginUser GetUser()
        {
            var paramFn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "loginuser");
            if (!File.Exists(paramFn)) return loginUser;
            var userJson = File.ReadAllText(paramFn);
            if (!string.IsNullOrEmpty(userJson))
            {
                loginUser = JsonSerializer.Deserialize<LoginUser>(userJson);
            }
            return loginUser;
        }
        void WriteUser()
        {
            var paramFn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "loginuser");
            File.WriteAllText(paramFn, JsonSerializer.Serialize(loginUser));
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            loginUser.UserName = txtUserName.Text.Trim();
            WriteUser();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            loginUser.Password = txtPassword.Text.Trim();
            WriteUser();
        }

        private void chkAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            loginUser.IsAutoLogin = chkAutoLogin.Checked;
            WriteUser();
        }
    }

    public class LoginUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAutoLogin { get; set; }
    }

}
