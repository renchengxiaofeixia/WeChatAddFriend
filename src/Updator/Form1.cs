using ICSharpCode.SharpZipLib.Zip;
using System.Collections;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Updator
{
    public partial class Form1 : Form
    {
        string url = "http://112.74.19.214:31005/upload";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewVer.Text))
            {
                MessageBox.Show("[版本]必填！！！！");
                return;
            }
            var dialog = new FolderBrowserDialog();
            dialog.Description = "选择文件夹:";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = dialog.SelectedPath;
            }
        }

        private void btnFileUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFile.Text) || string.IsNullOrEmpty(txtNewVer.Text) || string.IsNullOrEmpty(txtTip.Text))
            {
                MessageBox.Show("[文件][版本][提示]必填！！！！");
                return;
            }
            if (!Regex.IsMatch(txtNewVer.Text.Trim(), "^v[0-9].[0-9].[0-9]"))
            {
                MessageBox.Show("版本格式不正确，格式必须为[v8.0.0]！！！！");
                return;
            }

            //压缩选择的文件夹
            ZipFolder();

            //上传到服务器
            var dir = txtFile.Text.Trim();
            var patchFile = Path.Combine(dir.Substring(0, dir.LastIndexOf("\\")), txtNewVer.Text.Trim() + "_Patch.zip");
            var patchVer = ConvertStringToVersion(txtNewVer.Text.Trim());
            var forceUpdate = chkForceUpdate.Checked;
            var tip = txtTip.Text.Trim();

            var pms = new Dictionary<string, string>() {
                {"isForceUpdate",forceUpdate.ToString().ToLower()},
                {"tip",tip},
                {"patchVersion",patchVer.ToString()},
                {"patchFileName",txtNewVer.Text.Trim() + "_Patch.zip"}
            };

            var svrUrl = BuildGetUrl(url, pms);
            if (UploadFile(svrUrl, patchFile))
            {
                MessageBox.Show("上传成功!!");
            }
        }

        public bool UploadFile(string url, string patchFn)
        {
            int BufferSize = 1024;
            HttpClient client = new HttpClient();
            using (var fileStream = new FileStream(patchFn, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, useAsync: true))
            {
                // Create a stream content for the file
                var content = new StreamContent(fileStream, BufferSize);
                content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
                // Create Multipart form data content, add our submitter data and our stream content
                var formData = new MultipartFormDataContent();
                formData.Add(content, "fi", patchFn);

                // Post the MIME multipart form data upload with the file
                var rm = client.PostAsync(url, formData).Result;
                return rm.IsSuccessStatusCode;
            }
        }

        public static string ConvertVersionToString(int v)
        {
            int v1 = v / 10000;
            int v2 = v % 10000 / 100;
            int v3 = v % 100;
            return string.Format("v{0}.{1}.{2}", v1, v2, v3);
        }

        public static int ConvertStringToVersion(string vstr)
        {
            vstr = vstr.Trim().ToLower();
            var vs = vstr.Split('.');
            int v1 = Convert.ToInt32(vs[0].Substring(1));
            int v2 = Convert.ToInt32(vs[1]);
            int v3 = Convert.ToInt32(vs[2]);
            return v1 * 10000 + v2 * 100 + v3;
        }



        private void ZipFolder()
        {
            var dir = txtFile.Text.Trim();
            if (string.IsNullOrEmpty(txtFile.Text) || string.IsNullOrEmpty(txtNewVer.Text))
            {
                MessageBox.Show("[文件][版本]必填！！！！");
                return;
            }
            var zipfile = System.IO.Path.Combine(dir.Substring(0, dir.LastIndexOf("\\")), txtNewVer.Text.Trim() + "_Patch.zip");
            if (Directory.Exists(dir))
            {
                Zip.ZipDir(dir, zipfile, 5);
            }
        }

        /// <summary>
        /// 组装GET请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>带参数的GET请求URL</returns>
        public String BuildGetUrl(string url, Dictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }
            return url;
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(Dictionary<string, string> parameters)
        {
            var postData = new StringBuilder();
            bool hasParam = false;

            parameters.Keys.ToList().ForEach(key =>
            {
                var name = key;
                var value = parameters[key];
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(HttpUtility.UrlEncode(value));
                    hasParam = true;
                }
            });
            return postData.ToString();
        }
    }
}