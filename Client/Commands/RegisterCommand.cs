using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Client.Features.Register;
using Client.Services;
using Common.Hash;
using Protocol.Register;

namespace Client.Commands
{
    public class RegisterCommand : ICommand<RegisterInformations, int>
    {
        private readonly IServerConnection _serverConnection;
        private readonly IHashService _hashService;

        public RegisterCommand(
            IServerConnection serverConnection,
            IHashService hashService)
        {
            _serverConnection = serverConnection;
            _hashService = hashService;
        }

        public int Execute(RegisterInformations registerInformations)
        {
            string password = registerInformations.Password;
            string passwordConfirmation = registerInformations.PasswordConfirmation;

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

            return registerResponse.AccountNumber;
        }
    }
}
