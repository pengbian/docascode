using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class NamespaceMembersMemberMetadata : BaseMetadata, INamespaceMembersMember
    {
        private List<INamespaceMembersMember> _members = new List<INamespaceMembersMember>();

        public List<INamespaceMembersMember> Members { get { return _members; } set { _members = value; } }
    }
}
