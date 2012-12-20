// Guids.cs
// MUST match guids.h

using System;

namespace Toe.ToeVsExt
{
    static class GuidList
    {
        public const string guidToeVisualStudioExtensionPkgString = "cef6b0ee-98c6-451f-938c-dc0552e5fa55";
        public const string guidToeVisualStudioExtensionCmdSetString = "68d601df-ff53-4acc-8536-f9d52d0e7d11";
        public const string guidToeVisualStudioExtensionEditorFactoryString = "8849afd9-0123-432e-a555-91b1fc487d67";

        public static readonly Guid guidToeVisualStudioExtensionCmdSet = new Guid(guidToeVisualStudioExtensionCmdSetString);
        public static readonly Guid guidToeVisualStudioExtensionEditorFactory = new Guid(guidToeVisualStudioExtensionEditorFactoryString);
    };
}