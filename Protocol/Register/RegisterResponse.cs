using System;

namespace Protocol.Register
{
    [Serializable]
    public class RegisterResponse : IResponse
    {
        public bool WasSuccessfull { get; set; }
        public int AccountNumber { get; set; }
    }
}
