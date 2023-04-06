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

namespace WeChatAddFriend.Version
{
    public class ClientUpdater
    {
		private static bool _isUpdating;
		private static string _patchFn;
		private static string _baseFn;
		static ClientUpdater()
		{
			_patchFn = PathEx.ParentOfExePath + "patch";
			_baseFn = PathEx.ParentOfExePath + "base";
		}

        public static void UpdateForTip(appversion appver)
		{
			if (!appver.IsForceUpdate)
			{
				var ver = ShareUtil.ConvertVersionToString(appver.PatchVersion);
				var msg = string.Format("发现新版【{0}】，解决问题：\r\n\r\n{1}\r\n\r\n是否升级?", ver, appver.Tip);
				if(MessageBox.Show(msg, "升级提示",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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

        private static void UpdateAsync(appversion appver)
		{
			Task.Factory.StartNew(()=>Update(appver), TaskCreationOptions.LongRunning);
		}

		private async static void ManualUpdate()
        {
            try
            {
                var curappver = InstalledVersionManager.GetNewestVersion();
                var newappver = await SycmApi.GetNewVer(curappver.Version.ToString());
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

        private static void Update(appversion appver)
		{
			if (!_isUpdating)
			{
				_isUpdating = true;
				try
				{
					Log.Info(string.Format("开始升级，补丁={0}", Util.SerializeWithTypeName(appver)));
					var newVerDir = PathEx.ParentOfExePath + ShareUtil.ConvertVersionToString(appver.PatchVersion);
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
                        if (MessageBox.Show(msg, "提示",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
						{
							Reboot();
						}
					}
				}
				catch (Exception ex)
				{
					Log.Exception(ex);
                    DispatcherEx.xInvoke(() => MessageBox.Show(string.Format("升级失败，原因={0}", ex.Message)));
				}
				Log.Info("结束升级补丁");
				_isUpdating = false;
			}
		}

        public static void Reboot()
        {
	        var fn = PathEx.ParentOfExePath + "Booter.exe";
	        Process.Start(fn, "reboot");
	        DispatcherEx.xInvoke(()=>
	        {
		        if (Application.Current != null)
		        {
			        Application.Current.Shutdown();
		        }
	        });
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
}
