using System.Text;

using SteganographyTool.Scripts;







namespace SteganographyTool.MenuOptions
{
    internal class HideInformation
    {
        internal static void Entry()
        {
        LabelMethodEntry:



            Console.Title = "Steganography Tool";
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = true;

            Console.Clear();



        LabelReadImagePath:
            Console.SetCursorPosition(0, 4);

            Console.ForegroundColor = ConsoleColor.White;
            DrawHeader();

            UserIntefaceHelper.ClearLine();
            Console.WriteLine();
            UserIntefaceHelper.ClearLine();
            Console.WriteLine();

            Console.SetCursorPosition(0, 10);



            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("             Enter the path to an image you want to hide information in:");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("             > ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            string? imagePath = Console.ReadLine();
            
            if (FileHandler.ValidImagePath(imagePath) == false)
            {
                goto LabelReadImagePath;
            }



        LabelReadContent:
            UserIntefaceHelper.ClearLine();
            Console.WriteLine();
            UserIntefaceHelper.ClearLine();
            Console.WriteLine();

            Console.SetCursorPosition(0, 13);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("             Enter a text you want to hide:");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("             > ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            string? contentToHide = Console.ReadLine();

            if (ValidContent(contentToHide) == false)
            {
                goto LabelReadContent;
            }
        }

        private static void DrawHeader()
        {
            Console.WriteLine("             ┓┏• ┓    ┳  ┏          •    ");
            Console.WriteLine("             ┣┫┓┏┫┏┓  ┃┏┓╋┏┓┏┓┏┳┓┏┓╋┓┏┓┏┓");
            Console.WriteLine("             ┛┗┗┗┻┗   ┻┛┗┛┗┛┛ ┛┗┗┗┻┗┗┗┛┛┗");
            Console.WriteLine("                                         ");
            Console.WriteLine("             {0}──────────────────────────────────────{1}", "\u001b[94m", "\u001b[97m");
            Console.WriteLine("                                                   ");
        }

        private static bool ValidContent(string? content)
        {
            if (content == null)
            {
                return false;
            }

            if (RegexPatterns.AllWhitespaces().Replace(content, string.Empty).Equals(string.Empty))
            {
                return false;
            }

            return true;
        }
    }
}