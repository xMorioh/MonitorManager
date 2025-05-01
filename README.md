# Morioh's Monitor Manager
# [Download the latest version here](https://github.com/xMorioh/MonitorManager/releases/latest)
# [Check out my other Projects on my Website](https://xmorioh.gitlab.io/index.html)


**About this Project**

The Goal is to make a small piece of software that would automatically lower monitor brightness as soon as SteamVR is started, this goal has been achieved and more.
<br>
This Application is made in .NET C# as a Forms Application.
<br>
This Application has a dependency which will be downloaded automatically after the program has launched and it doesn't exist yet: [winddcutil](https://github.com/scottaxcell/winddcutil)

**Application Features**:
* Lowers Monitors actual brightness by talking directly to the Monitors chip via VESA-MCCS over VCPs to save energy when a spicific application is launched, the old Brightness values will be restored after the application is closed again
* Has a Button to get list of all VCP functions your Monitors have
* Has a Custom input field to use all winddcutil functions from within this application
<br>

**How to use this Application**:
<br>
The "Application to watch" field is for you to enter the process name of the application you want to monitor so that if it is detected after a specific tick rate it will then lower your monitors brightness to 0 with default settings. Once the application is closed again it will restore the values from before the change to 0. This field also supports several entries by separating each application you want to watch by a comma aka. vrmonitor,chrome,firefox,notepad
<br>
The two fields behind it are for the action that should be taken once the process watcher finds the application you entered, by default it is 0x10 for brightness/luminance and 0 for the corresponding value. The fixed prefix for this Argument is "setvcp *MonitorLoopInteger* " -> VCPCode -> Value.
<br>
<br>
The "How many Monitors do you have?" field is to specify how many Monitors you have, this will be used for internal loops to tell the application watcher function and then "Get Monitor VCP" Button how often it should be executed to yield the desired amount of results.
<br>
<br>
The "Custom winddcutil Param" field is for entering a custom argument for the winddcutil application to execute, with the "Launch" Button you can then fire the command to execute winddcutil with the specified arguments.
<br>
<br>
The "Get Monitor VCP" Button will spawn a new Window with the raw VCP capabilities from your Monitor(s) including a decoded list of functionalities you can use in the "Custom winddcutil Param" field
<br>
<br>
The Application will start hidden by default, it will hide in the Tray Icons, you can open it again with a double click or right click and "Open", there you can also hide it again entirely or Exit the Application.
<br>
<br>
To run this application via Windows autostart just create a new task in Windows Task Scheduler for it.
<br>
<br>

**Preview**:

![MonitorManager-Preview](https://github.com/xMorioh/MonitorManager/blob/master/MonitorManager-Preview.png)
<br>
<br>
![MonitorManager-Preview2](https://github.com/xMorioh/MonitorManager/blob/master/MonitorManager-Preview2.png)
