﻿using System.ExtendedDateTimeFormat;

namespace NathanHarrenstein.Timeline
{
    public interface ITimelineEvent
    {
        ExtendedDateTimeInterval Dates { get; }
    }
}