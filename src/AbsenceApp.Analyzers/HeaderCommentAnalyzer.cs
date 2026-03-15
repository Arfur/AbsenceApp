/*
===============================================================================
 File        : HeaderCommentAnalyzer.cs
 Namespace   : AbsenceApp.Analyzers
 Author      : Michael
 Version     : 1.0.0
 Created     : 2026-03-13
 Updated     : 2026-03-13
-------------------------------------------------------------------------------
 Purpose     : Roslyn DiagnosticAnalyzer that reports AA0001 when a .cs file
               does not begin with the required block-header comment.
-------------------------------------------------------------------------------
 Description :
   The rule checks that the very first token in the compilation unit is a
   block comment that contains all of the required header fields:
     File, Namespace, Author, Version, Created, Updated, Purpose, Changes.
   The check is case-insensitive and looks for each label followed by
   optional whitespace and a colon.
-------------------------------------------------------------------------------
 Changes     :
   - 1.0.0  2026-03-13  Initial analyzer implementation.
===============================================================================
*/

using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AbsenceApp.Analyzers;

// ============================================================================
// HeaderCommentAnalyzer — enforces block-header comment at top of every file
// ============================================================================
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class HeaderCommentAnalyzer : DiagnosticAnalyzer
{
    // =========================================================================
    // Diagnostic descriptor
    // =========================================================================

    public const string DiagnosticId = "AA0001";

    private static readonly DiagnosticDescriptor _rule = new(
        id:                 DiagnosticId,
        title:              "Missing file header comment",
        messageFormat:      "File '{0}' does not contain the required block-header comment (AA0001). " +
                            "Add a /* ... */ comment at the top of the file containing: " +
                            "File, Namespace, Author, Version, Created, Updated, Purpose, Changes.",
        category:           "Style",
        defaultSeverity:    DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description:        "All AbsenceApp .cs files must begin with the standard block-header comment.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(_rule);

    // -------------------------------------------------------------------------
    // Required header labels (all must appear inside the leading /* ... */ block)
    // -------------------------------------------------------------------------
    private static readonly string[] RequiredLabels =
    {
        "File", "Namespace", "Author", "Version",
        "Created", "Updated", "Purpose", "Changes",
    };

    // =========================================================================
    // Initialisation
    // =========================================================================

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxTreeAction(AnalyzeTree);
    }

    // =========================================================================
    // Tree analysis
    // =========================================================================

    private static void AnalyzeTree(SyntaxTreeAnalysisContext context)
    {
        var root = context.Tree.GetRoot(context.CancellationToken);

        // Locate the first block comment attached to the compilation unit
        var leadingTrivia = root.GetLeadingTrivia();
        var blockComment  = leadingTrivia
            .FirstOrDefault(t => t.IsKind(SyntaxKind.MultiLineCommentTrivia));

        if (blockComment == default)
        {
            ReportMissing(context, root.GetLocation());
            return;
        }

        var commentText = blockComment.ToString();

        foreach (var label in RequiredLabels)
        {
            // Match " Label    :" or " Label:" anywhere in the comment (case-insensitive)
            if (!Regex.IsMatch(commentText, $@"\b{Regex.Escape(label)}\s*:", RegexOptions.IgnoreCase))
            {
                ReportMissing(context, root.GetLocation());
                return;
            }
        }
    }

    private static void ReportMissing(SyntaxTreeAnalysisContext context, Location location)
    {
        var fileName = System.IO.Path.GetFileName(context.Tree.FilePath);
        context.ReportDiagnostic(Diagnostic.Create(_rule, location, fileName));
    }
}
