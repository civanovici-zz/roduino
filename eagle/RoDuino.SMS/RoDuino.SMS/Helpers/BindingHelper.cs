using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RoDuino.SMS.Helpers
{
    public class BindingHelper
    {
        public delegate void FoundBindingCallbackDelegate(FrameworkElement element, Binding binding, DependencyProperty dp);

        /// <summary>
        /// method used to force the datbabinding for dynamic forms
        /// 
        /// </summary>
        /// <param name="view">view for witch binding is forced</param>
        public static void ForceDataBind(FrameworkElement view)
        {
            FindBindingsRecursively(view,
                                    delegate(FrameworkElement element, Binding binding, DependencyProperty dp)
                                    {
                                        if (element.DataContext != null)
                                        {
                                            UpdateSourceTrigger updateTrigger = binding.UpdateSourceTrigger;
                                            PropertyPath path = binding.Path;
                                            BindingMode mode = binding.Mode;
                                            IValueConverter converter = binding.Converter;
                                            ArrayList rules = new ArrayList();
                                            foreach (ValidationRule rule in binding.ValidationRules)
                                            {
                                                rules.Add(rule);
                                            }
                                            BindingOperations.ClearBinding(element, dp);

                                            Binding newBinding = new Binding();
                                            newBinding.Source = element.DataContext;
                                            newBinding.Mode = mode;
                                            newBinding.Converter = converter;
                                            newBinding.Path = path;
                                            newBinding.UpdateSourceTrigger = updateTrigger;
                                            foreach (ValidationRule rule in rules)
                                            {
                                                newBinding.ValidationRules.Add(rule);
                                            }
                                            BindingOperations.SetBinding(element, dp, newBinding);
                                        }

                                    });
        }


        /// <summary>
        /// Recursively goes through the control tree, looking for bindings on the current data context.
        /// </summary>
        /// <param name="element">The root element to start searching at.</param>
        /// <param name="callbackDelegate">A delegate called when a binding if found.</param>
        public static void FindBindingsRecursively(DependencyObject element, FoundBindingCallbackDelegate callbackDelegate)
        {
            // See if we should display the errors on this element
            MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Static |
                                                                BindingFlags.Public |
                                                                BindingFlags.FlattenHierarchy);

            foreach (MemberInfo member in members)
            {
                DependencyProperty dp = null;

                // Check to see if the field or property we were given is a dependency property
                if (member.MemberType == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    if (typeof(DependencyProperty).IsAssignableFrom(field.FieldType))
                    {
                        dp = (DependencyProperty)field.GetValue(element);
                    }
                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo prop = (PropertyInfo)member;
                    if (typeof(DependencyProperty).IsAssignableFrom(prop.PropertyType))
                    {
                        dp = (DependencyProperty)prop.GetValue(element, null);
                    }
                }

                if (dp != null)
                {
                    // Awesome, we have a dependency property. does it have a binding? If yes, is it bound to the property we're interested in?
                    Binding bb = BindingOperations.GetBinding(element, dp);
                    if (bb != null)
                    {
                        // This element has a DependencyProperty that we know of that is bound to the property we're interested in. 
                        // Now we just tell the callback and the caller will handle it.
                        if (element is FrameworkElement)
                        {
                            callbackDelegate((FrameworkElement)element, bb, dp);
                        }
                    }
                }
            }

            // Now, recurse through any child elements
            if (element is FrameworkElement || element is FrameworkContentElement)
            {
                foreach (object childElement in LogicalTreeHelper.GetChildren(element))
                {
                    if (childElement is DependencyObject)
                    {
                        FindBindingsRecursively((DependencyObject)childElement, callbackDelegate);
                    }
                }
            }
        }

    }
}
