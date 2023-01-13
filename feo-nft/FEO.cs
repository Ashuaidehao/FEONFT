using System;
using System.ComponentModel;
using System.Numerics;
using Neo;
using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Attributes;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;

namespace FEONFT
{
    [DisplayName("feo")]
    [ManifestExtra("Author", "NEO")]
    [ManifestExtra("Email", "developer@neo.org")]
    [ManifestExtra("Description", "This is a feo_nft")]
    [ContractPermission("*")]
    public partial class FEO : Nep11Token<FEOState>
    {

        /// <summary>
        /// 合约初始化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="update"></param>
        public static void _deploy(object data, bool update)
        {
            var sender = ((Transaction)Runtime.ScriptContainer).Sender;
            if (sender != null)
            {
                OwnerStorage.SetOwner(((Transaction)Runtime.ScriptContainer).Sender);
                SetWhitelistLimitMinted(200);
                SetMaxWLMint(2);
                SetPPrice(2);
                SetWlPrice(1);
                SetMaxSupply(400);
            }
        }

        public override string Symbol() => "E1";


        public static void WhitelistMint(ByteString[] merkleProof, int quantity)
        {
            Assert(GetSaleState(), "Not allowed to mint");
            // verify limit per account
            Assert(quantity > 0, "Wrong amount of minting");
            var sender = ((Transaction)Runtime.ScriptContainer).Sender;
            var mintCount = MintStorage.GetMintedCount(sender);
            Assert(mintCount + quantity <= GetMaxWLMint(), "Exceed the limit of mint amount");

            // verify merkle
            var leaf = Hash256(sender);
            Assert(VerifyMerkleRoot(merkleProof, GetMerkleRoot(), leaf), "NOT incorporated in the whitelists");


            // verify start/end time
            var block = Ledger.GetBlock(Ledger.CurrentIndex);
            var wlMintTime = GetWLMintTime();
            Assert(block.Timestamp > wlMintTime[0], "Wrong start time for whitelist to mint");
            Assert(block.Timestamp < wlMintTime[1], "Wrong end time for whitelist to mint");

            // verify mint fee
            var wlPrice = GetWlPrice();
            SafeTransfer(GAS.Hash, sender, Runtime.ExecutingScriptHash, wlPrice * quantity);

            // verify mint max count
            var wlTotalMinted = GetWLTotalMinted();
            var wlLimit = GetWhitelistLimitMinted();
            var maxSupply = GetMaxSupply();
            Assert(wlTotalMinted + quantity <= wlLimit, "sold out");
            Assert(TotalSupply() + quantity <= maxSupply, "sold out");
            SetWLTotalMinted(wlTotalMinted + quantity);
            SafeMint(sender, quantity);
        }


        /// <summary>
        /// public mint
        /// </summary>
        /// <param name="quantity"></param>
        public static void PublicMint(int quantity)
        {
            Assert(GetSaleState(), "Not allowed to mint");
            // verify limit per account
            Assert(quantity > 0, "Wrong amount of minting");
            var sender = ((Transaction)Runtime.ScriptContainer).Sender;

            // verify start/end time
            var block = Ledger.GetBlock(Ledger.CurrentIndex);
            var pMintTime = GetPMintTime();
            Assert(block.Timestamp > pMintTime[0], "Wrong start time for public to mint");
            Assert(block.Timestamp < pMintTime[1], "Wrong end time for public to mint");

            // verify mint fee
            var pPrice = GetPPrice();
            Assert(Transfer(GAS.Hash, sender, Runtime.ExecutingScriptHash, pPrice * quantity), "Insufficient value");

            // verify mint max count
            var maxSupply = GetMaxSupply();
            Assert(TotalSupply() + quantity <= maxSupply, "sold out");

            TotalMintStorage.AddPublicTotalMinted(quantity);
            TotalMintStorage.AddFEOTotalMinted(quantity);
            SafeMint(sender, quantity);
        }

        private static void SafeMint(UInt160 owner, int quantity)
        {
            if (quantity <= 0) return;
            var enterKey = "safemint";
            Assert(EnteredStorage.TryEnter(enterKey), "re-entered error:safemint");

            var startIndex = IndexStorage.GetStartIndex();
            for (BigInteger i = 0; i < quantity; i++)
            {
                var tokenId = startIndex + i;
                Mint((ByteString)tokenId, new FEOState() { Owner = owner, Name = tokenId.ToString() });
            }
            IndexStorage.SetStartIndex(startIndex + quantity);
            MintStorage.AddMintedCount(owner, quantity);
            EnteredStorage.Out(enterKey);
        }


        public static bool VerifyMerkleRoot(ByteString[] proof, ByteString root, ByteString leaf)
        {
            return ProcessProof(proof, leaf) == root;
        }

        public static ByteString ProcessProof(ByteString[] proof, ByteString leaf)
        {
            var computeHash = leaf;
            for (int i = 0; i < proof.Length; i++)
            {
                computeHash = HashPair(computeHash, proof[i]);
                //CryptoLib.Sha256(leaf.Concat(proof[i]))
            }
            return computeHash;
        }

        //public ByteString HashPair(byte[] left, byte[] right)
        //{
        //    return LessThan(left, right) ? SHA(left.Concat(right)) : SHA(right.Concat(left));
        //}


        public static ByteString HashPair(ByteString left, ByteString right)
        {
            return LessThan(left, right) ? Hash256(left.Concat(right)) : Hash256(right.Concat(left));
        }

        public static ByteString Hash256(ByteString data)
        {
            return CryptoLib.Sha256(data);
        }


        /// <summary>
        /// 接受nep17 token必备方法
        /// </summary>
        /// <param name="from"></param>
        /// <param name="amount"></param>
        /// <param name="data"></param>
        public static void OnNEP17Payment(UInt160 from, BigInteger amount, object data)
        {
            //UInt160 asset = Runtime.CallingScriptHash;
            //Assert(asset == Token0 || asset == Token1, "Invalid Asset");
        }
    }

}

