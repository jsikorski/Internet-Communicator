using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace Client.Utils
{
    public class UniqueTypeParameter : TypedParameter
    {
        /// <summary>
        /// Use this class only when your parameter have unique types
        /// </summary>
        /// <param name="parameter"></param>
        public UniqueTypeParameter(object parameter)
            : base(parameter.GetType(), parameter)
        {
        }
    }
}
