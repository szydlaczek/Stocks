using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CompanyComunication
{
    public class ResourceServerRestClient
    {
        public Uri BaseAddress { get; private set; }
        private Error _error;
        private HttpResponseMessage responseMessage = null;

        public ResourceServerRestClient(Uri baseAddress)
        {
            BaseAddress = baseAddress;
        }

        public async Task<IResponse> GetAsync(string uri)
        {
            int step = 1;
            do
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = this.BaseAddress;
                    client.Timeout = new TimeSpan(0, 1, 0);
                    try
                    {
                        responseMessage = await client.GetAsync(uri);
                    }
                    catch (Exception ex) when (ex.InnerException is WebException)
                    {
                        var webException = ex.InnerException as WebException;
                        _error = new Error(webException.Status.ToString(), webException.Message);
                    }
                    catch (Exception ex)
                    {
                        _error = new Error("DefaultError", ex.Message);
                    }
                }
                step++;
            } while (step <= 10 && (responseMessage == null || responseMessage?.StatusCode != HttpStatusCode.OK));

            return await GetResponse();
        }

        private async Task<IResponse> GetResponse()
        {
            if (responseMessage is null)
            {
                return new Response(_error);
            }

            string jsonResult;
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                jsonResult = await responseMessage.Content.ReadAsStringAsync();
                return new Response(jsonResult);
            }
            else
            {
                jsonResult = await responseMessage.Content.ReadAsStringAsync();
                ErrorResponse errorResponse = null;
                if (responseMessage.Headers.Contains("Blocked-to"))
                {
                    IEnumerable<string> values;
                    responseMessage.Headers.TryGetValues("Blocked-to", out values);
                    errorResponse = new ErrorResponse();
                    errorResponse.Error = "Requests has been blocked to: " + values.FirstOrDefault();
                }
                else
                {
                    var js = new JavaScriptSerializer();
                    errorResponse = js.Deserialize<ErrorResponse>(jsonResult);
                }

                return new Response(new Error(responseMessage.StatusCode.ToString(), errorResponse?.Error));
            }
        }
    }
}