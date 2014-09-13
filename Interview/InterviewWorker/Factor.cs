using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interview.InterviewWorker
{
    public class Factor
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var factor = obj as Factor;
            if ((Name == null) | (factor.Name == null))
                return false;
            return Name == factor.Name;
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }
}
