using CommunityToolkit.Mvvm.Messaging.Messages;
using TrackApp.Models;

namespace TrackApp.Messages;

public class HistoryTrackSelectedChangedMessage : ValueChangedMessage<CustomTrack>
{
    public HistoryTrackSelectedChangedMessage(CustomTrack value)
        : base(value)
    {        
    }
}