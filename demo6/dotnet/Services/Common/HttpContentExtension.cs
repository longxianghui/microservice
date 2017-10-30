using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common
{
    public static class HttpContentExtension
    {
        public static async Task<T> ReadAsObjectAsync<T>(this HttpContent content, bool ensureSuccess = false,
            JsonSerializerSettings settings = null)
        {
            var responseString = await content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<T>(responseString, settings);
            if (ensureSuccess && obj == null)
            {
                throw new Exception(responseString);
            }
            return obj;
        }
    }
}