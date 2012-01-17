using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using Client.Commands;

namespace Client.Insrastructure
{
    class CommandResult: IResult
    {
        private readonly ICommand _command;

        public CommandResult(ICommand command)
        {
            _command = command;
        }

        public void Execute(ActionExecutionContext context)
        {
            CommandInvoker.Invoke(_command,
                                  exception => Completed(this, new ResultCompletionEventArgs {Error = exception}));
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = (sender, args) => { };
    }
}
