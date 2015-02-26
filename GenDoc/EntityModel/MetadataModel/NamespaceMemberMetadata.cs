using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class NamespaceMemberMetadata : BaseMetadata, INamespaceMember
    {
        public List<INamespaceMembersMember> Members { get; set; }

        public Stack<Identity> InheritanceHierarchy { get; set; }
    }
}
