namespace Microsoft.Content.BuildEngine.MRef.Stubbing
{
    using Microsoft.Content.Build.ReflectionXmlSyntax;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Xml.XPath;

    public class HierarchySyntaxVisitor
        : ISyntaxVisitor<SyntaxVisitorContext>
    {
        #region Fields
        private readonly IApiResolver m_resolver;
        #endregion

        #region Ctors

        public HierarchySyntaxVisitor(
            string coreFile,
            Func<string, XPathDocument> documentLoader)
        {
            m_resolver = ApiResolver.CreateFileBaseResolver(coreFile, documentLoader);
        }

        #endregion

        #region Public methods

        public string GetHierarchyText(string id, string file, bool showCMod = false)
        {
            var api = m_resolver.Resolve(new ApiReference { Id = id, File = file });
            var options = DisplayOptions.Default;
            var memberApi = api as MemberApi;
            if (memberApi != null)
            {
                if (memberApi.Overloads == null)
                {
                    options &= ~DisplayOptions.Overload;
                }
                else if (showCMod)
                {
                    options |= DisplayOptions.ShowCMod;
                }
            }
            return GetHierarchyText(api, file, options);
        }

        public string GetHierarchyText(string id, string file, DisplayOptions options)
        {
            var api = m_resolver.Resolve(new ApiReference { Id = id, File = file });
            return GetHierarchyText(api, file, options);
        }

        public string GetHierarchyText(IApi api, string file, DisplayOptions options = DisplayOptions.Default)
        {
            var writer = new StringWriter();
            api.Accept(this, new SyntaxVisitorContext(writer, options));
            return writer.ToString();
        }

        #endregion

        #region Private methods

        private void VisitSimpleType(SimpleTypeApi api, SyntaxVisitorContext context)
        {
            if (api.ContainerType != null)
            {
                api.ContainerType.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
                context.Write(".");
            }
            var index = api.Name.IndexOf("`");
            if (index == -1)
            {
                context.Write(api.Name);
            }
            else
            {
                context.Write(api.Name.Remove(index));
            }
            if (context.HasOptions(DisplayOptions.Template))
            {
                VisitTemplate(api as IHasGenericParameters, context.RemoveOptions(DisplayOptions.Hierarchy));
            }
        }

        private void VisitTemplate(IHasGenericParameters hasGenericParameters, SyntaxVisitorContext context)
        {
            if (hasGenericParameters == null)
                return;
            if (hasGenericParameters.Templates == null)
                return;
            if (hasGenericParameters.Templates.Count == 0)
                return;
            context.Write("(");
            if (hasGenericParameters.Specialization == null)
            {
                context.Write(string.Join(", ", from t in hasGenericParameters.Templates select t.Name));
            }
            else
            {
                for (int i = 0; i < hasGenericParameters.Specialization.Count; i++)
                {
                    if (i != 0)
                        context.Write(", ");
                    hasGenericParameters.Specialization[i].Accept(this, context);
                }
            }
            context.Write(")");
        }

        private void VisitParameters(IHasParameters hasParameteres, SyntaxVisitorContext context)
        {
            if (hasParameteres == null)
                return;
            if (hasParameteres.Parameters == null)
                return;
            if (hasParameteres.Parameters.Count == 0)
                return;
            context.Write(" (");
            for (int i = 0; i < hasParameteres.Parameters.Count; i++)
            {
                if (i != 0)
                    context.Write(", ");
                hasParameteres.Parameters[i].Type.Accept(this, context);
            }
            if (hasParameteres.HasVarArgs)
            {
                if (hasParameteres.Parameters.Count != 0)
                    context.Write(", ");
                context.Write("...");
            }
            context.Write(")");
        }

        private void WriteProcedureName(ProcedureApi api, SyntaxVisitorContext context)
        {
            if (!api.IsEii)
            {
                context.Write(api.Name);
            }
            else
            {
                Debug.Assert(api.Implements.Count == 1, string.Format("Expected implements count is 1 for eii, actual is {0}", api.Implements.Count));
                Debug.Assert(api.Implements[0] is ApiReference, string.Format("Expected implements first child is ApiReference, actual is {0}", api.Implements[0].GetType()));
                var apiRef = (ApiReference)api.Implements[0];
                Debug.Assert(apiRef.Container != null, "Expected implements first child container is not null");
                apiRef.Container.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
                context.Write(".");
                context.Write(api.Name);
            }
        }

        private void HandleEiiOverload(MemberOverloadsApi api, SyntaxVisitorContext context)
        {
            var firstMemberRef = api.Members.First();
            var firstMember = m_resolver.Resolve(new ApiReference { Id = firstMemberRef.Id, File = firstMemberRef.File });
            Debug.Assert(firstMember is ProcedureApi,
                string.Format("Unexpected member type, expected type ProcedureApi, actual {0}, id: {1}", firstMember.GetType(), firstMemberRef.Id));
            var procedure = (ProcedureApi)firstMember;
            if (procedure.IsEii)
            {
                Debug.Assert(procedure.Implements.Count == 1, string.Format("Expected implements count is 1 for eii, actual is {0}", procedure.Implements.Count));
                Debug.Assert(procedure.Implements[0] is ApiReference, string.Format("Expected implements first child is ApiReference, actual is {0}", procedure.Implements[0].GetType()));
                var apiRef = (ApiReference)procedure.Implements[0];
                Debug.Assert(apiRef.Container != null, "Expected implements first child container is not null");
                var containterReference = apiRef.Container;
                if (string.IsNullOrEmpty(apiRef.Container.File))
                {
                    containterReference = new ApiReference { Id = apiRef.Container.Id, File = firstMemberRef.File };
                }
                containterReference.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
                context.Write(".");
            }
        }

        #endregion

        #region ISyntaxVisitor<SyntaxVisitorContext>

        public void Visit(NamespaceGroupApi api, SyntaxVisitorContext context)
        {
            context.Write(api.Name);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Namespaces");
        }

        public void Visit(NamespaceApi api, SyntaxVisitorContext context)
        {
            context.Write(api.Name);
        }

        public void Visit(ClassApi api, SyntaxVisitorContext context)
        {
            VisitSimpleType(api, context);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Class");
        }

        public void Visit(StructureApi api, SyntaxVisitorContext context)
        {
            VisitSimpleType(api, context);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Structure");
        }

        public void Visit(InterfaceApi api, SyntaxVisitorContext context)
        {
            VisitSimpleType(api, context);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Interface");
        }

        public void Visit(DelegateApi api, SyntaxVisitorContext context)
        {
            VisitSimpleType(api, context);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Delegate");
        }

        public void Visit(EnumerationApi api, SyntaxVisitorContext context)
        {
            VisitSimpleType(api, context);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Enumeration");
        }

        public void Visit(MethodsApi api, SyntaxVisitorContext context)
        {
            api.DeclaringType.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Methods");
        }

        public void Visit(FieldsApi api, SyntaxVisitorContext context)
        {
            api.DeclaringType.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Fields");
        }

        public void Visit(PropertiesApi api, SyntaxVisitorContext context)
        {
            api.DeclaringType.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Properties");
        }

        public void Visit(EventsApi api, SyntaxVisitorContext context)
        {
            api.DeclaringType.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Events");
        }

        public void Visit(AttachedPropertiesApi api, SyntaxVisitorContext context)
        {
            api.DeclaringType.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Attached Properties");
        }

        public void Visit(AttachedEventsApi api, SyntaxVisitorContext context)
        {
            api.DeclaringType.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Attached Events");
        }

        public void Visit(OperatorsApi api, SyntaxVisitorContext context)
        {
            api.DeclaringType.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Operators and Type Conversions");
        }

        public void Visit(ConstructorOverloadsApi api, SyntaxVisitorContext context)
        {
            api.DeclaringType.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Constructor");
        }

        public void Visit(MethodOverloadsApi api, SyntaxVisitorContext context)
        {
            HandleEiiOverload(api, context);
            context.Write(api.Name);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Method");
        }

        public void Visit(PropertyOverloadsApi api, SyntaxVisitorContext context)
        {
            HandleEiiOverload(api, context);
            context.Write(api.Name);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Property");
        }

        public void Visit(OperatorOverloadsApi api, SyntaxVisitorContext context)
        {
            context.Write(api.Name);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                if (api.Name == "Explicit" || api.Name == "Implicit")
                    context.Write(" Conversion Operators");
                else
                    context.Write(" Operator");
        }

        public void Visit(FieldApi api, SyntaxVisitorContext context)
        {
            context.Write(api.Name);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Field");
        }

        public void Visit(ConstructorApi api, SyntaxVisitorContext context)
        {
            api.DeclaringType.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Constructor");
            if (context.HasOptions(DisplayOptions.Overload))
                VisitParameters(api, context.RemoveOptions(DisplayOptions.Hierarchy));
        }

        public void Visit(OperatorApi api, SyntaxVisitorContext context)
        {
            context.Write(api.Name);
            if (api.Name == "Explicit" || api.Name == "Implicit")
            {
                if (context.HasOptions(DisplayOptions.Hierarchy))
                    context.Write(" Conversion");
                context.Write(" (");
                // Only show return type for conversion which has no parameter
                if (api.Parameters.Count > 0)
                {
                    api.Parameters[0].Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
                    context.Write(" to ");
                }
                api.ReturnType.Accept(this, context.RemoveOptions(DisplayOptions.Hierarchy));
                context.Write(")");
            }
            else
            {
                if (context.HasOptions(DisplayOptions.Hierarchy))
                    context.Write(" Operator");
                if (context.HasOptions(DisplayOptions.Overload))
                    VisitParameters(api, context.RemoveOptions(DisplayOptions.Hierarchy));
            }
        }

        public void Visit(MethodApi api, SyntaxVisitorContext context)
        {
            WriteProcedureName(api, context);
            if (context.HasOptions(DisplayOptions.Template))
                VisitTemplate(api, context.RemoveOptions(DisplayOptions.Hierarchy));
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Method");
            if (context.HasOptions(DisplayOptions.Overload))
                VisitParameters(api, context.RemoveOptions(DisplayOptions.Hierarchy));
        }

        public void Visit(PropertyApi api, SyntaxVisitorContext context)
        {
            WriteProcedureName(api, context);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Property");
            if (context.HasOptions(DisplayOptions.Overload))
                VisitParameters(api, context.RemoveOptions(DisplayOptions.Hierarchy));
        }

        public void Visit(EventApi api, SyntaxVisitorContext context)
        {
            WriteProcedureName(api, context);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Event");
        }

        public void Visit(AttachedPropertyApi api, SyntaxVisitorContext context)
        {
            WriteProcedureName(api, context);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Attached Property");
            if (context.HasOptions(DisplayOptions.Overload))
                VisitParameters(api, context.RemoveOptions(DisplayOptions.Hierarchy));
        }

        public void Visit(AttachedEventApi api, SyntaxVisitorContext context)
        {
            WriteProcedureName(api, context);
            if (context.HasOptions(DisplayOptions.Hierarchy))
                context.Write(" Attached Event");
        }

        public void Visit(ApiReference reference, SyntaxVisitorContext context)
        {
            m_resolver.Resolve(reference).Accept(this, context);
        }

        public void Visit(SpecializedApiReference reference, SyntaxVisitorContext context)
        {
            m_resolver.Resolve(reference).Accept(this, context);
        }

        public void Visit(GenericParameterReference reference, SyntaxVisitorContext context)
        {
            context.Write(reference.Name);
        }

        public void Visit(PointerDecoratorApi reference, SyntaxVisitorContext context)
        {
            reference.Inner.Accept(this, context);
            context.Write("*");
        }

        public void Visit(ArrayDecoratorApi reference, SyntaxVisitorContext context)
        {
            reference.Inner.Accept(this, context);
            context.Write("[]");
        }

        public void Visit(ByRefDecoratorApi reference, SyntaxVisitorContext context)
        {
            reference.Inner.Accept(this, context);
        }

        public void Visit(CModDecoratorApi reference, SyntaxVisitorContext context)
        {
            reference.Inner.Accept(this, context);
            if (context.HasOptions(DisplayOptions.ShowCMod))
            {
                foreach (var item in reference.ReqCMod)
                {
                    context.Write("|");
                    item.Accept(this, context);
                }
                foreach (var item in reference.OptCMod)
                {
                    context.Write("!");
                    item.Accept(this, context);
                }
            }
        }

        public void Visit(GenericParameter gp, SyntaxVisitorContext context)
        {
            context.Write(gp.Name);
        }

        public void Visit(Parameter parameter, SyntaxVisitorContext context)
        {
            parameter.Type.Accept(this, context);
        }

        #endregion

    }
}
