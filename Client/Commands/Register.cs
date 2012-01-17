using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using Client.Features.Registration;
using Client.Messages;
using Client.Services;
using Common.Hash;
using Protocol.Register;

namespace Client.Commands
{
    public class Register : ICommand
    {
        private readonly IServerConnection _serverConnection;
        private readonly IHashService _hashService;
        private readonly RegisterInformations _registerInformations;
        private readonly IEventAggregator _eventAggregator;

        public Register(
            IServerConnection serverConnection,
            IHashService hashService,
            RegisterInformations registerInformations,
            IEventAggregator eventAggregator)
        {
            _serverConnection = serverConnection;
            _hashService = hashService;
            _registerInformations = registerInformations;
            _eventAggregator = eventAggregator;
        }

        public void Execute()
        {
            string password = _registerInformations.Password;
            string passwordConfirmation = _registerInformations.PasswordConfirmation;

            if (password != passwordConfirmation)
            {
                throw new Exception("Password and password confirmation have to be equal.");
            }

            var registerRequest = new RegisterRequest { Password = _hashService.GetHash(password) };
            RegisterResponse registerResponse = _serverConnection.SendRegisterRequest(registerRequest);

            if (!registerResponse.WasSuccessfull)
            {
                throw new Exception("Cannot create new account.");
            }

            _eventAggregator.Publish(new Registered(registerResponse.AccountNumber));
        }
    }
}
