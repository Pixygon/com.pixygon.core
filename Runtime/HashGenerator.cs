using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Pixygon.Core {
    public class HashGenerator : MonoBehaviour {
        public static string CreateHash(string s) {
            using(SHA256 hash = SHA256.Create()) {
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(s));
                StringBuilder stringBuilder = new StringBuilder();
                for(int i = 0; i < bytes.Length; i++) {
                    stringBuilder.Append(bytes[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }
        public static string Base64Encode(string s) {
            var plainTextBytes = Encoding.UTF8.GetBytes(s);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
