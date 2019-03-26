using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UniversalAnalyticsHttpWrapper
{
    internal class GoogleDataSender : IGoogleDataSender
    {
        private readonly HttpClient _httpClient;

        public GoogleDataSender()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Executes Google Analytics data delivery (hit)
        /// </summary>
        /// <param name="googleCollectionUri">URI to send data to</param>
        /// <param name="postData">Data to be sent as POST payload. Should be correct and valid Google Analytics Measurement Protocol parameters collection</param>
        /// <param name="readResponse">Boolean - whether to read response or not</param>
        /// <returns>Response text if readResponse parameter set to true or null otherwise</returns>
        public string SendData(Uri googleCollectionUri, string postData, bool readResponse = false)
        {
            string responseText = null;

            if (string.IsNullOrEmpty(postData))
                throw new ArgumentNullException("postData", "Request body cannot be empty.");

            var httpRequest = WebRequest.CreateHttp(googleCollectionUri);
            httpRequest.ContentLength = Encoding.UTF8.GetByteCount(postData);
            httpRequest.Method = "POST";

            using(var requestStream = httpRequest.GetRequestStream())
            {
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(postData);
                }
            }
  
            using(var webResponse = (HttpWebResponse)httpRequest.GetResponse())
            {
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpException((int)webResponse.StatusCode,
                                            "Google Analytics tracking did not return OK 200");
                }

                if (readResponse)
                {
                    responseText = ReadResponseAsText(webResponse);
                }
            }

            return responseText;
        }

        public async Task SendDataAsync(Uri googleCollectionUri, IEnumerable<KeyValuePair<string, string>> postData)
        {
            if (postData == default(IEnumerable<KeyValuePair<string, string>>))
                throw new ArgumentNullException("postData", "Request body cannot be empty.");

            var httpResponse = await _httpClient.PostAsync(googleCollectionUri, new FormUrlEncodedContent(postData));
            var content = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new HttpException((int)httpResponse.StatusCode,
                    "Google Analytics tracking did not return OK 200");
            }
        }


        #region Private
        /// <summary>
        /// Attempts to decode response text using the received encoding and returns the text
        /// </summary>
        /// <param name="webResponse">A valid and non-null HttpWebResponse object</param>
        /// <returns>Text returned by server in response or Null if unable to read response text</returns>
        private string ReadResponseAsText(HttpWebResponse webResponse)
        {
            string responseAsText = null;

            if (webResponse == null)
                throw new ArgumentException("webResponse", "webResponse cannot be null");

            using (Stream responseStream = webResponse.GetResponseStream())
            {
                var encoding = Encoding.Default;
                var contentType = new System.Net.Mime.ContentType(webResponse.Headers[HttpResponseHeader.ContentType]);
                if (!String.IsNullOrEmpty(contentType.CharSet))
                {
                    encoding = Encoding.GetEncoding(contentType.CharSet);
                }

                using (StreamReader reader = new StreamReader(responseStream, encoding))
                {
                    responseAsText = reader.ReadToEnd();
                }
            }

            return responseAsText;
        }
        #endregion

    }
}
