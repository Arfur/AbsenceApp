/*
===============================================================================
 File        : HeaderCommentCodeFixProvider.cs
 Namespace   : AbsenceApp.Analyzers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Roslyn CodeFixProvider paired with HeaderCommentAnalyzer (AA0001).
               Offers an "Add file header comment" quick-action that inserts
               the standard block-header template at the top of the file.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial code-fix implementation.
===============================================================================
*/

using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;

namespace AbsenceApp.Analyzers;

// ============================================================================
// HeaderCommentCodeFixProvider — inserts the standard header template
// ============================================================================
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(HeaderCommentCodeFixProvider))]
[Shared]
public sealed class HeaderCommentCodeFixProvider : CodeFixProvider
{
    // =========================================================================
    // Metadata
    // =========================================================================

    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(HeaderCommentAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() =>
        WellKnownFixAllProviders.BatchFixer;

    // =========================================================================
    // Register fix
    // =========================================================================

    public override async System.Threading.Tasks.Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken)
                                .ConfigureAwait(false);
        if (root is null) return;

        var diagnostic = context.Diagnostics[0];

        context.RegisterCodeFix(
            CodeAction.Create(
                title:            "Add file header comment",
                createChangedDocument: ct => AddHeaderAsync(context.Document, root, ct),
                equivalenceKey:   nameof(HeaderCommentCodeFixProvider)),
            diagnostic);
    }

    // =========================================================================
    // Fix implementation
    // =========================================================================

    private static System.Threading.Tasks.Task<Document> AddHeaderAsync(
        Document document,
        SyntaxNode root,
        System.Threading.CancellationToken cancellationToken)
    {
        var fileName  = System.IO.Path.GetFileName(document.FilePath ?? document.Name);
        var today     = System.DateTime.UtcNow.ToString("yyyy-MM-dd");
        var nameSpace = document.Project.DefaultNamespace ?? "AbsenceApp";

        var header = $@"/*
===============================================================================
 File        : {fileName}
 Namespace   : {nameSpace}
 Author      : 
 Version     : 1.0.0
 Created     : {today}
 Updated     : {today}
-------------------------------------------------------------------------------
 Purpose     : <describe the purpose of this file>
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  {today}  Initial creation.
===============================================================================
*/
";

        var commentTrivia = SyntaxFactory.Comment(header);
        var newLine       = SyntaxFactory.CarriageReturnLineFeed;

        var newRoot = root.WithLeadingTrivia(
            SyntaxFactory.TriviaList(commentTrivia, newLine)
                         .AddRange(root.GetLeadingTrivia()));

        return System.Threading.Tasks.Task.FromResult(document.WithSyntaxRoot(newRoot));
    }
}
