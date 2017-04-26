using System;
using System.IO;
using System.Security.Cryptography;

namespace BachMaiCR.Utilities
{
    public static class CheckSum
    {
        public static string Sha1OfFile(string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
                return CheckSum.Sha1OfFile((Stream)fileStream);
        }

        public static string Sha1OfFile(Stream fileStream)
        {
            return BitConverter.ToString(new SHA1Managed().ComputeHash(fileStream)).Replace("-", string.Empty);
        }

        public static string Md5OfFile(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
        }

        public static string Md5OfFile(string filePath)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                    return BitConverter.ToString(md5.ComputeHash((Stream)fileStream)).Replace("-", string.Empty);
            }
        }
    }
}