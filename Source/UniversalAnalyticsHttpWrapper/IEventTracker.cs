using System.Linq;
using System.Threading.Tasks;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// Interface for calls related to tracking events using the Universal Analytics Measurement Protocol.
    /// </summary>
    public interface IEventTracker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analyticsEvent"></param>
        TrackingResult TrackEvent(IUniversalAnalyticsEvent analyticsEvent);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analyticsEvent"></param>
        Task<TrackingResult> TrackEventAsync(IUniversalAnalyticsEvent analyticsEvent);
    }
}
