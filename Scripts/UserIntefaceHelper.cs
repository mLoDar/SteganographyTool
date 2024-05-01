using System.Runtime.InteropServices;





namespace SteganographyTool.Scripts
{
    internal partial class UserIntefaceHelper
    {
        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;



        [LibraryImport("user32.dll")]
        public static partial int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [LibraryImport("user32.dll")]
        private static partial IntPtr GetSystemMenu(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bRevert);

        [LibraryImport("kernel32.dll")]
        private static partial IntPtr GetConsoleWindow();





        internal static void DisableConsoleResizing()
        {
            _ = DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            _ = DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
        }

        internal static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
            Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
        }
    }
}