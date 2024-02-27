using System;
using System.Collections;
using UnityEngine;
using CasperSDK.Auth;
using CasperSDK.DataStructures;
using UnityEngine.Networking;

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
        
        Coroutine TransferRoutine;
        
        public IEnumerator StartTransferRoutine(string TokenID , string ContractHash , string TargetWalletPublicKey)
        {
            string ReturnedString = String.Empty;
            OnTransferStarted?.Invoke();


            yield return StartTransferNFT(TokenID,ContractHash,TargetWalletPublicKey,returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log(DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                TransferRoutine = StartCoroutine(CheckTransactionState(DTO.data));
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
                yield return CheckTransferNFT(TransactionToken , returnValue => {
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
            

        #region Web Request Section
        
        private IEnumerator StartTransferNFT(string TokenID , string ContractHash , string TargetWalletPublicKey,System.Action<string> callback)
        {
            string uri = Constants.TransferNFTUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("tokenId", TokenID);
            body.AddField("contractPackageHash", ContractHash);
            body.AddField("target", TargetWalletPublicKey);
            Debug.Log(AuthManager.Instance.CurrentSessionToken + " // " + TokenID + " // " + ContractHash + " // " + TargetWalletPublicKey);
            using (UnityWebRequest www = UnityWebRequest.Post(uri, body))
            {

                //www.SetRequestHeader("id", _walletAdress);

                yield return www.SendWebRequest();

                switch (www.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(": Error: " + www.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError( ": HTTP Error: " + www.error);
                        //DataProvider.GetRequest("Login", "Clear", "" ,x=> { });
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Received: " + www.downloadHandler.text+ "Result: " + www.result);

                        callback(www.downloadHandler.text);
                        break;
                }

            }

        }
        private IEnumerator CheckTransferNFT(string TransactionToken , Action<string> callback)
        {
            string uri = Constants.CheckTransferNFTUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", TransactionToken);

            using (UnityWebRequest www = UnityWebRequest.Post(uri, body))
            {

                //www.SetRequestHeader("id", _walletAdress);

                yield return www.SendWebRequest();

                switch (www.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(": Error: " + www.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError( ": HTTP Error: " + www.error);
                        //DataProvider.GetRequest("Login", "Clear", "" ,x=> { });
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Received: " + www.downloadHandler.text+ "Result: " + www.result);

                        callback(www.downloadHandler.text);
                        break;
                }

            }

        }
        #endregion
        #endregion
        
        #region BurnNFT
        public Action OnBurnStarted;
        public Action<ServiceResponse> OnBurnRespond;
        
        Coroutine BurnRoutine;
        
        public IEnumerator StartBurnRoutine(string TokenID , string ContractHash)
        {
            string ReturnedString = String.Empty;
            OnBurnStarted?.Invoke();


            yield return StartBurnNFT(TokenID,ContractHash,returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log("Burn : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                BurnRoutine = StartCoroutine(CheckBurnState(DTO.data));
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
                yield return CheckBurnNFT(TransactionToken , returnValue => {
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
            

        #region Web Request Section
        
        private IEnumerator StartBurnNFT(string TokenID , string ContractHash ,System.Action<string> callback)
        {
            string uri = Constants.BurnNFTUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("tokenId", TokenID);
            body.AddField("contractPackageHash", ContractHash);
            Debug.Log("Burn Started : "+AuthManager.Instance.CurrentSessionToken + " // " + TokenID + " // " + ContractHash );
            using (UnityWebRequest www = UnityWebRequest.Post(uri, body))
            {

                //www.SetRequestHeader("id", _walletAdress);

                yield return www.SendWebRequest();

                switch (www.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(": Error: " + www.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError( ": HTTP Error: " + www.error);
                        //DataProvider.GetRequest("Login", "Clear", "" ,x=> { });
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Received: " + www.downloadHandler.text+ "Result: " + www.result);

                        callback(www.downloadHandler.text);
                        break;
                }

            }

        }
        private IEnumerator CheckBurnNFT(string TransactionToken , Action<string> callback)
        {
            string uri = Constants.CheckBurnNFTUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", TransactionToken);

            using (UnityWebRequest www = UnityWebRequest.Post(uri, body))
            {

                //www.SetRequestHeader("id", _walletAdress);

                yield return www.SendWebRequest();

                switch (www.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(": Error: " + www.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError( ": HTTP Error: " + www.error);
                        //DataProvider.GetRequest("Login", "Clear", "" ,x=> { });
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Received: " + www.downloadHandler.text+ "Result: " + www.result);

                        callback(www.downloadHandler.text);
                        break;
                }

            }

        }
        #endregion
        #endregion
        
        #region MintNFT
        public Action OnMintStarted;
        public Action<ServiceResponse> OnMintRespond;
        
        Coroutine MintRoutine;
        
        public IEnumerator StartMintRoutine(string ContractHash , string TokenMetadata)
        {
            string ReturnedString = String.Empty;
            OnMintStarted?.Invoke();


            yield return StartMintNFT(ContractHash,TokenMetadata,returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log("Mint : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                MintRoutine = StartCoroutine(CheckMintState(DTO.data));
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
                yield return CheckMintNFT(TransactionToken , returnValue => {
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
            

        #region Web Request Section
        
        private IEnumerator StartMintNFT(string ContractHash ,string TokenMetaData ,System.Action<string> callback)
        {
            string uri = Constants.MintNFTri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("token_owner", AuthManager.Instance.CurrentAuthorisedWalletPublicKey);
            body.AddField("contractPackageHash", ContractHash);
            body.AddField("token_meta_data", TokenMetaData);
            Debug.Log("Mint Started : "+AuthManager.Instance.CurrentSessionToken + " // " + ContractHash + " // " + TokenMetaData );
            using (UnityWebRequest www = UnityWebRequest.Post(uri, body))
            {

                //www.SetRequestHeader("id", _walletAdress);

                yield return www.SendWebRequest();

                switch (www.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(": Error: " + www.error);
                        callback(": Error: " + www.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError( ": HTTP Error: " + www.error);
                        callback(": HTTP Error: " + www.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Received: " + www.downloadHandler.text+ "Result: " + www.result);

                        callback(www.downloadHandler.text);
                        break;
                }

            }

        }
        private IEnumerator CheckMintNFT(string TransactionToken , Action<string> callback)
        {
            string uri = Constants.CheckMintNFTUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", TransactionToken);

            using (UnityWebRequest www = UnityWebRequest.Post(uri, body))
            {

                //www.SetRequestHeader("id", _walletAdress);

                yield return www.SendWebRequest();

                switch (www.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(": Error: " + www.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError( ": HTTP Error: " + www.error);
                        //DataProvider.GetRequest("Login", "Clear", "" ,x=> { });
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Received: " + www.downloadHandler.text+ "Result: " + www.result);

                        callback(www.downloadHandler.text);
                        break;
                }

            }

        }
        #endregion
        #endregion
        
        #region RegisterOwnerNFT
        public Action OnRegisterOwnerStarted;
        public Action<ServiceResponse> OnRegisterOwnerRespond;
        
        Coroutine RegisterOwnerRoutine;
        
        public IEnumerator StartRegisterOwnerRoutine(string TokenID , string ContractHash)
        {
            string ReturnedString = String.Empty;
            OnRegisterOwnerStarted?.Invoke();


            yield return StartRegisterOwnerNFT(TokenID,ContractHash,returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);

            Debug.Log("Register Owner : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                RegisterOwnerRoutine = StartCoroutine(CheckRegisterOwnerState(DTO.data));
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
                yield return CheckRegisterOwnerNFT(TransactionToken , returnValue => {
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
            

        #region Web Request Section
        
        private IEnumerator StartRegisterOwnerNFT(string TokenID , string ContractHash ,System.Action<string> callback)
        {
            string uri = Constants.RegisterOwnerNFTUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("tokenId", TokenID);
            body.AddField("contractPackageHash", ContractHash);
            Debug.Log("Register Owner Started : "+AuthManager.Instance.CurrentSessionToken + " // " + TokenID + " // " + ContractHash );
            using (UnityWebRequest www = UnityWebRequest.Post(uri, body))
            {

                //www.SetRequestHeader("id", _walletAdress);

                yield return www.SendWebRequest();

                switch (www.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(": Error: " + www.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError( ": HTTP Error: " + www.error);
                        //DataProvider.GetRequest("Login", "Clear", "" ,x=> { });
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Received: " + www.downloadHandler.text+ "Result: " + www.result);

                        callback(www.downloadHandler.text);
                        break;
                }

            }

        }
        private IEnumerator CheckRegisterOwnerNFT(string TransactionToken , Action<string> callback)
        {
            string uri = Constants.CheckRegisterOwnerNFTUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", TransactionToken);

            using (UnityWebRequest www = UnityWebRequest.Post(uri, body))
            {

                //www.SetRequestHeader("id", _walletAdress);

                yield return www.SendWebRequest();

                switch (www.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(": Error: " + www.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError( ": HTTP Error: " + www.error);
                        //DataProvider.GetRequest("Login", "Clear", "" ,x=> { });
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log("Received: " + www.downloadHandler.text+ "Result: " + www.result);

                        callback(www.downloadHandler.text);
                        break;
                }

            }

        }
        #endregion
        #endregion
        
        
    }
}

