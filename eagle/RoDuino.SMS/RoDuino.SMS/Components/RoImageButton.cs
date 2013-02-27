using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Bll.Util;

namespace RoDuino.SMS.Components
{
    public class RoImageButton : Button
    {
        public static DependencyProperty DownProperty = DependencyProperty.Register("Down", typeof(ImageSource),
                                                                                    typeof(RoImageButton));

        public static DependencyProperty HoverProperty = DependencyProperty.Register("Hover", typeof(ImageSource),
                                                                                     typeof(RoImageButton));

        public static DependencyProperty UpProperty = DependencyProperty.Register("Up", typeof(ImageSource),
                                                                                  typeof(RoImageButton));

        public static DependencyProperty InactiveProperty = DependencyProperty.Register("Inactive", typeof(ImageSource),
                                                                                  typeof(RoImageButton));

        private bool isGenericContent;

        // Methods
        public RoImageButton()
        {
            try
            {
                base.Style = base.FindResource("ImageButton") as Style;
            }
            catch (System.Windows.ResourceReferenceKeyNotFoundException ex)
            {
                RoLog.Instance.WriteToLog(ex.Message + @"\n" + ex, TracedAttribute.ERROR);
            }
        }

        // Properties

        private string upPath;
        private string hoverPath;
        private string downPath;
        private string inactivePath;

        public string UpPath
        {
            set
            {
                upPath = value;
                base.SetValue(UpProperty, ReadResourceImage(upPath));
            }
            get { return upPath; }
        }

        public string HoverPath
        {
            set
            {
                hoverPath = value;
                base.SetValue(HoverProperty, ReadResourceImage(hoverPath));
            }
            get { return hoverPath; }
        }

        public string DownPath
        {
            set
            {
                downPath = value;
                base.SetValue(DownProperty, ReadResourceImage(downPath));
            }
            get { return downPath; }
        }

        public string InactivePath
        {
            set
            {
                inactivePath = value;
                base.SetValue(InactiveProperty, ReadResourceImage(inactivePath));
            }
            get { return inactivePath; }
        }





        public ImageSource Up
        {
            get { return (base.GetValue(UpProperty) as ImageSource); }
            set { base.SetValue(UpProperty, value); }
        }

        public ImageSource Hover
        {
            get { return (base.GetValue(HoverProperty) as ImageSource); }
            set { base.SetValue(HoverProperty, value); }
        }

        public ImageSource Down
        {
            get { return (base.GetValue(DownProperty) as ImageSource); }
            set { base.SetValue(DownProperty, value); }
        }

        public ImageSource Inactive
        {
            get { return (base.GetValue(InactiveProperty) as ImageSource); }
            set { base.SetValue(InactiveProperty, value); }
        }

        public bool IsGenericContent
        {
            get { return this.isGenericContent; }
            set
            {
                this.isGenericContent = value;
                if (this.isGenericContent)
                {
                    base.Style = base.FindResource("ImageButtonWithContent") as Style;
                }
            }
        }

        public ImageSource ReadResourceImage(string path)
        {
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                Stream iconStream = asm.GetManifestResourceStream(asm.GetName().Name + "." + path.Replace(@"\", "."));
                PngBitmapDecoder iconDecoder = new PngBitmapDecoder(iconStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                return iconDecoder.Frames[0];
            }
            catch (Exception e)
            {
                RoLog.Instance.WriteToLog(e.Message + @"\n" + e, TracedAttribute.ERROR);
                Console.WriteLine(e);
            }
            return null;
        }


    }
}
