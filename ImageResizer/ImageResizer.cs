using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace WillOmae
{
    /// <summary>
    /// Image resizing class
    /// </summary>
    public class ImageResizer
    {
        /// <summary>
        /// Desired height
        /// </summary>
        private int boxHeight;
        /// <summary>
        /// Desired width
        /// </summary>
        private int boxWidth;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="width">Desired width</param>
        /// <param name="height">Desired height</param>
        public ImageResizer(int width, int height)
        {
            boxHeight = height;
            boxWidth = width;
        }
        /// <summary>
        /// Resize the image
        /// </summary>
        /// <param name="srcPath">Path to the source image</param>
        /// <param name="destPath">Path to the resized image</param>
        /// <returns>Is successful?</returns>
        public bool Resize(string srcPath, string destPath)
        {
            if (string.IsNullOrEmpty(srcPath))
            {
                throw new ArgumentNullException("srcPath", "String is null or empty");
            }
            if (string.IsNullOrEmpty(destPath))
            {
                throw new ArgumentNullException("destPath", "String is null or empty");
            }

            // Check for the existence of the source image
            if (File.Exists(srcPath))
            {
                // Create a new image from the source path
                using (Image srcImage = Image.FromFile(srcPath))
                {
                    double srcWidth = srcImage.Width;
                    double srcHeight = srcImage.Height;
                    double scaleFactor = 0.0f;
                    // Scale while maintaining the aspect ratio
                    if (srcWidth >= srcHeight)
                        scaleFactor = boxWidth / srcWidth;
                    else
                        scaleFactor = boxHeight / srcHeight;

                    int scaledWidth = (int)(srcWidth * scaleFactor);
                    int scaledHeight = (int)(srcHeight * scaleFactor);

                    Rectangle destRectangle = new Rectangle(0, 0, scaledWidth, scaledHeight);

                    // Define the working bitmap
                    using (Bitmap destImage = new Bitmap(scaledWidth, scaledHeight))
                    {
                        destImage.SetResolution(srcImage.HorizontalResolution, srcImage.VerticalResolution);

                        using (Graphics g = Graphics.FromImage(destImage))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.CompositingMode = CompositingMode.SourceCopy;

                            g.DrawImage(srcImage, destRectangle, 0, 0, (int)srcWidth, (int)srcHeight, GraphicsUnit.Pixel);

                            destImage.Save(destPath);

                            return true;
                        }
                    }
                }
            }
            // Revert changes
            if (File.Exists(destPath))
            {
                File.Delete(destPath);
            }
            return false;
        }
    }
}