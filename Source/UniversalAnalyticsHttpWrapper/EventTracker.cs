using System;
using System.Threading.Tasks;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// This is the primary class with which the consumer will work.
    /// </summary>
    public class EventTracker : IEventTracker
    {
        private readonly IPostDataBuilder postDataBuilder;
        private readonly IGoogleDataSender googleDataSender;
        /// <summary>
        /// This is the current Google collection URI for version 1 of the measurement protocol
        /// </summary>
        public static readonly Uri GOOGLE_COLLECTION_URI = new Uri("https://www.google-analytics.com/collect");
        /// <summary>
        /// This assembly is built to work with this version of the measurement protocol.
        /// </summary>
        public const string MEASUREMENT_PROTOCOL_VERSION = "1";

        /// <summary>
        /// This constructor will create all of the appropriate default implementations.
        /// </summary>
        public EventTracker()
        {
            postDataBuilder = new PostDataBuilder();
            googleDataSender = new GoogleDataSender();
        }

        /// <summary>
        /// This constructor is only used if you want to inject your own implementations (most likely for unit testing)
        /// </summary>
        /// <param name="postDataBuilder"></param>
        /// <param name="googleDataSender"></param>
        internal EventTracker(IPostDataBuilder postDataBuilder, IGoogleDataSender googleDataSender)
        {
            this.postDataBuilder = postDataBuilder;
            this.googleDataSender = googleDataSender;
        }

        /// <summary>
        /// Pushes an event up to the Universal Analytics web property specified in the .config file.
        /// </summary>
        /// <param name="analyticsEvent">The event to be logged.</param>
        public TrackingResult TrackEvent(IUniversalAnalyticsEvent analyticsEvent)
        { 
            var result = new TrackingResult();
            string postData = postDataBuilder.BuildPostDataString(MEASUREMENT_PROTOCOL_VERSION, analyticsEvent);
            try
            {
                googleDataSender.SendData(GOOGLE_COLLECTION_URI, postData);
            }
            catch (Exception e)
            {
                result.Exception = e;
            }

            return result;
        }

        /// <summary>
        /// Pushes an event up to the Universal Analytics web property specified in the .config file.
        /// </summary>
        /// <param name="analyticsEvent">The event to be logged.</param>
        public async Task<TrackingResult> TrackEventAsync(IUniversalAnalyticsEvent analyticsEvent)
        {
            var result = new TrackingResult();
            var postData = postDataBuilder.BuildPostDataCollection(MEASUREMENT_PROTOCOL_VERSION, analyticsEvent);

            try
            {
                await googleDataSender.SendDataAsync(GOOGLE_COLLECTION_URI, postData);
            }
            catch (Exception e)
            {
                result.Exception = e;
            }

            return result;
        }
    }
}
