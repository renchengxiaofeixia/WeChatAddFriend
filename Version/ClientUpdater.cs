using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using WeChatAddFriend.Extensions;
using WeChatAddFriend.Net;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace WeChatAddFriend.Version
{
    public class ClientUpdater
    {
		private static bool _isUpdating;
		private static string _patchFn;
		private static string _baseFn;
        private static WindowsFormsSynchronizationContext windowsFormsSynchronizationContext;
        static ClientUpdater()
		{
			_patchFn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"patch");
			_baseFn = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "base");
            windowsFormsSynchronizationContext = new WindowsFormsSynchronizationContext();
        }

        public async static void Update() 
        {
            var loginInfo = await Login();
            if (loginInfo.UpdateEntity != null)
            {
                UpdateForTip(loginInfo.UpdateEntity);
            }
        }

        public async static Task<LoginDownloadEntity> Login()
        {
            var firstLoginMainNicks = new List<string> { "WeChatAddFriend" };
            var newAppver = InstalledVersionManager.GetNewestVersion();
            var url = "http://112.74.19.214:30010/api/bot/login";
            try
            {
                LoginDownloadEntity loginInfo = null;
                var client = new HttpClient();
                var parameters = new Dictionary<string, string>() {
                        {"nicks",string.Join(",",firstLoginMainNicks)},
                        {"firstLoginMainNicks",string.Join(",",firstLoginMainNicks)},
                        {"appver",newAppver==null ? "0" : newAppver.Version.ToString()},
                        {"instanceGuid",Guid.NewGuid().ToString() } };
                var res = client.PostAsync(url, new FormUrlEncodedContent(parameters)).Result;
                ApiResponse responseLogin = null;
                var json = await res.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    responseLogin = JsonSerializer.Deserialize<ApiResponse>(json);
                    if (responseLogin != null && responseLogin.Code == 200 && !string.IsNullOrEmpty(responseLogin.Data))
                    {
                        loginInfo = JsonSerializer.Deserialize<LoginDownloadEntity>(responseLogin.Data);
                    }
                    else
                    {
                        Log.Error("请求接口出错:" + responseLogin.Message);
                    }
                }

                if (loginInfo != null && loginInfo.UpdateEntity != null)
                {
                    loginInfo.UpdateEntity.PatchUrl = "http://112.74.19.214:30010/api/bot/downloadpatchfile?fn=" + loginInfo.UpdateEntity.PatchFileName;
                }
                return loginInfo;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        public static void UpdateForTip(UpdateDownloadEntity appver)
		{
			if (!appver.IsForceUpdate)
			{
				var ver = ShareUtil.ConvertVersionToString(appver.PatchVersion);
				var msg = string.Format("发现新版【{0}】，解决问题：\r\n\r\n{1}\r\n\r\n是否升级?", ver, appver.Tip);
				if(MessageBox.Show(msg, "升级提示",MessageBoxButtons.YesNo)  == DialogResult.Yes)
                {
                    UpdateAsync(appver);
                }
			}
			else
			{
				UpdateAsync(appver);
			}
		}

        public static void ManualUpdateAsync()
        {
            Task.Factory.StartNew(ManualUpdate, TaskCreationOptions.LongRunning);
        }

        private static void UpdateAsync(UpdateDownloadEntity updt)
		{
			Task.Factory.StartNew(()=>Update(updt), TaskCreationOptions.LongRunning);
		}

		private async static void ManualUpdate()
        {
            try
            {
                var curappver = InstalledVersionManager.GetNewestVersion();
                var newappver = await GetNewestVer(curappver.Version.ToString());
                if (newappver == null)
                {
                    throw new Exception("无法从服务器获取新版信息");
                }
                else
                {
                    MessageBox.Show(string.Format("正在下载升级文件，版本=【{0}】，不要关闭软件，大概需要3分钟。", ShareUtil.ConvertVersionToString(newappver.PatchVersion)));
                    Update(newappver);
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private static void Update(UpdateDownloadEntity appver)
		{
			if (!_isUpdating)
			{
				_isUpdating = true;
				try
				{
					Log.Info(string.Format("开始升级，补丁={0}", JsonSerializer.Serialize(appver)));
					var newVerDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ShareUtil.ConvertVersionToString(appver.PatchVersion));
					NetUtil.DownFile(appver.PatchUrl, _patchFn, appver.PatchSize);
                    DirectoryEx.DeleteC(newVerDir, true);
					CopyBaseFile(newVerDir);
					Zip.UnZipFile(_patchFn, newVerDir, null);
					File.Delete(_patchFn);
                    //DeleteOldVersion(ent.DeleteVersions, ent.DeleteVersionLessThan, ent.PatchVersion);
					InstalledVersionManager.SaveVersionToConfigFile(appver.PatchVersion);
					if (appver.IsForceUpdate)
					{
						var msg = string.Format("{0}已升级到版本{1},{0}将自动重启。\r\n\r\n升级信息:{2}", "软件", ShareUtil.ConvertVersionToString(appver.PatchVersion), appver.Tip);
                        MessageBox.Show(msg, "软件升级");
					}
					else
					{
                        var msg = string.Format("{0}已升级到版本{1},是否立即重启软件，使用新版本？", "软件", ShareUtil.ConvertVersionToString(appver.PatchVersion));
                        if (MessageBox.Show(msg, "提示",MessageBoxButtons.YesNo) == DialogResult.Yes)
						{
							Reboot();
						}
					}
				}
				catch (Exception ex)
				{
					Log.Exception(ex);
                    windowsFormsSynchronizationContext.Send(k => MessageBox.Show(string.Format("升级失败，原因={0}", ex.Message)),null);
				}
				Log.Info("结束升级补丁");
				_isUpdating = false;
			}
		}

        public static async Task<UpdateDownloadEntity> GetNewestVer(string appver)
        {
            try
            {
                var url = "http://112.74.19.214:30010/api/bot/getnewver";
                UpdateDownloadEntity newver = null;
                var client = new HttpClient();
                var content = new MultipartFormDataContent();
                content.Add(new StringContent("appver"), appver);
                var res = client.PostAsync(url, content).Result;
                ApiResponse responseVersion = null;
                var json = await res.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    responseVersion = JsonSerializer.Deserialize<ApiResponse>(json);
                    if (responseVersion != null && responseVersion.Code == 200 && !string.IsNullOrEmpty(responseVersion.Data))
                    {
                        newver = JsonSerializer.Deserialize<UpdateDownloadEntity>(responseVersion.Data);
                    }
                    else
                    {
                        Log.Error("请求接口出错:" + responseVersion.Message);
                    }
                }

                return newver;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        public static void Reboot()
        {
	        var fn = PathEx.ParentOfExePath + "Booter.exe";
	        Process.Start(fn, "reboot");
            windowsFormsSynchronizationContext.Send(k =>
            {
                Application.Exit();
	        },null);
        }

        private static void CopyBaseFile(string destDir)
		{
			var maxVerPath = GetMaxVersionPath();
			if (string.IsNullOrEmpty(maxVerPath))
			{
                //NetUtil.DownFile(baseDownloadUrl, _baseFn, baseLength);
                //Zip.UnZipFile(_baseFn, destDir, null);
                Log.Info("缺少基础版本.....无法升级到最新版");
                //MsgBox.ShowErrTip("无法升级到最新版....请手动安装!!");
			}
			else
			{
				DirectoryEx.Copy(maxVerPath, destDir);
			}
        }

        private static string GetMaxVersionPath(int v = 0)
		{
			string path = null;
			var newestVer= InstalledVersionManager.GetNewestVersion();
            if (newestVer != null && newestVer.Version >= v)
			{
                path = newestVer.Path;
			}
			return path;
		}
    }

    public class ApiResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }


    public class LoginDownloadEntity
    {
        public List<string> NickDatas;
        public List<string> ShopDatas;
        public string ClientBanReason;
        public string Tip;
        public UpdateDownloadEntity UpdateEntity;
    }

    public class UpdateDownloadEntity
    {
        public bool IsForceUpdate { get; set; }
        public int PatchVersion { get; set; }
        public string PatchUrl { get; set; }
        public string PatchFileName { get; set; }
        public int PatchSize { get; set; }
        public string Tip { get; set; }
    }
}
