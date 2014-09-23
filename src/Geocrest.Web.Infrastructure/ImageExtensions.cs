namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="T:System.Drawing.Image"/> objects.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Saves the image to disk.
        /// </summary>
        /// <param name="image">The image to save.</param>
        /// <param name="targetSize">Target size of the image to save.</param>
        /// <param name="path">The path where the image will be saved.</param>
        /// <param name="oldName">The previous name of the image.</param>
        /// <returns></returns>
        public static string SaveImageToDisk(this Image image, imageSize targetSize, string path, string oldName)
        {
            Throw.IfArgumentNull(image, "image");
            Throw.IfArgumentNullOrEmpty(path, "path");
            Throw.IfArgumentNullOrEmpty(oldName, "oldName");
            string newName = GetNewImageName(path, oldName, targetSize);
            //get the codec needed
            var i = ImageCodecInfo.GetImageEncoders();
            var imgCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == image.RawFormat.Guid);
            if (imgCodec == null) imgCodec = ImageCodecInfo.GetImageEncoders().First(c => c.MimeType == "image/jpeg");
            //make a paramater to adjust quality
            var codecParams = new EncoderParameters(1);

            //reduce to quality of 80 (from range of 0 (max compression) to 100 (no compression))
            codecParams.Param[0] = new EncoderParameter(Encoder.Quality, 80L);
            image.Save(Path.Combine(path, newName), imgCodec, codecParams);
            return Path.Combine(path, newName);
        }
        /// <summary>
        /// Resizes the specified image.
        /// </summary>
        /// <param name="oldImage">The image to resize.</param>
        /// <param name="targetSize">Target size of the new image (in pixels).</param>
        /// <param name="crop">if set to <c>true</c> crop the image to .</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">oldImage</exception>
        /// <exception cref="T:System.ArgumentNullException">oldImage</exception>
        public static Image Resize(this Image oldImage, int targetSize, bool crop)
        {
            Throw.IfArgumentNull(oldImage, "oldImage");
            if (oldImage.PropertyIdList.Contains(0x0112))
            {
                int rotationValue = oldImage.GetPropertyItem(0x0112).Value[0];
                switch (rotationValue)
                {
                    case 1: // landscape, do nothing
                        break;
                    case 8: // rotated 90 right
                        // de-rotate:
                        oldImage.RotateFlip(rotateFlipType: System.Drawing.RotateFlipType.Rotate270FlipNone);                        
                        break;
                    case 3: // bottoms up
                        oldImage.RotateFlip(rotateFlipType: System.Drawing.RotateFlipType.Rotate180FlipNone);
                        break;
                    case 6: // rotated 90 left
                        oldImage.RotateFlip(rotateFlipType: System.Drawing.RotateFlipType.Rotate90FlipNone);
                        break;
                }
            }
            Size newSize = CalculateDimensions(oldImage.Size, targetSize, crop);
            using (Image newImage = new Bitmap(newSize.Width + 1, newSize.Height + 1,
                    oldImage.PixelFormat))
            {
                (newImage as Bitmap).SetResolution(oldImage.HorizontalResolution, oldImage.VerticalResolution);
                using (Graphics canvas = Graphics.FromImage(newImage))
                {
                    canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    canvas.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    canvas.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    canvas.DrawImage(oldImage, 0, 0, newSize.Width + 1, newSize.Height + 1);
                    var i = (newImage as System.Drawing.Bitmap)
                        .Clone(new Rectangle(1, 1, newSize.Width, newSize.Height), oldImage.PixelFormat);
                    if (!crop) return i;
                    return CropImage(i, targetSize);
                }
            }
        }
        /// <summary>
        /// Calculates x/y dimensions based on a target size. If the original size is a portrait,
        /// the target size will be applied to the height and the width will be proportionally resized.
        /// If the original size is in landscape, the target size will be applied to the width and the 
        /// height will be proportionally resized.
        /// </summary>
        /// <param name="oldSize">The original size.</param>
        /// <param name="targetSize">Target size.</param>
        /// <param name="crop">if set to <c>true</c> crop.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Drawing.Size">System.Drawing.Size</see>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">oldSize</exception>
        public static Size CalculateDimensions(Size oldSize, int targetSize, bool crop)
        {
            Throw.IfArgumentNull(oldSize, "oldSize");
            Size newSize = new Size();
            if (!crop)
            {
                if (oldSize.Height > oldSize.Width) // portrait
                {
                    newSize.Width = (int)(oldSize.Width * (Single)((int)targetSize / (Single)oldSize.Height));
                    newSize.Height = (int)targetSize;
                }
                else // landscape
                {
                    newSize.Width = (int)targetSize;
                    newSize.Height = (int)(oldSize.Height * (Single)((int)targetSize / (Single)oldSize.Width));
                }
            }
            else
            {
                if (oldSize.Height > oldSize.Width) // portrait
                {
                    newSize.Width = (int)targetSize;
                    newSize.Height = (int)(oldSize.Height * (Single)((int)targetSize / (Single)oldSize.Width));
                }
                else // landscape
                {
                    newSize.Height = (int)targetSize;
                    newSize.Width = (int)(oldSize.Width * (Single)((int)targetSize / (Single)oldSize.Height));
                }
            }
            return newSize;
        }
        /// <summary>
        /// Crops the image to fit the target size.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="targetSize">Size of the target.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Drawing.Image">Image</see>.
        /// </returns>
        public static Image CropImage(Image image, int targetSize)
        {
            using (Image newImage = new Bitmap((int)targetSize, (int)targetSize, 
                PixelFormat.Format24bppRgb))
            {
                (newImage as Bitmap).SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (Graphics canvas = Graphics.FromImage(newImage))
                {
                    canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    canvas.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    canvas.DrawImage(image, new Rectangle(0, 0, (int)targetSize, (int)targetSize),
                        new Rectangle(0, 0, (int)targetSize, (int)targetSize), GraphicsUnit.Pixel);
                    return newImage;
                }
            }
        }
        private static string GetNewImageName(string path, string oldname, imageSize size)
        {
            string newName = Path.GetFileNameWithoutExtension(path + oldname);
            FileInfo info = new FileInfo(path + oldname);
            switch (size)
            {
                case imageSize.Mini:
                    newName += "_mini" + info.Extension;
                    break;
                case imageSize.Thumbnail:
                    newName += "_tn" + info.Extension;
                    break;
                case imageSize.Small:
                    newName += "_sm" + info.Extension;
                    break;
                case imageSize.Medium:
                    newName += "_med" + info.Extension;
                    break;
                case imageSize.Large:
                    newName += "_large" + info.Extension;
                    break;
                default:
                    newName += info.Extension;
                    break;
            }
            return newName;
        }
    }
}
