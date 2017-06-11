using MusicTimelineWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.MusicTimeline
{
    public static class WebApiInterface
    {
        public static string ApiRoot = "http://localhost:65140";
        static HttpClient client = new HttpClient();

        static WebApiInterface()
        {
            client.BaseAddress = new Uri(ApiRoot);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static async Task<T> GetAsync<T>(string apiRelativePath)
        {
            T dataTransferObject = default(T);
            HttpResponseMessage response = await client.GetAsync($"{ApiRoot}/api/{apiRelativePath}");

            if (response.IsSuccessStatusCode)
            {
                dataTransferObject = await response.Content.ReadAsAsync<T>();
            }

            return dataTransferObject;
        }

        private static async Task<IEnumerable<T>> GetAllAsync<T>(string pluralName)
        {
            return await GetAsync<IEnumerable<T>>($"{pluralName}/");
        }

        private static async Task<T> GetSingleAsync<T>(int id, string pluralName)
        {
            return await GetAsync<T>($"{pluralName}/{id}");
        }

        public static async Task<IEnumerable<Composer>> GetComposersAsync()
        {
            return await GetAllAsync<Composer>("Composers");
        }

        public static async Task<ComposerDetail> GetComposerAsync(int id)
        {
            return await GetSingleAsync<ComposerDetail>(id, "Composers");
        }

        public static async Task<IEnumerable<Era>> GetErasAsync()
        {
            return await GetAllAsync<Era>("Eras");
        }

        public static async Task<Era> GetEraAsync(int id)
        {
            return await GetSingleAsync<Era>(id, "Eras");
        }
    }
}
