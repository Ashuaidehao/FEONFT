using System.Security.Cryptography;

namespace FEOTestApp
{

    public static class Extensions
    {
        public static bool LessThan(this byte[] left, byte[] right)
        {
            if (left.Length != right.Length) throw new Exception("left and right have diff length");
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] == right[i]) continue;
                return left[i] < right[i];
            }
            return false;
        }


        public static byte[] Sha256(this byte[] data)
        {
            using SHA256 hash = SHA256.Create();
            return hash.ComputeHash(data);
        }


        public static string ToHex(this byte[] data)
        {
            return Convert.ToHexString(data);
        }


        public static byte[] HexToBytes(this string hex)
        {
            return Convert.FromHexString(hex);

        }
    }
}
