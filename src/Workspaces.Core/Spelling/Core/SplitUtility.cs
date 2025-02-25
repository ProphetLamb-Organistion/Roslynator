﻿// This code is originally from https://github.com/josefpihrt/orang. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.RegularExpressions;

namespace Roslynator.Spelling
{
    internal static class SplitUtility
    {
        private const string _splitCasePattern = @"
    (?<=
        \p{Lu}
    )
    (?=
        \p{Lu}\p{Ll}
    )
|
    (?<=
        \p{Ll}
    )
    (?=
        \p{Lu}
    )
";

        private static readonly Regex _splitCaseRegex = new(
            _splitCasePattern,
            RegexOptions.IgnorePatternWhitespace);

        private static readonly Regex _splitHyphenRegex = new("-");

        private static readonly Regex _splitCaseAndHyphenRegex = new(
            "-|" + _splitCasePattern,
            RegexOptions.IgnorePatternWhitespace);

        public static Regex GetSplitRegex(SplitMode splitMode)
        {
            return splitMode switch
            {
                SplitMode.None => null,
                SplitMode.Case => _splitCaseRegex,
                SplitMode.Hyphen => _splitHyphenRegex,
                SplitMode.CaseAndHyphen => _splitCaseAndHyphenRegex,
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
