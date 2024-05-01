using System.Drawing;
using System.Text;

using SteganographyTool.Scripts;





namespace SteganographyTool.MenuOptions
{
    internal class ReadInformation
    {
        internal static void Entry()
        {
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
            UserIntefaceHelper.ClearLine();

            Console.SetCursorPosition(0, 10);



            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("             │ Enter the path to an image you want to read information from:");
            Console.WriteLine("             └──────");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("               > ");

            Console.ForegroundColor = ConsoleColor.White;
            string? imagePath = Console.ReadLine();

            if (FileHandler.ValidImagePath(imagePath) == false)
            {
                goto LabelReadImagePath;
            }

            ImageHandler.DrawImage(imagePath, new Point(80, 4));



            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();



            string? result = FileHandler.ReadTextFromImage(imagePath);

            if (result?.StartsWith("[SUCCESS]") == true)
            {
                result = result[9..];

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("             [{0}SUCCESS{1}]", "\u001b[92m", "\u001b[97m");
                Console.WriteLine("             Found message: {0}", result);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("             [{0}ERROR{1}] -> {2}", "\u001b[91m", "\u001b[97m", result);

            }



            Console.WriteLine("\r\n\r\n");

            Console.CursorVisible = false;



            for (int redirectSeconds = 10; redirectSeconds > 0; redirectSeconds--)
            {
                UserIntefaceHelper.ClearLine();

                switch (redirectSeconds)
                {
                    case 1:
                        Console.Write("             Redirecting in {0}{1}{2} second", "\u001b[94m", redirectSeconds, "\u001b[97m");
                        break;

                    default:
                        Console.Write("             Redirecting in {0}{1}{2} seconds", "\u001b[94m", redirectSeconds, "\u001b[97m");
                        break;
                }

                Thread.Sleep(1000);
            }
        }
        private static void DrawHeader()
        {
            Console.WriteLine("             ┳┓     ┓  ┳  ┏          •    ");
            Console.WriteLine("             ┣┫┏┓┏┓┏┫  ┃┏┓╋┏┓┏┓┏┳┓┏┓╋┓┏┓┏┓");
            Console.WriteLine("             ┛┗┗ ┗┻┗┻  ┻┛┗┛┗┛┛ ┛┗┗┗┻┗┗┗┛┛┗");
            Console.WriteLine("                                          ");
            Console.WriteLine("             {0}──────────────────────────────────────{1}", "\u001b[94m", "\u001b[97m");
            Console.WriteLine("                                                   ");
        }
    }
}