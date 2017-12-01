[![NuGet version](http://img.shields.io/nuget/v/UniversalAnalyticsMeasurementProtocolWrapper.svg)](https://www.nuget.org/packages/UniversalAnalyticsMeasurementProtocolWrapper/)
[![Nuget downloads](http://img.shields.io/nuget/dt/UniversalAnalyticsMeasurementProtocolWrapper.svg)](http://www.nuget.org/packages/UniversalAnalyticsMeasurementProtocolWrapper/)

Universal-Analytics-For-DotNet
==============================

A .NET wrapper over top of Google's Universal Analytics Measurement Protocol HTTP API. For now, this wrapper allows you to push Events from server-side code. This offers the advantages of 1) not relying on client-side javascript and 2) allowing you to push more interesting events that may not be available on the client side. For example, if you have a website for collecting donations you could push an event to indicate that a donation occurred and push the "value" of that donation. See https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide#event for more details.

# Event Tracking

First, add your tracking id to your App.config/Web.config:
```
<configuration>
    <appSettings>
        <add key="UniversalAnalytics.TrackingId" value="UA-XXXXXXXX-X"/>
    </appSettings>
    ...
</configuration>
```

Then, create an event tracker and factory; you can use your favorite injection framework for the tracker and factory. Singletons preferred.
```
IEventTracker eventTracker = new EventTracker();
// The factory pulls your tracking ID from the .config so you don't have to.
IUniversalAnalyticsEventFactory eventFactory = new UniversalAnalyticsEventFactory();
```

Then, you must create an event to push to Google Analytics. Note that Google has defined that an event must have either a `cid`, [see here](https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid), or `uid`, [see here](https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid).
To create an event with an anonymous client id:
```
// Create a clientId with a random Guid...
IAnonymousClientId clientId = new AnonymousClientId();
// OR from a supplied Guid...
IAnonymousClientId clientId = new AnonymousClientId(new Guid("...");
// OR from a supplied string (creates a version 5 guid from the string as the client id).
IAnonymousClientId clientId = new AnonymousClientId("...");

var analyticsEvent = eventFactory.MakeUniversalAnalyticsEvent(
   // Required. The client id associated with this event.
   // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid for details.
   clientId,
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
   "10");
```

You can also push an client id event without using `AnonymousClientId`:
```
var analyticsEvent = eventFactory.MakeUniversalAnalyticsEvent(
	// Required. The client id for this event. 
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
	"10");
```

Or, to push an event with a user id:
```
// Create a user id from a string
IUserId userId = new UserId("user id");

// Create an event with a user id:
var analyticsEvent = eventFactory.MakeUniversalAnalyticsEvent(
   // Required. The user id associated with this event.
   // See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid for details.
   userId,
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
   "10");
```

You can also push an event with a user id without using `UserId`:
```
var analyticsEvent = eventFactory.MakeUniversalAnalyticsEvent(
   // This is the client id. To specify a user id as a string, this must be null.
   null,
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
   "user-id");
```

Finally, all you need to do is push the event to Google Analytics using the EventTracker:

```
var trackingResult = eventTracker.TrackEvent(analyticsEvent);

// Note that exceptions are contained in the result object and not thrown for stability reasons.
if (trackingResult.Failed)
{
	// Log to the appropriate error handler.
	Console.Error.WriteLine(trackingResult.Exception);
}
```

# Notes
The code is almost entirely unit/integration tested so it should be stable and easily updatable. I'm using it on my own site right now so you can find more specific examples at: https://github.com/jakejgordon/NemeStats 

For your own application you will probably want to create an additional wrapper over top of this so you can confine the EventCategory and EventAction values to something that makes sense for your own app (without having to hard-code magic strings for the parameters). My website has examples of this as well.

![Alt text](https://raw.githubusercontent.com/jakejgordon/Universal-Analytics-For-DotNet/master/universal_analytics_realtime_events_screenshot.jpg?raw=true "Screenshot of Real-Time Events After Pushing Data")

# NuGet
[Check out the NuGet page for this package](https://www.nuget.org/packages/UniversalAnalyticsMeasurementProtocolWrapper/)
