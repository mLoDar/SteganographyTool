﻿using System.Drawing;
using System.Runtime.InteropServices;



#pragma warning disable CA1416 // Validate platform compatibility
#pragma warning disable SYSLIB1054 // Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time





namespace SteganographyTool.Scripts
{
    internal partial class ImageHandler
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            int dwDesiredAccess,
            int dwShareMode,
            IntPtr lpSecurityAttributes,
            int dwCreationDisposition,
            int dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetCurrentConsoleFont(
            IntPtr hConsoleOutput,
            bool bMaximumWindow,
            [Out][MarshalAs(UnmanagedType.LPStruct)] ConsoleFontInfo lpConsoleCurrentFont);



        [StructLayout(LayoutKind.Sequential)]
        internal class ConsoleFontInfo
        {
            internal int nFont;
            internal Coord dwFontSize;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct Coord
        {

            [FieldOffset(0)]
            internal short X;
            [FieldOffset(2)]
            internal short Y;
        }



        private const int GENERIC_READ = unchecked((int)0x80000000);
        private const int GENERIC_WRITE = 0x40000000;
        private const int FILE_SHARE_READ = 1;
        private const int FILE_SHARE_WRITE = 2;
        private const int INVALID_HANDLE_VALUE = -1;
        private const int OPEN_EXISTING = 3;





        internal static void DrawImage(string? imagePath, Point imageLocation)
        {
            if (imagePath == null)
            {
                return;   
            }

            if (FileHandler.ValidImagePath(imagePath) == false)
            {
                return;
            }



            Size imageSize = new(14, 7);

            using Graphics g = Graphics.FromHwnd(GetConsoleWindow());
            using Image image = Image.FromFile(imagePath);

            Size fontSize = GetConsoleFontSize();

            Rectangle imageRect = new(
                imageLocation.X * fontSize.Width,
                imageLocation.Y * fontSize.Height,
                imageSize.Width * fontSize.Width,
                imageSize.Height * fontSize.Height);

            g.DrawImage(image, imageRect);
        }

        private static Size GetConsoleFontSize()
        {
            IntPtr outHandle = CreateFile("CONOUT$", GENERIC_READ | GENERIC_WRITE,
                FILE_SHARE_READ | FILE_SHARE_WRITE,
                IntPtr.Zero,
                OPEN_EXISTING,
                0,
                IntPtr.Zero);

            int errorCode = Marshal.GetLastWin32Error();

            if (outHandle.ToInt32() == INVALID_HANDLE_VALUE)
            {
                throw new IOException("Unable to open CONOUT$", errorCode);
            }

            ConsoleFontInfo cfi = new();

            if (!GetCurrentConsoleFont(outHandle, false, cfi))
            {
                throw new InvalidOperationException("Unable to get font information.");
            }

            return new Size(cfi.dwFontSize.X, cfi.dwFontSize.Y);
        }
    }
}