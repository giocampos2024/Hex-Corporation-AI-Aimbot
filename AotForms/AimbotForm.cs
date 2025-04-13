using AotForms;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Memory;

namespace Client
{
    public partial class AimbotForm : Form
    {

        //Mem Memory
        Mem memory = new Mem();

        public AimbotForm()
        {
            InitializeComponent();
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            Config.AimBot = guna2ToggleSwitch1.Checked;
        }

        private void guna2TrackBar2_Scroll(object sender, ScrollEventArgs e)
        {
            /*var delay = guna2TrackBar2.Value;

            lblDistance.Text = $"({delay})";

            Config.aimdelay = delay;
        */
        }

        private void guna2ToggleSwitch4_CheckedChanged(object sender, EventArgs e)
        {
            // Chama a função de notificação que você já tem.
            Config.Notif();

            // Atualiza a configuração do NoRecoil
            Config.NoRecoil = guna2ToggleSwitch4.Checked;

            // Lê e escreve na memória com base no estado do toggle
            var undercamacess = InternalMemory.Read<uint>(Offsets.Il2Cpp + Offsets.CNIKONPMDHF, out var undercamacess2);
            var undercamStaticClass = InternalMemory.Read<uint>(undercamacess2 + 0x5c, out var undercamStaticClass2);

            // Define o valor de 'undercam' dependendo se o toggle está ativado ou não
            InternalMemory.Write(undercamStaticClass2 + Offsets.undercam, guna2ToggleSwitch4.Checked ? 4.5f : 1.0f);
        }


        private void guna2ToggleSwitch5_CheckedChanged(object sender, EventArgs e)
        {
            Config.Notif();
            Config.aimdelay = guna2ToggleSwitch5.Checked;
        }

        private void guna2ToggleSwitch6_CheckedChanged(object sender, EventArgs e)
        {
            Config.Notif();
            Config.Aimfovc = guna2ToggleSwitch6.Checked;
        }

        private void guna2TrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
            var aimfov1 = guna2TrackBar1.Value;

            aimfovtxt.Text = $"({aimfov1})";

            Config.Aimfov = aimfov1;
        }

        private void guna2TrackBar3_Scroll(object sender, ScrollEventArgs e)
        {

            var distance = guna2TrackBar3.Value;

            rangeaim.Text = $"({distance})";

            Config.AimBotMaxDistance = distance;
        }

        private void AimbotForm_Load(object sender, EventArgs e)
        {

            this.TopMost = false;
        }

        private void guna2ToggleSwitch3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private async void guna2ToggleSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            string search1 = "ff ff ff ff 08 00 00 00 00 00 60 40 cd cc 8c 3f 8f c2 f5 3c cd cc cc 3d 06 00 00 00 00 00 00 00 00 00 00 00 00 00 f0 41 00 00 48 42 00 00 00 3f 33 33 13 40 00 00 b0 3f 00 00 80 3f 01 00 00 00";
            string replace1 = "ff ff ff ff 08 00 00 00 00 00 60 40 e0 b1 ff ff e0 b1 ff ff e0 b1 ff ff e0 b1 ff ff e0 b1 ff ff 00 00 00 00 00 00 f0 41 00 00 48 42 00 00 00 3f 33 33 13 40 00 00 b0 3f 00 00 80 3f 01 00 00 00";
            string processName = "HD-Player";
            string injectingMessage = "AimAwm AWM Injetando...";
            string successMessage = "AimAwm Injetado!";
            string failureMessage = "Falha ao aplicar AimAwm.";

            await ApplyHack(processName, search1, replace1, successMessage, failureMessage, injectingMessage);

        }
        public Mem MemLib = new Mem();

        private async Task ApplyHack(string processName, string searchPattern, string replacePattern, string successMessage, string failureMessage, string injectingMessage)
        {
            if (Process.GetProcessesByName(processName).Length == 0)
            {
                label12.Text = "Emulador não encontrado.";
                return;
            }

            MemLib.OpenProcess(processName);
            label12.Text = injectingMessage;

            IEnumerable<long> addresses = await MemLib.AoBScan(searchPattern, writable: true);

            if (addresses.Any())
            {
                foreach (long address in addresses)
                {
                    MemLib.WriteMemory(address.ToString("X"), "bytes", replacePattern);
                }

                label12.Text = successMessage;

            }
            else
            {
                label12.Text = failureMessage;
            }
        }

        private async void guna2ToggleSwitch3_CheckedChanged_1(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch3.Checked)
            {
                try
                {
                    // Atualiza o texto da label antes de verificar
                    label12.Text = "Procurando processo...";

                    // Busca o processo "HD-Player"
                    Process hdPlayer = Process.GetProcessesByName("HD-Player").FirstOrDefault();
                    if (hdPlayer == null)
                    {
                        label12.Text = "HD-Player não encontrado.";
                        return;
                    }

                    // Abre o processo para manipulação
                    memory.OpenProcess(hdPlayer.Id);
                    label12.Text = "Aplicando...";

                    // Realiza a busca de padrões
                    var result = await memory.AoBScan("FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 A5 43 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? ?? ?? ?? ?? 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 BF", true, true);

                    if (result.Any())
                    {
                        foreach (var currentAddress in result)
                        {
                            Int64 readAddress = currentAddress + 0x70;
                            Int64 writeAddress = currentAddress + 0x6C;

                            int readValue = memory.ReadInt(readAddress.ToString("X"));
                            memory.WriteMemory(writeAddress.ToString("X"), "int", readValue.ToString());
                        }
                        label12.Text = "Aplicado!";
                        Console.Beep(500, 500); // Emite um som para indicar sucesso
                    }
                    else
                    {
                        label12.Text = "Padrão não encontrado!";
                    }
                }
                catch (Exception ex)
                {
                    label12.Text = $"Erro: {ex.Message}";
                }
            }
        }

        private async void guna2ToggleSwitch7_CheckedChanged(object sender, EventArgs e)
        {
            string search1 = "5c 43 00 00 90 42 00 00 b4 42 96 00 00 00 00 00 00 00 00 00 00 3f 00 00 80 3e";
            string replace1 = "5c 43 00 00 90 42 00 00 b4 42 96 00 00 00 00 00 00 00 00 00 00 cf 00 00 00 1c";
            string processName = "HD-Player";
            string injectingMessage = "Troca Rapida Injetando...";
            string successMessage = "Troca Rapida Injetado!";
            string failureMessage = "Falha ao aplicar.";

            await ApplyHack(processName, search1, replace1, successMessage, failureMessage, injectingMessage);

        }

        private void guna2PictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void guna2ToggleSwitch8_CheckedChanged(object sender, EventArgs e)
        {
            Config.Notif();
            if (guna2ToggleSwitch8.Checked)
            {
                guna2ToggleSwitch9.Checked = false;
                guna2ToggleSwitch10.Checked = false;
                Config.AimBot = true;
                Config.Aimfov = 100;
            }
            else
            {
                Config.AimBot = false;
            }
        }

        private void guna2ToggleSwitch9_CheckedChanged(object sender, EventArgs e)
        {
            Config.Notif();
            if (guna2ToggleSwitch9.Checked)

            {
            }

        }

        private void guna2ToggleSwitch10_CheckedChanged(object sender, EventArgs e)
        {
            Config.Notif();
            if (guna2ToggleSwitch10.Checked)
            {
                guna2ToggleSwitch8.Checked = false;
                guna2ToggleSwitch9.Checked = false;
                Config.AimBot = true;
                Config.aimlegit = -0.02f;
                Config.Aimbotype = 150;
                //Config.aimdelay = -001;
                Config.Aimfov = 4;
            }
            else
            {
                Config.AimBot = false;
            }
        }

        private void guna2PictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox5_Click_1(object sender, EventArgs e)
        {

        }
    }
}
