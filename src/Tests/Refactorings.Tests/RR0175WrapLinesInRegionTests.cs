﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Roslynator.Testing.CSharp;
using Xunit;

namespace Roslynator.CSharp.Refactorings.Tests
{
    public class RR0175WrapLinesInRegionTests : AbstractCSharpRefactoringVerifier
    {
        public override string RefactoringId { get; } = RefactoringIdentifiers.WrapLinesInRegion;

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task Test_EndsAtStartOfLine()
        {
            await VerifyRefactoringAsync(@"
class C
{
[|    void M()
    {
    }
|]}
", @"
class C
{
    #region
    void M()
    {
    }
    #endregion
}
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task Test_EndsAtEndOfLine()
        {
            await VerifyRefactoringAsync(@"
class C
{
[|    void M()
    {
    }|]
}
", @"
class C
{
    #region
    void M()
    {
    }
    #endregion
}
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task Test_StartsAfterSingeLineDocumentationComment()
        {
            await VerifyRefactoringAsync(@"
class C
{
    /// <summary>
    /// 
    /// </summary>
[|    void M()
    {
    }
|]}
", @"
class C
{
    /// <summary>
    /// 
    /// </summary>
    #region
    void M()
    {
    }
    #endregion
}
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task Test_StartsAndEndsInSingleLineDocumentationComment()
        {
            await VerifyRefactoringAsync(@"
class C
{
    /// <summary>
[|    /// 
|]    /// </summary>
    void M()
    {
    }
}
", @"
class C
{
    /// <summary>
    #region
    /// 
    #endregion
    /// </summary>
    void M()
    {
    }
}
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task Test_EndsAtEndOfSingleLineDocumentationComment()
        {
            await VerifyRefactoringAsync(@"
class C
{
[|    /// <summary>
    /// 
    /// </summary>|]
    void M()
    {
    }
}
", @"
class C
{
    #region
    /// <summary>
    /// 
    /// </summary>
    #endregion
    void M()
    {
    }
}
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task Test_MultiLineDocumentationComment()
        {
            await VerifyRefactoringAsync(@"
class C
{
[|    /**
     * <summary></summary>
     */
|]    void M()
    {
    }
}
", @"
class C
{
    #region
    /**
     * <summary></summary>
     */
    #endregion
    void M()
    {
    }
}
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task Test_EndsAtEndOfMultiLineDocumentationComment()
        {
            await VerifyRefactoringAsync(@"
class C
{
[|    /**
     * <summary></summary>
     */|]
    void M()
    {
    }
}
", @"
class C
{
    #region
    /**
     * <summary></summary>
     */
    #endregion
    void M()
    {
    }
}
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task Test_EntireText()
        {
            await VerifyRefactoringAsync(@"[|class C
{
    void M()
    {
    }
}
|]", @"#region
class C
{
    void M()
    {
    }
}
#endregion
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task TestNoRefactoring_SpanIsEmpty()
        {
            await VerifyNoRefactoringAsync(@"
class C
{
    void M()
    {
[||]
    }
}
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task TestNoRefactoring_StartInMultiLineDocumentationComment()
        {
            await VerifyNoRefactoringAsync(@"
class C
{
    /**
[|
     * <summary></summary>
     */
    void M()
|]    {
    }
}
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task TestNoRefactoring_EndsInMultiLineDocumentationComment()
        {
            await VerifyNoRefactoringAsync(@"
class C
[|{
    /**
     * <summary></summary>
|]     */
    void M()
    {
    }
}
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }

        [Fact, Trait(Traits.Refactoring, RefactoringIdentifiers.WrapLinesInRegion)]
        public async Task TestNoRefactoring_EndsInMultiLineDocumentationComment2()
        {
            await VerifyNoRefactoringAsync(@"
class C
[|{
    /**
     * <summary></summary>|]
     */
    void M()
    {
    }
}
", equivalenceKey: EquivalenceKey.Create(RefactoringId));
        }
    }
}
