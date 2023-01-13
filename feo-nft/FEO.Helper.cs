using Neo;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services;
using System;
using System.Numerics;

namespace FEONFT
{
    public partial class FEO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception(message);
                // ExecutionEngine.Assert(false, message);
            }
        }


        private static ByteString StorageGet(ByteString key)
        {
            return Storage.Get(Storage.CurrentContext, key);
        }

        private static ByteString StorageGet(byte[] key)
        {
            return Storage.Get(Storage.CurrentContext, key);
        }

        private static void StoragePut(ByteString key, ByteString value)
        {
            Storage.Put(Storage.CurrentContext, key, value);
        }

        private static void StoragePut(ByteString key, byte[] value)
        {
            Storage.Put(Storage.CurrentContext, key, (ByteString)value);
        }

        private static void StoragePut(ByteString key, BigInteger value)
        {
            Storage.Put(Storage.CurrentContext, key, value);
        }

        private static void StoragePut(byte[] key, byte[] value)
        {
            Storage.Put(Storage.CurrentContext, key, (ByteString)value);
        }

        private static void StoragePut(byte[] key, BigInteger value)
        {
            Storage.Put(Storage.CurrentContext, key, value);
        }

        private static void StoragePut(byte[] key, ByteString value)
        {
            Storage.Put(Storage.CurrentContext, key, value);
        }



        private static void StorageDelete(ByteString key)
        {
            Storage.Delete(Storage.CurrentContext, key);
        }

        private static void StorageDelete(byte[] key)
        {
            Storage.Delete(Storage.CurrentContext, (ByteString)key);
        }


        /// <summary>
        /// Is Valid and not Zero address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private static bool IsAddress(UInt160 address)
        {
            return address.IsValid && !address.IsZero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static void SafeTransfer(UInt160 token, UInt160 from, UInt160 to, BigInteger amount, byte[] data = null)
        {
            var result = (bool)Contract.Call(token, "transfer", CallFlags.All, new object[] { from, to, amount, data });
            Assert(result, "Transfer Fail in Pair");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        private static bool Transfer(UInt160 token, UInt160 from, UInt160 to, BigInteger amount, byte[] data = null)
        {
            return (bool)Contract.Call(token, "transfer", CallFlags.All, new object[] { from, to, amount, data });
        }

        //private static bool GreatThan(byte[] left, byte[] right)
        //{
        //    if (left.Length != right.Length) throw new Exception("left and right have diff length");
        //    for (int i = 0; i < left.Length; i++)
        //    {
        //        if (left[i] > right[i]) return true;
        //    }

        //    return false;
        //}


        //private static bool LessThan(byte[] left, byte[] right)
        //{
        //    if (left.Length != right.Length) throw new Exception("left and right have diff length");
        //    for (int i = 0; i < left.Length; i++)
        //    {
        //        if (left[i] < right[i]) return true;
        //    }

        //    return false;
        //}


        private static bool LessThan(ByteString left, ByteString right)
        {
            if (left.Length != right.Length) throw new Exception("left and right have diff length");
            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] == right[i]) continue;
                return left[i] < right[i];
            }

            return false;
        }
    }
}