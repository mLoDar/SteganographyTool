using System.Drawing;



#pragma warning disable CA1416 // Validate platform compatibility





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
                Image.FromFile(path).Dispose();
            }
            catch
            {
                return false;
            }

            return true;
        }

        internal static string? WriteTextToImage(string? imagePath, string? contentToHide)
        {
            if (imagePath == null || contentToHide == null)
            {
                return "Invalid argument provided.";
            }

            if (ValidImagePath(imagePath) == false)
            {
                return "Invalid image path provided.";
            }

            if (RegexPatterns.AllWhitespaces().Replace(contentToHide, string.Empty).Equals(string.Empty) == true)
            {
                return "Invalid content to hide provided.";
            }



            Point contentLocation = new(0, 1);
            int contentBinaryLength = contentToHide.Length * 8;

            string formattedInfo = $"|{contentLocation.X};{contentLocation.Y}|{contentBinaryLength}|";



            string binaryInfo = BinaryHelper.TextToBinary(formattedInfo);
            string binaryContent = BinaryHelper.TextToBinary(contentToHide);
            
            Bitmap originalImage;
            Bitmap modifiedImage;

            try
            {
                originalImage = new(imagePath);
                modifiedImage = new(originalImage);
            }
            catch
            {
                return "Failed to load image from provided path.";
            }



            if (binaryInfo.Length > modifiedImage.Width)
            {
                return $"Provided image needs a width of at least {binaryInfo.Length} pixels.";
            }



            for (int i = 0; i < binaryInfo.Length; i++)
            {
                string bitToHide = binaryInfo[i].ToString();
                Color currentPixelColor = modifiedImage.GetPixel(i, 0);

                string blueValueInBinary = Convert.ToString(currentPixelColor.B, 2).PadLeft(8, '0'); ;
                blueValueInBinary = blueValueInBinary[..^1] + bitToHide;

                Color modifiedColor = Color.FromArgb(currentPixelColor.R, currentPixelColor.G, Convert.ToInt32(blueValueInBinary, 2));

                modifiedImage.SetPixel(i, 0, modifiedColor);
            }



            if (contentLocation.Y <= 0)
            {
                contentLocation.Y = 1;
            }

            int currentPositionX = contentLocation.X;
            int currentPositionY = contentLocation.Y;



            for (int i = 0; i < binaryContent.Length; i++)
            {
                string bitToHide = binaryContent[i].ToString();

                if (currentPositionX >= modifiedImage.Width)
                {
                    currentPositionX = 0;
                    currentPositionY++;
                }

                Color currentPixelColor = modifiedImage.GetPixel(currentPositionX, currentPositionY);


                string blueValueInBinary = Convert.ToString(currentPixelColor.B, 2).PadLeft(8, '0');
                blueValueInBinary = blueValueInBinary[..^1] + bitToHide;


                Color modifiedColor = Color.FromArgb(currentPixelColor.R, currentPixelColor.G, Convert.ToInt32(blueValueInBinary, 2));

                modifiedImage.SetPixel(currentPositionX, currentPositionY, modifiedColor);


                currentPositionX++;
            }


            string imageName = imagePath.Split(@"\").Last();
            string newImagePath = imagePath.Replace(imageName, $"modified-{imageName}");

            if (File.Exists(newImagePath))
            {
                try
                {
                    File.Delete(newImagePath);
                }
                catch
                {
                    return $"Failed to delete existing file '{newImagePath}'.";
                }
            }

            try
            {
                modifiedImage.Save(newImagePath);
            }
            catch
            {
                return $"Failed to save image to '{newImagePath}'.";
            }
            

            return string.Empty;
        }
    }
}