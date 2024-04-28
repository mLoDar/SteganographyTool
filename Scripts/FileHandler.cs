using System.Drawing;





namespace SteganographyTool.Scripts
{
    internal class FileHandler
    {
        internal static bool ValidImagePath(string? path)
        {
            if (path == null)
            {
                return false;
            }

            if (RegexPatterns.AllWhitespaces().Replace(path, string.Empty).Equals(string.Empty))
            {
                return false;
            }

            if (File.Exists(path) == false)
            {
                return false;
            }

            try
            {
#pragma warning disable CA1416 // Validate platform compatibility
                Image.FromFile(path).Dispose();
#pragma warning restore CA1416 // Validate platform compatibility

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}