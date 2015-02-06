using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace MessengerClient
{
    class FlashWindow
    {
        private const UInt32 FlashwStop = 0; //Stop flashing. The system restores the window to its original state.        private const UInt32 FLASHW_CAPTION = 1; //Flash the window caption.        
        private const UInt32 FlashwTray = 2; //Flash the taskbar button.        
        private const UInt32 FlashwAll = 3; //Flash both the window caption and taskbar button.        
        private const UInt32 FlashwTimer = 4; //Flash continuously, until the FLASHW_STOP flag is set.        
        private const UInt32 FlashwTimernofg = 12; //Flash continuously until the window comes to the foreground.  


        [StructLayout(LayoutKind.Sequential)]
        private struct Flashwinfo
        {
            public UInt32 cbSize; //The size of the structure in bytes.            
            public IntPtr hwnd; //A Handle to the Window to be Flashed. The window can be either opened or minimized.


            public UInt32 dwFlags; //The Flash Status.            
            public UInt32 uCount; // number of times to flash the window            
            public UInt32 dwTimeout; //The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.        
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref Flashwinfo pwfi);



        public static void Flash(Window win, UInt32 count = UInt32.MaxValue)
        {
            //Don't flash if the window is active            
            if (win.WindowState == WindowState.Maximized) return;
            var h = new WindowInteropHelper(win);
            var info = new Flashwinfo
            {
                hwnd = h.Handle,
                dwFlags = FlashwAll | FlashwTimernofg,
                uCount = count,
                dwTimeout = 0
            };

            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            FlashWindowEx(ref info);
        }

        public static void StopFlashingWindow(Window win)
        {
            var h = new WindowInteropHelper(win);
            var info = new Flashwinfo {hwnd = h.Handle};
            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            info.dwFlags = FlashwStop;
            info.uCount = UInt32.MaxValue;
            info.dwTimeout = 0;
            FlashWindowEx(ref info);
        }
    }
}
