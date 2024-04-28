using System.Text;





namespace SteganographyTool.Scripts
{
    internal class BinaryHelper
    {
        internal static string TextToBinary(string input)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            string binary = "";

            foreach (byte b in bytes)
            {
                binary += Convert.ToString(b, 2).PadLeft(8, '0');
            }

            return binary;
        }

        internal static string BinaryToText(string binary)
        {
            StringBuilder textBuilder = new();

            for (int i = 0; i < binary.Length; i += 8)
            {
                string binaryByte = binary.Substring(i, 8);

                int asciiValue = Convert.ToInt32(binaryByte, 2);
                char character = Convert.ToChar(asciiValue);

                textBuilder.Append(character);
            }

            return textBuilder.ToString();
        }
    }
}