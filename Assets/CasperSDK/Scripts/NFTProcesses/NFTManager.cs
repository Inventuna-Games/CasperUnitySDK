using System;
using System.Collections;
using UnityEngine;
using CasperSDK.Auth;
using CasperSDK.DataStructures;
using CasperSDK.WebRequests;

namespace CasperSDK.NFT
{
    public class NFTManager : MonoBehaviour
    {
        
        public static NFTManager Instance;
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
        }
        
        #region TransferNFT
        public Action OnTransferStarted;
        public Action<ServiceResponse> OnTransferRespond;
        
        public IEnumerator StartTransferRoutine(string TokenID , string ContractHash , string TargetWalletPublicKey)
        {
            string ReturnedString = String.Empty;
            OnTransferStarted?.Invoke();

            string uri = Constants.TransferNFTUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("tokenId", TokenID);
            body.AddField("contractPackageHash", ContractHash);
            body.AddField("target", TargetWalletPublicKey);
            Debug.Log(AuthManager.Instance.CurrentSessionToken + " // " + TokenID + " // " + ContractHash + " // " + TargetWalletPublicKey);

            yield return WebRequestManager.PostRequest(uri , body , returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log(DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                StartCoroutine(CheckTransactionState(DTO.data));
            }
            else 
            {
                ServiceResponse Respond = new ServiceResponse();
                Respond.status = RequestStatus.Success;
                Respond.message = "Transaction Token Returned Null";
                OnTransferRespond?.Invoke(Respond);
            }
        }

        

        private IEnumerator CheckTransactionState(string TransactionToken)
        {
            string ReturnedString = String.Empty;
            while (true)
            {
                string uri = Constants.CheckTransferNFTUri;
            
                WWWForm body = new WWWForm();
                body.AddField("token", TransactionToken);

                yield return WebRequestManager.PostRequest(uri , body ,returnValue => {
                    ReturnedString = returnValue;
                });

                GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);
                Debug.Log(DTO.data + " : " + DTO.status);

                if (DTO.status == RequestStatus.Success)
                {
                    ServiceResponse DataProcessRespond = new ServiceResponse();
                    DataProcessRespond.status = RequestStatus.Success;
                    DataProcessRespond.message = "Transaction Completed";
                    OnTransferRespond?.Invoke(DataProcessRespond);
                    Debug.Log("Transaction Completed with ID : " + DTO.data);
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(1);
                }
            }
        }
        #endregion
        
        #region BurnNFT
        public Action OnBurnStarted;
        public Action<ServiceResponse> OnBurnRespond;
        
        public IEnumerator StartBurnRoutine(string TokenID , string ContractHash)
        {
            string ReturnedString = String.Empty;
            OnBurnStarted?.Invoke();

            string uri = Constants.BurnNFTUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("tokenId", TokenID);
            body.AddField("contractPackageHash", ContractHash);
            Debug.Log("Burn Started : "+AuthManager.Instance.CurrentSessionToken + " // " + TokenID + " // " + ContractHash );
            yield return WebRequestManager.PostRequest(uri , body , returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log("Burn : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                StartCoroutine(CheckBurnState(DTO.data));
            }
            else 
            {
                ServiceResponse Respond = new ServiceResponse();
                Respond.status = RequestStatus.Success;
                Respond.message = "Burn Token Returned Null";
                OnBurnRespond?.Invoke(Respond);
            }
        }

        

        private IEnumerator CheckBurnState(string TransactionToken)
        {
            string ReturnedString = String.Empty;
            while (true)
            {
                string uri = Constants.CheckBurnNFTUri;
            
                WWWForm body = new WWWForm();
                body.AddField("token", TransactionToken);

                yield return WebRequestManager.PostRequest(uri , body , returnValue => {
                    ReturnedString = returnValue;
                });

                GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);
                Debug.Log(DTO.data + " : " + DTO.status);

                if (DTO.status == RequestStatus.Success)
                {
                    ServiceResponse DataProcessRespond = new ServiceResponse();
                    DataProcessRespond.status = RequestStatus.Success;
                    DataProcessRespond.message = "Burn Completed";
                    OnBurnRespond?.Invoke(DataProcessRespond);
                    Debug.Log("Burn Completed with ID : " + DTO.data);
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(1);
                }
            }
        }
        #endregion
        
        #region MintNFT
        public Action OnMintStarted;
        public Action<ServiceResponse> OnMintRespond;
        
        public IEnumerator StartMintRoutine(string ContractHash , string TokenMetadata)
        {
            string ReturnedString = String.Empty;
            OnMintStarted?.Invoke();

            string uri = Constants.MintNFTri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("token_owner", AuthManager.Instance.CurrentAuthorisedWalletPublicKey);
            body.AddField("contractPackageHash", ContractHash);
            body.AddField("token_meta_data", TokenMetadata);
            Debug.Log("Mint Started : "+AuthManager.Instance.CurrentSessionToken + " // " + ContractHash + " // " + TokenMetadata );
            yield return WebRequestManager.PostRequest(uri , body ,returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log("Mint : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                StartCoroutine(CheckMintState(DTO.data));
            }
            else 
            {
                ServiceResponse Respond = new ServiceResponse();
                Respond.status = RequestStatus.Success;
                Respond.message = "Mint Token Returned Null";
                OnMintRespond?.Invoke(Respond);
            }
        }

        

        private IEnumerator CheckMintState(string TransactionToken)
        {
            string ReturnedString = String.Empty;
            while (true)
            {
                string uri = Constants.CheckMintNFTUri;
            
                WWWForm body = new WWWForm();
                body.AddField("token", TransactionToken);

                yield return WebRequestManager.PostRequest(uri , body , returnValue => {
                    ReturnedString = returnValue;
                });

                GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);
                Debug.Log(DTO.data + " : " + DTO.status);

                if (DTO.status == RequestStatus.Success)
                {
                    ServiceResponse DataProcessRespond = new ServiceResponse();
                    DataProcessRespond.status = RequestStatus.Success;
                    DataProcessRespond.message = "Burn Completed";
                    OnMintRespond?.Invoke(DataProcessRespond);
                    Debug.Log("Mint Completed with ID : " + DTO.data);
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(1);
                }
            }
        }
        
        #endregion
        
        #region RegisterOwnerNFT
        public Action OnRegisterOwnerStarted;
        public Action<ServiceResponse> OnRegisterOwnerRespond;
        
        public IEnumerator StartRegisterOwnerRoutine(string TokenID , string ContractHash)
        {
            string ReturnedString = String.Empty;
            OnRegisterOwnerStarted?.Invoke();

            string uri = Constants.RegisterOwnerNFTUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("tokenId", TokenID);
            body.AddField("contractPackageHash", ContractHash);
            Debug.Log("Register Owner Started : "+AuthManager.Instance.CurrentSessionToken + " // " + TokenID + " // " + ContractHash );
            
            yield return WebRequestManager.PostRequest(uri , body , returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log("Register Owner : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                StartCoroutine(CheckRegisterOwnerState(DTO.data));
            }
            else 
            {
                ServiceResponse Respond = new ServiceResponse();
                Respond.status = RequestStatus.Success;
                Respond.message = "Register Owner Token Returned Null";
                OnRegisterOwnerRespond?.Invoke(Respond);
            }
        }

        

        private IEnumerator CheckRegisterOwnerState(string TransactionToken)
        {
            string ReturnedString = String.Empty;
            while (true)
            {
                
                string uri = Constants.CheckRegisterOwnerNFTUri;
            
                WWWForm body = new WWWForm();
                body.AddField("token", TransactionToken);

                yield return WebRequestManager.PostRequest(uri , body ,  returnValue => {
                    ReturnedString = returnValue;
                });

                GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);
                Debug.Log(DTO.data + " : " + DTO.status);

                if (DTO.status == RequestStatus.Success)
                {
                    ServiceResponse DataProcessRespond = new ServiceResponse();
                    DataProcessRespond.status = RequestStatus.Success;
                    DataProcessRespond.message = "Register Owner Completed";
                    OnRegisterOwnerRespond?.Invoke(DataProcessRespond);
                    Debug.Log("Register Owner Completed with ID : " + DTO.data);
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(1);
                }
            }
        }
        #endregion
        
        
    }
}

