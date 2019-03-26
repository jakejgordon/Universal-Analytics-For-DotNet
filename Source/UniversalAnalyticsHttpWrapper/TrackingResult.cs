using System;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// Result of a tracking request.
    /// </summary>
    public class TrackingResult
    {
        /// <summary>
        /// Creates a tracking result based on the HTTP status code as well as an exception, if any.
        /// </summary>
        /// <param name="exception"></param>
        public TrackingResult(Exception exception = null)
        {
            Exception = exception;
        }

        /// <summary>
        /// Checks if the tracking attempt was successful.
        /// </summary>
        public bool Failed => Exception != null;
        /// <summary>
        /// An exception caught during the tracking process. Not thrown for stability.
        /// </summary>
        public Exception Exception { get; internal set; }


        /// <summary>
        /// Complete response of the Google Analytics Measurement Protocol hit validation debug endpoint
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/validating-hits
        /// </summary>
        public string ValidationResult { get; set; }

        /// <summary>
        /// If true - hit is valid and might be accepted by Google Analytics
        ///  Returns value of hitParsingResult[0].valid
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/validating-hits
        /// </summary>
        public bool ValidationStatus
        {
            get
            {
                var ret = ValidationResult.Contains("\"valid\": true");

                return ret;
            }

        }

    }
}
