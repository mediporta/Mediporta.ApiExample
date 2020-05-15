using System.Net.Http;

namespace Mediporta.CommonLogic
{
    public static class HttpClientWrapper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static HttpResponseMessage Get(string requestUrl)
        {
            return _httpClient.GetAsync(requestUrl).Result;
        }

        public static HttpResponseMessage Post(string requestUrl, HttpContent content)
        {
            return _httpClient.PostAsync(requestUrl, content).Result;
        }
    }
}
