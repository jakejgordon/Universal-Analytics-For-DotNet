using System;

namespace UniversalAnalyticsHttpWrapper
{
    public interface IAnonymousClientId
    {
        /// <summary>
        /// Guid representing the cid parameter.
        /// </summary>
        Guid Id { get; }
    }
}