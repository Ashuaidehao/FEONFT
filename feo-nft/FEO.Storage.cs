using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services;
using System.Numerics;
using Neo;

namespace FEONFT
{
    public partial class FEO
    {
        private static readonly byte[] _prefixOwner = { 0xfe, 0x01 };
        private static readonly byte[] _prefixMerkleRoot = { 0xfe, 0x02 };
        private static readonly byte[] _prefixSaleIsActive = { 0xfe, 0x03 };
        private static readonly byte[] _prefixPMintStartTime = { 0xfe, 0x04 };
        private static readonly byte[] _prefixPMintEndTime = { 0xfe, 0x05 };
        private static readonly byte[] _prefixPPrice = { 0xfe, 0x06 };

        private static readonly byte[] _prefixMaxWLMinted = { 0xfe, 0x07 };
        private static readonly byte[] _prefixWLPrice = { 0xfe, 0x08 };
        private static readonly byte[] _prefixWLMintStartTime = { 0xfe, 0x09 };
        private static readonly byte[] _prefixWLMintEndTime = { 0xfe, 0x10 };
        private static readonly byte[] _prefixPublicTotalMinted = { 0xfe, 0x11 };
        private static readonly byte[] _prefixWLTotalMinted = { 0xfe, 0x12 };
        private static readonly byte[] _prefixMaxSupply = { 0xfe, 0x13 };
        private static readonly byte[] _prefixWhitelistLimitMinted = { 0xfe, 0x14 };

        private static readonly byte[] _prefixStartIndex = { 0xfe, 0x15 };

        private static readonly byte[] _prefixFEOMinted = { 0xfe, 0x16 };
        private static readonly byte[] _prefixMintedCount = { 0xfe, 0x17 };
        private static readonly byte[] _prefixEntered = { 0xfe, 0x18 };



        //private static readonly byte[] BalancePrefix = "balance".ToByteArray();




        class OwnerStorage
        {
            public static UInt160 GetOwner()
            {
                var data = StorageGet(_prefixOwner);
                return data?.Length == 20 ? (UInt160)data : superAdmin;
            }

            public static void SetOwner(UInt160 owner)
            {
                StoragePut(_prefixOwner, owner);
            }
        }


        class TotalMintStorage
        {
            /// <summary>
            /// 获取public 已mint数量
            /// </summary>
            /// <returns></returns>
            public static BigInteger GetPublicTotalMinted()
            {
                var data = (BigInteger)StorageGet(_prefixPublicTotalMinted);
                return data;
            }

            /// <summary>
            /// 新增public 已mint数量
            /// </summary>
            /// <param name="newPublicMinted"></param>
            public static void AddPublicTotalMinted(BigInteger newPublicMinted)
            {
                StoragePut(_prefixPublicTotalMinted, GetPublicTotalMinted() + newPublicMinted);
            }

            /// <summary>
            /// 设置public 已mint数量
            /// </summary>
            /// <param name="publicTotalMinted"></param>
            public static void SetPublicTotalMinted(BigInteger publicTotalMinted)
            {
                StoragePut(_prefixPublicTotalMinted, publicTotalMinted);
            }




            /// <summary>
            /// 获取 FEO 已mint数量
            /// </summary>
            /// <returns></returns>
            public static BigInteger GetFEOTotalMinted()
            {
                var data = (BigInteger)StorageGet(_prefixFEOMinted);
                return data;
            }

            /// <summary>
            /// 新增 FEO 已mint数量
            /// </summary>
            /// <param name="newMinted"></param>
            public static void AddFEOTotalMinted(BigInteger newMinted)
            {
                StoragePut(_prefixFEOMinted, GetFEOTotalMinted() + newMinted);
            }

            /// <summary>
            /// 设置 FEO 已mint数量
            /// </summary>
            /// <param name="totalMinted"></param>
            public static void SetFEOTotalMinted(BigInteger totalMinted)
            {
                StoragePut(_prefixFEOMinted, totalMinted);
            }
        }

        class MintStorage
        {
            /// <summary>
            /// 获取账号mint的数量
            /// </summary>
            /// <param name="sender"></param>
            /// <returns></returns>
            public static BigInteger GetMintedCount(UInt160 sender)
            {
                var data = (BigInteger)StorageGet(_prefixMintedCount.Concat(sender));
                return data;
            }

            /// <summary>
            /// 增加账号mint的数量
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="quantity"></param>
            /// <returns></returns>
            public static BigInteger AddMintedCount(UInt160 sender, BigInteger quantity)
            {
                var minted = GetMintedCount(sender) + quantity;
                StoragePut(_prefixMintedCount.Concat(sender), minted);
                return minted;
            }
        }

        class IndexStorage
        {
            /// <summary>
            /// 获取当前可mint的token起始index
            /// </summary>
            /// <returns></returns>
            public static BigInteger GetStartIndex()
            {
                var data = (BigInteger)StorageGet(_prefixStartIndex);
                return data;
            }

            public static void AddStartIndex(BigInteger quantity)
            {
                StoragePut(_prefixStartIndex, GetStartIndex() + quantity);
            }

            /// <summary>
            /// 设置当前可mint的token起始index
            /// </summary>
            /// <param name="startIndex"></param>
            public static void SetStartIndex(BigInteger startIndex)
            {
                StoragePut(_prefixStartIndex, startIndex);
            }
        }


        public class EnteredStorage
        {
            public static bool TryEnter(ByteString key)
            {
                if (IsEntered(key)) return false;
                StoragePut(_prefixEntered.Concat(key), 1);
                return true;
            }

            public static void Enter(ByteString key)
            {
                StoragePut(_prefixEntered.Concat(key), 1);
            }

            public static bool IsEntered(ByteString key)
            {
                return (BigInteger)StorageGet(_prefixEntered.Concat(key)) > 0;
            }

            public static void Out(ByteString key)
            {
                StorageDelete(_prefixEntered.Concat(key));
            }
        }
    }
}