using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class Identity
    {
        Tuple<string, string[]> _fullyQualifiedName;

        public Identity(string fullyQualifiedName, params string[] suffix)
        {
            _fullyQualifiedName = Tuple.Create(fullyQualifiedName, suffix);
        }

        public override string ToString()
        {
            return _fullyQualifiedName.ToString();
        }

        public static implicit operator string (Identity memberIdentity)
        {
            return memberIdentity.ToString();
        }

        public static implicit operator Identity(string name)
        {
            return new Identity(name);
        }
    }
}
