using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using RoDuino.SMS.Bll.Attributes;

namespace RoDuino.SMS.Bll.Util
{
    public class TextureUtil
    {
        public static int folded;
        public static Hashtable textureCache = new Hashtable();

//        public static object RemoveTexture(object model)
//        {
//            Model3DGroup m = (Model3DGroup)model;
//            foreach (GeometryModel3D coll in m.Children)
//            {
//                if (coll.Material is MaterialGroup)
//                {
//                    MaterialGroup group = (MaterialGroup)coll.Material;
//                    if (group.Children.Count > 1) group.Children.RemoveAt(1);
//                }
//            }
//            return m;
//        }

//        public static object SetResolution(object model, bool isHighResolution)
//        {
//            Model3DGroup m = (Model3DGroup)model;
//            foreach (GeometryModel3D coll in m.Children)
//            {
//                if (coll.Material is MaterialGroup)
//                {
//                    MaterialGroup group = (MaterialGroup)coll.Material;
//                    foreach (Material mat in group.Children)
//                    {
//                        DiffuseMaterial material = (DiffuseMaterial)mat;
//                        if (material.Brush is ImageBrush)
//                        {
//                            string path = ((ImageBrush)(material.Brush)).ImageSource.ToString().Replace("\\", "/");
//                            if (path.IndexOf("pack://siteoforigin") != -1)
//                            {
//                                string filename = path.Substring(path.LastIndexOf("/") + 1);
//                                path = path.Replace(filename, "");
//                                if (filename.ToLower().Contains("folded")) folded++;
//
                                //                                string imagePath = "/" + path + (isHighResolution ? "/" : "/Thumbnails/") + name + ".png";
                                //                                Uri uri = new Uri(path + (isHighResolution ? "" : "Thumbnails/") + filename);
//                                Uri uri = new Uri(path + filename);
//                                try
//                                {
//                                    material.Brush = GetCacheTexture(uri);
                                    //bmpImage.EndInit();
//                                }
//                                catch (Exception e)
//                                {
//                                    VRLog.Instance.WriteToLog(e.ToString(), TracedAttribute.ERROR);
//                                    Console.WriteLine("image is missing for file={0} (requested path:{1})", filename,
//                                                      uri.AbsolutePath);
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            return m;
//        }

        public static byte[] CreateResizedImage(byte[] imageData, int decodePixelWidth, int decodePixelHeight)
        {
            if (imageData == null) return null;
            BitmapImage image = ConvertByteToImageAndResize(imageData, decodePixelWidth, decodePixelHeight);
            return ConvertImageToByte(image);
        }
        public static byte[] CreateResizedImage(byte[] imageData, double widthPerHeightRatio, int decodePixelWidth)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();

            if (widthPerHeightRatio >= 1)
            {
                image.DecodePixelWidth = decodePixelWidth;
            }
            else
            {
                image.DecodePixelHeight = decodePixelWidth;
            }

            image.StreamSource = new MemoryStream(imageData);
            image.CreateOptions = BitmapCreateOptions.None;
            image.CacheOption = BitmapCacheOption.Default;
            image.EndInit();
            byte[] x = ConvertImageToByte(image);
            image.StreamSource = null;
            image = null;
            return x;
        }
        public static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            // Save to memory using the Jpeg format
            bitmap.Save(ms, ImageFormat.Png);
            // read to end
            bitmap.Dispose();
            ms.Flush();
            BitmapImage bImage = new BitmapImage();
            bImage.BeginInit();
            bImage.StreamSource = ms;
            bImage.EndInit();
            return bImage;
        }
        public static BitmapImage ConvertByteToImageAndResize(byte[] imageData)
        {
            return ConvertByteToImageAndResize(imageData, 0, 0);
        }
        public static BitmapImage ConvertByteToImageAndResize(byte[] imageData, int decodePixelWidth,
                                                              int decodePixelHeight)
        {
            MemoryStream ms = new MemoryStream(imageData);
            ms.Seek(0, SeekOrigin.Begin);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            if (decodePixelWidth > 0)
            {
                image.DecodePixelWidth = decodePixelWidth;
            }
            if (decodePixelHeight > 0)
            {
                image.DecodePixelHeight = decodePixelHeight;
            }
            image.StreamSource = ms;
            image.CreateOptions = BitmapCreateOptions.None;
            image.CacheOption = BitmapCacheOption.Default;
            image.EndInit();
            return image;
        }
//        public static BitmapImage LoadBitmapImageAsMemoryStream(string path)
//        {
//            return LoadBitmapImageAsMemoryStream(path, false);
//        }

//        public static BitmapImage LoadBitmapImageAsMemoryStream(string path, bool isMin)
//        {
//
//            path = isMin ? path.Replace(TextureCache.TEXTURES_PATH_PREFIX, TextureCache.TEXTURES_PATH_PREFIX_MIN) : path;
//            FileStream fileStream = File.OpenRead(path);
//            MemoryStream memoryStream = new MemoryStream();
//
//            memoryStream.SetLength(fileStream.Length);
//            try
//            {
//                fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);
//            }
//            catch (Exception ex)
//            {
//                RoLog.Instance.WriteToLog(ex.ToString(), TracedAttribute.ERROR);
//            }
//            finally
//            {
//                memoryStream.Flush();
//                fileStream.Close();
//            }
//
//            BitmapImage bImage = new BitmapImage();
//            bImage.BeginInit();
//            bImage.StreamSource = memoryStream;
//            bImage.EndInit();
//            return bImage;
//        }

        public static BitmapImage LoadBitmapImageWithoutMemoryStream(string path)
        {
            //            FileStream fileStream = File.OpenRead(path);
            //            MemoryStream memoryStream = new MemoryStream();
            //
            //            memoryStream.SetLength(fileStream.Length);
            //            try
            //            {
            //                fileStream.Read(memoryStream.GetBuffer(), 0, (int) fileStream.Length);
            //            }
            //            catch (Exception ex)
            //            {
            //                VRLog.Instance.WriteToLog(ex.ToString(), TracedAttribute.ERROR);
            //            }
            //            finally
            //            {
            //                memoryStream.Flush();
            //                fileStream.Close();
            //            }
            //
            //            BitmapImage bImage = new BitmapImage();
            //            bImage.BeginInit();
            //            bImage.StreamSource = memoryStream;
            //            bImage.EndInit();
            BitmapImage bImage = new BitmapImage(new Uri(path, UriKind.Relative));
            return bImage;
        }
        [Traced(TracedAttribute.INFO)]
        public static byte[] ConvertImageToByte(BitmapSource image)
        {
            MemoryStream stream = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            byte[] result = new byte[stream.Length];
            BinaryReader br = new BinaryReader(stream);
            br.Read(result, 0, (int)stream.Length);
            br.Close();
            stream.Close();
            stream = null;


            return result;
        }


        /// <summary>
        /// Returns the image of the visual
        /// </summary>       
        /// <returns></returns>
        [Traced(TracedAttribute.INFO)]
        public static byte[] TakeScreenShot(Visual visual, int width, int height)
        {
            if (visual != null)
            {
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(width,
                                                                         height, 96, 96,
                                                                         PixelFormats.Default);
                renderBitmap.Render(visual);
                MemoryStream stream = new MemoryStream();
                PngBitmapEncoder encoder1 = new PngBitmapEncoder();
                encoder1.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder1.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                byte[] result = new byte[stream.Length];
                BinaryReader br = new BinaryReader(stream);
                br.Read(result, 0, (int)stream.Length);
                br.Close();
                stream.Close();
                //                ConvertByteToPNG(ConvertByteToImageAndResize(result), "d:\\page"+DateTime.Now.Ticks+".png");// test save to a file
                return result;
            }
            return null;
        }
        [Traced(TracedAttribute.INFO)]
        private static ImageBrush GetCacheTexture(Uri uri)
        {
            if (textureCache[uri] == null)
            {
                BitmapImage bmpImage = new BitmapImage(uri);
                //bmpImage.BeginInit();
                //bmpImage.CacheOption = BitmapCacheOption.None;
                //bmpImage.CreateOptions = BitmapCreateOptions.None;
                textureCache[uri] = new ImageBrush(bmpImage);
            }
            ImageBrush newImgBrush = textureCache[uri] as ImageBrush;
            return newImgBrush;
        }
        [Traced(TracedAttribute.INFO)]
        public static byte[] ReadFile(string file)
        {
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Read);
            byte[] myData = null;
            try
            {
                myData = new byte[fs.Length];
                fs.Read(myData, 0, Convert.ToInt32(fs.Length));
            }
            catch (Exception e)
            {
                RoLog.Instance.WriteToLog(e.ToString(), TracedAttribute.ERROR);
            }
            finally
            {
                fs.Close();
            }
            return myData;
        }


        [Traced(TracedAttribute.DEBUG, "ErrorCroppingImage")]
        public static byte[] CropImageFile(byte[] imageFile, int targetW, int targetH, int targetX, int targetY)
        {
            //               Image imgPhoto = Image.FromStream(new MemoryStream(imageFile));
            //               Bitmap bmPhoto = new Bitmap(targetW, targetH, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //               bmPhoto.SetResolution(72, 72);
            //               Graphics grPhoto = Graphics.FromImage(bmPhoto);
            //               grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            //               grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //               grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //               grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, targetW, targetH), targetX, targetY, targetW, targetH, GraphicsUnit.Pixel);
            //               // Save out to memory and then to a file.  We dispose of all objects to make sure the files don't stay locked.
            //               MemoryStream mm = new MemoryStream();
            //               bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
            //               imgPhoto.Dispose();
            //               bmPhoto.Dispose();
            //               grPhoto.Dispose();
            //               return mm.GetBuffer();

            BitmapImage bmImage = new BitmapImage();
            bmImage.BeginInit();
            bmImage.StreamSource = new MemoryStream(imageFile);
            bmImage.CreateOptions = BitmapCreateOptions.None;
            bmImage.CacheOption = BitmapCacheOption.Default;
            bmImage.EndInit();

            targetX = targetX < 0 ? 0 : targetX;
            targetY = targetY < 0 ? 0 : targetY;
            targetW = targetW + targetX > bmImage.PixelWidth ? bmImage.PixelWidth - targetX : targetW;
            targetH = targetH + targetY > bmImage.PixelHeight ? bmImage.PixelHeight - targetY : targetH;

            CroppedBitmap cropedImage = new CroppedBitmap(bmImage, new Int32Rect(targetX, targetY, targetW, targetH));
            MemoryStream stream = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(cropedImage));
            encoder.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            byte[] result = new byte[stream.Length];
            BinaryReader br = new BinaryReader(stream);
            br.Read(result, 0, (int)stream.Length);
            br.Close();
            stream.Close();
            return result;
        }

//        [Traced(TracedAttribute.INFO)]
//        public static Material GetMaterial(string textureURI)
//        {
//            return GetMaterial(textureURI, false);
//        }
//
        /// <summary>
        /// Load material from file
        /// </summary>
        /// <param name="textureURI"></param>
        /// <param name="setUri"></param>
        /// <returns></returns>
//        public static Material GetMaterial(string textureURI, bool setUri)
//        {
//            DiffuseMaterial diffuseMaterial;
//            if (textureURI.ToLower() == "transparent")
//            {
//                SolidColorBrush brush = new SolidColorBrush(Colors.Gray);
//                brush.Opacity = 0.6;
//                diffuseMaterial = new DiffuseMaterial(brush);
//            }
//            else
//            {
//                DateTime now = DateTime.Now;
//                string path = Directory.GetCurrentDirectory() + "/";
//                FileStream fileStream = null;
//                VRMemoryStream memoryStream = null;
//
//                try
//                {
//                    fileStream = File.OpenRead(path + textureURI);
//                    memoryStream = new VRMemoryStream { Path = textureURI };
//                    memoryStream.SetLength(fileStream.Length);
//                    fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);
//                }
//                catch (Exception e)
//                {
//                    VRLog.Instance.WriteToLog(e.ToString(), TracedAttribute.ERROR);
//                }
//
//                finally
//                {
//                    memoryStream.Flush();
//                    fileStream.Close();
//                }
//
//                BitmapImage bImage = new BitmapImage();
//                bImage.BeginInit();
//                Uri uri = new Uri(textureURI, UriKind.Relative);
//                if (setUri)
//                    bImage.UriSource = uri; // "pack://siteoforigin:,,,/Content");
//                bImage.StreamSource = memoryStream;
//                bImage.EndInit();
//
//                ImageBrush image = new ImageBrush(bImage);
//                image.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
//                diffuseMaterial = new DiffuseMaterial(image);
//
                //                BitmapImage bImage = new BitmapImage();
                //                bImage.BeginInit();
                //                bImage.CacheOption = BitmapCacheOption.OnLoad;
                //                bImage.UriSource = new Uri(@textureURI,UriKind.Relative);
                //                bImage.EndInit();
                //
                //                ImageBrush image = new ImageBrush(bImage);
                //                image.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
                //                diffuseMaterial = new DiffuseMaterial(image);
//
//                Console.WriteLine("loaded {0} in {1} ms", textureURI, DateTime.Now.Subtract(now).TotalMilliseconds);
//            }
//            return diffuseMaterial;
//        }

        /// <summary>
        /// convert to JPEG image
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="path"></param>
        [Traced(TracedAttribute.INFO)]
        public static void ConvertByteToJPEG(BitmapSource imageData, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageData));
            encoder.Save(stream);
            encoder = null;
            stream.Close();
        }

        [Traced(TracedAttribute.INFO)]
        public static void ConvertByteToTIF(BitmapSource imageData, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            TiffBitmapEncoder encoder = new TiffBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageData));
            encoder.Save(stream);
            encoder = null;
            stream.Close();
        }
        [Traced(TracedAttribute.INFO)]
        public static void ConvertByteToBMP(BitmapSource imageData, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageData));
            encoder.Save(stream);
            encoder = null;
            stream.Close();
        }
        [Traced(TracedAttribute.INFO)]
        public static void ConvertByteToGIF(BitmapSource imageData, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            GifBitmapEncoder encoder = new GifBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageData));
            encoder.Save(stream);
            encoder = null;
            stream.Close();
        }
        [Traced(TracedAttribute.INFO)]
        public static void ConvertByteToPNG(BitmapSource imageData, string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageData));
            encoder.Save(stream);
            encoder = null;
            stream.Close();
        }


    }
}
