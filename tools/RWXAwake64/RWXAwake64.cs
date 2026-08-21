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

        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public uint type;
            public MOUSEKEYBDHARDWAREINPUT data;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        internal const uint INPUT_MOUSE = 0;
        internal const uint MOUSEEVENTF_MOVE = 0x0001;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
    }

    internal sealed class TrayContext : ApplicationContext
    {
        private readonly NotifyIcon tray;
        private readonly ToolStripMenuItem statusItem;
        private readonly System.Windows.Forms.Timer uiTimer;
        private readonly System.Windows.Forms.Timer activityTimer;

        private bool active;
        private DateTime? endTime;
        private DateTime? lastActivityPulse;

        // Abaixo de timeouts comuns de lock (5, 10, 15 min) e sem gerar carga perceptivel.
        private const int ActivityPulseIntervalMs = 45000;

        internal TrayContext()
        {
            statusItem = new ToolStripMenuItem("Status: Desativado");
            statusItem.Enabled = false;

            var enableForever = new ToolStripMenuItem("Ativar indefinidamente", null, delegate { Enable(null); });
            var enable30 = new ToolStripMenuItem("30 minutos", null, delegate { Enable(TimeSpan.FromMinutes(30)); });
            var enable60 = new ToolStripMenuItem("1 hora", null, delegate { Enable(TimeSpan.FromHours(1)); });
            var enable120 = new ToolStripMenuItem("2 horas", null, delegate { Enable(TimeSpan.FromHours(2)); });
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
            menu.Items.Add(new ToolStripMenuItem("Modo ativo: sistema + tela + pulso de mouse") { Enabled = false });
            menu.Items.Add(new ToolStripMenuItem("Pulso a cada 45 segundos") { Enabled = false });
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(disable);
            menu.Items.Add(exit);

            tray = new NotifyIcon();
            tray.Icon = SystemIcons.Application;
            tray.Text = "RWXAwake64 v1.1 - Desativado";
            tray.ContextMenuStrip = menu;
            tray.Visible = true;
            tray.DoubleClick += delegate
            {
                if (active) Disable(true); else Enable(null);
            };

            uiTimer = new System.Windows.Forms.Timer();
            uiTimer.Interval = 1000;
            uiTimer.Tick += UiTimerTick;
            uiTimer.Start();

            activityTimer = new System.Windows.Forms.Timer();
            activityTimer.Interval = ActivityPulseIntervalMs;
            activityTimer.Tick += ActivityTimerTick;

            Application.ApplicationExit += delegate { RestoreNormalPowerState(); };
            tray.ShowBalloonTip(1800, "RWXAwake64 v1.1", "Pronto. Ative pelo menu para manter sistema, tela e sessão ativos.", ToolTipIcon.Info);
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
                tray.ShowBalloonTip(2500, "RWXAwake64 v1.1", "O Windows não aceitou a solicitação de energia.", ToolTipIcon.Error);
                return;
            }

            // Emite um pulso inicial e depois renova periodicamente.
            SendActivityPulse();
            activityTimer.Start();

            UpdateStatus();
            string msg = duration.HasValue
                ? "Ativo por " + FormatDuration(duration.Value) + ". Sistema, tela e pulso de atividade ligados."
                : "Ativo indefinidamente. Sistema, tela e pulso de atividade ligados.";
            tray.ShowBalloonTip(1800, "RWXAwake64 v1.1", msg, ToolTipIcon.Info);
        }

        private bool ApplyExecutionState()
        {
            NativeMethods.EXECUTION_STATE flags =
                NativeMethods.EXECUTION_STATE.ES_CONTINUOUS |
                NativeMethods.EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                NativeMethods.EXECUTION_STATE.ES_DISPLAY_REQUIRED;

            return NativeMethods.SetThreadExecutionState(flags) != 0;
        }

        private void SendActivityPulse()
        {
            // Movimento relativo de +1px e -1px. O cursor termina no mesmo ponto,
            // mas o Windows recebe input real via SendInput e renova o idle timer.
            var inputs = new NativeMethods.INPUT[2];

            inputs[0].type = NativeMethods.INPUT_MOUSE;
            inputs[0].data.mi.dx = 1;
            inputs[0].data.mi.dy = 0;
            inputs[0].data.mi.mouseData = 0;
            inputs[0].data.mi.dwFlags = NativeMethods.MOUSEEVENTF_MOVE;
            inputs[0].data.mi.time = 0;
            inputs[0].data.mi.dwExtraInfo = UIntPtr.Zero;

            inputs[1].type = NativeMethods.INPUT_MOUSE;
            inputs[1].data.mi.dx = -1;
            inputs[1].data.mi.dy = 0;
            inputs[1].data.mi.mouseData = 0;
            inputs[1].data.mi.dwFlags = NativeMethods.MOUSEEVENTF_MOVE;
            inputs[1].data.mi.time = 0;
            inputs[1].data.mi.dwExtraInfo = UIntPtr.Zero;

            uint sent = NativeMethods.SendInput(
                (uint)inputs.Length,
                inputs,
                Marshal.SizeOf(typeof(NativeMethods.INPUT))
            );

            if (sent == inputs.Length)
                lastActivityPulse = DateTime.Now;
        }

        private void ActivityTimerTick(object sender, EventArgs e)
        {
            if (!active) return;
            ApplyExecutionState();
            SendActivityPulse();
        }

        private void Disable(bool notify)
        {
            active = false;
            endTime = null;
            lastActivityPulse = null;
            activityTimer.Stop();
            RestoreNormalPowerState();
            UpdateStatus();

            if (notify)
                tray.ShowBalloonTip(1400, "RWXAwake64 v1.1", "Desativado. O Windows voltou ao comportamento normal.", ToolTipIcon.Info);
        }

        private void RestoreNormalPowerState()
        {
            NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
        }

        private void UiTimerTick(object sender, EventArgs e)
        {
            if (!active) return;

            if (endTime.HasValue && DateTime.Now >= endTime.Value)
            {
                Disable(false);
                tray.ShowBalloonTip(1800, "RWXAwake64 v1.1", "Tempo concluído. O Windows voltou ao comportamento normal.", ToolTipIcon.Info);
                return;
            }

            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (!active)
            {
                statusItem.Text = "Status: Desativado";
                tray.Text = "RWXAwake64 v1.1 - Desativado";
                return;
            }

            string suffix = " | tela + atividade";

            if (endTime.HasValue)
            {
                TimeSpan remaining = endTime.Value - DateTime.Now;
                if (remaining < TimeSpan.Zero) remaining = TimeSpan.Zero;

                string shortTime = remaining.TotalHours >= 1
                    ? string.Format("{0}:{1:00}:{2:00}", (int)remaining.TotalHours, remaining.Minutes, remaining.Seconds)
                    : string.Format("{0}:{1:00}", (int)remaining.TotalMinutes, remaining.Seconds);

                statusItem.Text = "Status: Ativo - " + shortTime + suffix;
                tray.Text = "RWXAwake64 v1.1 - Ativo " + shortTime;
            }
            else
            {
                statusItem.Text = "Status: Ativo" + suffix;
                tray.Text = "RWXAwake64 v1.1 - Ativo";
            }
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
            uiTimer.Stop();
            activityTimer.Stop();
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
                MessageBox.Show(
                    "RWXAwake64 já está em execução na bandeja do sistema.",
                    "RWXAwake64",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var context = new TrayContext())
                Application.Run(context);

            mutex.ReleaseMutex();
            mutex.Dispose();
        }
    }
}
