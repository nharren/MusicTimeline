using System.EDTF;

namespace NathanHarrenstein.Timeline
{
    public interface ITimelineEra
    {
        ExtendedDateTimeInterval Dates { get; }
    }
}