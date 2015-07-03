using System.EDTF;

namespace NathanHarrenstein.Timeline
{
    public interface ITimelineEvent
    {
        ExtendedDateTimeInterval Dates { get; }
    }
}