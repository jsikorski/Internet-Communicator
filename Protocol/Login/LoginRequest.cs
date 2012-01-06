using System;

namespace Protocol.Login
{
    [Serializable]
    public class LoginRequest : IRequest
    {
        public int Number { get; set; }
        public string PasswordHash { get; set; }
    }
}
