using AbbyyLS.ReSharper;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity(AttributeWarningHighlighting.SeverityId,
  null,
  HighlightingGroupIds.ConstraintViolation,
  "Attribute warning",
  "",
  Severity.WARNING,
  false)]

namespace AbbyyLS.ReSharper
{
	[ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING)]
	public class AttributeWarningHighlighting : IHighlighting
	{
		public const string SeverityId = "AttributeWarning";

		private readonly ITreeNode _expression;
		private readonly string _message;
		private readonly IBulbAction[] _fixes;

		public AttributeWarningHighlighting(ITreeNode expression, string message, IBulbAction[] fixes)
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