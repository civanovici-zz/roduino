using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using RoDuino.SMS.Components;
using RoDuino.SMS.Controllers.Base;
using RoDuino.SMS.Exceptions;
using RoDuino.SMS.Helpers;


namespace RoDuino.SMS.Views
{
    public class View : ContentControl, IView
    {
        private static DataBindingUtil databindUtil = new DataBindingUtil();

        public static DependencyProperty DraggingElementProperty = DependencyProperty.RegisterAttached(
            "DraggingElement", typeof(object), typeof(Page), new FrameworkPropertyMetadata(null));

        public static DependencyProperty IsDownProperty = DependencyProperty.RegisterAttached(
            "IsDown", typeof(bool), typeof(Page), new FrameworkPropertyMetadata(false));

        public static DependencyProperty StartPointProperty = DependencyProperty.RegisterAttached(
            "StartPoint", typeof(Point), typeof(Page), new FrameworkPropertyMetadata(default(Point)));

        private TransformGroup group = new TransformGroup();
        private bool hasVisualBrush = true;
        private Hashtable propertyBag = new Hashtable();
        private RotateTransform rotateTransform = new RotateTransform(0, 0, 0);
        private ScaleTransform scaleTransform = new ScaleTransform(1, 1);
        private SkewTransform skewTransform = new SkewTransform(0, 0);
        private TranslateTransform translateTransform = new TranslateTransform(0, 0);

        private List<IView> views = new List<IView>();


        //delegate used for test binding validation rules
        private delegate void FoundBindingCallbackDelegate(FrameworkElement element, Binding binding, DependencyProperty dp);
        private FrameworkElement _firstInvalidElement; // used to test invalid element for binding validation rules
        protected List<string> errorMessages; //list of errors found on binding validation rules


        public View()
        {
            group.Children.Add(rotateTransform);
            group.Children.Add(scaleTransform);
            group.Children.Add(translateTransform);
            group.Children.Add(skewTransform);
            this.RenderTransform = group;
            //            this.databindUtil = new DataBindingUtil();
        }

        public string Title { get; set; }

        /// <summary>
        /// used by store or other for application image
        /// </summary>
        public virtual Brush Brush { get; set; }

        public static string Version
        {
            get { return ConfigurationManager.AppSettings["version"]; }
        }

        #region IView Members

        public Hashtable PropertyBag
        {
            get { return propertyBag; }
            set { propertyBag = value; }
        }

        public List<IView> Views
        {
            get { return views; }
            set { views = value; }
        }


        public virtual void DataBind()
        {
        }

        public virtual void Navigated()
        {
        }

        public virtual void Navigating()
        {
        }

        public virtual void Close()
        {
            Navigating();
        }

        /// <summary>
        /// whether the menu should be displayed
        /// </summary>
        public virtual Visibility MenuVisible
        {
            get { return Visibility.Visible; }
        }


        /// <summary>
        /// whether should create a visual brush that is to be displayed in the tabs
        /// IMPORTANT:it is important that store doesn't create one as it slows down the store significantly
        /// </summary>
        public virtual bool HasVisualBrush
        {
            get { return hasVisualBrush; }
            set { hasVisualBrush = value; }
        }

        #endregion

        #region databinding

        protected virtual void DataBind(TextBox txt, string properyPath)
        {
            databindUtil.DataBind(txt, properyPath, this.PropertyBag);
        }

        protected virtual void DataBind(TextBox txt, string properyPath, IValueConverter converter)
        {
            databindUtil.DataBind(txt, properyPath, converter, this.PropertyBag);
        }

        protected virtual void DataBind(Button txt, DependencyProperty controlproperty, string properyPath)
        {
            databindUtil.DataBind(txt, controlproperty, properyPath, this.PropertyBag);
        }

        protected virtual void DataBind(Slider slider, string properyPath)
        {
            databindUtil.DataBind(slider, properyPath, this.PropertyBag);
        }

        protected void DataBind(Slider slider, string properyPath, IValueConverter conv)
        {
            databindUtil.DataBind(slider, properyPath, conv, this.PropertyBag);
        }

        protected void DataBind(ItemsControl menu, string properyPath)
        {
            databindUtil.DataBind(menu, properyPath, this.PropertyBag);
        }

        protected virtual void DataBind(TextBlock lbl, string properyPath)
        {
            databindUtil.DataBind(lbl, properyPath, null, this.PropertyBag);
        }

        protected virtual void DataBind(TextBlock lbl, string properyPath, IValueConverter converter)
        {
            databindUtil.DataBind(lbl, properyPath, converter, this.PropertyBag);
        }

        protected virtual void DataBind(CheckBox lbl, string properyPath)
        {
            databindUtil.DataBind(lbl, properyPath, this.PropertyBag);
        }

        protected virtual void DataBind(ComboBox lbl, string properySourcePath, string selectedValuePath)
        {
            databindUtil.DataBind(lbl, properySourcePath, selectedValuePath, this.PropertyBag);
        }

        protected virtual void DataBind(ComboBox cbx, string properyPath)
        {
            databindUtil.DataBind(cbx, properyPath, this.PropertyBag);
        }

        protected virtual void DataBind(ListView txt, CollectionViewSource collectionViewSource)
        {
            databindUtil.DataBind(txt, collectionViewSource);
        }

        protected virtual void DataBind(ListBox txt, CollectionViewSource collectionViewSource)
        {
            databindUtil.DataBind(txt, collectionViewSource);
        }

        protected virtual void DataBind(ListView txt, string properyPath)
        {
            databindUtil.DataBind(txt, properyPath, this.propertyBag);
        }

        protected virtual void DataBind(ListBox lbox, string properyPath)
        {
            databindUtil.DataBind(lbox, properyPath, this.propertyBag);
        }

        protected virtual void DataBind(Image img, string properyPath, IValueConverter converter)
        {
            databindUtil.DataBind(img, properyPath, converter, this.propertyBag);
        }

        protected void DataBind(FrameworkElement control, DependencyProperty dp, object source, string path)
        {
            databindUtil.DataBind(control, dp, source, path);
        }

        protected void DataBind(FrameworkElement control, DependencyProperty dp, object source, string path,
                                IValueConverter converter)
        {
            databindUtil.DataBind(control, dp, source, path, converter);
        }

        protected void DataBind(FrameworkElement control, DependencyProperty dp, object source, string path,
                                BindingMode mode)
        {
            databindUtil.DataBind(control, dp, source, path, mode);
        }

        #endregion

        #region "testing"

        /// <summary>
        /// used to set environment as TestMode
        /// </summary>
        public bool IsTestMode { get; set; }

        public BaseMessageBox popupWindow { get; set; }

        public FrameworkElement Find(string name)
        {
            return (FrameworkElement)FindName(name);
        }

        


        #endregion

        public virtual void Ajaxpopup(object source, RoutedEventArgs args)
        {
            Control b = (Control)source;
            RoDuinoPopupWindow popup = new RoDuinoPopupWindow(b.Tag + "", propertyBag);
            NavigatorFactory.Navigator.RoDuinoPopupWindow = popup;
            //extract setting from Tag

            popup.ParseSettings(b.Tag.ToString());
            popup.ShowDialog();
        }

        public virtual void OnLoad(object sender, RoutedEventArgs e)
        {
            FadeOut();
        }

        public void SlideLeftRight(FrameworkElement leftElement, FrameworkElement rightElement)
        {
            SlideLeft(leftElement);
            SlideRight(rightElement);
            FadeOut();
        }

        public void SlideLeft(FrameworkElement element)
        {
            if (element == null) return;

            if (element.Name.Trim().Equals(string.Empty))
                return;
            double width = element.DesiredSize.Width;
            SlideElement(element, -width, 0, "pageContentSlideLeft");
        }

        public void SlideRight(FrameworkElement element)
        {
            if (element == null) return;

            if (element.Name.Trim().Equals(string.Empty))
                return;
            double width = element.DesiredSize.Width;
            SlideElement(element, width, 0, "pageContentSlideRight");
        }

        private void SlideElement(FrameworkElement element, double from, double to, string storyBoardName)
        {
            element.RenderTransform = new TranslateTransform(1, 1);
            Storyboard myStoryboard = ((Storyboard)this.TryFindResource(storyBoardName)).Clone();
            foreach (Timeline child in myStoryboard.Children)
            {
                Storyboard.SetTargetName(child, element.Name);
                if (child is DoubleAnimation)
                {
                    ((DoubleAnimation)child).From = from;
                    ((DoubleAnimation)child).To = to;
                }
            }
            myStoryboard.Begin(element);
        }

        public virtual void FadeOut()
        {
            Storyboard myStoryboard = new Storyboard();


            DoubleAnimation fadeOutAnimation = new DoubleAnimation();
            fadeOutAnimation.From = 0.2;
            fadeOutAnimation.To = 1;
            fadeOutAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.6));
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(OpacityProperty));

            myStoryboard.Children.Add(fadeOutAnimation);

            myStoryboard.Begin((FrameworkElement)this.Content);
        }


        /// <summary>
        /// used to navigate to a new uri, trough its controller
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public void Navigate(object source, RoutedEventArgs args)
        {
            FrameworkElement b = (FrameworkElement)source;
            //navigate
            NavigatorFactory.Navigator.Navigate(b.Tag + "", PropertyBag);
        }

        /// <summary>
        /// used to update a frame with a page from a uri, trough a controller
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        public void Update(object source, RoutedEventArgs args)
        {
            FrameworkElement b = (FrameworkElement)source;
            string controlName = GetControlName(b.Tag.ToString());
            ContentControl control = (ContentControl)FindName(controlName);
            if (control == null)
                throw new FrameNotFoundException(
                    string.Format("No contentControl with name '{0}' could be found in partialView '{1}'", controlName,
                                  this.GetType()));

            NavigatorFactory.Navigator.Load(this, control, GetUri(b.Tag.ToString()), PropertyBag);
        }

        /// <summary>
        /// display a confirmation message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ShowVRConfirmationBox(string message)
        {
            if (IsTestMode) return true;

            RoConfirmationBox msg = new RoConfirmationBox(message);
            msg.ShowDialog();
            return msg.MessageBoxResult;
            return false;
        }

        /// <summary>
        /// display a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public void ShowVRAlertBox(string message)
        {
            if (IsTestMode) return;
            RoAlertBox msg = new RoAlertBox(message);
            msg.Topmost = true;
            msg.ShowDialog();
        }

        protected string GetControlName(string uri)
        {
            return uri.Split(',')[0];
        }

        protected string GetUri(string uri)
        {
            return uri.Split(',')[1];
        }


        protected void DisplayErrorMessagesIfAny()
        {
            Flash errors = (Flash)PropertyBag["flash"];
            if (errors != null)
            {
                string err = "";
                if (errors.ValidationErrorMessages.Count > 0)
                    err = errors.ValidationMessage;
                else
                    err = errors.Message;

                this.popupWindow = new RoAlertBox(err);
                popupWindow.ShowDialog();
            }
        }

        /// <summary>
        /// Validates all properties on the current data source.
        /// </summary>
        /// <returns>True if there are no errors displayed, otherwise false.</returns>
        /// <remarks>
        /// Note that only errors on properties that are displayed are included. Other errors, such as errors for properties that are not displayed, 
        /// will not be validated by this method.
        /// </remarks>
        protected bool Validate()
        {
            bool isValid = true;
            _firstInvalidElement = null;
            errorMessages = new List<string>();

            BindingHelper.FindBindingsRecursively(Parent,
                                    delegate(FrameworkElement element, Binding binding, DependencyProperty dp)
                                    {
                                        foreach (ValidationRule rule in binding.ValidationRules)
                                        {
                                            ValidationResult valid = rule.Validate(element.GetValue(dp), CultureInfo.CurrentUICulture);
                                            if (!valid.IsValid)
                                            {
                                                if (isValid)
                                                {
                                                    isValid = false;
                                                    _firstInvalidElement = element;
                                                }

                                                BindingExpression expression = element.GetBindingExpression(dp);
                                                ValidationError error = new ValidationError(rule, expression, valid.ErrorContent, null);
                                                Validation.MarkInvalid(expression, error);

                                                string errorMessage = valid.ErrorContent.ToString();
                                                if (!errorMessages.Contains(errorMessage))
                                                    errorMessages.Add(errorMessage);
                                            }
                                        }
                                    });

            return isValid;
        }


        /// <summary>
        /// find an element by name on current view reading LogicalTree
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FrameworkElement FindElement(string name)
        {
            return FindElementRecursive(name, this);
        }

        /// <summary>
        /// find an element in visual tree
        /// </summary>
        /// <param name="name">name of requested element</param>
        /// <param name="element">container where to look for element(the view for example)</param>
        /// <returns></returns>
        private FrameworkElement FindElementRecursive(string name, DependencyObject element)
        {
            FrameworkElement el = null;

            foreach (object childElement in LogicalTreeHelper.GetChildren(element))
            {
                if (childElement is DependencyObject)
                {
                    try
                    {
                        if (childElement is FrameworkElement)
                        {
                            //                            Console.WriteLine("child name: " + ((FrameworkElement) childElement).Name);
                            if (name.Equals(((FrameworkElement)childElement).Name))
                            {
                                el = (FrameworkElement)childElement;
                                break;
                            }
                        }
                        //                        else
                        //                            Console.WriteLine("child type: " + childElement);
                        if (el == null)
                            el = FindElementRecursive(name, (DependencyObject)childElement);
                        else
                            break; //exit loop
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    //                    Console.WriteLine("not dep obj:"+childElement);
                }

            }
            return el;
        }
    }
}
