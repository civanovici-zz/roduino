using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace RoDuino.SMS.Helpers
{
    public class GlassHelper
    {
        // Methods
        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern bool DwmIsCompositionEnabled();

        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern void DwmSetWindowAttribute(IntPtr hwnd, uint dwAttribute, IntPtr pvAttribute,
                                                         IntPtr cbAttribute);

        public static bool ExtendGlassFrame(Window window, Thickness margin)
        {
            if ((Environment.OSVersion.Version.Major >= 6))
            {
                if (!DwmIsCompositionEnabled())
                    return false;

                IntPtr hwnd = new WindowInteropHelper(window).Handle;
                if (hwnd == IntPtr.Zero)
                    throw new InvalidOperationException("The Window must be shown before extending glass.");

                // Set the background to transparent from both the WPF and Win32 perspectives
                window.Background = Brushes.Transparent;
                HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;

                MARGINS margins = new MARGINS(margin);
                DwmExtendFrameIntoClientArea(hwnd, ref margins);
            }
            return true;
        }

        #region Nested type: MARGINS

        private struct MARGINS
        {
            public int Bottom;
            public int Left;
            public int Right;
            public int Top;

            public MARGINS(Thickness t)
            {
                this.Left = (int)t.Left;
                this.Right = (int)t.Right;
                this.Top = (int)t.Top;
                this.Bottom = (int)t.Bottom;
            }
        }

        #endregion
    }
}
