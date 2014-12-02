using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace DocAsCode
{
    class DeveloperCommentsTransform
    {
        private readonly XslCompiledTransform CommentsXslCompiledTranform;

        public DeveloperCommentsTransform(XslCompiledTransform transform)
        {
            CommentsXslCompiledTranform = transform;
        }

    }
}
