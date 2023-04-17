using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeChatAddFriend
{

    class UserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int AppVersion { get; set; }
    }

    class LoginDto
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("patch")]
        public AppPatchDto Patch { get; set; }
    }

    public class AppPatchDto
    {
        public bool isForceUpdate { get; set; }
        public int patchVersion { get; set; }
        public string patchFileName { get; set; }
        public int patchSize { get; set; }
        public string tip { get; set; }
    }


    public class WeChatPhoneDto
    {
        [JsonPropertyName("phoneNo")]
        public string PhoneNo { get; set; }
        [JsonPropertyName("creator")]
        public string? Creator { get; set; }
    }
}
