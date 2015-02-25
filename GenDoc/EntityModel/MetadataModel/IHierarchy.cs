using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public interface IHierarchy
    {
        Stack<string> InheritanceHierarchy { get; set; }
    }
}
