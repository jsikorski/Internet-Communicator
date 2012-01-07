using System;

namespace Protocol.Registration
{
    [Serializable]
    public class RegisterResponse : IResponse
    {
        public bool WasSuccessfull { get; set; }
        public int AccountNumber { get; set; }
    }
}
