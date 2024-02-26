using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CasperSDK.DataStructures;
using UnityEngine.Networking;

namespace CasperSDK.Auth
{
    public class AuthManager : MonoBehaviour
    {
        
        public static AuthManager Instance;
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
        
        public string CurrentSessionToken = string.Empty;
        public string CurrentAuthorisedWalletPublicKey = string.Empty;
        Coroutine AuthorizationRoutine;
        
        public Action OnAuthStarted;
        public Action<ServiceResponse> OnAuthRespond;
        
        public IEnumerator StartAuthorization()
        {
            string ReturnedString = String.Empty;
            while (true)
            {
                OnAuthStarted?.Invoke();
                yield return StartSessionRequest(returnValue => {
                    ReturnedString = returnValue;
                });

                var DTO = JsonUtility.FromJson<AuthDTO>(ReturnedString);
                Debug.Log(DTO.data + " : " + DTO.status);
                CurrentSessionToken = DTO.data;

                if (DTO.status == RequestStatus.Success)
                {
                    Application.OpenURL(Constants.AuthorizeUri + CurrentSessionToken);
                    AuthorizationRoutine = StartCoroutine(CheckAuthorizationState());
                    break;
                }
                else yield return new WaitForSeconds(2);
            }
        }
        private IEnumerator CheckAuthorizationState()
        {
            string s = String.Empty;
            while (true)
            {
                yield return CheckSessionRequest(returnValue => {
                    s = returnValue;
                });

                var DTO = JsonUtility.FromJson<CheckLoginDTO>(s);
                Debug.Log(DTO.data + " : " + DTO.status);

                if (DTO.status == RequestStatus.Success)
                {
                    ServiceResponse DataProcessRespond = new ServiceResponse();
                    DataProcessRespond.status = RequestStatus.Success;
                    DataProcessRespond.message = "Auth Completed";
                    OnAuthRespond?.Invoke(DataProcessRespond);
                    Debug.Log("Logged in : " + DTO.data);
                    CurrentAuthorisedWalletPublicKey = DTO.data;
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(1);
                }
            }
        }
        #region WebRequest Section
        private IEnumerator StartSessionRequest(System.Action<string> callback)
        {
            var uri = Constants.LoginUri;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        ServiceResponse DataProcessRespond = new ServiceResponse();
                        DataProcessRespond.status = RequestStatus.Failed;
                        DataProcessRespond.message = "Data Processing error on Auth Session";
                        OnAuthRespond?.Invoke(DataProcessRespond);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        ServiceResponse ProtocolRespond = new ServiceResponse();
                        ProtocolRespond.status = RequestStatus.Failed;
                        ProtocolRespond.message = "Data Processing error on Auth Session";
                        OnAuthRespond?.Invoke(ProtocolRespond);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                        callback(webRequest.downloadHandler.text);

                        break;
                }
            }
            yield return null;
        }

        private IEnumerator CheckSessionRequest(System.Action<string> callback)
        {
            var uri = Constants.CheckLoginUri;
            
            WWWForm body = new WWWForm();
            body.AddField("token", CurrentSessionToken);

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
    }
}

