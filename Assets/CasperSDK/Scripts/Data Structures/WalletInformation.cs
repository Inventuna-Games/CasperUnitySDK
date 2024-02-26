
using System.Collections.Generic;

namespace CasperSDK.DataStructures
{
    [System.Serializable]
    public struct WalletInformation
    {
        public string walletPublicKey;
        public List<NFTInformation> walletNFTs;
        public List<TokenInformation> walletTokens;
        public float walletBalance;
    }
}
