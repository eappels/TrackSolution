using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TrackApp.Messages;

public class LocationUpdatedMessage : ValueChangedMessage<Location>
{
    public LocationUpdatedMessage(Location value)
        : base(value)
    {
    }
}