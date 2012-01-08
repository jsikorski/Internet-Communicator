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
        public string GetHash(string text)
        {
            string salt = BCryptHelper.GenerateSalt();
            return BCryptHelper.HashPassword(text, salt);
        }
    }
}
