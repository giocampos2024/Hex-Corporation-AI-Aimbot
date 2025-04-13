using AotForms;
using Client;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
//using Helper;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AotForms.MouseHook;
using System.Runtime.InteropServices;
using System.Media;
using System.Reflection;
using Microsoft.VisualBasic.Logging;
using Reborn;
using System.Timers; // Garante o uso de System.Timers.Timer

namespace AotForms

{
    public partial class MainMenu : Form
    {
        public static api KeyAuthApp = new api(
           name: "Reflexo2023's Application", // Application Name
           ownerid: "BdQypV7jsG", // Owner ID
           secret: "5cd5981e0b8d1cae27f5f8432b4dbfd28bb16638c998203c824f9b1948b8f54d", // Application Secret
           version: "1.0"
       );
        IntPtr mainHandle;
        //  KeyHelper kh = new KeyHelper();
        bool keybind1 = false;
        bool shift;
        private AimbotForm aimform = new AimbotForm();
        private EspForm espform = new EspForm();
        private ConfigForm configform = new ConfigForm();
        bool aimbothks;
        bool esphks;
        bool confighks;

        private System.Timers.Timer _timer;


        public MainMenu(IntPtr handle)
        {
            KeyAuthApp.init();
            mainHandle = handle;
            InitializeComponent();
            MouseHook.SetHook();
            MouseHook.LeftButtonDown += MouseHook_LButtonDown;
            MouseHook.LeftButtonUp += MouseHook_LButtonUp;
            Application.ApplicationExit += (sender, e) => MouseHook.Unhook();


            // Configura o Timer
            _timer = new System.Timers.Timer(930); // 1 ms = 1 milissegundo
            _timer.Elapsed += TimerElapsed; // Evento a ser executado
            _timer.AutoReset = true; // Repetir após cada intervalo
            _timer.Start(); // Inicia o Timer

        }


        private void Kh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey) shift = false;
        }
        private async void Kh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey) shift = true;
            if (e.KeyCode == Keys.F1)
            {
                if (Config.FixEsp)
                {
                    Core.Entities = new();
                    InternalMemory.Cache = new();
                }
                else { }
            }



            if (e.KeyCode == Keys.Insert)
            {
                Config.Notif();
                if (keybind1 == false)
                {
                    aimbothks = false;
                    esphks = false;
                    confighks = false;
                    this.Hide();
                    aimform.Hide();
                    espform.Hide();
                    keybind1 = true;
                }
                else
                {
                    this.Show();

                    SetStreamMode();

                    void SetStreamMode()
                    {
                        foreach (var obj in Application.OpenForms)
                        {
                            var form = obj as Form;

                            if (Config.StreamMode)
                            {
                                SetWindowDisplayAffinity(form.Handle, WDA_NONE);
                                SetWindowDisplayAffinity(form.Handle, WDA_EXCLUDEFROMCAPTURE);

                            }
                            else
                            {



                            }
                        }
                    }

                    keybind1 = false;
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            KeyAuthApp.init();
            KeyAuthApp.check();
            this.TopMost = false;
        }
        bool startk = false;

        private CancellationTokenSource _cts;
        private async void MouseHook_LButtonDown()
        {
            if (startk)
            {
                _cts = new CancellationTokenSource();
                try
                {
                    await Task.Delay(Config.Aimbotype, _cts.Token); // Wait for 5 seconds or until canceled
                    Config.AimBotLeft = true;
                }
                catch (TaskCanceledException)
                {
                    // Task was canceled, do nothing
                }
            }

        }

        private void MouseHook_LButtonUp()
        {
            if (startk)
            {
                if (_cts != null)
                {
                    _cts.Cancel(); // Cancel the delay when the button is released
                    Config.AimBotLeft = false;
                }
            }
        }






        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            Config.Notif();
            guna2PictureBox3.FillColor = Color.Yellow;
            var processes = Process.GetProcessesByName("HD-Player");

            if (processes.Length != 1)
            {
                guna2PictureBox3.FillColor = Color.Red;
                return;
            }

            var process = processes[0];
            var mainModulePath = Path.GetDirectoryName(process.MainModule.FileName);
            var adbPath = Path.Combine(mainModulePath, "HD-Adb.exe");

            if (!File.Exists(adbPath))
            {
                guna2PictureBox3.FillColor = Color.Red;
                return;
            }


            var adb = new Adb(adbPath);
            await adb.Kill();

            var started = await adb.Start();
            if (!started)
            {
                guna2PictureBox3.FillColor = Color.Red;
                return;
            }

            String pkg = "com.dts.freefireth";
            String lib = "libil2cpp.so";

            bool isFreeFireMax = false;
            if (isFreeFireMax)
            {
                pkg = "com.dts.freefiremax";
            }

            var moduleAddr = await adb.FindModule(pkg, lib);
            if (moduleAddr == 0) // If the module address is not found
            {
                guna2PictureBox3.FillColor = Color.Red;
                return;
            }

            Offsets.Il2Cpp = moduleAddr;
            Core.Handle = FindRenderWindow(mainHandle);

            var esp = new ESP();
            await esp.Start();

            new Thread(Data.Work) { IsBackground = true }.Start();
            new Thread(Aimbot.Work) { IsBackground = true }.Start();
            guna2PictureBox3.FillColor = Color.LimeGreen;

        }
        static IntPtr FindRenderWindow(IntPtr parent)
        {
            IntPtr renderWindow = IntPtr.Zero;
            WinAPI.EnumChildWindows(parent, (hWnd, lParam) =>
            {
                StringBuilder sb = new StringBuilder(256);
                WinAPI.GetWindowText(hWnd, sb, sb.Capacity);
                string windowName = sb.ToString();
                if (!string.IsNullOrEmpty(windowName))
                {
                    if (windowName != "HD-Player")
                    {
                        renderWindow = hWnd;
                    }
                }
                return true;
            }, IntPtr.Zero);

            return renderWindow;
        }
        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void guna2CustomCheckBox1_Click(object sender, EventArgs e)
        {
            Config.AimBot = guna2CustomCheckBox1.Checked;
            Config.ESPLine = guna2CustomCheckBox1.Checked;
        }


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Config.Notif();
            if (!aimbothks)
            {
                aimform.StartPosition = FormStartPosition.Manual;
                int spacing = 10;
                int form2X = this.Location.X + this.Width + spacing;
                int form2Y = this.Location.Y;
                aimform.Location = new Point(form2X, form2Y);
                aimform.Show();

                aimbothks = true;
            }
            else
            {
                aimform.Hide();

                aimbothks = false;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Config.Notif();
            if (!esphks)
            {
                espform.StartPosition = FormStartPosition.Manual;
                int spacing = 10;
                int form2X = this.Location.X + this.Width + spacing;
                int form2Y = this.Location.Y;
                espform.Location = new Point(form2X, form2Y);
                espform.Show();

                esphks = true;
            }
            else
            {
                espform.Hide();

                esphks = false;
            }

        }


        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Executa os comandos
            Core.Entities = new();
            InternalMemory.Cache = new();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Core.Entities = new();
            InternalMemory.Cache = new();
        }

        private void guna2CustomCheckBox3_Click(object sender, EventArgs e)
        {
            Config.Notif();
            KillHDPlayerProcess();
            Environment.Exit(0);
        }
        private void KillHDPlayerProcess()
        {
            try
            {
                // Get all processes with the name "HD-Player"
                var processes = Process.GetProcessesByName("HD-Player");

                foreach (var process in processes)
                {
                    // Kill the process
                    process.Kill();
                    process.WaitForExit(); // Optionally wait for the process to exit
                }


            }
            catch (Exception ex)
            {

            }
        }

        private void guna2CustomCheckBox1_Click_1(object sender, EventArgs e)
        {

            if (guna2CustomCheckBox1.Checked) { guna2CustomCheckBox2.Checked = true; }
            else { guna2CustomCheckBox2.Checked = false; }

            SetStreamMode(guna2CustomCheckBox1.Checked);
            Config.StreamMode = guna2CustomCheckBox1.Checked;
            Config.sound = guna2CustomCheckBox1.Checked;
            void SetStreamMode(bool state)
            {
                foreach (var obj in Application.OpenForms)
                {
                    var form = obj as Form;

                    if (state)
                    {
                        SetWindowDisplayAffinity(form.Handle, WDA_EXCLUDEFROMCAPTURE);

                    }
                    else
                    {

                        SetWindowDisplayAffinity(form.Handle, WDA_NONE);

                    }
                }
            }
        }
        [DllImport("user32.dll")]
        static extern uint SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

        const uint WDA_NONE = 0x00000000;
        const uint WDA_MONITOR = 0x00000001;
        const uint WDA_EXCLUDEFROMCAPTURE = 0x00000011;

        private void guna2CustomCheckBox2_Click(object sender, EventArgs e)
        {
            Config.sound = guna2CustomCheckBox2.Checked;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            try
            {
                Config.Notif(); // Assuming this is a valid method
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "https://discord.gg/",
                    UseShellExecute = true // Ensures that it opens the URL in the default browser
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void guna2Button6_Click(object sender, EventArgs e)
        {
            Config.Notif();
            statuslbl.Text = "Status : Verifying User...";

            await Task.Delay(300);

            // Run the login task on a separate thread to avoid blocking the UI
            var loginTask = Task.Run(() => KeyAuthApp.login(usernametxt.Text, passwordtxt.Text));

            // Wait for the login task to complete
            await loginTask;

            // Update the UI after the login attempt
            if (KeyAuthApp.response.success)
            {
                statuslbl.Text = "Status : Login Success!!";
                await Task.Delay(1000);
                guna2Panel1.Hide();  // Hide the login panel on success
                startk = true;
            }
            else
            {
                // Display the error message from KeyAuth
                statuslbl.Text = "Status : " + KeyAuthApp.response.message;
                await Task.Delay(300);
                //  Application.Exit();
                // guna2Panel1.Hide();  // Hide the login panel on failure
                startk = false;
            }
        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void usernametxt_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
