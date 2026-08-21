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
            public INPUTUNION data;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct INPUTUNION
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
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

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        internal const uint INPUT_MOUSE = 0;
        internal const uint INPUT_KEYBOARD = 1;
        internal const uint MOUSEEVENTF_MOVE = 0x0001;
        internal const uint KEYEVENTF_KEYUP = 0x0002;
        internal const ushort VK_F15 = 0x7E;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
    }

    internal enum PulseMode
    {
        F15,
        Mouse
    }

    internal sealed class TrayContext : ApplicationContext
    {
        private readonly NotifyIcon tray;
        private readonly ToolStripMenuItem statusItem;
        private readonly ToolStripMenuItem f15ModeItem;
        private readonly ToolStripMenuItem mouseModeItem;
        private readonly System.Windows.Forms.Timer uiTimer;
        private readonly System.Windows.Forms.Timer activityTimer;

        private bool active;
        private DateTime? endTime;
        private DateTime? lastActivityPulse;
        private PulseMode pulseMode = PulseMode.F15;
        private bool lastPulseUsedFallback;

        private const int ActivityPulseIntervalMs = 59000;

        internal TrayContext()
        {
            statusItem = new ToolStripMenuItem("Status: Desativado") { Enabled = false };

            var enableForever = new ToolStripMenuItem("Ativar indefinidamente", null, delegate { Enable(null); });
            var enable30 = new ToolStripMenuItem("30 minutos", null, delegate { Enable(TimeSpan.FromMinutes(30)); });
            var enable60 = new ToolStripMenuItem("1 hora", null, delegate { Enable(TimeSpan.FromHours(1)); });
            var enable120 = new ToolStripMenuItem("2 horas", null, delegate { Enable(TimeSpan.FromHours(2)); });

            f15ModeItem = new ToolStripMenuItem("Pulso F15 (recomendado)");
            f15ModeItem.CheckOnClick = true;
            f15ModeItem.Click += delegate { SetPulseMode(PulseMode.F15); };

            mouseModeItem = new ToolStripMenuItem("Pulso de mouse +1/-1 px");
            mouseModeItem.CheckOnClick = true;
            mouseModeItem.Click += delegate { SetPulseMode(PulseMode.Mouse); };

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
            menu.Items.Add(new ToolStripMenuItem("Modo de atividade") { Enabled = false });
            menu.Items.Add(f15ModeItem);
            menu.Items.Add(mouseModeItem);
            menu.Items.Add(new ToolStripMenuItem("Pulso a cada 59 segundos") { Enabled = false });
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(disable);
            menu.Items.Add(exit);

            tray = new NotifyIcon();
            tray.Icon = SystemIcons.Application;
            tray.Text = "RWXAwake64 v1.2 - Desativado";
            tray.ContextMenuStrip = menu;
            tray.Visible = true;
            tray.DoubleClick += delegate { if (active) Disable(true); else Enable(null); };

            uiTimer = new System.Windows.Forms.Timer();
            uiTimer.Interval = 1000;
            uiTimer.Tick += UiTimerTick;
            uiTimer.Start();

            activityTimer = new System.Windows.Forms.Timer();
            activityTimer.Interval = ActivityPulseIntervalMs;
            activityTimer.Tick += ActivityTimerTick;

            SetPulseMode(PulseMode.F15);
            Application.ApplicationExit += delegate { RestoreNormalPowerState(); };

            tray.ShowBalloonTip(
                1800,
                "RWXAwake64 v1.2",
                "Pronto. F15 é o modo padrão; mouse pulse está disponível como compatibilidade.",
                ToolTipIcon.Info
            );
        }

        private void SetPulseMode(PulseMode mode)
        {
            pulseMode = mode;
            f15ModeItem.Checked = mode == PulseMode.F15;
            mouseModeItem.Checked = mode == PulseMode.Mouse;
            UpdateStatus();
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
                tray.ShowBalloonTip(2500, "RWXAwake64 v1.2", "O Windows não aceitou a solicitação de energia.", ToolTipIcon.Error);
                return;
            }

            SendActivityPulse();
            activityTimer.Start();
            UpdateStatus();

            string msg = duration.HasValue
                ? "Ativo por " + FormatDuration(duration.Value) + ". Sistema, tela e atividade ligados."
                : "Ativo indefinidamente. Sistema, tela e atividade ligados.";

            tray.ShowBalloonTip(1800, "RWXAwake64 v1.2", msg, ToolTipIcon.Info);
        }

        private bool ApplyExecutionState()
        {
            var flags = NativeMethods.EXECUTION_STATE.ES_CONTINUOUS |
                        NativeMethods.EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                        NativeMethods.EXECUTION_STATE.ES_DISPLAY_REQUIRED;
            return NativeMethods.SetThreadExecutionState(flags) != 0;
        }

        private void SendActivityPulse()
        {
            lastPulseUsedFallback = false;
            bool ok = pulseMode == PulseMode.F15 ? SendF15Pulse() : SendMousePulse();

            if (!ok && pulseMode == PulseMode.F15)
            {
                ok = SendMousePulse();
                lastPulseUsedFallback = ok;
            }

            if (ok)
                lastActivityPulse = DateTime.Now;
        }

        private bool SendF15Pulse()
        {
            var inputs = new NativeMethods.INPUT[1];
            inputs[0].type = NativeMethods.INPUT_KEYBOARD;
            inputs[0].data.ki.wVk = NativeMethods.VK_F15;
            inputs[0].data.ki.wScan = 0;
            inputs[0].data.ki.dwFlags = NativeMethods.KEYEVENTF_KEYUP;
            inputs[0].data.ki.time = 0;
            inputs[0].data.ki.dwExtraInfo = UIntPtr.Zero;

            return NativeMethods.SendInput(1, inputs, Marshal.SizeOf(typeof(NativeMethods.INPUT))) == 1;
        }

        private bool SendMousePulse()
        {
            var inputs = new NativeMethods.INPUT[2];

            inputs[0].type = NativeMethods.INPUT_MOUSE;
            inputs[0].data.mi.dx = 1;
            inputs[0].data.mi.dy = 0;
            inputs[0].data.mi.dwFlags = NativeMethods.MOUSEEVENTF_MOVE;
            inputs[0].data.mi.dwExtraInfo = UIntPtr.Zero;

            inputs[1].type = NativeMethods.INPUT_MOUSE;
            inputs[1].data.mi.dx = -1;
            inputs[1].data.mi.dy = 0;
            inputs[1].data.mi.dwFlags = NativeMethods.MOUSEEVENTF_MOVE;
            inputs[1].data.mi.dwExtraInfo = UIntPtr.Zero;

            return NativeMethods.SendInput(2, inputs, Marshal.SizeOf(typeof(NativeMethods.INPUT))) == 2;
        }

        private void ActivityTimerTick(object sender, EventArgs e)
        {
            if (!active) return;
            ApplyExecutionState();
            SendActivityPulse();
            UpdateStatus();
        }

        private void Disable(bool notify)
        {
            active = false;
            endTime = null;
            lastActivityPulse = null;
            lastPulseUsedFallback = false;
            activityTimer.Stop();
            RestoreNormalPowerState();
            UpdateStatus();

            if (notify)
                tray.ShowBalloonTip(1400, "RWXAwake64 v1.2", "Desativado. O Windows voltou ao comportamento normal.", ToolTipIcon.Info);
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
                tray.ShowBalloonTip(1800, "RWXAwake64 v1.2", "Tempo concluído. O Windows voltou ao comportamento normal.", ToolTipIcon.Info);
                return;
            }

            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (!active)
            {
                statusItem.Text = "Status: Desativado";
                tray.Text = "RWXAwake64 v1.2 - Desativado";
                return;
            }

            string modeText = pulseMode == PulseMode.F15 ? "F15" : "Mouse";
            if (lastPulseUsedFallback) modeText += " -> Mouse fallback";

            if (endTime.HasValue)
            {
                TimeSpan remaining = endTime.Value - DateTime.Now;
                if (remaining < TimeSpan.Zero) remaining = TimeSpan.Zero;

                string shortTime = remaining.TotalHours >= 1
                    ? string.Format("{0}:{1:00}:{2:00}", (int)remaining.TotalHours, remaining.Minutes, remaining.Seconds)
                    : string.Format("{0}:{1:00}", (int)remaining.TotalMinutes, remaining.Seconds);

                statusItem.Text = "Status: Ativo - " + shortTime + " | " + modeText + " | 59s";
                tray.Text = "RWXAwake64 v1.2 - Ativo " + shortTime;
            }
            else
            {
                statusItem.Text = "Status: Ativo | " + modeText + " | 59s";
                tray.Text = "RWXAwake64 v1.2 - Ativo";
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
                MessageBox.Show("RWXAwake64 já está em execução na bandeja do sistema.", "RWXAwake64", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
