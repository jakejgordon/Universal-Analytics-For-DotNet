using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;
using Rhino.Mocks;

namespace UniversalAnalyticsHttpWrapper.Tests
{
    [TestFixture]
    public class EventTrackerTests
    {
        private EventTracker _eventTracker;
        private IUniversalAnalyticsEvent _analyticsEvent;
        private IPostDataBuilder _postDataBuilderMock;
        private IGoogleDataSender _googleDataSenderMock;

        [SetUp]
        public void SetUp()
        {
            _postDataBuilderMock = MockRepository.GenerateMock<IPostDataBuilder>();
            _googleDataSenderMock = MockRepository.GenerateMock<IGoogleDataSender>();
            _eventTracker = new EventTracker(_postDataBuilderMock, _googleDataSenderMock);

            _analyticsEvent = MockRepository.GenerateMock<IUniversalAnalyticsEvent>();
        }

        [Test]
        public void ItSendsTheDataToGoogle()
        {
            string expectedPostData = "some amazing string that matches what google requires";
            
            _postDataBuilderMock.Expect(mock => mock.BuildPostDataString(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything,
                    Arg<NameValueCollection>.Is.Anything))
                .Return(expectedPostData);

            _eventTracker.TrackEvent(_analyticsEvent);

            _googleDataSenderMock.AssertWasCalled(mock => 
                mock.SendData(EventTracker.GOOGLE_COLLECTION_URI, expectedPostData));
        }

        [Test]
        public async Task ItSendsTheDataToGoogleAsync()
        {
            var expectedPostData = new List<KeyValuePair<string, string>>();

            _postDataBuilderMock.Expect(mock => mock.BuildPostDataCollection(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything,
                    Arg<NameValueCollection>.Is.Anything))
                .Return(expectedPostData);

            _googleDataSenderMock.Expect(mock => mock.SendDataAsync(
                    Arg<Uri>.Is.Anything,
                    Arg<IEnumerable<KeyValuePair<string, string>>>.Is.Anything))
                .Return(Task.FromResult(0));
            
            await _eventTracker.TrackEventAsync(_analyticsEvent);

            _googleDataSenderMock.AssertWasCalled(mock =>
                mock.SendDataAsync(EventTracker.GOOGLE_COLLECTION_URI, expectedPostData));
        }

        [Test]
        public void ItBubblesExceptionsToTheTrackingResult()
        {
            var expectedException = new HttpException(400, "bad request");

            _postDataBuilderMock.Expect(mock => mock.BuildPostDataCollection(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything,
                    Arg<NameValueCollection>.Is.Anything
                    ))
                .Return(new List<KeyValuePair<string, string>>());

            _googleDataSenderMock.Expect(mock => mock.SendData(Arg<Uri>.Is.Anything,
                    Arg<string>.Is.Anything))
                .Throw(expectedException);

            var result = _eventTracker.TrackEvent(_analyticsEvent);

            Assert.AreEqual(expectedException, result.Exception);
        }

        [Test]
        public async void ItBubblesExceptionsToTheTrackingResultAsync()
        {
            var expectedException = new HttpException(400, "bad request");

            _postDataBuilderMock.Expect(mock => mock.BuildPostDataCollection(
                    Arg<string>.Is.Anything,
                    Arg<UniversalAnalyticsEvent>.Is.Anything,
                    Arg<NameValueCollection>.Is.Anything))
                .Return(new List<KeyValuePair<string, string>>());

            _googleDataSenderMock.Expect(mock => mock.SendDataAsync(Arg<Uri>.Is.Anything,
                    Arg<IEnumerable<KeyValuePair<string, string>>>.Is.Anything))
                .Throw(expectedException);

            var result = await _eventTracker.TrackEventAsync(_analyticsEvent);

            Assert.AreEqual(expectedException, result.Exception);
        }


        [Test]
        public void ItRaisesExceptionIfSupportedParameterSentToCustomPayloadUserId()
        {
            string parameterName = PostDataBuilder.PARAMETER_KEY_USER_ID;
            IEventTracker realEventTracker = new EventTracker();

            Assert.Throws<ArgumentException>(() => realEventTracker.AddToCustomPayload(parameterName, "muDummyUserId"), "EventTracker.AddToCustomPayload must throw exception when supported parameter is passed as an argument");
        }

        [Test]
        public void ItRaisesExceptionIfSupportedParameterSentToCustomPayloadTrackingId()
        {
            string parameterName = PostDataBuilder.PARAMETER_KEY_TRACKING_ID;
            IEventTracker realEventTracker = new EventTracker();

            Assert.Throws<ArgumentException>(() => realEventTracker.AddToCustomPayload(parameterName, "UA-123123-1"), "EventTracker.AddToCustomPayload must throw exception when supported parameter is passed as an argument");
        }

        [Test]
        public void ValidateHitShouldPass()
        {
            IEventTracker realEventTracker = new EventTracker();
            IUniversalAnalyticsEvent analyticsEvent = new UniversalAnalyticsEventFactory().MakeUniversalAnalyticsEvent("DummyClientId", "Dummy Category", "Dummy Action", "DummyLabel");

            TrackingResult res = realEventTracker.ValidateHit(analyticsEvent);

            Assert.IsTrue(!res.Failed, "Test analytics event object (_analyticsEvent) must be valid for this test method to work");

            Assert.IsTrue(res.ValidationStatus, "Analytics event validation has failed. Need to reconfigure test analytics event");
        }

        [Test]
        public void ValidateHitShouldFailIncorrectEventValue()
        {
            IEventTracker realEventTracker = new EventTracker();
            IUniversalAnalyticsEvent analyticsEvent = new UniversalAnalyticsEventFactory().MakeUniversalAnalyticsEvent("DummyClientId", "Dummy Category", "Dummy Action", "DummyLabel");

            realEventTracker.AddToCustomPayload("qt", "12313123123123123"); //parameter value for queue time is way above accepted values
            TrackingResult res = realEventTracker.ValidateHit(analyticsEvent);

            /*
              Expected output from GA
              {
               "hitParsingResult": [ {
                    "valid": false,
                    "parserMessage": [ {
                        "messageType": "ERROR",
                        "description": "The value provided for parameter 'qt' is out of bounds. Please see http://goo.gl/a8d4RP#qt for details.",
                        "messageCode": "VALUE_OUT_OF_BOUNDS",
                        "parameter": "qt"
                    } ],
                "hit": "/debug/collect?v=1\u0026cid=sdf\u0026tid=UA-12323423-2\u0026t=event\u0026ec=123\u0026el=123\u0026ev=1\u0026qt=12313123123123123"
                } ],
                "parserMessage": [ {
                "messageType": "INFO",
                "description": "Found 1 hit in the request."
                } ]
            }
            */

            Assert.IsTrue(!res.ValidationStatus, "Validation should fail here as parameter value for queue time is way above accepted values");
        }

    }
}
