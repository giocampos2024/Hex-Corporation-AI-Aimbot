using AotForms;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class EspForm : Form
    {
        public EspForm()
        {
            InitializeComponent();
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            Config.Notif();
            Config.ESPLine = guna2ToggleSwitch1.Checked;
        }

        private void EspForm_Load(object sender, EventArgs e)
        {
            this.TopMost = false;
            guna2ComboBox1.Text = "Up";
        }

        private void guna2ToggleSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            Config.Notif();
            Config.ESPBox2 = guna2ToggleSwitch2.Checked;
        }

        private void guna2ToggleSwitch3_CheckedChanged(object sender, EventArgs e)
        {
            Config.Notif();
            Config.ESPFillBox = guna2ToggleSwitch3.Checked;
        }

        private void guna2ToggleSwitch4_CheckedChanged(object sender, EventArgs e)
        {
            Config.Notif();
            Config.ESPBox = guna2ToggleSwitch4.Checked;
        }

        private void guna2ToggleSwitch5_CheckedChanged(object sender, EventArgs e)
        {
            Config.Notif();
            Config.ESPSkeleton = guna2ToggleSwitch5.Checked;

        }

        private void guna2ToggleSwitch6_CheckedChanged(object sender, EventArgs e)
        {
            Config.Notif();
            Config.ESPName = guna2ToggleSwitch6.Checked;
            Config.ESPHealth = guna2ToggleSwitch6.Checked;
            Config.espbg = guna2ToggleSwitch6.Checked;
            Config.esptotalplyer = guna2ToggleSwitch6.Checked;
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedItem = (string)comboBox.SelectedItem;

            if (selectedItem == "Bottom")
            {
                Config.espcfx = true;
            }
            else if (selectedItem == "Up")
            {
                Config.espcfx = false;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Config.Notif();
            var picker = new ColorDialog();
            var result = picker.ShowDialog();

            if (result == DialogResult.OK)
            {
                guna2Button1.FillColor = picker.Color;
                Config.ESPLineColor = picker.Color;
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Config.Notif();
            var picker = new ColorDialog();
            var result = picker.ShowDialog();

            if (result == DialogResult.OK)
            {
                guna2Button2.FillColor = picker.Color;
                Config.ESPBoxColor = picker.Color;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Config.Notif();
            var picker = new ColorDialog();
            var result = picker.ShowDialog();

            if (result == DialogResult.OK)
            {
                guna2Button3.FillColor = picker.Color;
                Config.ESPFillBoxColor = picker.Color;
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Config.Notif();
            var picker = new ColorDialog();
            var result = picker.ShowDialog();

            if (result == DialogResult.OK)
            {
                guna2Button4.FillColor = picker.Color;
                Config.ESPSkeletonColor = picker.Color;
            }
        }

        private void guna2PictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void guna2ToggleSwitch7_CheckedChanged(object sender, EventArgs e)
        {
            Config.FixEsp = guna2ToggleSwitch7.Checked;
            Core.Entities = new();
            InternalMemory.Cache = new();
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
        uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        const int PROCESS_CREATE_THREAD = 0x0002;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_READ = 0x0010;
        const uint MEM_COMMIT = 0x00001000;
        const uint MEM_RESERVE = 0x00002000;
        const uint PAGE_READWRITE = 4;

        private WebClient webclient = new WebClient();
        private IEnumerable<object> result;
        private async void guna2ToggleSwitch8_CheckedChanged(object sender, EventArgs e)
        {
            Console.Beep(300, 300);
            string fileName = "C:\\Windows\\System32\\wallZ.dll";
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            string adress = "";
            bool flag = File.Exists(fileName);
            if (flag)
            {
                File.Delete(fileName);
            }
            this.webclient.DownloadFile(adress, fileName);
            Process targetProcess = Process.GetProcessesByName("HD-Player")[0];
            IntPtr procHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);
            IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            string dllName = "wallZ.dll";
            IntPtr allocMemAddress = VirtualAllocEx(procHandle, IntPtr.Zero, (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
            UIntPtr bytesWritten;
            WriteProcessMemory(procHandle, allocMemAddress, Encoding.Default.GetBytes(dllName), (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten);
            CreateRemoteThread(procHandle, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
