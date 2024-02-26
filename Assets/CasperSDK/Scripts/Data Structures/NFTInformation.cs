namespace CasperSDK.DataStructures
{
    [System.Serializable]
    public struct NFTInformation
    {
        public string contract_package_hash;
        public bool is_burned;
        public string onchain_metadata; //This is game spesific , replace string with your own struct/class to serialise.
        public string token_id;
    }
}