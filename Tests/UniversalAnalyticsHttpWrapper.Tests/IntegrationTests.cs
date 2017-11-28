﻿using NUnit.Framework;
using System;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture, Ignore("Integration tests.")]
    public class IntegrationTests
    {
        private readonly UniversalAnalyticsEventFactory EVENT_FACTORY = new UniversalAnalyticsEventFactory();

        [Test]
        public void SampleCodeForGitHubReadMeUsingFactoryToGetUserIdEventObject()
        {
            // Use your favorite dependency injection framework for these. Singletons preferred.
            IEventTracker eventTracker = new EventTracker();
            IUniversalAnalyticsEventFactory eventFactory = new UniversalAnalyticsEventFactory();

            // Method 1: Track via 'clientId'
            var analyticsEvent = eventFactory.MakeEventForClientId(
               //Required. client id. 
               //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.
               "anonymous-client-id",
               //Required. The event category for the event. 
               // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.
               "test category",
               //Required. The event action for the event. 
               //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.
               "test action",
               //Optional. The event label for the event.
               // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.
               "test label",
               //Optional. The event value for the event.
               // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.
               "10");
            eventTracker.TrackEvent(analyticsEvent);

            // Method 2: 'Track via userId'
            analyticsEvent = eventFactory.MakeEventForUserId(
                //Required. user id. 
                //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid for details.
                "non-pii-user-id",
                //Required. The event category for the event. 
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.
                "test category",
                //Required. The event action for the event. 
                //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.
                "test action",
                //Optional. The event label for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.
                "test label",
                //Optional. The event value for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.
                "10");
            eventTracker.TrackEvent(analyticsEvent);
        }

        [Test]
        public void SampleCodeForGitHubReadMeManuallyConstructingEventObject()
        {
            //create a new EventTracker
            IEventTracker eventTracker = new EventTracker();
            //create a new event to pass to the event tracker
            IUniversalAnalyticsEvent analyticsEvent = new UniversalAnalyticsEvent(
                //Required. The universal analytics tracking id for the property 
                // that events will be logged to. If you don't want to pass this every time, set the UniversalAnalytics.TrackingId 
                // app setting and use the UniversalAnalyticsEventFactory to get instances of this class.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#tid for details.
                "UA-52982625-3",
                //Required. client id. 
                //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.
                "developer",
                //Required. The event category for the event. 
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.
                "test category",
                //Required. The event action for the event. 
                //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.
                "test action",
                //Optional. The event label for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.
                "test label",
                //Optional. The event value for the event.
                // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.
                "10");
            eventTracker.TrackEvent(analyticsEvent);
        }

        [Test]
        public void Send100EventsAsQuicklyAsPossibleAndARateLimitStillWontOccur()
        {
            var eventTracker = new EventTracker();

            for (var i = 1; i <= 100; i++)
            {
                try
                {
                    var analyticsEvent = this.EVENT_FACTORY.MakeUniversalAnalyticsEvent(
                         "rate limit test user",
                         "rate limit test category",
                         "rate limit test action",
                         "rate limit test label",
                         i.ToString());
                    eventTracker.TrackEvent(analyticsEvent);
                }catch(Exception e)
                {
                    Assert.Fail("failed after " + i + " transmissions with the following exeption: "+ e.Message);
                }
            }
        }

        [Test]
        public void SendingEventsFromTwoDifferentUsersShowsUpAsTwoActiveSessionsInUniversalAnalytics()
        {
            //you actually have to watch the analytics property to see this one happening. No assertion necessary.
            var eventTracker = new EventTracker();

            var analyticsEventForUser1 = this.EVENT_FACTORY.MakeUniversalAnalyticsEvent(
                    "test user 1",
                    "rate limit test category",
                    "rate limit test action",
                    "rate limit test lable");
            eventTracker.TrackEvent(analyticsEventForUser1);

            var analyticsEventForUser2 = this.EVENT_FACTORY.MakeUniversalAnalyticsEvent(
                "test user 2",
                "test category",
                "test action",
                "test label");
            eventTracker.TrackEvent(analyticsEventForUser2);
        }
    }
}
