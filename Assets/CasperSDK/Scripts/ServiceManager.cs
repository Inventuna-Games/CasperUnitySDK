using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using CasperSDK.DataStructures;
using CasperSDK.NFT;
using CasperSDK.Auth;
using CasperSDK.WalletData;

namespace CasperSDK
{
    public class ServiceManager : MonoBehaviour
    {
        #region Singleton Instance
        public static ServiceManager Instance;
        #endregion
        
        #region Mono Functions
        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
            
            if (AuthManager.Instance == null)
            {
                gameObject.AddComponent<AuthManager>();
            }
            if (WalletDataManager.Instance == null)
            {
                gameObject.AddComponent<WalletDataManager>();
            }
            if (NFTManager.Instance == null)
            {
                gameObject.AddComponent<NFTManager>();
            }
            if (CEP18Manager.Instance == null)
            {
                gameObject.AddComponent<CEP18Manager>();
            }
            
            
        }
        #endregion

        #region Exposed Functions
        /// <summary>
        /// Starts Authorization Process
        /// </summary>
        public void Authorize()
        {
            StartCoroutine(AuthManager.Instance.StartAuthorization());
        }
        /// <summary>
        /// Gets all wallet information(NFTs , Tokens , Casper Balance) and writes it into CurrentWalletInformation , also fires OnWalletInformationUpdated in a completed update.
        /// </summary>
        public void UpdateWalletInformation()
        {
            StartCoroutine(WalletDataManager.Instance.UpdateWalletInformationRunner());
        }
        /// <summary>
        /// Starts the transfer with given NFT and TargetWallet.
        /// </summary>
        public void StartTransferNFT(NFTInformation NFTInformation , string TargetWallet)
        {
            StartCoroutine(NFTManager.Instance.StartTransferRoutine(NFTInformation.token_id , NFTInformation.contract_package_hash ,TargetWallet));
        }
        
        public void StartBurnNFT(NFTInformation NFTInformation)
        {
            Debug.Log(NFTInformation.token_id);
            StartCoroutine(NFTManager.Instance.StartBurnRoutine(NFTInformation.token_id , NFTInformation.contract_package_hash));
        }
        public void StartMintNFT( string contractPackageHash, string MetaData)
        {
            StartCoroutine(NFTManager.Instance.StartMintRoutine(contractPackageHash ,MetaData));
        }
        public void StartRegisterOwnerNFT(NFTInformation NFTInformation)
        {
            StartCoroutine(NFTManager.Instance.StartRegisterOwnerRoutine(NFTInformation.token_id , NFTInformation.contract_package_hash));
        }
        
        public void StartTransferCEP18(float Amount ,string ContractHash , string TargetWallet)
        {
            StartCoroutine(CEP18Manager.Instance.StartTransferCEP18Routine(Amount , ContractHash ,TargetWallet));
        }
        
        public void StartBurnCEP18(float Amount ,string ContractHash)
        {
            StartCoroutine(CEP18Manager.Instance.StartBurnCEP18Routine(Amount , ContractHash));
        }
        public void StartMintCEP18(float Amount ,string ContractHash)
        {
            StartCoroutine(CEP18Manager.Instance.StartMintRoutine(Amount ,ContractHash));
        }
        public void StartApproveCEP18(float Amount ,string ContractHash)
        {
            StartCoroutine(CEP18Manager.Instance.StartApproveCEP18Routine(Amount , ContractHash));
        }

        #endregion
    }
}
