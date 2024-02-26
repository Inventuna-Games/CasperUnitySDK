using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CasperSDK;
using CasperSDK.Auth;
using CasperSDK.DataStructures;
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
        
        Coroutine TransferCEP18Routine;
        
        public IEnumerator StartTransferCEP18Routine(float Amount , string ContractHash , string TargetWalletPublicKey)
        {
            string ReturnedString = String.Empty;
            OnTransferCEP18Started?.Invoke();


            yield return StartTransferCEP18(Amount,ContractHash,TargetWalletPublicKey,returnValue => {
                ReturnedString = returnValue;
            });

            var DTO = JsonUtility.FromJson<StartTransferCEP18DTO>(ReturnedString);

            Debug.Log(DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                TransferCEP18Routine = StartCoroutine(CheckTransactionCEP18State(DTO.data));
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
                yield return CheckTransferCEP18(TransactionToken , returnValue => {
                    ReturnedString = returnValue;
                });

                var DTO = JsonUtility.FromJson<CheckTransferRequestCEP18DTO>(ReturnedString);
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
            

        #region Web Request Section
        
        private IEnumerator StartTransferCEP18(float Amount , string ContractHash , string TargetWalletPublicKey,System.Action<string> callback)
        {
            var uri = Constants.TransferCEP18Uri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("amount", Amount.ToString());
            body.AddField("contractPackageHash", ContractHash);
            body.AddField("recipient", TargetWalletPublicKey);
            Debug.Log(AuthManager.Instance.CurrentSessionToken + " // " + Amount + " // " + ContractHash + " // " + TargetWalletPublicKey);
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
        private IEnumerator CheckTransferCEP18(string TransactionToken , Action<string> callback)
        {
            var uri = Constants.CheckTransferCEP18Uri;
            
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
        public Action OnBurnCEP18Started;
        public Action<ServiceResponse> OnBurnCEP18Respond;
        
        Coroutine BurnCEP18Routine;
        
        public IEnumerator StartBurnCEP18Routine(float Amount , string ContractHash)
        {
            string ReturnedString = String.Empty;
            OnBurnCEP18Started?.Invoke();


            yield return StartBurnCEP18(Amount,ContractHash,returnValue => {
                ReturnedString = returnValue;
            });

            var DTO = JsonUtility.FromJson<StartBurnCEP18DTO>(ReturnedString);

            Debug.Log("Burn : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                BurnCEP18Routine = StartCoroutine(CheckBurnCEP18State(DTO.data));
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
                yield return CheckBurnCEP18(TransactionToken , returnValue => {
                    ReturnedString = returnValue;
                });

                var DTO = JsonUtility.FromJson<CheckBurnRequestCEP18DTO>(ReturnedString);
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
            

        #region Web Request Section
        
        private IEnumerator StartBurnCEP18(float Amount , string ContractHash ,System.Action<string> callback)
        {
            var uri = Constants.BurnCEP18Uri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("amount", Amount.ToString());
            body.AddField("owner", AuthManager.Instance.CurrentAuthorisedWalletPublicKey);
            body.AddField("contractPackageHash", ContractHash);
            Debug.Log("Burn Started : "+AuthManager.Instance.CurrentSessionToken + " // " + Amount + " // " + ContractHash );
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
        private IEnumerator CheckBurnCEP18(string TransactionToken , Action<string> callback)
        {
            var uri = Constants.CheckBurnCEP18Uri;
            
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
        public Action OnMintCEP18Started;
        public Action<ServiceResponse> OnMintCEP18Respond;
        
        Coroutine MintCEP18Routine;
        
        public IEnumerator StartMintRoutine(float Amount , string ContractHash)
        {
            string ReturnedString = String.Empty;
            OnMintCEP18Started?.Invoke();


            yield return StartMintCEP18(Amount,ContractHash,returnValue => {
                ReturnedString = returnValue;
            });

            var DTO = JsonUtility.FromJson<StartMintCEP18DTO>(ReturnedString);

            Debug.Log("Mint : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                MintCEP18Routine = StartCoroutine(CheckMintCEP18State(DTO.data));
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
                yield return CheckMintCEP18(TransactionToken , returnValue => {
                    ReturnedString = returnValue;
                });

                var DTO = JsonUtility.FromJson<CheckMintRequestCEP18DTO>(ReturnedString);
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
            

        #region Web Request Section
        
        private IEnumerator StartMintCEP18(float Amount , string ContractHash ,System.Action<string> callback)
        {
            var uri = Constants.MintCEP18ri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("amount", Amount.ToString());
            body.AddField("owner", AuthManager.Instance.CurrentAuthorisedWalletPublicKey);
            body.AddField("contractPackageHash", ContractHash);
            Debug.Log("Mint Started : "+AuthManager.Instance.CurrentSessionToken + " // " + Amount + " // " + ContractHash );
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
        private IEnumerator CheckMintCEP18(string TransactionToken , Action<string> callback)
        {
            var uri = Constants.CheckMintCEP18Uri;
            
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
        
        #region ApproveCEP18
        public Action OnApproveCEP18Started;
        public Action<ServiceResponse> OnApproveCEP18Respond;
        
        Coroutine ApproveRoutine;
        
        public IEnumerator StartApproveCEP18Routine(float Amount , string ContractHash)
        {
            string ReturnedString = String.Empty;
            OnApproveCEP18Started?.Invoke();


            yield return StartApproveCEP18(Amount,ContractHash,returnValue => {
                ReturnedString = returnValue;
            });

            var DTO = JsonUtility.FromJson<StartApproveCEP18DTO>(ReturnedString);

            Debug.Log("Approve : "+DTO.data + " : " + DTO.status);

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.WebsiteTokenConnectionURL + DTO.data);
                ApproveRoutine = StartCoroutine(CheckApproveCEP18State(DTO.data));
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
                yield return CheckApproveCEP18(TransactionToken , returnValue => {
                    ReturnedString = returnValue;
                });

                var DTO = JsonUtility.FromJson<CheckApproveRequestCEP18DTO>(ReturnedString);
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
            

        #region Web Request Section
        
        private IEnumerator StartApproveCEP18(float Amount , string ContractHash ,System.Action<string> callback)
        {
            var uri = Constants.ApproveUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);
            body.AddField("amount", Amount.ToString());
            body.AddField("spender", AuthManager.Instance.CurrentAuthorisedWalletPublicKey);
            body.AddField("contractPackageHash", ContractHash);
            Debug.Log("Register Owner Started : "+AuthManager.Instance.CurrentSessionToken + " // " + Amount + " // " + ContractHash );
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
        private IEnumerator CheckApproveCEP18(string TransactionToken , Action<string> callback)
        {
            var uri = Constants.CheckApproveUri;
            
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

