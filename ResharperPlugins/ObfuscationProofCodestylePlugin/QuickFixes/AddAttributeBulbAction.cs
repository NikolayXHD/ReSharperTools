using JetBrains.Application.Progress;
using JetBrains.DocumentManagers;
using JetBrains.DocumentModel;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.CodeStyle;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.TextControl;
using JetBrains.Util;

namespace AbbyyLS.ReSharper
{
	public abstract class AddAttributeBulbAction<T> : IBulbAction
		where T : ICSharpDeclaration, IAttributesOwnerDeclaration
	{
		private readonly T _propertyDeclaration;

		protected AddAttributeBulbAction(T propertyDeclaration)
		{
			_propertyDeclaration = propertyDeclaration;
		}

		public void Execute(ISolution solution, ITextControl textControl)
		{
			var psiModule = _propertyDeclaration.GetPsiModule();
			var resolveContext = _propertyDeclaration.GetProject().GetResolveContext();

			var attributeType = getAttributeType(psiModule, resolveContext, AttributeName);
			var attributeDeclaration = createAttributeDeclaration(attributeType, FixedParamValue, NamedParamName, NamedParamValue, psiModule, resolveContext);

			addOrReplaceAttribute(_propertyDeclaration, attributeDeclaration, attributeType, GetType().Name);
			reformatSourceCode(_propertyDeclaration, solution);
		}

		private static ITypeElement getAttributeType(IPsiModule psiModule, IModuleReferenceResolveContext resolveContext, string attributeName)
		{
			return TypeElementUtil.GetTypeElementByClrName(new ClrTypeName(attributeName), psiModule, resolveContext);
		}

		private static IAttribute createAttributeDeclaration(ITypeElement attributeType, string fixedParamValue, string namedParamName, string namedParamValue, IPsiModule psiModule, IModuleReferenceResolveContext resolveContext)
		{
			var fixedArguments = createFixedArguments(fixedParamValue, psiModule, resolveContext);
			var namedArguments = createNamedArguments(namedParamName, namedParamValue, psiModule, resolveContext);

			var elementFactory = CSharpElementFactory.GetInstance(psiModule);
			var attributeDeclaration = elementFactory.CreateAttribute(attributeType, fixedArguments, namedArguments);
			return attributeDeclaration;
		}

		private static AttributeValue[] createFixedArguments(string fixedParamValue, IPsiModule psiModule, IModuleReferenceResolveContext resolveContext)
		{
			var fixedArguments = fixedParamValue != null
				? new[] { new AttributeValue(new ConstantValue(fixedParamValue, psiModule, resolveContext)) }
				: new AttributeValue[0];
			return fixedArguments;
		}

		private static Pair<string, AttributeValue>[] createNamedArguments(string positionalParamName, string positionalParamValue, IPsiModule psiModule, IModuleReferenceResolveContext resolveContext)
		{
			var namedArguments = positionalParamName != null
				? new[] { new Pair<string, AttributeValue>(positionalParamName, new AttributeValue(new ConstantValue(positionalParamValue, psiModule, resolveContext))) }
				: new Pair<string, AttributeValue>[0];
			return namedArguments;
		}

		private static void addOrReplaceAttribute(T propertyDeclaration, IAttribute attributeDeclaration, ITypeElement attributeType, string commandName)
		{
			propertyDeclaration.GetPsiServices().Transactions.Execute(commandName, () =>
			{
				var originalAttribute = propertyDeclaration.Attributes
					.FirstOrDefault(a => Equals(a.GetAttributeType(), attributeType));

				if (originalAttribute != null)
					replaceAttribute(originalAttribute, attributeDeclaration);
				else
					insertAttribute(propertyDeclaration, attributeDeclaration);
			});
		}

		private static void replaceAttribute(IAttribute originalAttribute, IAttribute attributeDeclaration)
		{
			ModificationUtil.ReplaceChild(originalAttribute, attributeDeclaration);
		}

		private static void insertAttribute(T propertyDeclaration, IAttribute attributeDeclaration)
		{
			var existingAttributes = propertyDeclaration.Attributes;

			var anchorAttribute = existingAttributes.Count > 0
				? existingAttributes[existingAttributes.Count - 1]
				: null;

			propertyDeclaration.AddAttributeAfter(attributeDeclaration, anchorAttribute);
		}

		private static void reformatSourceCode(T propertyDeclaration, ISolution solution)
		{
			IFile containingFile = propertyDeclaration.GetContainingFile();
			IRangeMarker marker = propertyDeclaration.GetDocumentRange().CreateRangeMarker(solution.GetComponent<DocumentManager>());
			containingFile.OptimizeImportsAndRefs(marker, false, true, NullProgressIndicator.Instance);
		}

		public string Text
		{
			get { return string.Format(DescriptionPattern, _propertyDeclaration.NameIdentifier.Name); }
		}

		protected abstract string AttributeName { get; }

		protected virtual string FixedParamValue { get { return PropertyName; } }

		protected virtual string NamedParamName { get { return null; } }

		protected virtual string NamedParamValue { get { return null; } }

		protected abstract string DescriptionPattern { get; }

		protected string PropertyName { get { return _propertyDeclaration.NameIdentifier.Name; } }
	}
}