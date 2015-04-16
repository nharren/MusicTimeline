using System.ExtendedDateTimeFormat;

namespace NathanHarrenstein.Timeline
{
    public interface ITimelineEra
    {
        ExtendedDateTimeInterval Dates { get; }
    }
}