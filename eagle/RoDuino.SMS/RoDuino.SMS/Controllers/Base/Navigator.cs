using System;
using System.Collections;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using RoDuino.SMS.Bll;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Bll.Util;
using RoDuino.SMS.Components;
using RoDuino.SMS.Exceptions;
using RoDuino.SMS.Views;

namespace RoDuino.SMS.Controllers.Base
{
    public class Navigator : INavigator
    {
        protected Controller controller;
        protected Hashtable controllers = new Hashtable();
        protected bool isRedirect;
        protected NavigationService navigationService;
        protected Hashtable partialViews = new Hashtable();
        protected RoDuinoPopupWindow popupWindow;
        protected Hashtable views = new Hashtable();
        protected string viewToRender = "";


        public Navigator()
        {
            CollectControllers();
            CollectViews();
            //            CollectPartialViews();
        }

        #region INavigator Members



        public virtual string Start(string controllerName, string actionName, IDictionary args)
        {
            viewToRender = InvokeActionInController(controllerName, actionName, args, true);
            RenderView(viewToRender);
            return String.Format("Views/{0}.xaml", viewToRender);
        }

        public string Start(string cont, string action)
        {
            return Start(cont, action, null);
        }

        public virtual void Navigate(string uri)
        {
            Navigate(uri, (IDictionary)null);
        }


        public virtual void Navigate(string uri, IDictionary args)
        {
            string controllerName = uri.Substring(0, uri.IndexOf("/"));
            uri = uri.Replace(controllerName + "/", "");
            string actionName = uri.Substring(0, uri.IndexOf(".xaml"));
            uri = uri.Replace(actionName + ".xaml", "");
            if (uri.Trim().Length > 1)
            {
                string[] param = uri.Substring(1).Split('&');
                foreach (string s in param)
                {
                    if (s.IndexOf('=') > 0)
                    {
                        if (args == null) args = new Hashtable();
                        args[s.Split('=')[0]] = s.Split('=')[1];
                    }
                }
            }
            Navigate(controllerName, actionName, args);
        }


        /// <summary>
        /// this will instantiate the controller, invoke the action and in the action navigate to the page and display it
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="args"></param>
        public virtual void Navigate(string controllerName, string actionName, IDictionary args)
        // params object[] args)
        {
            try
            {
                View formerView = Tabs.Instance.Current.View;

                //ShowLoading
                Mouse.OverrideCursor = Cursors.Wait;

                //invoke controller action
                viewToRender = InvokeActionInController(controllerName, actionName, args, true);

                //navigating
                if (formerView != null)
                {
                    FireNavigating(formerView);
                }


                if (!isRedirect)
                {
                    //render page
                    View p = RenderView(viewToRender);
                    Tabs.Instance.Current.View = p;
                    //                    navigationService.Navigate(p);
                    //                    p.FadeOut();
                    p.Navigated();
                    ClosePopupIfExists();
                }
                else
                {
                    isRedirect = false;
                }
            }
            catch (Exception e)
            {
                RoLog.Instance.WriteToLog(e.ToString(), TracedAttribute.ERROR);
                throw e;
            }
            finally
            {
                //hide loading
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }


        /// <summary>
        /// makes the roundtrip to controller
        /// BUT does not rerender the view, just gets the data back
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual IDictionary Get(string controller, string action, IDictionary args)
        {
            try
            {
                //ShowLoading
                Mouse.OverrideCursor = Cursors.Wait;

                //invoke controller action
                viewToRender = InvokeActionInController(controller, action, args, true);
            }
            catch (Exception e)
            {
                RoLog.Instance.WriteToLog(e.ToString(), TracedAttribute.ERROR);
                throw e;
            }
            finally
            {
                //hide loading
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            return args;
        }

        /// <summary>
        /// this will instantiate the controller, invoke the action and in the action navigate to the page and display it
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="args"></param>
        /// <param name="openNewTab"></param>
        public virtual void Navigate(string uri, IDictionary args, bool openNewTab)
        {
            if (openNewTab)
            {
                Tabs.Instance.NewTab(uri, args);
            }
            else
            {
                Navigate(uri, args);
            }
        }


        /// <summary>
        /// this will instantiate the controller, invoke the action and in the action navigate to the page and display it
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="args"></param>
        public virtual void Redirect(string controllerName, string actionName, IDictionary args)
        // params object[] args)
        {
            isRedirect = true;
            viewToRender = InvokeActionInController(controllerName, actionName, args, false);
            //render page
            View p = RenderView(viewToRender);
            Tabs.Instance.Current.View = p;
            p.Navigated();
            //            navigationService.Navigate(p);
            ClosePopupIfExists();
        }

        private void FireNavigating(View p)
        {
            //if has children
            foreach (View childView in p.Views)
            {
                childView.Navigating();
            }

            //navigated for parent
            p.Navigating();
            //clear all children
            p.Views.Clear();
        }

        public virtual void Load(View parentView, ContentControl control, string uri, IDictionary args)
        {
            //previous view in content control
            if (control.Content is View)
            {
                View previousView = ((View)control.Content);
                previousView.Navigating();

                //remove from parent
                if (parentView != null)
                    parentView.Views.Remove(previousView);
            }

            string controllerName = uri.Substring(0, uri.IndexOf("/"));
            uri = uri.Replace(controllerName + "/", "");
            string actionName = uri.Substring(0, uri.IndexOf(".xaml"));
            uri = uri.Replace(actionName + ".xaml", "");
            if (uri.Trim().Length > 1 && args != null)
            {
                string[] param = uri.Substring(1).Split(';');
                foreach (string s in param)
                {
                    if (s.IndexOf('=') > 0)
                        args[s.Split('=')[0]] = s.Split('=')[1];
                }
            }
            viewToRender = InvokeActionInController(controllerName, actionName, args, false);
            //render page and navigate to it in frame
            View view = RenderView(viewToRender);
            control.Content = null;
            control.Content = view;

            //add to parent
            if (parentView != null)
                parentView.Views.Add(view);

            //fire events
            view.Navigated();
            ClosePopupIfExists();
        }

        public void Navigate(string cont, string action)
        {
            this.Navigate(cont, action, null);
        }

        #endregion

        #region "protected"

        /// <summary>
        /// if from cache is true gets the page from cache and invoked databind on it - statefull pages
        /// otherwise instantiates it again - stateless pages
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        protected View RenderView(string viewName)
        {
            View p = InstantiateAndAddUnLoadHandler(viewName);
            if (p == null) throw new ViewNotFoundException(String.Format("View with name {0} not found", viewName));
            //transfer data to view, then databind
            p.PropertyBag = controller.PropertyBag;

            try
            {
                p.DataBind();
            }
            catch (Exception e)
            {
                throw new ViewDataBindException(
                    String.Format("An exception {1} ocurred in DataBind method of view {0}", viewName, e), e);
            }

            //IMPORTANT: clear the propertybag for the view after databind, so that memory is used more efficiently
            p.PropertyBag.Clear();

            return p;
        }


        //        [Traced(TracedAttribute.INFO)]
        //        protected PartialView RenderPartialView(string viewName, bool fromCache)
        //        {
        //            PartialView p = fromCache ? (PartialView)this.partialViews[viewName] : ReinstantiatePartialView(viewName);
        //            if (p == null) throw new ViewNotFoundException(String.Format("PartialView with name {0} not found", viewName));
        //            //transfer data to view, then databind
        //            p.PropertyBag = controller.PropertyBag;
        //
        //            try
        //            {
        //                p.DataBind();
        //            }
        //            catch (Exception e)
        //            {
        //                throw new ViewDataBindException(String.Format("An exception {1} ocurred in DataBind method of view {0}", viewName, e), e);
        //            }
        //            return p;
        //
        //        }


        protected virtual string InvokeActionInController(string controllerName, string actionName, IDictionary args,
                                                          bool clearPropertyBagAfterDatabind)
        {
            //instattiate controller
            if (controllerName.Contains(","))
                throw new AmbiguousControllerNameException(
                    String.Format(
                        "Controller name {0} contains ,. Are you invoiking an Update or a Navigate in the view?",
                        controllerName), null);
            controller = (Controller)this.controllers[controllerName];

            if (controller == null)
                throw new ControllerNotFoundException(
                    String.Format(
                        "Controller with name {0} not found in Navigator collected controllers. Are you sure it's added?",
                        controllerName));

            controller.PropertyBag = args != null ? (Hashtable)args : new Hashtable();

            //invoke action
            controller.ViewToRender = controllerName + "/" + actionName;
            //            MethodBase action = controller.GetType().GetMethod(actionName);
            //            try
            //            {
            //                action.Invoke(controller, null);
            //            }
            //            catch (TargetInvocationException ex)
            //            {
            //                throw ex.InnerException;
            //            }

            ActionInvoker.InvokeActionInController(controller, actionName, clearPropertyBagAfterDatabind);

            //view to render
            return controller.ViewToRender;
        }


        protected virtual void CollectControllers()
        {
            CollectControllersForAssembly("RoDuino.SMS");
//            CollectControllersForAssembly("VR.Core.DatabaseSetup");
        }

        protected void CollectViews()
        {
            CollectViewsForAssembly("RoDuino.SMS");
//            CollectViewsForAssembly("VR.Core.DatabaseSetup");
        }

        //        [Traced(TracedAttribute.INFO)]
        //        protected void CollectPartialViews()
        //        {
        //            CollectPartialViewsForAssembly("VR.Core.UI");
        //            CollectPartialViewsForAssembly("VR.Core.DatabaseSetup");
        //        }

        private void CollectControllersForAssembly(string assemblyName)
        {
            RoLog.Instance.WriteToLog("Collect controllers from: " + assemblyName);
            foreach (Type t in Assembly.Load(assemblyName).GetTypes())
            {
                if (t.IsSubclassOf(typeof(Controller)))
                {
                    string replace = t.Name.Replace("Controller", "");
                    //                    Console.WriteLine(replace);
                    //VRLog.Instance.WriteToLog("  Adding : "+replace+" from " + assemblyName);

                    try
                    {
                        this.controllers[replace] = Activator.CreateInstance(t);
                    }
                    catch (Exception exception)
                    {
                        RoLog.Instance.WriteToLog(
                            String.Format(
                                "EXCEPTION:{0},\n collecting controller {3} Message='{1},\n StackTrace={2}', Exception={4}",
                                exception.GetType(),
                                exception.Message, exception.StackTrace, replace, exception), TracedAttribute.ERROR);
                    }


                    //VRLog.Instance.WriteToLog("  Added : " + replace + " from " + assemblyName);
                }
            }
        }

        private void CollectViewsForAssembly(string assemblyName)
        {
            foreach (Type t in Assembly.Load(assemblyName).GetTypes())
            {
                if (t.IsSubclassOf(typeof(View)))
                {
                    string name = string.Format("{0}/{1}", t.Namespace.Substring(t.Namespace.LastIndexOf(".") + 1),
                                                t.Name);
                    //                    Console.WriteLine(name);
                    try
                    {
                        this.views[name] = t; //Activator.CreateInstance(t);
                    }
                    catch (Exception exception)
                    {
                        RoLog.Instance.WriteToLog(
                            String.Format(
                                "EXCEPTION:{0},\n collecting view {3} Message='{1},\n StackTrace={2}', Exception={4}",
                                exception.InnerException.GetType(),
                                exception.InnerException.Message, exception.InnerException.StackTrace, name, exception), TracedAttribute.ERROR);
                    }
                }
            }
        }

        //        private void CollectPartialViewsForAssembly(string assemblyName)
        //        {
        //            foreach (Type t in Assembly.Load(assemblyName).GetTypes())
        //            {
        //                if (t.IsSubclassOf(typeof(PartialView)))
        //                {
        //                    string name = string.Format("{0}/{1}", t.Namespace.Substring(t.Namespace.LastIndexOf(".") + 1), t.Name);
        //                    //                    Console.WriteLine(name);
        //                    try
        //                    {
        //                        this.partialViews[name] = Activator.CreateInstance(t);
        //                    }
        //                    catch (Exception exception)
        //                    {
        //                        VRLog.Instance.WriteToLog(
        //                            String.Format("EXCEPTION:{0},\n collecting partialView {3} Message='{1},\n StackTrace={2}'", exception.InnerException.GetType(),
        //                                          exception.InnerException.Message, exception.InnerException.StackTrace, name));
        //                    }
        //                }
        //            }
        //        }

        /// <summary>
        /// reinstantiates the view, and reinsert it in the views cache
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private View InstantiateAndAddUnLoadHandler(string viewName)
        {
            Type ty = (Type)views[viewName];
            View view = (View)Activator.CreateInstance(ty);
            //view.Unloaded += view_Unloaded;
            return view;
        }

        /// <summary>
        /// occurs when a view is unloaded from the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void view_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        //        private PartialView ReinstantiatePartialView(string viewName)
        //        {
        //            Type ty = partialViews[viewName].GetType();
        //            PartialView view = (PartialView)Activator.CreateInstance(ty);
        //            this.partialViews[viewName] = view;
        //            return view;
        //        }

        private void ClosePopupIfExists()
        {
            if (popupWindow != null)
            {
                popupWindow.Close();
                popupWindow = null;
            }
        }

        #endregion

        #region "properties"

        public string ViewToRender
        {
            get { return viewToRender; }
            set { viewToRender = value; }
        }

        public Controller Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        public NavigationService NavigationService
        {
            get { return navigationService; }
            set { navigationService = value; }
        }


        public RoDuinoPopupWindow RoDuinoPopupWindow
        {
            get { return popupWindow; }
            set { popupWindow = value; }
        }

        #endregion
    }
}
