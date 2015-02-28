﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    public interface IMetadata
    {
        Identity Identity { get; set; }

        IMetadata Parent { get; set; }

        IMetadata OwnerNamespace { get; set; }

        MemberType MemberType { get; set; }

        Version MscorlibVersion { get; set; }

        string FilePath { get; set; }

        string AssemblyName { get; set; }

        ISyntaxDescriptionGroup SyntaxDescriptionGroup { get; set; }

        Task AcceptAsync<TContext>(IMetadataVisitor<TContext> visitor, TContext context);
    }

    public enum MemberType
    {
        Assembly,
        Namespace,
        Class,
        Interface,
        Struct,
        Delegate,
        Enum,
        Field,
        Property,
        Event,
        Constructor,
        Method,
    }
}
