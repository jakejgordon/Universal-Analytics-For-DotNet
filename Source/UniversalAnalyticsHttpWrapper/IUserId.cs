namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// Represents a user id.
    /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid
    /// </summary>
    public interface IUserId
    {
        /// <summary>
        /// string representing some uid.
        /// </summary>
        string Id { get; }
    }
}