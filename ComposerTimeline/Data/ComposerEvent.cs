using NathanHarrenstein.ComposerDatabase;
using NathanHarrenstein.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NathanHarrenstein.ComposerTimeline.Data
{
    public class ComposerEvent : Event
    {
        public Composer Composer { get; set; }

        public string this[string key]
        {
            get
            {
                var targetProperty = Composer.ComposerProperties.FirstOrDefault(property => property.Key == key);

                if (targetProperty == null)
                {
                    targetProperty = new ComposerProperty { Key = key };

                    Composer.ComposerProperties.Add(targetProperty);
                }

                return targetProperty.Value;
            }
        }
    }
}