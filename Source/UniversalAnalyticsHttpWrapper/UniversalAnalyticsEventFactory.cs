namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// Class for making instances of IUniversalAnalyticsEvent objects
    /// </summary>
    public class UniversalAnalyticsEventFactory : IUniversalAnalyticsEventFactory
    {
        private readonly IConfigurationManager _configurationManager;

        /// <summary>
        /// This key is required in the .config. Find this value from your Universal Analytics property that was set up on
        /// www.google.com/analytics/
        /// </summary>
        public const string APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID = "UniversalAnalytics.TrackingId";

        /// <summary>
        /// Default constructor. Could be a singleton but this is easier for the average developer to consume
        /// </summary>
        public UniversalAnalyticsEventFactory()
        {
            _configurationManager = new ConfigurationManagerWrapper();
        }

        internal UniversalAnalyticsEventFactory(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        /// <inheritdoc />
        public IUniversalAnalyticsEvent MakeUniversalAnalyticsEvent(
            string anonymousClientId, 
            string eventCategory, 
            string eventAction, 
            string eventLabel, 
            string eventValue = null) 
        {
            return new UniversalAnalyticsEvent(
                GetAnalyticsTrackingIdFromConfig(),
                anonymousClientId,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue);
        }

        /// <inheritdoc />
        public IUniversalAnalyticsEvent MakeAnonymousClientIdUniversalAnalyticsEvent(string anonymousClientId,
            string eventCategory,
            string eventAction,
            string eventLabel,
            string eventValue = null)
        {
            return new UniversalAnalyticsEvent(this.GetAnalyticsTrackingIdFromConfig(),
                anonymousClientId, 
                eventCategory,
                eventAction,
                eventLabel,
                eventValue);
        }

        /// <inheritdoc />
        public IUniversalAnalyticsEvent MakeUserIdUniversalAnalyticsEvent(string userId, string eventCategory, string eventAction, string eventLabel, string eventValue = null)
        {
            return new UniversalAnalyticsEvent(this.GetAnalyticsTrackingIdFromConfig(),
                null,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue,
                userId);
        }

        private string GetAnalyticsTrackingIdFromConfig()
        {
            return _configurationManager.GetAppSetting(APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID);
        }
    }
}
