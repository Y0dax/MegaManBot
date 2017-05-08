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

        public async Task<ApiResponse<T>> GetJSONAsync(Uri uri)
        {
            ApiResponse<T> apiResponse = new ApiResponse<T>();
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                apiResponse.Success = response.IsSuccessStatusCode;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    apiResponse.responseObject = JsonConvert.DeserializeObject<T>(responseContent);
                    apiResponse.Success = response.IsSuccessStatusCode;
                }
            }
            catch(Exception e)
            {
                apiResponse.Success = false;
            }

            return apiResponse;
        }

    }

    public class ApiResponse<T>
    {
        public T responseObject { get; set; }
        public bool Success { get; set; }
    }
}
