using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Client.Services;
using Protocol.Register;

namespace Client.Commands
{
    public class RegisterCommand : ICommand<Tuple<string, string>>
    {
        private readonly IServerConnection _serverConnection;

        public RegisterCommand(IServerConnection serverConnection)
        {
            _serverConnection = serverConnection;
        }

        public void Execute(Tuple<string, string> passwords)
        {
            string password = passwords.Item1;
            string passwordConfirmation = passwords.Item2;

            if (password != passwordConfirmation)
            {
                MessageBox.Show("Password and password confirmation have to be equal.", "Password confirmation");
                throw new Exception();
            }

            var registerRequest = new RegisterRequest {Password = password};
            RegisterResponse registerResponse = _serverConnection.SendRegisterRequest(registerRequest);

            if (registerResponse.WasSuccessfull)
            {
                MessageBox.Show(string.Format("Account was successfully created. Your number is {0}.",
                                              registerResponse.AccountNumber), "Registration completed");
            }
            else
            {
                MessageBox.Show("Cannot create new account.", "Registration error");
                throw new Exception();
            }
        }
    }
}
