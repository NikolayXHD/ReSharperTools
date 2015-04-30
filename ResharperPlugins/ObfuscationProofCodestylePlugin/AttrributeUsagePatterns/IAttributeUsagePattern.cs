using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	interface IAttributeUsagePattern
	{
		bool IsClassAttribute(IAttribute a, out string errorMessage, out string warningMessage);

		bool IsFieldAttribute(IAttribute a, out string errorMessage, out string warningMessage);

		bool MandatoryAttributeOnField { get; }

		bool MandatoryAttributeOnClass { get; }

		string MissingFieldAttributeErrorMessage { get; }

		string MissingClassAttributeErrorMessage { get; }

		IBulbAction[] GetPropertyFixes(IPropertyDeclaration declaration);

		IBulbAction[] GetClassFixes(IClassDeclaration declaration);

		bool ShouldFollowPattern(IClassDeclaration declaration);
	}
}