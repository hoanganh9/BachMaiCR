using System;
using System.Security.Cryptography;
using System.Text;

namespace BachMaiCR.Utilities
{
  public static class Encrypt
  {
    private static readonly Random Rng = new Random();
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

    public static string Sha2HashWithHex(string input)
    {
      SHA256Managed shA256Managed = new SHA256Managed();
      byte[] bytes = Encoding.UTF8.GetBytes(input);
      byte[] hash = shA256Managed.ComputeHash(bytes);
      shA256Managed.Clear();
      return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    public static string Sha1HashWithHex(string input)
    {
      return BitConverter.ToString(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
    }

    public static string Sha1Hash(string input)
    {
      return Encrypt.ToBase64(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input)));
    }

    public static string ToBase64(byte[] data)
    {
      if (data == null || data.Length == 0)
        return string.Empty;
      return Convert.ToBase64String(data);
    }

    public static string DecodeBase64(string encodeString)
    {
      return Encoding.UTF8.GetString(Convert.FromBase64String(encodeString));
    }

    public static byte[] GetBytes(string str)
    {
      byte[] numArray = new byte[str.Length * 2];
      Buffer.BlockCopy((Array) str.ToCharArray(), 0, (Array) numArray, 0, numArray.Length);
      return numArray;
    }

    public static string RandomString(int size)
    {
      char[] chArray = new char[size];
      for (int index = 0; index < size; ++index)
        chArray[index] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890"[Encrypt.Rng.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890".Length)];
      return new string(chArray);
    }
  }
}
