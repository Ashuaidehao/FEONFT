using System.Numerics;
using Neo;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services;

namespace FEONFT
{
    public partial class FEO
    {
        // When this contract address is included in the transaction signature,
        // this method will be triggered as a VerificationTrigger to verify that the signature is correct.
        // For example, this method needs to be called when withdrawing token from the contract.
        public static bool Verify() => Runtime.CheckWitness(GetOwner());

        #region Owner

        /// <summary>
        /// 获取合约管理员
        /// </summary>
        /// <returns></returns>
        public static UInt160 GetOwner()
        {
            return OwnerStorage.GetOwner();
        }

        /// <summary>
        /// 设置合约管理员
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static bool SetOwner(UInt160 owner)
        {
            Assert(IsAddress(owner), "Invalid Address");
            Assert(Runtime.CheckWitness(GetOwner()), "Forbidden");
            OwnerStorage.SetOwner(owner);
            return true;
        }

        #endregion

        #region SaleState

        public static bool GetSaleState()
        {
            var data = (BigInteger)StorageGet(_prefixSaleIsActive);
            return data > 0;
        }

        public static void SetSaleState(int state)
        {
            Assert(Verify(), "No authorization.");
            StoragePut(_prefixSaleIsActive, state);
        }

        public static void FlipSaleState()
        {
            Assert(Verify(), "No authorization.");
            var state = GetSaleState();
            StoragePut(_prefixSaleIsActive, state ? 0 : 1);
        }

        #endregion

        #region MerkleRoot

        public static ByteString GetMerkleRoot()
        {
            var data = StorageGet(_prefixMerkleRoot);
            return data;
        }

        public static void SetMerkleRoot(byte[] merkleRoot)
        {
            Assert(Verify(), "No authorization.");
            Assert(merkleRoot.Length == 32, "Invalid Merkle Root Length");
            StoragePut(_prefixMerkleRoot, merkleRoot);
        }

        #endregion

        #region WlPrice

        public static BigInteger GetWlPrice()
        {
            var data = (BigInteger)StorageGet(_prefixWLPrice);
            return data;
        }

        public static void SetWlPrice(int wlPrice)
        {
            Assert(Verify(), "No authorization.");
            StoragePut(_prefixWLPrice, wlPrice);
        }

        #endregion

        #region WLMintTime

        public static BigInteger[] GetWLMintTime()
        {
            var start = (BigInteger)StorageGet(_prefixWLMintStartTime);
            var end = (BigInteger)StorageGet(_prefixWLMintEndTime);
            return new BigInteger[] { start, end };
        }

        public static void SetWLMintTime(BigInteger start, BigInteger end)
        {
            Assert(Verify(), "No authorization.");
            StoragePut(_prefixWLMintStartTime, start);
            StoragePut(_prefixWLMintEndTime, end);
        }

        #endregion

        #region MaxWLMint

        /// <summary>
        /// 每个白名单账号允许mint的数量
        /// </summary>
        /// <returns></returns>
        public static BigInteger GetMaxWLMint()
        {
            var data = (BigInteger)StorageGet(_prefixMaxWLMinted);
            return data;
        }

        /// <summary>
        /// 设置每个白名单账号允许mint的数量
        /// </summary>
        /// <param name="maxWLMint"></param>
        public static void SetMaxWLMint(int maxWLMint)
        {
            Assert(Verify(), "No authorization.");
            StoragePut(_prefixMaxWLMinted, maxWLMint);
        }

        #endregion


        #region WLTotalMinted

        /// <summary>
        /// 获取白名单已mint数量
        /// </summary>
        /// <returns></returns>
        public static BigInteger GetWLTotalMinted()
        {
            var data = (BigInteger)StorageGet(_prefixWLTotalMinted);
            return data;
        }

        private static void SetWLTotalMinted(BigInteger wlTotalMinted)
        {
            StoragePut(_prefixWLTotalMinted, wlTotalMinted);
        }

        #endregion


        #region publicTotalMinted

        /// <summary>
        /// 获取public已mint数量
        /// </summary>
        /// <returns></returns>
        public static BigInteger GetPublicTotalMinted()
        {
            return TotalMintStorage.GetPublicTotalMinted();
        }


        #endregion

        #region PPrice

        /// <summary>
        /// public mint费
        /// </summary>
        /// <returns></returns>
        public static BigInteger GetPPrice()
        {
            var data = (BigInteger)StorageGet(_prefixPPrice);
            return data;
        }

        public static void SetPPrice(int pPrice)
        {
            Assert(Verify(), "No authorization.");
            StoragePut(_prefixPPrice, pPrice);
        }

        #endregion

        #region PMintTime

        public static BigInteger[] GetPMintTime()
        {
            var start = (BigInteger)StorageGet(_prefixPMintStartTime);
            var end = (BigInteger)StorageGet(_prefixPMintEndTime);
            return new BigInteger[] { start, end };
        }

        public static void SetPMintTime(BigInteger start, BigInteger end)
        {
            Assert(Verify(), "No authorization.");
            StoragePut(_prefixPMintStartTime, start);
            StoragePut(_prefixPMintEndTime, end);
        }

        #endregion


        #region whitelistLimitMinted

        /// <summary>
        /// 白名单总共可mint数量
        /// </summary>
        /// <returns></returns>
        public static BigInteger GetWhitelistLimitMinted()
        {
            var data = (BigInteger)StorageGet(_prefixWhitelistLimitMinted);
            return data;
        }

        public static void SetWhitelistLimitMinted(int whitelistLimitMinted)
        {
            Assert(Verify(), "No authorization.");
            StoragePut(_prefixWhitelistLimitMinted, whitelistLimitMinted);
        }

        #endregion

        #region MaxSupply

        /// <summary>
        /// 总共可mint数量
        /// </summary>
        /// <returns></returns>
        public static BigInteger GetMaxSupply()
        {
            var data = (BigInteger)StorageGet(_prefixMaxSupply);
            return data;
        }

        public static void SetMaxSupply(int maxSupply)
        {
            Assert(Verify(), "No authorization.");
            StoragePut(_prefixMaxSupply, maxSupply);
        }

        #endregion

        #region StartIndex

        
        public static BigInteger GetStartIndex()
        {
            var data = (BigInteger)StorageGet(_prefixStartIndex);
            return data;
        }

        public static void SetStartIndex(BigInteger startIndex)
        {
            Assert(Verify(), "No authorization.");
            StoragePut(_prefixStartIndex, startIndex);
        }

        #endregion

        #region Upgrade

        /// <summary>
        /// 升级
        /// </summary>
        /// <param name="nefFile"></param>
        /// <param name="manifest"></param>
        /// <param name="data"></param>
        public static void Update(ByteString nefFile, string manifest, object data)
        {
            Assert(Verify(), "No authorization.");
            ContractManagement.Update(nefFile, manifest, data);
        }

        #endregion

    }

}