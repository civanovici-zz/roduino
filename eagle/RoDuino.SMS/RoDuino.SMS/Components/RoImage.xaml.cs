using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Bll.Util;

namespace RoDuino.SMS.Components
{
    /// <summary>
    /// class used to load image from memorystream to avoid memory leak
    /// </summary>
    public partial class RoImage
    {
        public RoImage()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register("ImagePath",
                                                                                                  typeof(string),
                                                                                                  typeof(RoImage));

        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        public new ImageSource Source
        {
            get { return GetImageFromImagePath(); }
            set { base.Source = value; }
        }

        private ImageSource GetImageFromImagePath()
        {
            string file = Directory.GetCurrentDirectory() + "\\" + ImagePath;
            FileStream fs = null;
            BitmapImage image = null;
            try
            {
                if (File.Exists(file))
                {
                    //remove Debug from path
                    file = file.Replace("Debug", "");

                    fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    byte[] imageBytes = br.ReadBytes((int)fs.Length);
                    br.Close();
                    image = TextureUtil.ConvertByteToImageAndResize(imageBytes, 0, 0);
                }
            }
            catch (Exception ex)
            {
                RoLog.Instance.WriteToLog(ex.Message, TracedAttribute.ERROR);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return image;
        }
    }
}
