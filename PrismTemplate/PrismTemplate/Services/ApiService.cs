using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
using PrismTemplate.Entities;

namespace PrismTemplate.Services
{
    public interface IApiService
    {
        bool IsNetworkConnected();
        Task<ApiResponse<T>> RequestObject<T>(Method method, Uri uri, object param = null);

    }

    public enum Method
    {
        Get,
        Post,
        Put,
        Delete
    }

    public class ApiService : IApiService
    {
        const string JsonContentMediaType = "application/json";

        public const int NO_NETWORK_ERROR_CODE = -1;
        public const int SSLEXCEPTION_ERROR_CODE = -4;

        //private readonly IConfigurationService _configurationService;

        public ApiService(IConfigurationService configurationService)
        {
           // _configurationService = configurationService;
        }

        public static bool HasError(int statusCode)
        {
            return statusCode != 200;
        }

        public bool IsNetworkConnected()
        {
            return CrossConnectivity.Current.IsConnected;
        }

        

        public async Task<ApiResponse<T>> RequestObject<T>(Method method, Uri uri, object param = null)
        {
            if (!IsNetworkConnected())
            {
                return new ApiResponse<T>(statusCode: NO_NETWORK_ERROR_CODE);
            }

            Request requestObject = new Request { RequestMethod = method, RequestUri = uri, RequestParam = param };

            HttpResponseMessage response = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 1, 0);

                    HttpContent content = null;

                    if (param != null && param is string)
                    {
                        content = new StringContent((string)param);
                        System.Diagnostics.Debug.WriteLine("Request add param: " + param);
                    }
                    else
                    {
                        var json = JsonConvert.SerializeObject(param);
                        content = new StringContent(json);
                        System.Diagnostics.Debug.WriteLine("Request add param: " + json);
                    }
                    content.Headers.ContentType = new MediaTypeHeaderValue(JsonContentMediaType);

                    switch (method)
                    {
                        case Method.Post:
                            response = await client.PostAsync(uri, content);
                            break;
                        case Method.Put:
                            response = await client.PutAsync(uri, content);
                            break;
                        case Method.Delete:
                            response = await client.DeleteAsync(uri);
                            break;
                        case Method.Get:
                            response = await client.GetAsync(uri);
                            break;
                        default:
                            throw new Exception("Set a method");
                    }
                    response.EnsureSuccessStatusCode();

                    if (typeof(T) != typeof(string)
                        && string.Equals(response.Content.Headers.ContentType.MediaType, JsonContentMediaType, StringComparison.OrdinalIgnoreCase))
                    {
                        var stringContent = await response.Content.ReadAsStringAsync();
                        return new ApiResponse<T>(JsonConvert.DeserializeObject<T>(stringContent), (int)response.StatusCode);
                    }
                    var value2 = await response.Content.ReadAsStringAsync();
                    return new ApiResponse<T>((T)Convert.ChangeType(value2, typeof(T)), (int)response.StatusCode);
                }


            }
            catch (System.Net.WebException webException)
            {
                if (webException.Status == System.Net.WebExceptionStatus.TrustFailure)
                {
                    return new ApiResponse<T>(statusCode: SSLEXCEPTION_ERROR_CODE, errorMessage: webException.Message);
                }
                return await HandleRequestObjectException<T>(webException, response, requestObject);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("uri:{0}", uri.AbsolutePath);
                return await HandleRequestObjectException<T>(e, response, requestObject);
            }
        }
        public static bool HasCommonError(int statusCode)
        {
            return statusCode == NO_NETWORK_ERROR_CODE;
        }
        private async Task<ApiResponse<T>> HandleRequestObjectException<T>(Exception e, HttpResponseMessage response, Request requestObject)
        {
            System.Diagnostics.Debug.WriteLine("message:{0}", e.Message);
            System.Diagnostics.Debug.WriteLine("IE.message:{0}", e.InnerException?.Message);
            if (response != null)
            {
                int statusCode = (int)response.StatusCode;
                string errorMessage = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("error StatusCode: " + statusCode);
                System.Diagnostics.Debug.WriteLine("error message: " + errorMessage);

                return new ApiResponse<T>(statusCode: statusCode, errorMessage: errorMessage);
            }
            return new ApiResponse<T>(default(T));
        }
    }
}
