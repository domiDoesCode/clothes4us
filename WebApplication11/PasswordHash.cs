using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication11
{
    public class PasswordHash
    {
        private readonly StringBuilder _sb;
        public PasswordHash() {
            _sb = new StringBuilder();
        }

        public string HashPassword(string password) {
            foreach (byte b in GetHash(password)) {
                _sb.Append(b.ToString("X2"));
            }
            return _sb.ToString();
        }

        public byte[] GetHash(string password) {
            using (HashAlgorithm algorithm = SHA256.Create()) {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
