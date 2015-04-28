using AbbyyLS.ReSharper;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity(MissingAttributeHighlighting.SeverityId,
  null,
  HighlightingGroupIds.ConstraintViolation,
  "Missing required attribute",
  "",
  Severity.ERROR,
  false)]

namespace AbbyyLS.ReSharper
{
	[ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING)]
	public class MissingAttributeHighlighting : IHighlighting
	{
		public const string SeverityId = "MissingRequiredAttribute";

		private readonly ITreeNode _expression;
		private readonly string _message;
		private readonly IBulbAction[] _fixes;

		public MissingAttributeHighlighting(ITreeNode expression, string message, IBulbAction[] fixes)
		{
			_expression = expression;
			_message = message;
			_fixes = fixes;
		}

		public string ToolTip
		{
			get { return _message; }
		}

		public string ErrorStripeToolTip
		{
			get { return ToolTip; }
		}

		public int NavigationOffsetPatch
		{
			get { return 0; }
		}

		public bool IsValid()
		{
			return _expression == null || _expression.IsValid();
		}

		public IBulbAction[] Fixes { get { return _fixes; } }
	}
}