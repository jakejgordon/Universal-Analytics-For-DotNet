using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class UniversalAnalyticsEventFactoryTests
    {
        private IConfigurationManager configurationManagerMock;
        UniversalAnalyticsEventFactory factory;
        private string trackingId = "tracking id";
        private string clientId = "anonymous client id";
        private string eventCategory = "event category";
        private string eventAction = "event action";
        private string eventLabel = "event label";
        private string eventValue = "500";
        private string userId = "user id";

        [SetUp]
        public void SetUp()
        {
            configurationManagerMock = MockRepository.GenerateMock<IConfigurationManager>();

             factory = new UniversalAnalyticsEventFactory(configurationManagerMock);
        }

        [Obsolete("Remove when the factory method is finally removed.")]
        [Test]
        public void ItReturnsANewUniversalAnalyticsEvent()
        {
            configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(trackingId);

            var analyticsEvent = factory.MakeUniversalAnalyticsEvent(
                clientId,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue);

            //generally prefer to have a separate test for each case but this will do just fine.
            Assert.AreEqual(trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(clientId, analyticsEvent.AnonymousClientId);
            Assert.AreEqual(eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(eventValue, analyticsEvent.EventValue);
        }

        [Test]
        public void ItReturnsAUniversalAnalyticsEventWithUserId()
        {
            configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(trackingId);

            var analyticsEvent = factory.MakeEventForUserId(
                userId,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue);

            Assert.AreEqual(trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(userId, analyticsEvent.UserId);
            Assert.AreEqual(eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(eventValue, analyticsEvent.EventValue);
        }

        [Test]
        public void ItReturnsAUniversalAnalyticsEventWithClientId()
        {
            configurationManagerMock.Expect(mock => mock.GetAppSetting(UniversalAnalyticsEventFactory.APP_KEY_UNIVERSAL_ANALYTICS_TRACKING_ID))
                .Return(trackingId);

            var analyticsEvent = factory.MakeEventForClientId(
                clientId,
                eventCategory,
                eventAction,
                eventLabel,
                eventValue);

            Assert.AreEqual(trackingId, analyticsEvent.TrackingId);
            Assert.AreEqual(clientId, analyticsEvent.AnonymousClientId);
            Assert.AreEqual(eventCategory, analyticsEvent.EventCategory);
            Assert.AreEqual(eventAction, analyticsEvent.EventAction);
            Assert.AreEqual(eventLabel, analyticsEvent.EventLabel);
            Assert.AreEqual(eventValue, analyticsEvent.EventValue);
        }
    }
}
