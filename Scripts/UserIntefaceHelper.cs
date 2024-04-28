namespace SteganographyTool.Scripts
{
    internal class UserIntefaceHelper
    {
        internal static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
            Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
        }
    }
}