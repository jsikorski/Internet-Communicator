using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Client.Commands;
using Client.Utils;

namespace Client.Insrastructure
{
    public class CommandInvoker
    {
        public static void Invoke(ICommand command, Action<Exception> onCompleted = null)
        {
            ThreadPool.QueueUserWorkItem(o =>
                                             {
                                                 Exception error = null;
                                                 try
                                                 {
                                                     command.Execute();
                                                 }
                                                 catch (Exception exception)
                                                 {
                                                     ErrorMessageBox.Show(exception);
                                                     error = exception;
                                                 }

                                                 if (onCompleted != null)
                                                 {
                                                     onCompleted(error);
                                                 }
                                             });
        }

        public static void InvokeBusy(ICommand command, IBusyScope busyScope, 
            Action<Exception> onCompleted = null)
        {
            busyScope.IsBusy = true;
            Invoke(command, exception =>
                                {
                                    busyScope.IsBusy = false;
                                    if (onCompleted != null) 
                                        onCompleted(exception);
                                });
        }

        public static void Execute(ICommand command)
        {
            try
            {
                command.Execute();
            }
            catch (Exception exception)
            {
                ErrorMessageBox.Show(exception);
            }
        }
    }
}
