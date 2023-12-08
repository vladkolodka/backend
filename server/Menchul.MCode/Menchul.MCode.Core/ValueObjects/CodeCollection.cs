using Menchul.MCode.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menchul.MCode.Core.ValueObjects
{
    public class CodeCollection : Dictionary<CodeType, string>, IEquatable<Dictionary<CodeType, string>>
    {
        public bool Equals(Dictionary<CodeType, string> other)
        {
            if (other == null)
            {
                return false;
            }

            return Count == other.Count && !this.Except(other).Any();
        }
    }
}