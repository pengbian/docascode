// Guids.cs
// MUST match guids.h
using System;

namespace MicrosoftIT.DocProject
{
    static class GuidList
    {
        public const string guidDocProjectPkgString = "197d8579-59cd-4d80-bf9e-8e01e270775c";
        public const string guidDocProjectCmdSetString = "21e1986e-b1b5-4c1c-8361-eaa2dbc8cdbf";
        public const string guidDocProjectFactoryString = "11A2578D-D1B7-4B68-87F4-CFFB2F21F86E";

        public static readonly Guid guidDocProjectCmdSet = new Guid(guidDocProjectCmdSetString);
        public static readonly Guid guidDocProjectFactory = new Guid(guidDocProjectFactoryString);
    };
}