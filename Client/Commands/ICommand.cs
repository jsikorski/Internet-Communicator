using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Commands
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<T>
    {
        void Execute(T param);
    }
}
