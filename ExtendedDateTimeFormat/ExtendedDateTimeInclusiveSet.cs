using NathanHarrenstein.ExtendedDateTimeFormat.Serializers;
using System.Collections.ObjectModel;

namespace NathanHarrenstein.ExtendedDateTimeFormat
{
    public class ExtendedDateTimeInclusiveSet : Collection<IExtendedDateTimeSetType>, IExtendedDateTimeType
    {
        public override string ToString()
        {
            return ExtendedDateTimeInclusiveSetSerializer.Serialize(this);
        }
    }
}