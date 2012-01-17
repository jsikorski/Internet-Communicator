using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using Client.Features.Login;
using Client.Messages;
using Client.Services;
using Common.Hash;
using Protocol.Login;

namespace Client.Commands
{
    public class Login : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly LoggedUser _loggedUser;
        private readonly IHashService _hashService;
        private readonly LoginInformations _loginInformations;
        private readonly IEventAggregator _eventAggregator;

        public Login(
            IServerConnection serverConnection,
            LoggedUser loggedUser,
            IHashService hashService,
            LoginInformations loginInformations,
            IEventAggregator eventAggregator)
        {
            _serverConnection = serverConnection;
            _loggedUser = loggedUser;
            _hashService = hashService;
            _loginInformations = loginInformations;
            _eventAggregator = eventAggregator;
        }

        public void Execute()
        {
            var loginRequest = new LoginRequest
                                   {
                                       Number = _loginInformations.Number,
                                       PasswordHash = _hashService.GetHash(_loginInformations.Password)
                                   };

            if (_serverConnection.SendLoginRequest(loginRequest).WasSuccessfull)
            {
                _loggedUser.Number = loginRequest.Number;
            }
            else
            {
                throw new Exception("Incorrect number or password.");
            }

            _eventAggregator.Publish(new Logged());
        }
    }
}
