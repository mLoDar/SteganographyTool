using SteganographyTool.Scripts;
using System.Text;





namespace SteganographyTool
{
    internal class Program
    {
        static int selectedOption = 1;





        static void Main()
        {
        LabelMethodEntry:



            Console.Title = "Steganography Tool";
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            Console.Clear();



        LabelDrawUi:
            Console.SetCursorPosition(0, 4);

            DrawUi();



        LabelReadKey:
            ConsoleKey pressedKey = Console.ReadKey(true).Key;

            switch (pressedKey)
            {
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;

                case ConsoleKey.DownArrow:
                    selectedOption = (selectedOption + 1 <= 3) ? selectedOption + 1 : selectedOption;
                    goto LabelDrawUi;

                case ConsoleKey.UpArrow:
                    selectedOption = (selectedOption - 1 >= 1) ? selectedOption - 1 : selectedOption;
                    goto LabelDrawUi;

                case ConsoleKey.Enter:
                    RedirectToOption();
                    break;

                default:
                    goto LabelReadKey;
            }

            goto LabelMethodEntry;
        }

        

        static void DrawUi()
        {
            string[] options =
            {
                "Hide information",
                "Read information",
                "Show documentation",
            };



            Console.WriteLine("             ┏┓                   ┓    ┏┳┓    ┓    ");
            Console.WriteLine("             ┗┓╋┏┓┏┓┏┓┏┓┏┓┏┓┏┓┏┓┏┓┣┓┓┏  ┃ ┏┓┏┓┃    ");
            Console.WriteLine("             ┗┛┗┗ ┗┫┗┻┛┗┗┛┗┫┛ ┗┻┣┛┛┗┗┫  ┻ ┗┛┗┛┗    ");
            Console.WriteLine("                   ┛       ┛    ┛    ┛             ");
            Console.WriteLine("                                                   ");
            Console.WriteLine("             {0}──────────────────────────────────────{1}", "\u001b[94m", "\u001b[97m");
            Console.WriteLine("                                                   ");
            Console.WriteLine("             {0} {1}                  ", selectedOption == 1 ? "[\x1B[94m>\x1B[97m]" : "[ ]", selectedOption == 1 ? $"{options[0].ToUpper()}" : $"{options[0]}");
            Console.WriteLine("             {0} {1}                  ", selectedOption == 2 ? "[\x1B[94m>\x1B[97m]" : "[ ]", selectedOption == 2 ? $"{options[1].ToUpper()}" : $"{options[1]}");
            Console.WriteLine("                                                   ");
            Console.WriteLine("             {0} {1}                  ", selectedOption == 3 ? "[\x1B[94m>\x1B[97m]" : "[ ]", selectedOption == 3 ? $"{options[2].ToUpper()}" : $"{options[2]}");
            Console.WriteLine("                                                   ");
            Console.WriteLine("                                                   ");
            Console.WriteLine("                                                   ");
            Console.WriteLine("             (Use the arrow keys and ENTER to navigate)");
        }

        static void RedirectToOption()
        {
            switch (selectedOption)
            {
                case 1:
                    HideInformation.Entry();
                    break;

                case 2:
                    ReadInformation.Entry();
                    break;

                case 3:
                    ShowDocumentation.Entry();
                    break;

                default:
                    return;
            }
        }
    }
}