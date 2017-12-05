﻿using NUnit.Framework;
using System;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture, Ignore("Integration tests.")]
    public class IntegrationTests
    {
        private readonly UniversalAnalyticsEventFactory _eventFactory = new UniversalAnalyticsEventFactory();

        [Test]
        public void SampleCodeForGitHubReadMeUsingFactoryToGetEventObject()
        {
            // Use your favorite dependency injection framework for the tracker and factory. Singletons preferred.
            IEventTracker eventTracker = new EventTracker();
            // The factory pulls your tracking ID from the .config so you don't have to.
            IUniversalAnalyticsEventFactory eventFactory = new UniversalAnalyticsEventFactory();

            var analyticsEvent = eventFactory.MakeUniversalAnalyticsEvent(
               // Required if no user id. 
               // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.
               "35009a79-1a05-49d7-b876-2b884d0f825b",
               // Required. The event category for the event. 
               // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ec for details.
               "test category",
               // Required. The event action for the event. 
               //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ea for details.
               "test action",
               // Optional. The event label for the event.
               // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#el for details.
               "test label",
               // Optional. The event value for the event.
               // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ev for details.
               "10",
               // Required if no client id. 
               // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid for details.
               "user-id"
               );

            // Exceptions are contained in the result object and not thrown for stability reasons.
            var trackingResult = eventTracker.TrackEvent(analyticsEvent);

            if (trackingResult.Failed)
            {
                // Log to the appropriate error handler.
                Console.Error.WriteLine(trackingResult.Exception);
            }
        }

        [Test]
        public void SampleCodeForGitHubReadMeManuallyConstructingEventObject()
        {
            //Create a new EventTracker
            IEventTracker eventTracker = new EventTracker();
            //Create a new event to pass to the event tracker
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
                "10",
               //Required if no client id. 
               //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid for details.
               "user-id");
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
                    var analyticsEvent = this._eventFactory.MakeUniversalAnalyticsEvent(
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

            var analyticsEventForUser1 = this._eventFactory.MakeUniversalAnalyticsEvent(
                    "test user 1",
                    "rate limit test category",
                    "rate limit test action",
                    "rate limit test lable");
            eventTracker.TrackEvent(analyticsEventForUser1);

            var analyticsEventForUser2 = this._eventFactory.MakeUniversalAnalyticsEvent(
                "test user 2",
                "test category",
                "test action",
                "test label");
            eventTracker.TrackEvent(analyticsEventForUser2);
        }
    }
}
