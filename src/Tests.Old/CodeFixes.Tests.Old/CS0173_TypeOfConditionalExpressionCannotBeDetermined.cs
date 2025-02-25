﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text.RegularExpressions;

namespace Roslynator.CSharp.CodeFixes.Tests
{
    internal static class CS0173_TypeOfConditionalExpressionCannotBeDetermined
    {
        private static void Foo()
        {
            var derived1 = default(Derived1);
            var derived2 = default(Derived2);

            bool condition = false;

            var x = (condition) ? derived1 : derived2;
        }

        private class Base
        {
        }

        private class Derived1 : Base
        {
        }

        private class Derived2 : Base
        {
        }
    }
}
