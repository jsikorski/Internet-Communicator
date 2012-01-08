using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Client.Features.Login;
using Client.Services;
using Common.Hash;
using Protocol.Login;

namespace Client.Commands
{
    public class LoginCommand : ICommand<LoginInformations>
    {
        private readonly IServerConnection _serverConnection;
        private readonly LoggedUser _loggedUser;
        private readonly IHashService _hashService;

        public LoginCommand(
            IServerConnection serverConnection,
            LoggedUser loggedUser,
            IHashService hashService)
        {
            _serverConnection = serverConnection;
            _loggedUser = loggedUser;
            _hashService = hashService;
        }

        public void Execute(LoginInformations loginInformations)
        {
            var loginRequest = new LoginRequest
                                   {
                                       Number = loginInformations.Number,
                                       PasswordHash = _hashService.GetHash(loginInformations.Password)
                                   };

            if (_serverConnection.SendLoginRequest(loginRequest).WasSuccessfull)
            {
                _loggedUser.Number = loginRequest.Number;
            }
            else
            {
                throw new Exception("Incorrect number or password.");
            }
        }
    }
}
