# WeChatAddFriend
Android群控微信加好友

# 导出手机当前界面的xml
adb shell /system/bin/uiautomator dump --compressed /sdcard/win.uix
# 截屏
adb shell /system/bin/screencap -p /sdcard/screenshot.png

# 局域网WIFI连接手机
adb tcpip 5555
# 连接
adb connect 192.168.1.100:5555
# 连接另一台手机
adb -s [手机serial] tcpip 5555
# 连接
adb connect 192.168.1.101:5555
# 断开连接
adb disconnect

# 查看用户列表
adb shell pm list users
# 启动分身微信   0是主用户应用   999是分身用户应用
adb shell am start --user 0 com.tencent.mm/.ui.LauncherUI  
adb shell am start --user 999 com.tencent.mm/.ui.LauncherUI  

# 根据用户id查询用户空间的所有包
adb shell pm list packages --user {userid}
# 获取所有用户
adb shell pm list users


