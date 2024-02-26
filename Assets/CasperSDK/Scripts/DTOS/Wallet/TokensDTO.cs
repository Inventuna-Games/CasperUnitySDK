using System.Collections.Generic;

namespace CasperSDK.DataStructures
{
    [System.Serializable]
    public struct TokensDTO
    {
        public RequestStatus status;
        public List<TokenInformation> data;
    }
}