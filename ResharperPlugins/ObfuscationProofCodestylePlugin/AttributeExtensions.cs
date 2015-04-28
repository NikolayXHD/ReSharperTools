using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Impl.reflection2.elements.Context;
using JetBrains.ReSharper.Psi.JavaScript.Resolve;

namespace AbbyyLS.ReSharper
{
	static class AttributeExtensions
	{
		public static ITypeElement GetAttributeType(this IAttribute a)
		{
			var reference = a.Reference;
			if (reference == null)
				return null;

			var resolveResult = reference.CurrentResolveResult;
			if (resolveResult == null)
				return null;

			if (!resolveResult.ResolveErrorType.IsResolvedOk())
				return null;

			var constructorElement = resolveResult.Result.DeclaredElement as ConstructorElement;

			if (constructorElement == null)
				return null;

			var type = constructorElement.GetContainingType();
			return type;
		}

		public static IList<IParameter> GetAttributeParams(this IAttribute a)
		{
			var reference = a.Reference;
			if (reference == null)
				return null;

			var resolveResult = reference.CurrentResolveResult;
			if (resolveResult == null)
				return null;

			if (!resolveResult.ResolveErrorType.IsResolvedOk())
				return null;

			var constructorElement = resolveResult.Result.DeclaredElement as ConstructorElement;

			if (constructorElement == null)
				return null;

			return constructorElement.Parameters;
		}
	}
}