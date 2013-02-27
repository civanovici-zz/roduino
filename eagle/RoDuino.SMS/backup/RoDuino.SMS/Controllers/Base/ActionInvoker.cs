using System;
using System.Reflection;
using RoDuino.SMS.Exceptions;

namespace RoDuino.SMS.Controllers.Base
{
    public class ActionInvoker
    {
        /// <summary>
        /// invokes the action in a controller
        /// databindign the parameters from the propertybag
        /// the convetion is to user the same name
        /// propertybag["users"]
        /// action(User[] users) will be bound, while if the name in the property bag is users and 
        /// the parameter is
        /// action(Users[] u) it won't
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="actionName"></param>
        public static void InvokeActionInController(Controller controller, string actionName)
        {
            InvokeActionInController(controller, actionName, true);
        }

        /// <summary>
        /// invokes the action in a controller
        /// databindign the parameters from the propertybag
        /// the convetion is to user the same name
        /// propertybag["users"]
        /// action(User[] users) will be bound, while if the name in the property bag is users and 
        /// the parameter is
        /// action(Users[] u) it won't
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="actionName"></param>
        /// <param name="clearPropertyBagAfterDataBind"></param>
        public static void InvokeActionInController(Controller controller, string actionName,
                                                    bool clearPropertyBagAfterDataBind)
        {
            //invoke action
            MethodBase action;
            try
            {
                action = controller.GetType().GetMethod(actionName);
            }
            catch (AmbiguousMatchException ex)
            {
                throw new AmbiguousActionNameException(controller.GetType().Name, actionName, ex);
            }


            if (action == null) throw new ActionNotFoundException(controller.GetType().Name, actionName);
            try
            {
                //databind parameters with the same name
                object[] args = DataBindParameters(controller, action);
                //clear property bag
                if (clearPropertyBagAfterDataBind) controller.PropertyBag.Clear();
                //invoke action
                action.Invoke(controller, args);
            }
            catch (TargetInvocationException ex)
            {
                throw new ControllerActionInternalException(
                    String.Format(
                        "Exception invoking action {1} in controller {0}. Check the values of the parameters in the log.",
                        controller.GetType(), actionName), ex.InnerException);
                // new ActionNotFoundException(String.Format("Controller {0} does not contain an action {1}",controller.GetType().Path,actionName),ex);
            }
        }


        private static object[] DataBindParameters(Controller controller, MethodBase action)
        {
            ParameterInfo[] param = action.GetParameters();
            object[] args = new object[param.Length];
            foreach (ParameterInfo info in param)
            {
                string name = info.Name;
                Type type = info.ParameterType;

                if (controller.PropertyBag[name] != null)
                {
                    //                    Console.WriteLine("type:"+type);
                    //                    Console.WriteLine("Base:" + controller.PropertyBag[name].GetType().BaseType + " :" + controller.PropertyBag[name].GetType().BaseType.Equals(type));
                    //                    Console.WriteLine("getType:" + controller.PropertyBag[name].GetType() + " :" + controller.PropertyBag[name].GetType().Equals(type));
                    //                    Console.WriteLine("issubclass:"+controller.PropertyBag[name].GetType().IsSubclassOf(type));

                    if (controller.PropertyBag[name].GetType().BaseType.Equals(type) ||
                        controller.PropertyBag[name].GetType().Equals(type) ||
                        controller.PropertyBag[name].GetType().IsSubclassOf(type))
                    {
                        //info.RawDefaultValue = 
                        args[info.Position] = controller.PropertyBag[name];
                    }
                }
            }
            return args;
        }
    }
}
