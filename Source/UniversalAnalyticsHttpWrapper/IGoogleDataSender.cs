using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversalAnalyticsHttpWrapper
{
    internal interface IGoogleDataSender
    {
        /// <summary>
        /// Sends postData as payload to the googleCollectionUri and returns response as text if readResponse is true
        /// </summary>
        /// <param name="googleCollectionUri">URI to send data to</param>
        /// <param name="postData">Data to be sent as POST payload. Should be correct and valid Google Analytics Measurement Protocol parameters collection</param>
        /// <param name="readResponse">Read response or not. By default it is false. Response is ONLY required for hit validation</param>
        /// <returns></returns>
        string SendData(Uri googleCollectionUri, string postData, bool readResponse = false);

        Task SendDataAsync(Uri googleCollectionUri, IEnumerable<KeyValuePair<string, string>> postData);
    }
}
