using NathanHarrenstein.ExtendedDateTimeFormat.Serializers;

namespace NathanHarrenstein.ExtendedDateTimeFormat
{
    public class ExtendedDateTimeRange : IExtendedDateTimeSetType
    {
        public ISingleExtendedDateTimeType Start { get; set; }

        public ISingleExtendedDateTimeType End { get; set; }

        public override string ToString()
        {
            return ExtendedDateTimeRangeSerializer.Serialize(this);
        }
    }
}