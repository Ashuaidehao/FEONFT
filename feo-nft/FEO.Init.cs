using Neo.SmartContract.Framework.Attributes;
using Neo;

namespace FEONFT
{
    public partial class FEO
    {
        [InitialValue("NPS3U9PduobRCai5ZUdK2P3Y8RjwzMVfSg", Neo.SmartContract.ContractParameterType.Hash160)]
        static readonly UInt160 superAdmin = default;
    }
}