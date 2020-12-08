namespace Try {
  using System;
  using System.Linq;

  using Microsoft.CodeAnalysis;
  using Microsoft.CodeAnalysis.CSharp;
  using Microsoft.CodeAnalysis.CSharp.Syntax;

  class Program {
    static void Main(string[] args) {
      Console.WriteLine("Hello World!");
      //var Methods = typeof(A).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
      // .Where(E => E.ReturnType == typeof(void))
      // .Select(E => (E, E.GetCustomAttributes<SignalRClientMethodAttribute>()))
      // .Where(E => E.Item2.Count() > 0)
      // .SelectMany(E => E.Item2.Select(EE => { EE.Method = E.E; return EE; }))
      // .GroupBy(E => E.TargetMethodName(E.Method))
      // .ToDictionary(E => E.Key, E => E.Select(EE => EE).ToArray());
      //Console.WriteLine(Methods.Values.FirstOrDefault().FirstOrDefault().Method.ReturnType);
      //Console.WriteLine(Methods.Values.FirstOrDefault().FirstOrDefault().Method.GetParameters());
      //A A1 = new A();
      //var Fp = typeof(Action<>).MakeGenericType(Methods.Values.FirstOrDefault().FirstOrDefault().Method.GetParameters().Select(E => E.ParameterType).ToArray());
      //Console.WriteLine(Fp);
      //Console.WriteLine(Methods.Values.FirstOrDefault().FirstOrDefault().Method.CreateDelegate(Fp, A1).GetType());

      //var P = typeof(HubConnectionExtensions).GetMethods();
      //var PP = P.MakeGenericMethod(new Type[] { typeof(string) });
      //Console.WriteLine(PP);




      Console.ReadKey();
    }
    //private static async Task<bool> ModifySolutionUsingSyntaxRewriter(string solutionPath) {
    //  using (var workspace = MSBuildWorkspace.Create()) {
    //    // Selects a Solution File
    //    var solution = await workspace.OpenSolutionAsync(solutionPath);
    //    // Iterates through every project
    //    foreach (var project in solution.Projects) {
    //      // Iterates through every file
    //      foreach (var document in project.Documents) {
    //        // Selects the syntax tree
    //        var syntaxTree = await document.GetSyntaxTreeAsync();
    //        var root = syntaxTree.GetRoot();

    //        // Generates the syntax rewriter
    //        var rewriter = new AttributeStatementChanger();
    //        root = rewriter.Visit(root);

    //        // Exchanges the document in the solution by the newly generated document
    //        solution = solution.WithDocumentSyntaxRoot(document.Id, root);
    //      }
    //    }
    //    // applies the changes to the solution
    //    var result = workspace.TryApplyChanges(solution);
    //    return result;
    //  }
    //}

  }

  public class AttributeStatementChanger : CSharpSyntaxRewriter {
    /// Visited for all AttributeListSyntax nodes
    /// The method replaces all PreviousAttribute attributes annotating a method by ReplacementAttribute attributes
    public override SyntaxNode VisitAttributeList(AttributeListSyntax node) {
      // If the parent is a MethodDeclaration (= the attribute annotes a method)
      if (node.Parent is MethodDeclarationSyntax &&
          // and if the attribute name is PreviousAttribute
          node.Attributes.Any(
              currentAttribute => currentAttribute.Name.GetText().ToString() == "PreviousAttribute")) {
        // Return an alternate node that is injected instead of the current node
        return SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("ReplacementAttribute"),
                            SyntaxFactory.AttributeArgumentList(
                                SyntaxFactory.SeparatedList(new[]
                                {
                                    SyntaxFactory.AttributeArgument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(@"Sample"))
                                        )
                                })))));
      }
      // Otherwise the node is left untouched
      return base.VisitAttributeList(node);
    }
  }

  /// The method calling the Syntax Rewriter


  //  public class A {
  //  [SignalRClientMethod("1")]
  //  public void Method1(string P1) { }
  //  [SignalRClientMethod("1")]
  //  public void Method2() { }
  //  [SignalRClientMethod("1")]
  //  public void Method3() { }

  //  [SignalRClientMethod("2")]
  //  public void Method4() { }
  //  [SignalRClientMethod("2")]
  //  public void Method5() { }
  //}



}
