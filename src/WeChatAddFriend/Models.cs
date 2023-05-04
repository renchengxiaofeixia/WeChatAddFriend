using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeChatAddFriend
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int AppVersion { get; set; }
    }

    public class LoginDto
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("patch")]
        public AppPatchDto Patch { get; set; }
    }

    public class AppPatchDto
    {
        [JsonPropertyName("isForceUpdate")]
        public bool IsForceUpdate { get; set; }
        [JsonPropertyName("patchVersion")]
        public int PatchVersion { get; set; }
        [JsonPropertyName("patchFileName")]
        public string PatchFileName { get; set; }
        [JsonPropertyName("patchSize")]
        public int PatchSize { get; set; }
        [JsonPropertyName("tip")]
        public string Tip { get; set; }
    }

    public class UserPhoneDto
    {
        [JsonPropertyName("serial")]
        public string Serial { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("product")]
        public string Product { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("phoneId")]
        public int PhoneId { get; set; }
        [JsonPropertyName("creator")]
        public string? Creator { get; set; }
    }

    public class WeChatPhoneDto
    {
        [JsonPropertyName("phoneNo")]
        public string PhoneNo { get; set; }
        [JsonPropertyName("creator")]
        public string? Creator { get; set; }
    }
}
