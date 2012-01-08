using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Client.Features.Login;
using Client.Services;
using Protocol.Login;

namespace Client.Commands
{
    public class LoginCommand : ICommand<LoginRequest>
    {
        private readonly IServerConnection _serverConnection;
        private readonly LoggedUser _loggedUser;

        public LoginCommand(IServerConnection serverConnection,
            LoggedUser loggedUser)
        {
            _serverConnection = serverConnection;
            _loggedUser = loggedUser;
        }

        public void Execute(LoginRequest loginRequest)
        {
            if (_serverConnection.SendLoginRequest(loginRequest).WasSuccessfull)
            {
                MessageBox.Show("Success");
                _loggedUser.Number = loginRequest.Number;
            }
            else
            {
                MessageBox.Show("Incorrect number or password.", "Login error");
                throw new Exception();
            }
        }
    }
}
