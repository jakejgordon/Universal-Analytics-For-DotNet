using System;

namespace UniversalAnalyticsHttpWrapper
{
    /// <summary>
    /// Represents an anonymous client id.
    /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid
    /// </summary>
    public interface IAnonymousClientId
    {
        /// <summary>
        /// Guid representing the cid parameter.
        /// </summary>
        Guid Id { get; }
    }
}