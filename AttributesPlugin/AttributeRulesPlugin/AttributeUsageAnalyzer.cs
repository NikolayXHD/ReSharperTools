using System.Collections.Generic;
using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace AttributeRulesPlugin
{
	[ElementProblemAnalyzer(new[] { typeof(IClassDeclaration) }, HighlightingTypes = new[] { typeof(MissingAttributeHighlighting) })]
	public class AttributeUsageAnalyzer : IElementProblemAnalyzer
	{
		private static readonly IAttributeUsagePattern[] Patterns =
		{
			new BlToolkitAttributePattern(),
			new MongoAttributePattern(),
			new DataContractAttributePattern()
		};

		public void Run(ITreeNode element, ElementProblemAnalyzerData analyzerData, IHighlightingConsumer consumer)
		{
			if (!(element is IClassDeclaration))
				return;

			var classDeclaration = (IClassDeclaration)element;

			bool[] classMarked = new bool[Patterns.Length];

			foreach (var attribute in classDeclaration.Attributes)
				for (int i = 0; i < Patterns.Length; i++)
				{
					string errorMessage;
					if (!classMarked[i] && Patterns[i].IsClassAttribute(attribute, out errorMessage))
					{
						classMarked[i] = true;
						if (errorMessage != null)
							consumer.AddHighlighting(
								new MissingAttributeHighlighting(attribute, errorMessage),
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

				for (int i = 0; i < Patterns.Length; i++)
					propertyMarked[i] = false;

				foreach (var attribute in member.AttributesEnumerable)
					for (int i = 0; i < Patterns.Length; i++)
					{
						string errorMessage;
						if (!propertyMarked[i] && Patterns[i].IsFieldAttribute(attribute, out errorMessage))
						{
							propertyMarked[i] = true;
							if (errorMessage != null)
								consumer.AddHighlighting(
									new MissingAttributeHighlighting(attribute, errorMessage),
									attribute.GetDocumentRange(),
									attribute.GetContainingFile());
						}
					}

				for (int i = 0; i < Patterns.Length; i++)
					if (propertyMarked[i])
						markedProperties[i].Add((IPropertyDeclaration)member);
					else
						notMarkedProperties[i].Add((IPropertyDeclaration)member);
			}

			for (int i = 0; i < Patterns.Length; i++)
			{
				if (classMarked[i] || markedProperties[i].Count > 0 && Patterns[i].MandatoryAttributeOnField)
				{
					foreach (var notMarkerdPoperty in notMarkedProperties[i])
					{
						var highlightedElement = notMarkerdPoperty.NameIdentifier;

						consumer.AddHighlighting(
							new MissingAttributeHighlighting(highlightedElement, Patterns[i].MissingFieldAttributeErrorMessage),
							highlightedElement.GetDocumentRange(),
							highlightedElement.GetContainingFile());
					}
				}

				if (!classMarked[i] && markedProperties[i].Count > 0 && Patterns[i].MandatoryAttributeOnClass)
				{
					var highlightedElement = classDeclaration.NameIdentifier;

					consumer.AddHighlighting(
						new MissingAttributeHighlighting(highlightedElement, Patterns[i].MissingClassAttributeErrorMessage),
						highlightedElement.GetDocumentRange(),
						highlightedElement.GetContainingFile());
				}
			}
		}
	}
}
