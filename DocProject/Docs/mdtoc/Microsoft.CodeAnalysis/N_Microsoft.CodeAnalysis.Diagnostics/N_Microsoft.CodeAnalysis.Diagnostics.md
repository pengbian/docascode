---
namespace: Microsoft.CodeAnalysis.Diagnostics
---

---
class: Microsoft.CodeAnalysis.Diagnostics.CompilationStartedEvent
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationStartedEvent.#ctor(Microsoft.CodeAnalysis.Compilation)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationStartedEvent.ToString
---

---
class: Microsoft.CodeAnalysis.Diagnostics.CompilationCompletedEvent
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationCompletedEvent.#ctor(Microsoft.CodeAnalysis.Compilation)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationCompletedEvent.ToString
---

---
class: Microsoft.CodeAnalysis.Diagnostics.UnresolvedAnalyzerReference
---

---
property: Microsoft.CodeAnalysis.Diagnostics.UnresolvedAnalyzerReference.IsUnresolved
---

---
property: Microsoft.CodeAnalysis.Diagnostics.UnresolvedAnalyzerReference.Display
---

---
property: Microsoft.CodeAnalysis.Diagnostics.UnresolvedAnalyzerReference.FullPath
---

---
method: Microsoft.CodeAnalysis.Diagnostics.UnresolvedAnalyzerReference.#ctor(System.String)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.UnresolvedAnalyzerReference.GetAnalyzers(System.String)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.UnresolvedAnalyzerReference.GetAnalyzersForAllLanguages
---

---
class: Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions.SyntaxTreeActionsCount
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions.SyntaxNodeActionsCount
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions.SemanticModelActionsCount
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions.CodeBlockStartActionsCount
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions.CompilationEndActionsCount
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions.CodeBlockEndActionsCount
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions.SymbolActionsCount
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions.CompilationStartActionsCount
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions.Append(Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver
---

---
field: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.analyzerOptions
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.CompilationEventQueue
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.DiagnosticQueue
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.#ctor(System.Collections.Immutable.ImmutableArray{Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer},Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions,System.Threading.CancellationToken,System.Func{System.Exception,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Boolean})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.WhenCompletedAsync
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.GetSessionAnalyzerActions(Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Action{Microsoft.CodeAnalysis.Diagnostic},System.Func{System.Exception,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Boolean},System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.ExecuteCompilationEndActions(Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions,Microsoft.CodeAnalysis.Compilation,Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions,System.Action{Microsoft.CodeAnalysis.Diagnostic},System.Func{System.Exception,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Boolean},System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.ExecuteSyntaxTreeActions(Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions,Microsoft.CodeAnalysis.SyntaxTree,Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions,System.Action{Microsoft.CodeAnalysis.Diagnostic},System.Func{System.Exception,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Boolean},System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.GetAnalyzerDiagnosticsAsync(Microsoft.CodeAnalysis.Compilation,System.Collections.Immutable.ImmutableArray{Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer},Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions,System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.GetAnalyzerDiagnosticsAsync(Microsoft.CodeAnalysis.Compilation,System.Collections.Immutable.ImmutableArray{Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer},System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.ExecuteCompilationStartActions(Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,Microsoft.CodeAnalysis.Compilation,Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions,System.Action{Microsoft.CodeAnalysis.Diagnostic},System.Func{System.Exception,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Boolean},System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.ExecuteAndCatchIfThrows(Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Action{Microsoft.CodeAnalysis.Diagnostic},System.Func{System.Exception,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Boolean},System.Action,System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.GetAnalyzerDiagnostic(Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Exception)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.GetDiagnosticsAsync
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.Dispose
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.ExecuteSymbolActions(Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions,System.Collections.Generic.IEnumerable{Microsoft.CodeAnalysis.ISymbol},Microsoft.CodeAnalysis.Compilation,Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions,System.Action{Microsoft.CodeAnalysis.Diagnostic},System.Func{System.Exception,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Boolean},System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.GetEffectiveDiagnostics(System.Collections.Generic.IEnumerable{Microsoft.CodeAnalysis.Diagnostic},Microsoft.CodeAnalysis.Compilation)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.AnalyzeDeclaringReferenceAsync(Microsoft.CodeAnalysis.Diagnostics.SymbolDeclaredCompilationEvent,Microsoft.CodeAnalysis.SyntaxReference,System.Action{Microsoft.CodeAnalysis.Diagnostic},System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.ExecuteSemanticModelActions(Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions,Microsoft.CodeAnalysis.SemanticModel,Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions,System.Action{Microsoft.CodeAnalysis.Diagnostic},System.Func{System.Exception,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Boolean},System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.IsDiagnosticAnalyzerSuppressed(Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,Microsoft.CodeAnalysis.CompilationOptions,System.Func{System.Exception,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Boolean})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver.Create(Microsoft.CodeAnalysis.Compilation,System.Collections.Immutable.ImmutableArray{Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer},Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions,Microsoft.CodeAnalysis.Compilation@,System.Threading.CancellationToken)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.CompilationUnitCompletedEvent
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CompilationUnitCompletedEvent.CompilationUnit
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CompilationUnitCompletedEvent.SemanticModel
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationUnitCompletedEvent.#ctor(Microsoft.CodeAnalysis.Compilation,Microsoft.CodeAnalysis.SyntaxTree)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationUnitCompletedEvent.FlushCache
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationUnitCompletedEvent.ToString
---

---
class: Microsoft.CodeAnalysis.Diagnostics.AnalyzerImageReference
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerImageReference.Display
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerImageReference.FullPath
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerImageReference.#ctor(System.Collections.Immutable.ImmutableArray{Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer},System.String,System.String)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerImageReference.GetAnalyzers(System.String)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerImageReference.GetAnalyzersForAllLanguages
---

---
class: Microsoft.CodeAnalysis.Diagnostics.CompilationEvent
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CompilationEvent.Compilation
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationEvent.FlushCache
---

---
class: Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzerAttribute
---

---
property: Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzerAttribute.SupportedLanguage
---

---
method: Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzerAttribute.#ctor
---

---
method: Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzerAttribute.#ctor(System.String)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver`1
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver`1.Dispose
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver`1.AnalyzeDeclaringReferenceAsync(Microsoft.CodeAnalysis.Diagnostics.SymbolDeclaredCompilationEvent,Microsoft.CodeAnalysis.SyntaxReference,System.Action{Microsoft.CodeAnalysis.Diagnostic},System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver`1.ExecuteCodeBlockActions(Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions,System.Collections.Generic.IEnumerable{Microsoft.CodeAnalysis.DeclarationInfo},Microsoft.CodeAnalysis.SemanticModel,Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions,System.Action{Microsoft.CodeAnalysis.Diagnostic},System.Func{System.Exception,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Boolean},System.Func{Microsoft.CodeAnalysis.SyntaxNode,`0},System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerDriver`1.ExecuteSyntaxNodeActions(Microsoft.CodeAnalysis.Diagnostics.AnalyzerActions,System.Collections.Generic.IEnumerable{Microsoft.CodeAnalysis.SyntaxNode},Microsoft.CodeAnalysis.SemanticModel,Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions,System.Action{Microsoft.CodeAnalysis.Diagnostic},System.Func{System.Exception,Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer,System.Boolean},System.Func{Microsoft.CodeAnalysis.SyntaxNode,`0},System.Threading.CancellationToken)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.SymbolDeclaredCompilationEvent
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SymbolDeclaredCompilationEvent.Symbol
---

---
method: Microsoft.CodeAnalysis.Diagnostics.SymbolDeclaredCompilationEvent.#ctor(Microsoft.CodeAnalysis.Compilation,Microsoft.CodeAnalysis.ISymbol)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.SymbolDeclaredCompilationEvent.#ctor(Microsoft.CodeAnalysis.Compilation,Microsoft.CodeAnalysis.ISymbol,System.Lazy{Microsoft.CodeAnalysis.SemanticModel})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.SymbolDeclaredCompilationEvent.ToString
---

---
method: Microsoft.CodeAnalysis.Diagnostics.SymbolDeclaredCompilationEvent.SemanticModel(Microsoft.CodeAnalysis.SyntaxReference)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.SymbolDeclaredCompilationEvent.FlushCache
---

---
class: Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference.Display
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference.FullPath
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference.IsUnresolved
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference.#ctor
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference.GetAnalyzersForAllLanguages
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerReference.GetAnalyzers(System.String)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.AsyncQueue`1
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AsyncQueue`1.WhenCompletedAsync
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AsyncQueue`1.IsCompleted
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AsyncQueue`1.Count
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AsyncQueue`1.#ctor(System.Threading.CancellationToken)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AsyncQueue`1.ToString
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AsyncQueue`1.Enqueue(`0)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AsyncQueue`1.TryDequeue(`0@)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AsyncQueue`1.SetException(System.Exception)
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AsyncQueue`1.Complete
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AsyncQueue`1.DequeueAsync(System.Threading.CancellationToken)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer
---

---
property: Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer.SupportedDiagnostics
---

---
method: Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer.Initialize(Microsoft.CodeAnalysis.Diagnostics.AnalysisContext)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions
---

---
property: Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions.AdditionalStreams
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions.#ctor(System.Collections.Immutable.ImmutableArray{Microsoft.CodeAnalysis.AdditionalStream})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalyzerOptions.WithAdditionalStreams(System.Collections.Immutable.ImmutableArray{Microsoft.CodeAnalysis.AdditionalStream})
---

---
class: Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext`1
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext`1.SemanticModel
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext`1.CancellationToken
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext`1.CodeBlock
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext`1.OwningSymbol
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext`1.Options
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext`1.RegisterSyntaxNodeAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext},System.Collections.Immutable.ImmutableArray{`0})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext`1.RegisterSyntaxNodeAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext},`0[])
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext`1.RegisterCodeBlockEndAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.CodeBlockEndAnalysisContext})
---

---
class: Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext.Node
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext.SemanticModel
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext.Options
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext.CancellationToken
---

---
method: Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.SemanticModelAnalysisContext
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SemanticModelAnalysisContext.CancellationToken
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SemanticModelAnalysisContext.SemanticModel
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SemanticModelAnalysisContext.Options
---

---
method: Microsoft.CodeAnalysis.Diagnostics.SemanticModelAnalysisContext.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext.Options
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext.Compilation
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext.Symbol
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext.CancellationToken
---

---
method: Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.CodeBlockEndAnalysisContext
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CodeBlockEndAnalysisContext.OwningSymbol
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CodeBlockEndAnalysisContext.SemanticModel
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CodeBlockEndAnalysisContext.CancellationToken
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CodeBlockEndAnalysisContext.CodeBlock
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CodeBlockEndAnalysisContext.Options
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CodeBlockEndAnalysisContext.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.AnalysisContext
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalysisContext.RegisterSyntaxNodeAction``1(System.Action{Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext},System.Collections.Immutable.ImmutableArray{``0})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalysisContext.RegisterSymbolAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext},System.Collections.Immutable.ImmutableArray{Microsoft.CodeAnalysis.SymbolKind})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalysisContext.RegisterCompilationStartAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalysisContext.RegisterSyntaxTreeAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.SyntaxTreeAnalysisContext})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalysisContext.RegisterSemanticModelAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.SemanticModelAnalysisContext})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalysisContext.RegisterSyntaxNodeAction``1(System.Action{Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext},``0[])
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalysisContext.RegisterSymbolAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext},Microsoft.CodeAnalysis.SymbolKind[])
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalysisContext.RegisterCodeBlockStartAction``1(System.Action{Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext{``0}})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalysisContext.RegisterCodeBlockEndAction``1(System.Action{Microsoft.CodeAnalysis.Diagnostics.CodeBlockEndAnalysisContext})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.AnalysisContext.RegisterCompilationEndAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.CompilationEndAnalysisContext})
---

---
class: Microsoft.CodeAnalysis.Diagnostics.CompilationEndAnalysisContext
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CompilationEndAnalysisContext.Options
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CompilationEndAnalysisContext.CancellationToken
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CompilationEndAnalysisContext.Compilation
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationEndAnalysisContext.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic)
---

---
class: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.Compilation
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.CancellationToken
---

---
property: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.Options
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.RegisterSemanticModelAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.SemanticModelAnalysisContext})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.RegisterCodeBlockEndAction``1(System.Action{Microsoft.CodeAnalysis.Diagnostics.CodeBlockEndAnalysisContext})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.RegisterCompilationEndAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.CompilationEndAnalysisContext})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.RegisterSyntaxNodeAction``1(System.Action{Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext},``0[])
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.RegisterSymbolAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext},System.Collections.Immutable.ImmutableArray{Microsoft.CodeAnalysis.SymbolKind})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.RegisterCodeBlockStartAction``1(System.Action{Microsoft.CodeAnalysis.Diagnostics.CodeBlockStartAnalysisContext{``0}})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.RegisterSyntaxNodeAction``1(System.Action{Microsoft.CodeAnalysis.Diagnostics.SyntaxNodeAnalysisContext},System.Collections.Immutable.ImmutableArray{``0})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.RegisterSyntaxTreeAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.SyntaxTreeAnalysisContext})
---

---
method: Microsoft.CodeAnalysis.Diagnostics.CompilationStartAnalysisContext.RegisterSymbolAction(System.Action{Microsoft.CodeAnalysis.Diagnostics.SymbolAnalysisContext},Microsoft.CodeAnalysis.SymbolKind[])
---

---
class: Microsoft.CodeAnalysis.Diagnostics.SyntaxTreeAnalysisContext
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SyntaxTreeAnalysisContext.Options
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SyntaxTreeAnalysisContext.Tree
---

---
property: Microsoft.CodeAnalysis.Diagnostics.SyntaxTreeAnalysisContext.CancellationToken
---

---
method: Microsoft.CodeAnalysis.Diagnostics.SyntaxTreeAnalysisContext.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic)
---

