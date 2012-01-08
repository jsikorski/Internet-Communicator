using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Client.Services;
using Protocol.Login;

namespace Client.Commands
{
    public class LoginCommand : ICommand<LoginRequest>
    {
        private readonly IServerConnection _serverConnection;

        public LoginCommand(IServerConnection serverConnection)
        {
            _serverConnection = serverConnection;
        }

        public void Execute(LoginRequest loginRequest)
        {
            if (_serverConnection.SendLoginRequest(loginRequest).WasSuccessfull)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Incorrect number or password.", "Login error");
                throw new Exception();
            }
        }
    }
}
