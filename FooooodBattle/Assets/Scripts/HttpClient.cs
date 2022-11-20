using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public static class HttpClient
    {
        public static async Task<T> Get<T>(string endpoint)
        {
            var getRequest = CreateRequest(endpoint);
            getRequest.SendWebRequest();

            while (!getRequest.isDone) await Task.Delay(10);
            var get = JsonConvert.DeserializeObject<T>(getRequest.downloadHandler.text);
            getRequest.Dispose();
            return get;
        }

        public static async Task<T> Post<T>(string endpoint, object payload)
        {
            var postRequest = CreateRequest(endpoint, RequestType.Post, payload);
            //UnityWebRequest.Post(endpoint, payload);
            Console.WriteLine();
            postRequest.SendWebRequest();
           

            while (!postRequest.isDone) await Task.Delay(10);
            var post = JsonConvert.DeserializeObject<T>(postRequest.downloadHandler.text);
            postRequest.Dispose();
            return post;
        }

        private static UnityWebRequest CreateRequest(string path, RequestType type = RequestType.Get, object data = null)
        {
            var request = new UnityWebRequest(path, type.ToString());

            if(data != null)
            {
                var bodyRaw = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));
                //var bodyRaw = Encoding.ASCII.GetBytes(JsonUtility.ToJson(data));
                
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }

        private static void AttachHeader(UnityWebRequest request, string key, string value)
        {
            request.SetRequestHeader(key, value);
        }
    }


}

public enum RequestType
{
    Get = 0,
    Post= 1
}
