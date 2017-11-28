[![NuGet version](http://img.shields.io/nuget/v/UniversalAnalyticsMeasurementProtocolWrapper.svg)](https://www.nuget.org/packages/UniversalAnalyticsMeasurementProtocolWrapper/)
[![Nuget downloads](http://img.shields.io/nuget/dt/UniversalAnalyticsMeasurementProtocolWrapper.svg)](http://www.nuget.org/packages/UniversalAnalyticsMeasurementProtocolWrapper/)

Universal-Analytics-For-DotNet
==============================

A .NET wrapper over top of Google's Universal Analytics Measurement Protocol HTTP API. For now, this wrapper allows you to push Events from server-side code. This offers the advantages of 1) not relying on client-side javascript and 2) allowing you to push more interesting events that may not be available on the client side. For example, if you have a website for collecting donations you could push an event to indicate that a donation occurred and push the "value" of that donation. See https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide#event for more details.

Pushing an event is as simple as the following:

1. Add your tracking id to your App.config/Web.config:
```
<configuration>
    <appSettings>
        <add key="UniversalAnalytics.TrackingId" value="UA-XXXXXXXX-X"/>
    </appSettings>
    ...
</configuration>
```

2. Push an event using Google's `anonymousClientId` (https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid) and/or `userId` (https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid):
```
    // Use your favorite dependency injection framework for these. Singletons preferred.
    IEventTracker eventTracker = new EventTracker();
    // The factory will use the config file to set the tracking Id.
    IUniversalAnalyticsEventFactory eventFactory = new UniversalAnalyticsEventFactory();

    var analyticsEvent = eventFactory.MakeUniversalAnalyticsEvent(
       //Required if no user id. 
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
       "10",
       //Required if no client id. 
       //See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid for details.
       "user-id"
       );
    eventTracker.TrackEvent(analyticsEvent);
```

The code is almost entirely unit/integration tested so it should be stable and easily updatable. I'm using it on my own site right now so you can find more specific examples at: https://github.com/jakejgordon/NemeStats 

For your own application you will probably want to create an additional wrapper over top of this so you can confine the EventCategory and EventAction values to something that makes sense for your own app (without having to hard-code magic strings for the parameters). My website has examples of this as well.

![Alt text](https://raw.githubusercontent.com/jakejgordon/Universal-Analytics-For-DotNet/master/universal_analytics_realtime_events_screenshot.jpg?raw=true "Screenshot of Real-Time Events After Pushing Data")

# NuGet
[Check out the NuGet page for this package](https://www.nuget.org/packages/UniversalAnalyticsMeasurementProtocolWrapper/)
