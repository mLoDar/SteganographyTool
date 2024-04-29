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
            
            Bitmap providedImage;
            Bitmap modifiedImage;

            try
            {
                providedImage = new(imagePath);
                modifiedImage = new(providedImage);
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



            providedImage.Dispose();
            modifiedImage.Dispose();



            return string.Empty;
        }

        internal static string? ReadTextFromImage(string? imagePath)
        {
            if (imagePath == null)
            {
                return "Invalid argument provided.";
            }

            if (ValidImagePath(imagePath) == false)
            {
                return "Invalid image path provided.";
            }



            Bitmap providedImage = new(imagePath);

            string binaryInfo = string.Empty;

            for (int x = 0; x < providedImage.Width; x++)
            {
                Color pixelColor = providedImage.GetPixel(x, 0);

                int blue = pixelColor.B;

                string binaryBlue = Convert.ToString(blue, 2).PadLeft(8, '0');
                binaryInfo += binaryBlue.Last();
            }

            string contentInfo = BinaryHelper.BinaryToText(binaryInfo);
            
            

            string contentLength = string.Empty;

            string contentLocationX = string.Empty;
            string contentLocationY = string.Empty;

            try
            {
                string contentLocation = contentInfo.Split('|')[1];
                contentLength = contentInfo.Split('|')[2];

                contentLocationX = contentLocation.Split(';')[0];
                contentLocationY = contentLocation.Split(';')[1];
            }
            catch
            {
                return "Invalid content info within image. Failed to split info.";
            }



            if (int.TryParse(contentLength, out int lenghtCounter) == false)
            {
                return "Invalid content info within image. Content length is not a number.";
            }

            if (int.TryParse(contentLocationX, out int currentPositionX) == false)
            {
                return "Invalid content info within image. Content x-location is not a number.";
            }

            if (int.TryParse(contentLocationY, out int currentPositionY) == false)
            {
                return "Invalid content info within image. Content y-location is not a number.";
            }



            string contentInBinary = string.Empty;



            if (currentPositionY <= 0)
            {
                currentPositionY = 1;
            }

            for (int i = lenghtCounter; i > 0; i--)
            {
                if (currentPositionX >= providedImage.Width)
                {
                    currentPositionX = 0;
                    currentPositionY++;
                }

                Color currentPixelColor = providedImage.GetPixel(currentPositionX, currentPositionY);

                contentInBinary += Convert.ToString(currentPixelColor.B, 2).PadLeft(8, '0').Last();

                currentPositionX++;
            }



            providedImage.Dispose();

            string foundContent = BinaryHelper.BinaryToText(contentInBinary);

            return $"[SUCCESS]{foundContent}";
        }
    }
}