using System;

namespace Protocol.Register
{
    [Serializable]
    public class RegisterRequest : IRequest
    {
        public string Password { get; set; }
    }
}
