using System;
using System.Text;
using UnityEngine.Networking;

/// <summary>
/// JWebService provides utility methods to send asynchronous HTTP POST and GET requests.
/// </summary>
public class JWebService
{
    /// <summary>
    /// Sends an HTTP POST request with JSON data.
    /// </summary>
    /// <param name="url">Destination URL.</param>
    /// <param name="jsonData">JSON data to send.</param>
    /// <param name="onSuccess">Callback on success (response text).</param>
    /// <param name="onError">Callback on error (error message).</param>
    public static void SendPOSTRequest(string url, string jsonData, Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            UnityWebRequestAsyncOperation operation = request.SendWebRequest();

            operation.completed += (asyncOp) =>
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    onError?.Invoke(request.error);
                }
            };
        }
    }

    /// <summary>
    /// Sends an HTTP GET request.
    /// </summary>
    /// <param name="url">Destination URL.</param>
    /// <param name="onSuccess">Callback on success (response text).</param>
    /// <param name="onError">Callback on error (error message).</param>
    public static void SendGETRequest(string url, Action<string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();

            operation.completed += (asyncOp) =>
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    onError?.Invoke(request.error);
                }
            };
        }
    }
}
