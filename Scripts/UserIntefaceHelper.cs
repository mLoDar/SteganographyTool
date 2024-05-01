using System.Text;
using System.Drawing;
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





        internal static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
            Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
        }

        internal static void DisableConsoleResizing()
        {
            _ = DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);
            _ = DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);
        }

        internal static string ReadLineImageLivePreview()
        {
            StringBuilder sb = new();

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        return string.Empty;

                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        return sb.ToString();

                    case ConsoleKey.Backspace:
                        if (sb.Length >= 1)
                        {
                            sb.Remove(sb.Length - 1, 1);
                            Console.Write("\b \b");
                        }
                        break;

                    default:
                        if (char.IsLetterOrDigit(keyInfo.KeyChar) || char.IsWhiteSpace(keyInfo.KeyChar) || char.IsSeparator(keyInfo.KeyChar) || char.IsPunctuation(keyInfo.KeyChar))
                        {
                            if (sb.Length < 4094)
                            {
                                sb.Append(keyInfo.KeyChar);
                                Console.Write(keyInfo.KeyChar);
                            }
                        }
                        break;
                }

                if (FileHandler.ValidImagePath(sb.ToString()) == true)
                {
                    ImageHandler.DrawImage(sb.ToString(), new Point(80, 4));
                }
            }
        }
    }
}