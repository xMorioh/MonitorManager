using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MonitorManager
{
    public partial class Form1: Form
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem menuItem1, menuItem2, menuItem3;
        public Form1()
        {
            InitializeComponent();
            DownloadDependencies();

            //Load all User saved Components
            ApplicationName_textbox.Text = Properties.Settings.Default.ApplicationName_textbox;
            MonitorAmount_comboBox.Text = Properties.Settings.Default.MonitorAmount_comboBox;
            Custom_winddcutil_Param_textbox.Text = Properties.Settings.Default.Custom_winddcutil_Param_textbox;
            Watcher_winddcutil_Param_Code_textbox.Text = Properties.Settings.Default.Watcher_winddcutil_Param_Code_textbox;
            Watcher_winddcutil_Param_Value_textbox.Text = Properties.Settings.Default.Watcher_winddcutil_Param_Value_textbox;

            //Initiate TrayIcon
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.menuItem1, this.menuItem2, this.menuItem3 });

            // Initialize menuItems
            this.menuItem1.Index = 2;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.NotifyIcon_Exit);
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Open";
            this.menuItem2.Click += new System.EventHandler(this.NotifyIcon_Open);
            this.menuItem3.Index = 0;
            this.menuItem3.Text = "Hide";
            this.menuItem3.Click += new System.EventHandler(this.NotifyIcon_Hide);

            // Set up how the form should be displayed.
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Text = "Monitor Manager";

            // Create the NotifyIcon.
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon.Icon = this.Icon;

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon.ContextMenu = this.contextMenu;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon.Text = this.Text;
            notifyIcon.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_Open);
        }

        //Set Application to start hidden
        protected override void OnLoad(EventArgs e)
        {
            //Setting the Windows to SizableToolWindow will make it disappear from ALT+TAB
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Visible = false;
            Opacity = 0.00;

            base.OnLoad(e);
        }
        private void NotifyIcon_Open(object Sender, EventArgs e)
        {
            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            //Setting the Windows to Sizable will make it appear in ALT+TAB again
            this.FormBorderStyle = FormBorderStyle.Sizable;
            Visible = true;
            Opacity = 1.00;
            this.Activate();
        }
        private void NotifyIcon_Exit(object Sender, EventArgs e)
        {
            // Close the form, which closes the application and clears the NotifyIcon.
            // Also remove the application from the Clipboard Listener again.
            notifyIcon.Visible = false;
            this.Close();
        }
        private void NotifyIcon_Hide(object Sender, EventArgs e)
        {
            // Hides the form
            Visible = false;
        }

        //Define initial state, after the application starts we set the Brightness, if the application stops running we set the old brightness levels
        bool ApplicationStarted = false;

        private async void DownloadDependencies()
        {
            //Update|Download winddcutil, need to do this first so the User knows when everything is done after yt-dlp has finished last.
            if (!System.IO.File.Exists(SpecifiedAppdataFolder() + "winddcutil.exe"))
            {
                string pathWithExeFile = SpecifiedAppdataFolder() + "winddcutil.exe";

                try //Catchblock if the Remote Server is down or the User has no Internet or the file does not exist anymore on the remote server etc.
                {
                    var httpClient = new HttpClient();

                    using (var HTTPstream = await httpClient.GetStreamAsync("https://github.com/scottaxcell/winddcutil/releases/download/v2.0.0/winddcutil.exe"))
                    {
                        using (var fileStream = new FileStream(pathWithExeFile, FileMode.CreateNew, FileAccess.ReadWrite))
                        {
                            await HTTPstream.CopyToAsync(fileStream);
                        }
                    }
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("WARNING\nFailed to download winddcutil.\nPlease download winddcutil.exe from:\nhttps://github.com/scottaxcell/winddcutil/releases/download/v2.0.0/winddcutil.exe\nand insert it into " + SpecifiedAppdataFolder());
                }
            }
        }
        private string SpecifiedAppdataFolder()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            directory += "\\Morioh\\MonitorManager\\dependencies\\";
            Directory.CreateDirectory(Path.GetDirectoryName(directory));
            return directory;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ApplicationName_textbox.Text != string.Empty)
            {
                string WinddcutilExePath = SpecifiedAppdataFolder() + "winddcutil.exe";
                int MonitorAmount = Convert.ToInt32(MonitorAmount_comboBox.Text);

                Process[] ProcessActive = Process.GetProcessesByName(ApplicationName_textbox.Text);
                if (ProcessActive.Length != 0)
                {
                    for (int i = 0; i < MonitorAmount; i++)
                    {
                        string GetArgs = "getvcp " + (i + 1) + " " + Watcher_winddcutil_Param_Code_textbox.Text;
                        string SetArgs = "setvcp " + (i + 1) + " " + Watcher_winddcutil_Param_Code_textbox.Text + " " + Watcher_winddcutil_Param_Value_textbox.Text;

                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.CreateNoWindow = true;
                        startInfo.UseShellExecute = false;
                        startInfo.FileName = WinddcutilExePath;
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.Arguments = GetArgs;
                        startInfo.RedirectStandardOutput = true;

                        if (ApplicationStarted == false)
                        {
                            try
                            {   // Start the process with the info we specified.
                                // Get, read and then store the old brightness values for later change reversal
                                using (Process exeProcess = Process.Start(startInfo))
                                {
                                    while (!exeProcess.StandardOutput.EndOfStream)
                                    {
                                        string oldBrightnessValue = exeProcess.StandardOutput.ReadLine();
                                        // Trim to only the last two digits
                                        int length = oldBrightnessValue.Length;
                                        int num = length - 3;
                                        int length2 = length - num;
                                        oldBrightnessValue = oldBrightnessValue.Substring(num, length2);
                                        oldBrightnessValue = oldBrightnessValue.Replace(" ", "");
                                        oldBrightnessValues.SetValue(oldBrightnessValue, i);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Windows.Forms.MessageBox.Show("Something went wrong fetching the luminance values from monitor " + (i + 1) + "...\n\n" + ex.Message);
                            }
                        }

                        startInfo.Arguments = SetArgs;
                        startInfo.RedirectStandardOutput = false;

                        try
                        {   // Start the process with the info we specified.
                            // Call WaitForExit and then the using statement will close.
                            using (Process exeProcess = Process.Start(startInfo))
                            {
                                exeProcess.WaitForExit();
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show("Something went wrong setting the luminance value on your monitor " + (i + 1) + "...\n\n" + ex.Message);
                        }
                    }
                    ApplicationStarted = true;
                }
                else if (ProcessActive.Length == 0 && ApplicationStarted == true)
                {
                    for (int i = 0; i < MonitorAmount; i++)
                    {
                        string oldBrightnessValue = oldBrightnessValues.GetValue(i).ToString();
                        // If the Monitor amount does not match the command line will spit out a error message with the ending of ds, this way we know that the monitor doesn't exist and we skip this step here for it.
                        if (oldBrightnessValue == "ds")
                        { continue; }

                        string SetArgs = "setvcp " + (i + 1) + " 0x10 " + oldBrightnessValue;

                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.CreateNoWindow = true;
                        startInfo.UseShellExecute = false;
                        startInfo.FileName = WinddcutilExePath;
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.Arguments = SetArgs;
                        startInfo.RedirectStandardOutput = false;

                        try
                        {   // Start the process with the info we specified.
                            // Call WaitForExit and then the using statement will close.
                            using (Process exeProcess = Process.Start(startInfo))
                            {
                                exeProcess.WaitForExit();
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show("Something went wrong setting the old luminance value on your monitor " + (i + 1) + "...\n\n" + ex.Message);
                        }
                    }
                    ApplicationStarted = false;
                }
            }
        }

        //FORM VARIABLES
        public static string[] MonitorAmounts = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public static string[] oldBrightnessValues = new string[] { "M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8", "M9" };

        public static string[] VCPCodes = new string[] { "04", "05", "06", "01", "A2", "1E", "0E", "3E", "10", "6B", "6D", "6F", "71", "12", "1C", "8C", "52", "54", "72", "56", "58", "7C", "86", "82", "84", "AA", "88", "8B", "8E", "92", "B0", "CA", "CC", "D4", "DA", "DB", "DC", "08", "0A", "1F", "14", "0B", "0C", "8A", "90", "9B", "9C", "9D", "9E", "9F", "A0", "59", "5A", "5B", "5C", "5D", "5E", "11", "17", "16", "18", "1A", "6C", "6E", "70", "2E", "42", "2A", "2C", "40", "24", "26", "20", "22", "28", "29", "43", "3A", "3C", "41", "34", "36", "30", "32", "38", "39", "44", "62", "63", "64", "65", "8D", "8F", "91", "95", "96", "97", "98", "A4", "A5", "9A", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE" };
        public static string[] VCPCodeDescription = new string[] { "Image Restore factory defaults", "Image Restore factory luminance / contrast values", "Image Restore factory TV defaults", "Image Degauss", "Image Auto setup on/off", "Image Auto setup", "Image Clock", "Image Clock phase", "Image Luminance", "Image Backlight control Backlight Level: White", "Image Backlight control Backlight Level: Red", "Image Backlight control Backlight Level: Green", "Image Backlight control Backlight Level: Blue", "Image Contrast", "Image Focus", "Image TV Sharpness", "Image Active control", "Image Performance preservation", "Image Gamma", "Image H moiré", "Image V moiré", "Image Adjust zoom", "Image Display scaling", "Image Horizontal mirror (flip)", "Image Vertical mirror (flip)", "Image Screen orientation", "Image Velocity scan modulation", "Image TV channel up / down", "Image TV contrast", "Image TV black level / luminance", "Image Store / Restore Settings", "Image OSD", "Image OSD Language", "Image Stereo video mode", "Image Scan mode", "Image Image mode", "Image Display application", "Color Adjustments Restore factory color defaults", "Color Adjustments Restore factory TV defaults", "Color Adjustments Auto color setup", "Color Adjustments Color temperature Select color preset", "Color Adjustments Color temperature increment", "Color Adjustments Color temperature request", "Color Adjustments Color saturation", "Color Adjustments Hue", "Color Adjustments 6-axis color 6-axis hue Red", "Color Adjustments 6-axis color 6-axis hue Yellow", "Color Adjustments 6-axis color 6-axis hue Green", "Color Adjustments 6-axis color 6-axis hue Cyan", "Color Adjustments 6-axis color 6-axis hue Blue", "Color Adjustments 6-axis color 6-axis hue Magenta", "Color Adjustments 6-axis color 6-axis saturation Red", "Color Adjustments 6-axis color 6-axis saturation Yellow", "Color Adjustments 6-axis color 6-axis saturation Green", "Color Adjustments 6-axis color 6-axis saturation Cyan", "Color Adjustments 6-axis color 6-axis saturation Blue", "Color Adjustments 6-axis color 6-axis saturation Magenta", "Color Adjustments Flesh tone enhancement", "Color Adjustments User vision compensation", "Color Adjustments Video Gain (drive) Red", "Color Adjustments Video Gain (drive) Green", "Color Adjustments Video Gain (drive) Blue", "Color Adjustments Video Black Level Red", "Color Adjustments Video Black Level Green", "Color Adjustments Video Black Level Blue", "Color Adjustments Grey scale expansion", "Geometry Adjustment Horizontal Keystone", "Geometry Adjustment Horizontal Linearity", "Geometry Adjustment Horizontal Linearity balance", "Geometry Adjustment Horizontal Parallelogram", "Geometry Adjustment Horizontal Pincushion", "Geometry Adjustment Horizontal Pincushion balance", "Geometry Adjustment Horizontal Position (phase)", "Geometry Adjustment Horizontal Size", "Geometry Adjustment Horizontal Convergence R/B", "Geometry Adjustment Horizontal Convergence M/G", "Geometry Adjustment Vertical Keystone", "Geometry Adjustment Vertical Linearity", "Geometry Adjustment Vertical Linearity balance", "Geometry Adjustment Vertical Parallelogram", "Geometry Adjustment Vertical Pincushion", "Geometry Adjustment Vertical Pincushion balance", "Geometry Adjustment Vertical Position (phase)", "Geometry Adjustment Vertical Size", "Geometry Adjustment Vertical Convergence R/B", "Geometry Adjustment Vertical Convergence M/G", "Geometry Adjustment Rotation", "Audio Audio: speaker volume", "Audio Audio: speaker pair select", "Audio Audio: microphone volume", "Audio Audio: jack connection status", "Audio Audio mute", "Audio Audio: treble", "Audio Audio: bass", "Position / Size Window position (TL_X)", "Position / Size Window position (TL_Y)", "Position / Size Window position (BR_X)", "Position / Size Window position (BR_Y)", "Control Window Mask Control", "Control Window select", "Window background", "DPVL Support Monitor status", "DPVL Support Packet count", "DPVL Support Monitor X origin", "DPVL Support Monitor Y origin", "DPVL Support Header error count", "DPVL Support Bad CRC error count", "DPVL Support Client ID", "DPVL Support Link control" };
        

        private void Form1_Load(object sender, EventArgs e)
        {
            MonitorAmount_comboBox.Items.AddRange(MonitorAmounts);
        }

        private void ApplicationName_textbox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ApplicationName_textbox = ApplicationName_textbox.Text;
            Properties.Settings.Default.Save();
        }

        private void Custom_winddcutil_Param_textbox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Custom_winddcutil_Param_textbox = Custom_winddcutil_Param_textbox.Text;
            Properties.Settings.Default.Save();
        }

        private void MonitorAmount_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MonitorAmount_comboBox = MonitorAmount_comboBox.Text;
            Properties.Settings.Default.Save();
        }

        private void Watcher_winddcutil_Param_Code_textbox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Watcher_winddcutil_Param_Code_textbox = Watcher_winddcutil_Param_Code_textbox.Text;
            Properties.Settings.Default.Save();
        }
        
        private void Watcher_winddcutil_Param_Value_textbox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Watcher_winddcutil_Param_Value_textbox = Watcher_winddcutil_Param_Value_textbox.Text;
            Properties.Settings.Default.Save();
        }
        private void winddcutil_Link_Label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/scottaxcell/winddcutil");
        }

        private void Launch_Custom_Param_Button_Click(object sender, EventArgs e)
        {
            if (Custom_winddcutil_Param_textbox.Text != string.Empty)
            {
                string WinddcutilExePath = SpecifiedAppdataFolder() + "winddcutil.exe";
                string GetArgs = Custom_winddcutil_Param_textbox.Text;

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.FileName = WinddcutilExePath;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = GetArgs;
                startInfo.RedirectStandardOutput = false;

                if (Custom_winddcutil_Param_textbox.Text.StartsWith("setvcp"))
                {
                    try
                    {   // Start the process with the info we specified.
                        // Call WaitForExit and then the using statement will close.
                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            exeProcess.WaitForExit();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Something went wrong...\n\n" + ex.Message);
                    }
                }
                else
                {
                    startInfo.RedirectStandardOutput = true;
                    try
                    {   // Start the process with the info we specified.
                        // Get, read and then store the old brightness values for later change reversal
                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            while (!exeProcess.StandardOutput.EndOfStream)
                            {
                                string Output = exeProcess.StandardOutput.ReadLine();
                                System.Windows.Forms.MessageBox.Show(Output, "Output from: " + Custom_winddcutil_Param_textbox.Text);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Something went wrong...\n\n" + ex.Message);
                    }
                }
            }
        }

        private void Get_Monitor_VCP_Button_Click(object sender, EventArgs e)
        {
            int MonitorAmount = Convert.ToInt32(MonitorAmount_comboBox.Text);

            for (int i = 0; i < MonitorAmount; i++)
            {
                string WinddcutilExePath = SpecifiedAppdataFolder() + "winddcutil.exe";
                string GetArgs = "capabilities " + (i + 1);

                string VCPCapabilities = string.Empty;
                string FullOutput = string.Empty;
                string FullVCPCapabilitiesString = string.Empty;

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.FileName = WinddcutilExePath;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = GetArgs;
                startInfo.RedirectStandardOutput = true;
                try
                {   // Start the process with the info we specified.
                    // Get, read and then store the old brightness values for later change reversal
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        while (!exeProcess.StandardOutput.EndOfStream)
                        {
                            VCPCapabilities = exeProcess.StandardOutput.ReadLine();
                        }
                    }
                    FullVCPCapabilitiesString = VCPCapabilities;

                    List<string> bracketContents = new List<string>();
                    List<string> preBracketChars = new List<string>();

                    // Remove everything from the raw vcp string but the vcp() part in preparation for Regex
                    int startParameterIndex = VCPCapabilities.IndexOf("vcp(", StringComparison.Ordinal);
                    int indexLeft = startParameterIndex + 4;
                    VCPCapabilities = VCPCapabilities.Remove(0, indexLeft);
                    int endParameterIndex = VCPCapabilities.IndexOf("mccs_ver", StringComparison.Ordinal);
                    int indexRight = endParameterIndex - 1;
                    VCPCapabilities = VCPCapabilities.Remove(indexRight, VCPCapabilities.Length - indexRight);

                    // Regular expression to match brackets and their contents
                    Regex bracketRegex = new Regex(@"\([^)]*\)");
                    MatchCollection matches = bracketRegex.Matches(VCPCapabilities);

                    // Add the entire bracket including its contents to the list
                    // Also define a new VCPCapabilities string that has all brackets and their contents removed, leaving only the VCPcodes
                    string VCPCapabilitiesBracketTrimmed = VCPCapabilities;
                    foreach (Match match in matches)
                    {
                        bracketContents.Add(match.Value);
                        VCPCapabilitiesBracketTrimmed = VCPCapabilitiesBracketTrimmed.Replace(match.Value, "");
                    }

                    // Regular expression to match two characters before each opening bracket
                    Regex preBracketRegex = new Regex(@"(?<=..)\(");
                    MatchCollection preBracketMatches = preBracketRegex.Matches(VCPCapabilities);

                    // Add two characters before each opening bracket to the list
                    foreach (Match match in preBracketMatches)
                        preBracketChars.Add(VCPCapabilities.Substring(match.Index - 2, 2));

                    // Convert lists to arrays
                    string[] bracketContentsArray = bracketContents.ToArray();
                    string[] preBracketCharsArray = preBracketChars.ToArray();

                    int curIndexBracketLoop = 0;
                    int curIndexAllCodesLoop = 0;
                    // Output the results
                    foreach (string chars in preBracketCharsArray)
                    {
                        if (VCPCodes.Contains(chars))
                        {   //Find Index of VCPCode in VCPCodes Array, this way we can find VCPCodeDescription since these Arrays are content matched
                            int vcpCodeIndex = Array.FindIndex(VCPCodes, p => p.Equals(chars, StringComparison.Ordinal));
                            FullOutput += chars + bracketContentsArray.GetValue(curIndexBracketLoop) + ": " + VCPCodeDescription.GetValue(vcpCodeIndex) + "\n";
                            // Remove Codes for next loop
                            VCPCapabilitiesBracketTrimmed = VCPCapabilitiesBracketTrimmed.Replace(chars, "");
                            VCPCapabilitiesBracketTrimmed = VCPCapabilitiesBracketTrimmed.Replace("  ", " ");
                        }
                        else
                        {
                            FullOutput += chars + bracketContentsArray.GetValue(curIndexBracketLoop) + ": ---no description available---" + "\n";
                        }
                        curIndexBracketLoop++;
                    }

                    string[] VCPCapabilitieCodes = VCPCapabilitiesBracketTrimmed.ToString().Split(' ').ToArray();
                    foreach (string chars in VCPCapabilitieCodes)
                    {
                        if (VCPCodes.Contains(chars))
                        {   //Find Index of VCPCode in VCPCodes Array, this way we can find VCPCodeDescription since these Arrays are content matched
                            int vcpCodeIndex = Array.FindIndex(VCPCodes, p => p.Equals(chars, StringComparison.Ordinal));
                            FullOutput += chars + ": " + VCPCodeDescription.GetValue(vcpCodeIndex) + "\n";
                        }
                        else
                        {
                            FullOutput += chars + ": ---no description available---" + "\n";
                        }
                        curIndexAllCodesLoop++;
                    }
                    System.Windows.Forms.MessageBox.Show("Raw capabilities string:\n" + FullVCPCapabilitiesString + "\n\n" + "Decoded capabilities:\n" + FullOutput, "VCP Capabilities from Monitor: " + (i + 1));
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Something went wrong fetching your Monitor VCP capabilities from monitor " + (i + 1) + "...\n\n" + ex.Message);
                }
            }
        }
    }
}
