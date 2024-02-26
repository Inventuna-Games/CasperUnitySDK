using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CasperSDK;
using CasperSDK.Auth;
using CasperSDK.DataStructures;
using UnityEngine.Networking;

namespace CasperSDK.WalletData
{
    public class WalletDataManager : MonoBehaviour
    {
        public static WalletDataManager Instance;
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
        #region Properties
        private WalletInformation currentAuthorizedWalletInformation;
        public WalletInformation CurrentAuthorizedWalletInformation{get{ return currentAuthorizedWalletInformation;} private set{}}
        #endregion

        #region Actions 
        public Action<WalletInformation> OnWalletInformationUpdated;
        #endregion
        
        
        #region Wallet Information
        public IEnumerator UpdateWalletInformationRunner()
        {
            yield return GetWalletTokens(returnValue => {
                currentAuthorizedWalletInformation.walletTokens = JsonUtility.FromJson<TokensDTO>(returnValue).data;
            });
            yield return GetWalletNFTs(returnValue => {
                currentAuthorizedWalletInformation.walletNFTs = JsonUtility.FromJson<NFTSDto>(returnValue).data;
            });
            yield return GetCasperBalance(returnValue => {
                currentAuthorizedWalletInformation.walletBalance = JsonUtility.FromJson<BalanceDTO>(returnValue).data;
            });
            OnWalletInformationUpdated?.Invoke(currentAuthorizedWalletInformation);
        }
        #region WebRequest Section
        private IEnumerator GetCasperBalance(System.Action<string> callback)
        {
            var uri = Constants.GetBalanceUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);

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
        private IEnumerator GetWalletTokens(System.Action<string> callback)
        {
            var uri = Constants.GetTokensUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);

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
        private IEnumerator GetWalletNFTs(System.Action<string> callback)
        {
            var uri = Constants.GetNFTsUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", AuthManager.Instance.CurrentSessionToken);

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

