using NathanHarrenstein.ExtendedDateTimeFormat.Serializers;
using System.Collections.ObjectModel;

namespace NathanHarrenstein.ExtendedDateTimeFormat
{
    public class ExtendedDateTimeExclusiveSet : Collection<IExtendedDateTimeSetType>, ISingleExtendedDateTimeType
    {
        public override string ToString()
        {
            return ExtendedDateTimeExclusiveSetSerializer.Serialize(this);
        }
    }
}