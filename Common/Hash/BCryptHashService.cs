using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DevOne.Security.Cryptography.BCrypt;

namespace Common.Hash
{
    public class BCryptHashService : IHashService
    {
        private const string Salt = "$2a$10$vI8aWBnW3fID.ZQ4/zo1G.q1lRps.9cGLcZEiGDMVr5yUP1KUOYTa";

        public string GetHash(string text)
        {
            return BCryptHelper.HashPassword(text, Salt);
        }
    }
}
