using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace RWXAwake64
{
    internal static class NativeMethods
    {
        [Flags]
        internal enum EXECUTION_STATE : uint
        {
            ES_CONTINUOUS = 0x80000000,
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
    }

    internal sealed class TrayContext : ApplicationContext
    {
        private readonly NotifyIcon tray;
        private readonly ToolStripMenuItem statusItem;
        private readonly ToolStripMenuItem displayItem;
        private readonly System.Windows.Forms.Timer timer;
        private bool active;
        private bool keepDisplayOn;
        private DateTime? endTime;

        internal TrayContext()
        {
            statusItem = new ToolStripMenuItem("Status: Desativado");
            statusItem.Enabled = false;

            var enableForever = new ToolStripMenuItem("Ativar indefinidamente", null, delegate { Enable(null); });
            var enable30 = new ToolStripMenuItem("30 minutos", null, delegate { Enable(TimeSpan.FromMinutes(30)); });
            var enable60 = new ToolStripMenuItem("1 hora", null, delegate { Enable(TimeSpan.FromHours(1)); });
            var enable120 = new ToolStripMenuItem("2 horas", null, delegate { Enable(TimeSpan.FromHours(2)); });

            displayItem = new ToolStripMenuItem("Manter tela ligada");
            displayItem.CheckOnClick = true;
            displayItem.CheckedChanged += delegate
            {
                keepDisplayOn = displayItem.Checked;
                if (active) ApplyExecutionState();
                UpdateStatus();
            };

            var disable = new ToolStripMenuItem("Desativar", null, delegate { Disable(true); });
            var exit = new ToolStripMenuItem("Sair", null, delegate { ExitApp(); });

            var menu = new ContextMenuStrip();
            menu.Items.Add(statusItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(enableForever);
            menu.Items.Add(enable30);
            menu.Items.Add(enable60);
            menu.Items.Add(enable120);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(displayItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(disable);
            menu.Items.Add(exit);

            tray = new NotifyIcon();
            tray.Icon = SystemIcons.Application;
            tray.Text = "RWXAwake64 - Desativado";
            tray.ContextMenuStrip = menu;
            tray.Visible = true;
            tray.DoubleClick += delegate
            {
                if (active) Disable(true); else Enable(null);
            };

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += TimerTick;
            timer.Start();

            Application.ApplicationExit += delegate { RestoreNormalPowerState(); };
            tray.ShowBalloonTip(1800, "RWXAwake64", "Pronto. Clique com o botão direito no ícone para ativar.", ToolTipIcon.Info);
        }

        private void Enable(TimeSpan? duration)
        {
            active = true;
            endTime = duration.HasValue ? DateTime.Now.Add(duration.Value) : (DateTime?)null;
            if (!ApplyExecutionState())
            {
                active = false;
                endTime = null;
                UpdateStatus();
                tray.ShowBalloonTip(2500, "RWXAwake64", "O Windows não aceitou a solicitação para manter o sistema acordado.", ToolTipIcon.Error);
                return;
            }
            UpdateStatus();
            string msg = duration.HasValue ? "Ativo por " + FormatDuration(duration.Value) + "." : "Ativo indefinidamente.";
            tray.ShowBalloonTip(1600, "RWXAwake64", msg, ToolTipIcon.Info);
        }

        private bool ApplyExecutionState()
        {
            NativeMethods.EXECUTION_STATE flags = NativeMethods.EXECUTION_STATE.ES_CONTINUOUS | NativeMethods.EXECUTION_STATE.ES_SYSTEM_REQUIRED;
            if (keepDisplayOn) flags |= NativeMethods.EXECUTION_STATE.ES_DISPLAY_REQUIRED;
            return NativeMethods.SetThreadExecutionState(flags) != 0;
        }

        private void Disable(bool notify)
        {
            active = false;
            endTime = null;
            RestoreNormalPowerState();
            UpdateStatus();
            if (notify) tray.ShowBalloonTip(1400, "RWXAwake64", "Desativado.", ToolTipIcon.Info);
        }

        private void RestoreNormalPowerState()
        {
            NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (!active) return;
            if (endTime.HasValue && DateTime.Now >= endTime.Value)
            {
                Disable(false);
                tray.ShowBalloonTip(1800, "RWXAwake64", "Tempo concluído. O Windows voltou ao comportamento normal.", ToolTipIcon.Info);
                return;
            }
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (!active)
            {
                statusItem.Text = "Status: Desativado";
                tray.Text = "RWXAwake64 - Desativado";
                return;
            }

            if (endTime.HasValue)
            {
                TimeSpan remaining = endTime.Value - DateTime.Now;
                if (remaining < TimeSpan.Zero) remaining = TimeSpan.Zero;
                string shortTime = remaining.TotalHours >= 1
                    ? string.Format("{0}:{1:00}:{2:00}", (int)remaining.TotalHours, remaining.Minutes, remaining.Seconds)
                    : string.Format("{0}:{1:00}", (int)remaining.TotalMinutes, remaining.Seconds);
                statusItem.Text = "Status: Ativo - " + shortTime;
                tray.Text = "RWXAwake64 - Ativo " + shortTime;
            }
            else
            {
                statusItem.Text = "Status: Ativo";
                tray.Text = "RWXAwake64 - Ativo";
            }

            if (keepDisplayOn) statusItem.Text += " + tela";
        }

        private static string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalHours >= 1)
            {
                int hours = (int)duration.TotalHours;
                return hours == 1 ? "1 hora" : hours + " horas";
            }
            return ((int)duration.TotalMinutes) + " minutos";
        }

        private void ExitApp()
        {
            timer.Stop();
            RestoreNormalPowerState();
            tray.Visible = false;
            tray.Dispose();
            ExitThread();
        }
    }

    internal static class Program
    {
        private static Mutex mutex;

        [STAThread]
        private static void Main()
        {
            bool createdNew;
            mutex = new Mutex(true, @"Local\RWXAwake64_SingleInstance", out createdNew);
            if (!createdNew)
            {
                MessageBox.Show("RWXAwake64 já está em execução na bandeja do sistema.", "RWXAwake64", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var context = new TrayContext()) Application.Run(context);
            mutex.ReleaseMutex();
            mutex.Dispose();
        }
    }
}
