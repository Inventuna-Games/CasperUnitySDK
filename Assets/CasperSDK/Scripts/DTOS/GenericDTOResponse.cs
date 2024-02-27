namespace CasperSDK.DataStructures
{
    [System.Serializable]
    public struct GenericDTOResponse<T>
    {
        public RequestStatus status;
        public T data;
    }
}