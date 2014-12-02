using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DocAsCode
{
    internal class AddDocumentOperation : CodeActionOperation
    {
        private ReferenceDocumentHeler helper;
        private string assemblyName;
        private string apiName;
        private string fileName;

        public AddDocumentOperation(IServiceProvider sp, string assembly, string qualifiedFileName, string qualifiedAPIName)
        {
            helper = new ReferenceDocumentHeler(sp);
            assemblyName = assembly;
            fileName = qualifiedFileName;
            apiName = qualifiedAPIName;
        }

        protected override void Apply(Workspace workspace, CancellationToken cancellationToken)
        {
            base.Apply(workspace, cancellationToken);

            helper.CreateReferenceDocument(assemblyName, fileName, apiName);
        }
    }
}
