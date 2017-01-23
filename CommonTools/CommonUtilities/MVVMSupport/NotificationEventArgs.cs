using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.MVVMSupport
{
    
    /// <summary>
    /// 
    /// <summary>
    /// Event class that supports passing input and output arguments. This can be used to support modal dialogs in an MVVM friendly way. 
    /// (See https://www.develop.com/modaldialogsmvvm)
    /// </summary>
    /// <typeparam name="ArgType">This is the type of the "payload" argument contained in this object</typeparam>
    /// <typeparam name="ResponseArgType">This is the type of the argument that is accepted by the Response action/delegate.</typeparam>
    public class NotificationEventArgs<ArgType, ResponseArgType> : EventArgs
    {
        /// <summary>
        /// Argument to pass to the observer.
        /// </summary>
        public ArgType Arg { get; set; }

        /// <summary>
        /// Used by an observer to respond back to the event dispatcher. (E.g., can be used for a View to respond back to a View Model.) 
        /// </summary>
        public Action<ResponseArgType> Respond { get; protected set; }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg">Argument ("payload") that is passed back through the this object.</param>
        /// <param name="responseHandler">Delgate that is called by the subscriber when it wants to respond to event.</param>
        public NotificationEventArgs( ArgType arg, Action<ResponseArgType> responseHandler)
        {
            Arg = arg;
            Respond = responseHandler;
        }
    }
}
