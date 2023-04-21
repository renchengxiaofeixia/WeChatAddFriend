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
        public static string ParentOfExePath
        {
            get
            {
                return Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName;
            }
        }

        static ClientUpdater()
        {
            _patchFn = Path.Combine(ParentOfExePath, "patch");
            _baseFn = Path.Combine(ParentOfExePath, "base");
            windowsFormsSynchronizationContext = new WindowsFormsSynchronizationContext();
        }

        public static void UpdateForTip(AppPatchDto appver)
        {
            if (!appver.IsForceUpdate)
            {
                var ver = ShareUtil.ConvertVersionToString(appver.PatchVersion);
                var msg = string.Format("发现新版【{0}】，解决问题：\r\n\r\n{1}\r\n\r\n是否升级?", ver, appver.Tip);
                if (MessageBox.Show(msg, "升级提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    UpdateAsync(appver);
                }
            }
            else
            {
                UpdateAsync(appver);
            }
        }

        private static void UpdateAsync(AppPatchDto updt)
        {
            Task.Factory.StartNew(() => Update(updt), TaskCreationOptions.LongRunning);
        }

        private static void Update(AppPatchDto appver)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;
                try
                {
                    Log.Info(string.Format("开始升级，补丁={0}", JsonSerializer.Serialize(appver)));
                    var newVerDir = Path.Combine(ParentOfExePath, ShareUtil.ConvertVersionToString(appver.PatchVersion));
                    NetUtil.DownFile($"{LoginForm.url}/files/{appver.PatchFileName}", _patchFn, appver.PatchSize);
                    DirectoryEx.DeleteC(newVerDir, true);
                    Log.Info($"新版本目录:{newVerDir}");
                    CopyBaseFile(newVerDir);
                    Zip.UnZipFile(_patchFn, newVerDir, null);
                    File.Delete(_patchFn);
                    //DeleteOldVersion(ent.DeleteVersions, ent.DeleteVersionLessThan, ent.PatchVersion);
                    InstalledVersionManager.SaveVersionToConfigFile(appver.PatchVersion);
                    if (appver.IsForceUpdate)
                    {
                        var msg = string.Format("{0}已升级到版本{1},{0}将自动重启。\r\n\r\n升级信息:{2}", "软件", ShareUtil.ConvertVersionToString(appver.PatchVersion), appver.Tip);
                        windowsFormsSynchronizationContext.Send(k =>MessageBox.Show(msg, "软件升级"), null);
                    }
                    else
                    {
                        var msg = string.Format("{0}已升级到版本{1},是否立即重启软件，使用新版本？", "软件", ShareUtil.ConvertVersionToString(appver.PatchVersion));

                        windowsFormsSynchronizationContext.Send(k =>
                        {
                            if (MessageBox.Show(msg, "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                Reboot();
                            }
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                    windowsFormsSynchronizationContext.Send(k => MessageBox.Show(string.Format("升级失败，原因={0}", ex.Message)), null);
                }
                Log.Info("结束升级补丁");
                _isUpdating = false;
            }
        }

        public static void Reboot()
        {
            var fn = PathEx.ParentOfExePath + "Booter.exe";
            Process.Start(fn, "reboot");
            windowsFormsSynchronizationContext.Send(k =>
            {
                Application.Exit();
            }, null);
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
            var newestVer = InstalledVersionManager.GetNewestVersion();
            if (newestVer != null && newestVer.Version >= v)
            {
                path = newestVer.Path;
            }
            return path;
        }
    }
}
