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


