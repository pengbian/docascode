using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public class NamespaceMetadata : BaseMetadata
    {
        public List<INamespaceMember> Members { get; set; }
    }
}
