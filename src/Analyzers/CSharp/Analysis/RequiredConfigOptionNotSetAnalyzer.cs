﻿// Copyright (c) Josef Pihrt and Contributors. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslynator.CSharp.Analysis
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal sealed class RequiredConfigOptionNotSetAnalyzer : AbstractRequiredConfigOptionNotSetAnalyzer
    {
        private static ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                if (_supportedDiagnostics.IsDefault)
                {
                    Immutable.InterlockedInitialize(
                        ref _supportedDiagnostics,
                        DiagnosticRules.AddOrRemoveAccessibilityModifiers,
                        DiagnosticRules.AddOrRemoveParenthesesFromConditionInConditionalOperator,
                        DiagnosticRules.ConfigureAwait,
                        DiagnosticRules.IncludeParenthesesWhenCreatingNewObject,
                        DiagnosticRules.NormalizeNullCheck,
                        DiagnosticRules.UseAnonymousFunctionOrMethodGroup,
                        DiagnosticRules.UseBlockBodyOrExpressionBody,
                        DiagnosticRules.UseEmptyStringLiteralOrStringEmpty,
                        DiagnosticRules.UseExplicitlyOrImplicitlyTypedArray,
                        DiagnosticRules.UseHasFlagMethodOrBitwiseOperator,
                        DiagnosticRules.UseImplicitOrExplicitObjectCreation,
                        CommonDiagnosticRules.RequiredConfigOptionNotSet);
                }

                return _supportedDiagnostics;
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterCompilationStartAction(compilationContext =>
            {
                var flags = Flags.None;

                CompilationOptions compilationOptions = compilationContext.Compilation.Options;

                compilationContext.RegisterSyntaxTreeAction(context =>
                {
                    if (!CommonDiagnosticRules.RequiredConfigOptionNotSet.IsEffective(context.Tree, compilationOptions, context.CancellationToken))
                        return;

                    AnalyzerConfigOptions options = context.GetConfigOptions();

                    Validate(ref context, compilationOptions, options, Flags.AddOrRemoveAccessibilityModifiers, ref flags, DiagnosticRules.AddOrRemoveAccessibilityModifiers, ConfigOptions.AccessibilityModifiers);
                    Validate(ref context, compilationOptions, options, Flags.AddOrRemoveParenthesesFromConditionInConditionalOperator, ref flags, DiagnosticRules.AddOrRemoveParenthesesFromConditionInConditionalOperator, ConfigOptions.ConditionalOperatorConditionParenthesesStyle);
                    Validate(ref context, compilationOptions, options, Flags.ConfigureAwait, ref flags, DiagnosticRules.ConfigureAwait, ConfigOptions.ConfigureAwait);
                    Validate(ref context, compilationOptions, options, Flags.IncludeParenthesesWhenCreatingNewObject, ref flags, DiagnosticRules.IncludeParenthesesWhenCreatingNewObject, ConfigOptions.ObjectCreationParenthesesStyle);
                    Validate(ref context, compilationOptions, options, Flags.NormalizeNullCheck, ref flags, DiagnosticRules.NormalizeNullCheck, ConfigOptions.NullCheckStyle);
                    Validate(ref context, compilationOptions, options, Flags.UseAnonymousFunctionOrMethodGroup, ref flags, DiagnosticRules.UseAnonymousFunctionOrMethodGroup, ConfigOptions.UseAnonymousFunctionOrMethodGroup);
                    Validate(ref context, compilationOptions, options, Flags.UseBlockBodyOrExpressionBody, ref flags, DiagnosticRules.UseBlockBodyOrExpressionBody, ConfigOptions.BodyStyle);
                    Validate(ref context, compilationOptions, options, Flags.UseEmptyStringLiteralOrStringEmpty, ref flags, DiagnosticRules.UseEmptyStringLiteralOrStringEmpty, ConfigOptions.EmptyStringStyle);
                    Validate(ref context, compilationOptions, options, Flags.UseExplicitlyOrImplicitlyTypedArray, ref flags, DiagnosticRules.UseExplicitlyOrImplicitlyTypedArray, ConfigOptions.ArrayCreationTypeStyle);
                    Validate(ref context, compilationOptions, options, Flags.UseHasFlagMethodOrBitwiseOperator, ref flags, DiagnosticRules.UseHasFlagMethodOrBitwiseOperator, ConfigOptions.EnumHasFlagStyle);
                    Validate(ref context, compilationOptions, options, Flags.UseImplicitOrExplicitObjectCreation, ref flags, DiagnosticRules.UseImplicitOrExplicitObjectCreation, ConfigOptions.ObjectCreationTypeStyle);
                });
            });
        }

        private static void Validate(
            ref SyntaxTreeAnalysisContext context,
            CompilationOptions compilationOptions,
            AnalyzerConfigOptions configOptions,
            Flags flag,
            ref Flags flags,
            DiagnosticDescriptor analyzer,
            ConfigOptionDescriptor option)
        {
            if (!flags.HasFlag(flag)
                && analyzer.IsEffective(context.Tree, compilationOptions, context.CancellationToken)
                && TryReportRequiredOptionNotSet(context, configOptions, analyzer, option))
            {
                flags |= flag;
            }
        }

        [Flags]
        private enum Flags
        {
            None,
            AddOrRemoveAccessibilityModifiers,
            AddOrRemoveParenthesesFromConditionInConditionalOperator,
            ConfigureAwait,
            IncludeParenthesesWhenCreatingNewObject,
            NormalizeNullCheck,
            UseAnonymousFunctionOrMethodGroup,
            UseBlockBodyOrExpressionBody,
            UseEmptyStringLiteralOrStringEmpty,
            UseExplicitlyOrImplicitlyTypedArray,
            UseHasFlagMethodOrBitwiseOperator,
            UseImplicitOrExplicitObjectCreation,
        }
    }
}
