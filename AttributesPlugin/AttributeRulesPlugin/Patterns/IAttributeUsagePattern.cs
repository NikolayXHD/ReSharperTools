using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AttributeRulesPlugin
{
	interface IAttributeUsagePattern
	{
		bool IsClassAttribute(IAttribute a, out string errorMessage);

		bool IsFieldAttribute(IAttribute a, out string errorMessage);

		bool MandatoryAttributeOnField { get; }

		bool MandatoryAttributeOnClass { get; }

		string MissingFieldAttributeErrorMessage { get; }

		string MissingClassAttributeErrorMessage { get; }
	}
}