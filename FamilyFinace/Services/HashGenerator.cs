using FamilyFinace.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FamilyFinace.Services
{
    public class HashGenerator : IHashGenerator
    {
        public string GetHash(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytesArr = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(bytesArr);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
                sb.Append(hashBytes[i].ToString("X2"));

            return sb.ToString();
        }
    }
}
