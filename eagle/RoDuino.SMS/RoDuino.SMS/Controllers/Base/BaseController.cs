using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RoDuino.SMS.Bll.Util;
using RoDuino.SMS.Helpers;
using RoDuino.SMS.Properties;

namespace RoDuino.SMS.Controllers.Base
{
    public class BaseController:Controller
    {internal static Guid ticket = Guid.Empty;
//        internal static List<FontFamily> defaultFonts = new List<FontFamily>();
//        private static IList<NamedColor> defaultColors = new List<NamedColor>();

        static BaseController()
        {
            RoSession.Instance["menus"] = GenerateFakeMenus();

            
        }

        

        #region Generate Menu List

        public static GroupList GenerateFakeMenus()
        {
            List<ListItem> items = new List<ListItem>();

            string appFolder = Directory.GetCurrentDirectory() + "/";
            string appIcoFolder = "Content/ApplicationIcons/";
//            byte[] shapeImg = TextureUtil.ReadFile(appFolder + appIcoFolder + "shape.png");
//            byte[] formsImg = TextureUtil.ReadFile(appFolder + appIcoFolder + "forms.png");
//            byte[] rangeImg = TextureUtil.ReadFile(appFolder + appIcoFolder + "range.png");
//            byte[] storyboardImg = TextureUtil.ReadFile(appFolder + appIcoFolder + "storyboard.png");

//            items.Add(new ListItem2(8321, "Visual Shape", "Visual Shape", "Shape/ViewShape.xaml?id=New Shape",
//                                    appIcoFolder + "shape.png", shapeImg));


//            items.Add(
//                new ListItem2(7121, "Visual Range", "Visual Range",
//                              "Range/List.xaml",
//                              appIcoFolder + "range.png", rangeImg));
//
//            items.Add(
//                new ListItem2(639, "Form Editor", "Form Editor",
//                              "FormEditor/LoadForm.xaml?name=",
//                              appIcoFolder + "forms.png", formsImg));
//
//
//            items.Add(
//                new ListItem2(8121, "Tag Values", "Tag Values",
//                              "TagValues/List.xaml",
//                              appIcoFolder + "range.png", rangeImg));
//
//
//            items.Add(
//                new ListItem2(8123, "Visual Analysis", "Visual Analysis",
//                              "Analysis/Index.xaml",
//                              appIcoFolder + "shape.png", shapeImg));
//
//            items.Add(
//               new ListItem2(8125, "Storyboard", "Visual Storyboard",
//                             "Storyboard/Index.xaml",
//                             appIcoFolder + "Storyboard.png", storyboardImg));

//            items.Add(
//                new ListItem2(721, "Lookup Type", "Lookup Type",
//                  "LookupTypes/Index.xaml",
//                  appIcoFolder + "forms.png", formsImg));
//
//                items.Add(
//        new ListItem2(821, "Lookup", "Lookup",
//          "Lookups/Index.xaml",
//          appIcoFolder + "forms.png", formsImg));


            

            GroupList group = new GroupList();
            group.Group(items, "Group", "ParentGroup", null, null, null);

            return group;
        }

        /// <summary>
        ///  translate each key in list 
        /// </summary>
        protected List<string> TranslateErrorMesages(string[] errors)
        {
            List<string> translatedErrors = new List<string>();
            foreach (string s in errors)
            {
                //validation error messages for dynamic fields are containing | character
                // EX: testfield|IsMandatory
                if (s.IndexOf("|") == -1)
                {
                    translatedErrors.Add(Resources.ResourceManager.GetString(s));
                }
                else
                {
                    string[] tmp = s.Split('|');
                    translatedErrors.Add(String.Format(Resources.ResourceManager.GetString(tmp[1]), tmp[0]));
                }
            }
            return translatedErrors;
        }



        
        #endregion



    }

    public class ListItem2 : ListItem
    {
        private string parentGroup = "";

        public ListItem2(long id, string parentGroup, string name, string picture, string group, byte[] image)
            : base(id, name, picture, group)
        {
            this.Image = image;
            this.parentGroup = parentGroup;
        }

        public ListItem2(long id, string parentGroup, string name, string picture, string group)
            : this(id, parentGroup, name, picture, group, null)
        {
        }


        public string ParentGroup
        {
            get { return parentGroup; }
            set { parentGroup = value; }
        }
    }
}
