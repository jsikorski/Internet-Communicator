using System;
using Caliburn.Micro;
using Client.Context;
using Client.Features.Login;
using Client.Messages;
using Client.Services;
using Common.Hash;
using Protocol.Login;

namespace Client.Commands.User
{
    public class Login : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly ICurrentContext _currentContext;
        private readonly IHashService _hashService;
        private readonly LoginInformations _loginInformations;
        private readonly IEventAggregator _eventAggregator;

        public Login(
            IServerConnection serverConnection,
            ICurrentContext currentContext,
            IHashService hashService,
            LoginInformations loginInformations,
            IEventAggregator eventAggregator)
        {
            _serverConnection = serverConnection;
            _currentContext = currentContext;
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
                _currentContext.LoggedUserNumber = loginRequest.Number;
            }
            else
            {
                throw new Exception("Incorrect number or password.");
            }

            _eventAggregator.Publish(new Logged());
        }
    }
}
