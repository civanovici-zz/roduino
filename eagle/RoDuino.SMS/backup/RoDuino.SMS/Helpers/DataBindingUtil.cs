using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RoDuino.SMS.Helpers
{
    public class DataBindingUtil
    {
        public void DataBind(Button txt, DependencyProperty controlproperty, string properyPath, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            string path = properyPath.Replace(key + ".", "");
            DataBind(txt, controlproperty, propertyBag[key], path);
        }

        public void DataBind(TextBox txt, string properyPath, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            string path = properyPath.Replace(key + ".", "");
            DataBind(txt, TextBox.TextProperty, propertyBag[key], path);
        }

        public void DataBind(TextBox txt, string properyPath, IValueConverter converter, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            string path = properyPath.Replace(key + ".", "");
            DataBind(txt, TextBox.TextProperty, propertyBag[key], path, converter);
        }

        public void DataBind(Slider slider, string properyPath, IValueConverter converter, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            string path = properyPath.Replace(key + ".", "");
            DataBind(slider, Slider.ValueProperty, propertyBag[key], path, converter);
        }

        public void DataBind(Slider slider, string properyPath, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            string path = properyPath.Replace(key + ".", "");
            DataBind(slider, Slider.ValueProperty, propertyBag[key], path);
        }

        public void DataBind(ItemsControl menu, string properyPath, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            string path = properyPath.Replace(key + ".", "");
            DataBind(menu, Slider.ValueProperty, propertyBag[key], path);
        }

        public void DataBind(TextBlock lbl, string properyPath, IValueConverter converter, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            string path = properyPath.Replace(key + ".", "");
            DataBind(lbl, TextBlock.TextProperty, propertyBag[key], path, converter);
        }

        public void DataBind(CheckBox lbl, string properyPath, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            string path = properyPath.Replace(key + ".", "");
            DataBind(lbl, CheckBox.IsCheckedProperty, propertyBag[key], path);
        }

        public void DataBind(ComboBox lbl, string properySourcePath, string selectedValuePath, Hashtable propertyBag)
        {
            //binding the list
            string key = properySourcePath.Split('.')[0];
            lbl.ItemsSource = (IEnumerable)propertyBag[key];

            //binding selected value
            key = selectedValuePath.Split('.')[0];
            string path = selectedValuePath.Replace(key + ".", "");
            Binding selectedValue = new Binding(path);
            selectedValue.Source = propertyBag[key];
            lbl.SetBinding(ComboBox.SelectedItemProperty, selectedValue);
        }

        public void DataBind(ComboBox cbx, string properyPath, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            cbx.ItemsSource = (IEnumerable)propertyBag[key];
        }

        public void DataBind(ListView txt, CollectionViewSource collectionViewSource)
        {
            txt.ItemsSource = collectionViewSource.View;
        }

        public void DataBind(ListBox txt, CollectionViewSource collectionViewSource)
        {
            txt.ItemsSource = collectionViewSource.View;
        }

        public void DataBind(ListView txt, string properyPath, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            txt.ItemsSource = (IEnumerable)propertyBag[key];
        }

        public void DataBind(ListBox lbox, string properyPath, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            lbox.ItemsSource = (IEnumerable)propertyBag[key];
        }

        public void DataBind(Image img, string properyPath, IValueConverter converter, Hashtable propertyBag)
        {
            string key = properyPath.Split('.')[0];
            string path = properyPath.Replace(key + ".", "");
            DataBind(img, Image.SourceProperty, propertyBag[key], path, converter);
        }

        public void DataBind(FrameworkElement control, DependencyProperty dp, object source, string path)
        {
            DataBind(control, dp, source, path, null);
        }

        public void DataBind(FrameworkElement control, DependencyProperty dp, object source, string path,
                             IValueConverter converter)
        {
            Binding binding = new Binding(path);
            binding.Source = source;
            if (converter != null) binding.Converter = converter;
            control.SetBinding(dp, binding);
        }

        public void DataBind(FrameworkElement control, DependencyProperty dp, object source, string path,
                             BindingMode mode)
        {
            Binding binding = new Binding(path);
            binding.Source = source;
            binding.Mode = mode;
            control.SetBinding(dp, binding);
        }
    }
}
