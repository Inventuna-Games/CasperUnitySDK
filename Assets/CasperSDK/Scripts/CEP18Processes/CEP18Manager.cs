using System;
using System.Collections;
using UnityEngine;
using CasperSDK.Auth;
using CasperSDK.DataStructures;
using CasperSDK.WebRequests;
using UnityEngine.Networking;

namespace CasperSDK.NFT
{
    public class CEP18Manager : MonoBehaviour
    {
        
        public static CEP18Manager Instance;
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
        public Action OnTransferCEP18Started;
        public Action<ServiceResponse> OnTransferCEP18Respond;
        
        public IEnumerator StartTransferCEP18Routine(float Amount , string ContractHash , string TargetWalletPublicKey)
        {
            string ReturnedString = String.Empty;
            OnTransferCEP18Started?.Invoke();


            string uri = Constants.TransferCEP18Uri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("amount", Amount.ToString());
            body.AddField("contractPackageHash", ContractHash);
            body.AddField("recipient", TargetWalletPublicKey);
            Debug.Log(AuthManager.Instance.CurrentSessionToken + " // " + Amount + " // " + ContractHash + " // " + TargetWalletPublicKey);
            yield return WebRequestManager.PostRequest(uri, body, returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log(DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                StartCoroutine(CheckTransactionCEP18State(DTO.data));
            }
            else 
            {
                ServiceResponse Respond = new ServiceResponse();
                Respond.status = RequestStatus.Success;
                Respond.message = "Transaction Token Returned Null";
                OnTransferCEP18Respond?.Invoke(Respond);
            }
        }

        

        private IEnumerator CheckTransactionCEP18State(string TransactionToken)
        {
            string ReturnedString = String.Empty;
            while (true)
            {
                string uri = Constants.CheckTransferCEP18Uri;
            
                WWWForm body = new WWWForm();
                body.AddField("token", TransactionToken);

                yield return WebRequestManager.PostRequest(uri, body,returnValue => {
                    ReturnedString = returnValue;
                });

                GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);
                Debug.Log(DTO.data + " : " + DTO.status);

                if (DTO.status == RequestStatus.Success)
                {
                    ServiceResponse DataProcessRespond = new ServiceResponse();
                    DataProcessRespond.status = RequestStatus.Success;
                    DataProcessRespond.message = "Transaction Completed";
                    OnTransferCEP18Respond?.Invoke(DataProcessRespond);
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
        public Action OnBurnCEP18Started;
        public Action<ServiceResponse> OnBurnCEP18Respond;
        
        public IEnumerator StartBurnCEP18Routine(float Amount , string ContractHash)
        {
            string ReturnedString = String.Empty;
            OnBurnCEP18Started?.Invoke();

            string uri = Constants.BurnCEP18Uri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("amount", Amount.ToString());
            body.AddField("owner", AuthManager.Instance.CurrentAuthorisedWalletPublicKey);
            body.AddField("contractPackageHash", ContractHash);
            Debug.Log("Burn Started : "+AuthManager.Instance.CurrentSessionToken + " // " + Amount + " // " + ContractHash );
            yield return WebRequestManager.PostRequest(uri, body,returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log("Burn : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                StartCoroutine(CheckBurnCEP18State(DTO.data));
            }
            else 
            {
                ServiceResponse Respond = new ServiceResponse();
                Respond.status = RequestStatus.Success;
                Respond.message = "Burn Token Returned Null";
                OnBurnCEP18Respond?.Invoke(Respond);
            }
        }

        

        private IEnumerator CheckBurnCEP18State(string TransactionToken)
        {
            string ReturnedString = String.Empty;
            while (true)
            {
                
                string uri = Constants.CheckBurnCEP18Uri;
            
                WWWForm body = new WWWForm();
                body.AddField("token", TransactionToken);

                yield return WebRequestManager.PostRequest(uri, body, returnValue => {
                    ReturnedString = returnValue;
                });

                GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);
                Debug.Log(DTO.data + " : " + DTO.status);

                if (DTO.status == RequestStatus.Success)
                {
                    ServiceResponse DataProcessRespond = new ServiceResponse();
                    DataProcessRespond.status = RequestStatus.Success;
                    DataProcessRespond.message = "Burn Completed";
                    OnBurnCEP18Respond?.Invoke(DataProcessRespond);
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
        public Action OnMintCEP18Started;
        public Action<ServiceResponse> OnMintCEP18Respond;
        public IEnumerator StartMintRoutine(float Amount , string ContractHash)
        {
            string ReturnedString = String.Empty;
            OnMintCEP18Started?.Invoke();


            string uri = Constants.MintCEP18ri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("amount", Amount.ToString());
            body.AddField("owner", AuthManager.Instance.CurrentAuthorisedWalletPublicKey);
            body.AddField("contractPackageHash", ContractHash);
            Debug.Log("Mint Started : "+AuthManager.Instance.CurrentSessionToken + " // " + Amount + " // " + ContractHash );
            yield return WebRequestManager.PostRequest(uri, body,returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log("Mint : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                StartCoroutine(CheckMintCEP18State(DTO.data));
            }
            else 
            {
                ServiceResponse Respond = new ServiceResponse();
                Respond.status = RequestStatus.Success;
                Respond.message = "Mint Token Returned Null";
                OnMintCEP18Respond?.Invoke(Respond);
            }
        }

        

        private IEnumerator CheckMintCEP18State(string TransactionToken)
        {
            string ReturnedString = String.Empty;
            while (true)
            {
                string uri = Constants.CheckMintCEP18Uri;
            
                WWWForm body = new WWWForm();
                body.AddField("token", TransactionToken);

                yield return WebRequestManager.PostRequest(uri, body, returnValue => {
                    ReturnedString = returnValue;
                });

                GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);
                Debug.Log(DTO.data + " : " + DTO.status);

                if (DTO.status == RequestStatus.Success)
                {
                    ServiceResponse DataProcessRespond = new ServiceResponse();
                    DataProcessRespond.status = RequestStatus.Success;
                    DataProcessRespond.message = "Burn Completed";
                    OnMintCEP18Respond?.Invoke(DataProcessRespond);
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
        
        #region ApproveCEP18
        public Action OnApproveCEP18Started;
        public Action<ServiceResponse> OnApproveCEP18Respond;
        
        public IEnumerator StartApproveCEP18Routine(float Amount , string ContractHash)
        {
            string ReturnedString = String.Empty;
            OnApproveCEP18Started?.Invoke();

            string uri = Constants.ApproveUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("amount", Amount.ToString());
            body.AddField("spender", AuthManager.Instance.CurrentAuthorisedWalletPublicKey);
            body.AddField("contractPackageHash", ContractHash);
            Debug.Log("Register Owner Started : "+AuthManager.Instance.CurrentSessionToken + " // " + Amount + " // " + ContractHash );
            yield return WebRequestManager.PostRequest(uri, body,returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log("Approve : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                StartCoroutine(CheckApproveCEP18State(DTO.data));
            }
            else 
            {
                ServiceResponse Respond = new ServiceResponse();
                Respond.status = RequestStatus.Success;
                Respond.message = "Approve Token Returned Null";
                OnApproveCEP18Respond?.Invoke(Respond);
            }
        }

        

        private IEnumerator CheckApproveCEP18State(string TransactionToken)
        {
            string ReturnedString = String.Empty;
            while (true)
            {
                string uri = Constants.CheckApproveUri;
            
                WWWForm body = new WWWForm();
                body.AddField("token", TransactionToken);

                yield return WebRequestManager.PostRequest(uri, body, returnValue => {
                    ReturnedString = returnValue;
                });

                GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);
                Debug.Log(DTO.data + " : " + DTO.status);

                if (DTO.status == RequestStatus.Success)
                {
                    ServiceResponse DataProcessRespond = new ServiceResponse();
                    DataProcessRespond.status = RequestStatus.Success;
                    DataProcessRespond.message = "Register Owner Completed";
                    OnApproveCEP18Respond?.Invoke(DataProcessRespond);
                    Debug.Log("Approve CEP18 Completed with ID : " + DTO.data);
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

