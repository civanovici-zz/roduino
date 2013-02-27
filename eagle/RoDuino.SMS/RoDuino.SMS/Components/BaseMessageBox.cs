using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace RoDuino.SMS.Components
{
    public class BaseMessageBox : Window
    {
        protected FrameworkElement elementToAnimate;

        public BaseMessageBox()
        {
            this.Topmost = true;
        }

        /// <summary>
        /// restore opacity for animated element
        /// </summary>
        protected void FadeIn()
        {
            if (elementToAnimate == null)
                return;

            Storyboard myStoryboard;
            DoubleAnimation fadeOutAnimation = new DoubleAnimation();
            fadeOutAnimation.From = 0.4;
            fadeOutAnimation.To = 1;
            fadeOutAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(fadeOutAnimation);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(OpacityProperty));
            myStoryboard.Begin(elementToAnimate);
        }

        /// <summary>
        /// set opacity for animated element
        /// </summary>
        protected void FadeOut()
        {
            if (elementToAnimate == null)
                return;

            Storyboard myStoryboard;
            DoubleAnimation fadeOutAnimation = new DoubleAnimation();
            fadeOutAnimation.From = 1;
            fadeOutAnimation.To = 0.4;
            fadeOutAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(fadeOutAnimation);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(OpacityProperty));
            myStoryboard.Begin(elementToAnimate);
        }

        /// <summary>
        /// OK button pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OKPressed(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            CloseAndFadeIn();
        }

        /// <summary>
        /// Cancel button pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CancelPressed(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            CloseAndFadeIn();
        }

        /// <summary>
        /// close messagebox and restore animation
        /// </summary>
        private void CloseAndFadeIn()
        {
            this.Close();
            FadeIn();
        }
    }
}
