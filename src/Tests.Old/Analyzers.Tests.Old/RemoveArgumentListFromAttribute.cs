﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Roslynator.CSharp.Analyzers.Tests
{
    internal static class RemoveArgumentListFromAttribute
    {
        [Obsolete()]
        private static void Foo()
        {
        }
    }
}
