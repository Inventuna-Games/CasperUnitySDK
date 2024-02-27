using System;
using System.Collections;
using UnityEngine;
using CasperSDK.DataStructures;
using CasperSDK.WebRequests;
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
        
        public Action OnAuthStarted;
        public Action<ServiceResponse> OnAuthRespond;
        
        public IEnumerator StartAuthorization()
        {
            string ReturnedString = String.Empty;
            OnAuthStarted?.Invoke();
            string uri = Constants.LoginUri;
            
            yield return WebRequestManager.GetRequest(uri ,returnValue => {
                ReturnedString = returnValue;
            });

            GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(ReturnedString);
            CurrentSessionToken = DTO.data;

            if (DTO.status == RequestStatus.Success)
            {
                Application.OpenURL(Constants.AuthorizeUri + CurrentSessionToken);
                StartCoroutine(CheckAuthorizationState());
            }
            else
            {
                ServiceResponse DataProcessRespond = new ServiceResponse();
                DataProcessRespond.status = RequestStatus.Failed;
                DataProcessRespond.message = ReturnedString;
                OnAuthRespond.Invoke(DataProcessRespond);
            }
        }
        private IEnumerator CheckAuthorizationState()
        {
            string s = String.Empty;
            while (true)
            {
                string uri = Constants.CheckLoginUri;
            
                WWWForm body = new WWWForm();
                body.AddField("token", CurrentSessionToken);
                
                yield return WebRequestManager.PostRequest(uri , body , returnValue => {
                    s = returnValue;
                });

                GenericDTOResponse<string> DTO = JsonUtility.FromJson<GenericDTOResponse<string>>(s);
                ServiceResponse DataProcessRespond = new ServiceResponse();
                
                if (DTO.status == RequestStatus.Success)
                {
                    DataProcessRespond.status = RequestStatus.Success;
                    DataProcessRespond.message = "Auth Completed";
                    OnAuthRespond?.Invoke(DataProcessRespond);
                    Debug.Log("Logged in : " + DTO.data);
                    CurrentAuthorisedWalletPublicKey = DTO.data;
                    break;
                }
                else if(DTO.status == RequestStatus.Failed)
                {
                    DataProcessRespond.status = RequestStatus.Failed;
                    DataProcessRespond.message = s;
                    OnAuthRespond?.Invoke(DataProcessRespond);
                    break;
                }
                else if (DTO.status == RequestStatus.Waiting)
                {
                    yield return new WaitForSecondsRealtime(2);
                }
            }
        }
    }
}

