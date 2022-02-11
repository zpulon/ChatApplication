using System;
using System.Security.Cryptography;
using System.Text;

namespace ApiCore.Utils
{
    public class SecurityHelper
    {
        /// <summary>
        /// AES密文处理
        /// </summary>
        public class AES
        {
            /// <summary>    
            /// AES支持128（16字节） 196（24字节） 256（32字节）位加密    
            /// </summary>    
            private string _key = "E52F23C158B8FD3F79C9FD0FACAFE6F9";


            /// <summary>
            /// AES加密
            /// </summary>
            /// <param name="text">明文</param>
            /// <param name="key">密钥</param>
            /// <returns>密文</returns>
            public string Encrypt(string text, string key)
            {
                if (string.IsNullOrEmpty(text)) return text;

                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(text);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }

            /// <summary>
            /// AES加密
            /// </summary>
            /// <param name="text">明文</param>
            /// <returns>密文</returns>
            public string Encrypt(string text)
            {
                return Encrypt(text, _key);
            }

            /// <summary>
            /// AES解密
            /// </summary>
            /// <param name="ciphertext">密文</param>
            /// <param name="key">密钥</param>
            /// <returns>明文</returns>
            public string Decrypt(string ciphertext, string key)
            {
                var res = string.Empty;
                if (string.IsNullOrEmpty(ciphertext))
                {
                    return ciphertext;
                }
                try
                {
                    byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
                    byte[] toEncryptArray = Convert.FromBase64String(ciphertext);

                    RijndaelManaged rDel = new RijndaelManaged();
                    rDel.Key = keyArray;
                    rDel.Mode = CipherMode.ECB;
                    rDel.Padding = PaddingMode.PKCS7;

                    ICryptoTransform cTransform = rDel.CreateDecryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    res = UTF8Encoding.UTF8.GetString(resultArray);
                }
                catch
                {
                    res = ciphertext;
                }
                return res;
            }

            /// <summary>
            /// AES解密
            /// </summary>
            /// <param name="ciphertext">密文</param>
            /// <returns>明文</returns>
            public string Decrypt(string ciphertext)
            {
                return Decrypt(ciphertext, _key);
            }
        }

        /// <summary>
        /// MD5加密处理
        /// </summary>
        public class MD5 {
            private string _key = "f118a78db5924e899fe8baf850eb3261";
            /// <summary>
            /// MD5加密
            /// </summary>
            /// <param name="text">明文</param>
            /// <returns>密文</returns>
            public string Create(string text)
            {
                if (string.IsNullOrEmpty(text)) return text;

                using (var md5 = System.Security.Cryptography.MD5.Create()) {
                    byte[] bytes = Encoding.UTF8.GetBytes(text);
                    var md5String = Convert.ToBase64String(md5.ComputeHash(bytes));
                    return md5String;
                }
            }

            /// <summary>
            /// MD5加密 追加签名固定值
            /// </summary>
            /// <param name="text">明文</param>
            /// <returns>密文</returns>
            public string CreateBySign(string text) {
                return Create(text + _key);
            }
        }
    }
}
