using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace LaptopTimer
{
    internal class KeyboardHook
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            // Check if the hook code is HC_ACTION (i.e. a keyboard event has occurred)
            if (nCode == 0)
            {

                // Check if the key was pressed (i.e. wParam == WM_KEYDOWN)
                if (wParam == (IntPtr)0x0100)
                {
                    // Cast the lParam parameter to a KBDLLHOOKSTRUCT structure
                    KBDLLHOOKSTRUCT keyboardHookStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                    return 1;
                }
            }

            // Call the next hook in the chain
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

    }
}
