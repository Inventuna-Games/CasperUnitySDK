using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace CasperSDK.WebRequests
{
    
public static class WebRequestManager
{
    public static IEnumerator GetRequest(string uri ,System.Action<string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    callback(webRequest.error);
                    
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    callback(webRequest.error);
                    
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    callback(webRequest.error);
                    
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    callback(webRequest.downloadHandler.text);
                    
                    break;
            }
        }
        yield return null;
    }

    public static IEnumerator PostRequest(string uri, WWWForm body, System.Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(uri, body))
        {
            yield return www.SendWebRequest();

            switch (www.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    callback(www.error);

                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + www.error);
                    callback(www.error);
                    
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError( ": HTTP Error: " + www.error);
                    callback(www.error);
                    
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + www.downloadHandler.text+ "Result: " + www.result);
                    callback(www.downloadHandler.text);
                    
                    break;
            }

        }

    }
}

}