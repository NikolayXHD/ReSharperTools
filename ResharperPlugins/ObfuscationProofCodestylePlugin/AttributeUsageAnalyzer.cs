using System.Collections.Generic;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AbbyyLS.ReSharper
{
	[ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(AttributeErrorHighlighting) })]
	public class AttributeUsageAnalyzer : IElementProblemAnalyzer
	{
		private static readonly IAttributeUsagePattern[] Patterns =
		{
			new BlToolkitPattern(),
			new MongoPattern(),
			new DataContractPattern()
		};

		public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer)
		{
			if (!(element is IClassDeclaration))
				return;

			var classDeclaration = (IClassDeclaration)element;

			if (classDeclaration.IsStatic)
				return;

			bool[] classMarked = new bool[Patterns.Length];
			bool[] classMustFollowPattern = new bool[Patterns.Length];

			for (int i = 0; i < Patterns.Length; i++)
				classMustFollowPattern[i] = Patterns[i].ShouldFollowPattern(classDeclaration);

			foreach (var attribute in classDeclaration.Attributes)
				for (int i = 0; i < Patterns.Length; i++)
				{
					string errorMessage;
					string warningMessage;
					if (!classMarked[i] && Patterns[i].IsClassAttribute(attribute, out errorMessage, out warningMessage))
					{
						classMarked[i] = true;
						if (errorMessage != null)
							consumer.AddHighlighting(
								new AttributeErrorHighlighting(attribute, errorMessage, Patterns[i].GetClassFixes(classDeclaration)),
								attribute.GetDocumentRange(),
								attribute.GetContainingFile());
						if (warningMessage != null)
							consumer.AddHighlighting(
								new AttributeWarningHighlighting(attribute, warningMessage, Patterns[i].GetClassFixes(classDeclaration)),
								attribute.GetDocumentRange(),
								attribute.GetContainingFile());
					}
				}

			var markedProperties = new List<IPropertyDeclaration>[Patterns.Length];
			var notMarkedProperties = new List<IPropertyDeclaration>[Patterns.Length];

			for (int i = 0; i < Patterns.Length; i++)
				markedProperties[i] = new List<IPropertyDeclaration>();

			for (int i = 0; i < Patterns.Length; i++)
				notMarkedProperties[i] = new List<IPropertyDeclaration>();

			bool[] propertyMarked = new bool[Patterns.Length];

			foreach (var member in classDeclaration.ClassMemberDeclarations)
			{
				if (!(member is IPropertyDeclaration))
					continue;

				var propertyDeclaration = (IPropertyDeclaration)member;

				if (propertyDeclaration.IsStatic)
					return;

				for (int i = 0; i < Patterns.Length; i++)
					propertyMarked[i] = false;

				foreach (var attribute in member.AttributesEnumerable)
					for (int i = 0; i < Patterns.Length; i++)
					{
						string errorMessage;
						string warningMessage;
						if (!propertyMarked[i] && Patterns[i].IsFieldAttribute(attribute, out errorMessage, out warningMessage))
						{
							propertyMarked[i] = true;
							if (errorMessage != null)
								consumer.AddHighlighting(
									new AttributeErrorHighlighting(attribute, errorMessage, Patterns[i].GetPropertyFixes(propertyDeclaration)),
									attribute.GetDocumentRange(),
									attribute.GetContainingFile());

							if (warningMessage != null)
								consumer.AddHighlighting(
									new AttributeErrorHighlighting(attribute, warningMessage, Patterns[i].GetPropertyFixes(propertyDeclaration)),
									attribute.GetDocumentRange(),
									attribute.GetContainingFile());
						}
					}

				for (int i = 0; i < Patterns.Length; i++)
				{
					if (propertyMarked[i])
						markedProperties[i].Add(propertyDeclaration);
					else
						notMarkedProperties[i].Add(propertyDeclaration);
				}
			}

			for (int i = 0; i < Patterns.Length; i++)
			{
				if (classMarked[i] || markedProperties[i].Count > 0 && Patterns[i].MandatoryAttributeOnField)
				{
					foreach (var notMarkerdPoperty in notMarkedProperties[i])
					{
						var highlightedElement = notMarkerdPoperty.NameIdentifier;

						consumer.AddHighlighting(
							new AttributeErrorHighlighting(highlightedElement, Patterns[i].MissingFieldAttributeErrorMessage, Patterns[i].GetPropertyFixes(notMarkerdPoperty)),
							highlightedElement.GetDocumentRange(),
							highlightedElement.GetContainingFile());
					}
				}

				if (!classMarked[i] && (markedProperties[i].Count > 0 || classMustFollowPattern[i]) && Patterns[i].MandatoryAttributeOnClass)
				{
					var highlightedElement = classDeclaration.NameIdentifier;

					consumer.AddHighlighting(
						new AttributeWarningHighlighting(highlightedElement, Patterns[i].MissingClassAttributeErrorMessage, Patterns[i].GetClassFixes(classDeclaration)),
						highlightedElement.GetDocumentRange(),
						highlightedElement.GetContainingFile());
				}
			}
		}
	}
}
