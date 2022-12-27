using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace LaptopTimer
{
    class KeyboardHook
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hwnd);

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        private IntPtr g_hHook;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // Check if the hook code is HC_ACTION (i.e. a keyboard event has occurred)
            if (nCode == 0)
            {

                // Cast the lParam parameter to a KBDLLHOOKSTRUCT structure
                KBDLLHOOKSTRUCT keyboardHookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                switch (wParam)
                {
                    case 256:
                        return 1;

                    case 260:
                        return 1;

                    default:
                        break;
                }
            }

            // Call the next hook in the chain
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        public KeyboardHook()
        {

        }
        public IntPtr InstallHook()
        {
            g_hHook = SetWindowsHookEx(13, KeyboardHookProc, 0, 0);
            return g_hHook;
        }

        public bool UninstallHook()
        {
            return UnhookWindowsHookEx(g_hHook);
        }
    }
}
