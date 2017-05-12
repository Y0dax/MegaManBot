using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MegaManDiscordBot.Services.Common
{
    public class ApiHandler<T>
    {
        private HttpClient client;
        public ApiHandler()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> GetJSONAsync(Uri uri)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseContent);
                    //apiResponse.Success = response.IsSuccessStatusCode;
                }
                return default(T);
            }
            catch(Exception e)
            {
                return default(T);
                //apiResponse.Success = false;
            }
        }
    }

    public class ApiResponse<T>
    {
        public T responseObject { get; set; }
        public bool Success { get; set; }
    }
}
