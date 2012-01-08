using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Features.Register
{
    public class RegisterInformations
    {
        public string Password { get; private set; }
        public string PasswordConfirmation { get; private set; }

        public RegisterInformations(string password, string passwordConfirmation)
        {
            Password = password;
            PasswordConfirmation = passwordConfirmation;
        }
    }
}
