using System;

namespace Protocol.Registration
{
    [Serializable]
    public class RegisterRequest : IRequest
    {
        public string Password { get; set; }
    }
}
