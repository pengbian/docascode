// Guids.cs
// MUST match guids.h
using System;

namespace Company.MyPackage
{
    static class GuidList
    {
        public const string guidMyPackagePkgString = "5f89250c-1405-4d24-a40b-8a15404cabeb";
        public const string guidMyPackageCmdSetString = "0be74276-7d6f-4c26-9029-402bc9b533e1";

        public static readonly Guid guidMyPackageCmdSet = new Guid(guidMyPackageCmdSetString);
    };
}