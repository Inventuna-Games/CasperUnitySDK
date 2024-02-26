using System.Collections.Generic;

namespace CasperSDK.DataStructures
{
    [System.Serializable]
    public struct NFTSDto
    {
        public RequestStatus status;
        public List<NFTInformation> data;
    }
}