namespace CasperSDK.DataStructures
{
    [System.Serializable]
    public struct TokenInformation
    {
        public double balance;
        public TokenContractPackage contract_package;
        public string contract_package_hash;
        public string owner_hash;
    }
}
