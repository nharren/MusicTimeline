using System;
using System.Collections.Generic;
using System.ExtendedDateTimeFormat;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public partial class Recording
    {
        public ExtendedDateTimeInterval Dates
        {
            get
            {
                return (ExtendedDateTimeInterval)ExtendedDateTimeFormatParser.Parse(DatesString);
            }
            set
            {
                DatesString = value.ToString();
            }
        }
    }
}
