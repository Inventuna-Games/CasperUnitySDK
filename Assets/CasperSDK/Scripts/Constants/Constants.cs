namespace CasperSDK.DataStructures
{
    static class Constants
    {
        public const string BaseURL = "http://3.74.185.68:210";
        
        public const string WebsiteTokenConnectionURL = BaseURL +"/index.html?token=";
        
        #region Auth
        
        public const string AuthorizeUri = BaseURL + "/index.html?token=";
        
        public const string AuthorizeTransferUri = BaseURL +"/index.html?token=";
        
        public const string LoginUri = BaseURL +"/api/login-request";
        
        public const string CheckLoginUri = BaseURL +"/api/check-login";
        
        #endregion

        #region WalletINFO

        public const string GetBalanceUri = BaseURL +"/api/get-balance";
            
        public const string GetTokensUri = BaseURL +"/api/get-tokens";
            
        public const string GetNFTsUri = BaseURL +"/api/get-nfts";

        #endregion

        #region NFTS

        public const string TransferNFTUri = BaseURL +"/api/transfer-nft-request";
            
        public const string CheckTransferNFTUri = BaseURL +"/api/check-transfer-nft";
        
        public const string MintNFTri = BaseURL +"/api/mint-nft-request";
        public const string CheckMintNFTUri = BaseURL +"/api/check-mint-nft-request";
        
        public const string BurnNFTUri = BaseURL +"/api/burn-nft-request";
        public const string CheckBurnNFTUri = BaseURL +"/api/check-burn-nft-request";
        
        public const string RegisterOwnerNFTUri = BaseURL +"/api/register-owner-nft-request";
        public const string CheckRegisterOwnerNFTUri = BaseURL +"/api/check-register-owner-nft-request";

        #endregion

        #region CEP18

        public const string TransferCEP18Uri = BaseURL +"/api/transfer-cep18-token-request";
            
        public const string CheckTransferCEP18Uri =BaseURL + "/api/check-transfer-cep18-token-request";
        
        public const string MintCEP18ri = BaseURL +"/api/mint-cep18-token-request";
        public const string CheckMintCEP18Uri = BaseURL +"/api/check-mint-cep18-token-request";
        
        public const string BurnCEP18Uri = BaseURL +"/api/burn-cep18-token-request";
        public const string CheckBurnCEP18Uri = BaseURL +"/api/check-burn-cep18-token-request";
        
        public const string ApproveUri = BaseURL + "/api/approve-cep18-token-request";
        public const string CheckApproveUri = BaseURL +"/api/check-approve-cep18-token-request";

        #endregion
            
    }
}
