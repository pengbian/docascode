using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class NamespaceMembersMemberMetadata : BaseMetadata, INamespaceMembersMember
    {
        public List<INamespaceMembersMember> Members { get; set; }

        public ISyntaxDescriptionGroup SyntaxDescriptionGroup { get; set; }
    }
}
