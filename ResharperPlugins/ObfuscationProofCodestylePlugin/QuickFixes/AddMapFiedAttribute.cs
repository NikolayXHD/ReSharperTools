using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	public class AddMapFiedAttribute : AddAttributeBulbAction<IPropertyDeclaration>
	{
		public AddMapFiedAttribute(IPropertyDeclaration propertyDeclaration)
			:base(propertyDeclaration)
		{
		}

		protected override string AttributeName
		{
			get { return "BLToolkit.Mapping.MapFieldAttribute"; }
		}

		protected override string DescriptionPattern
		{
			get { return "Add attribute [MapField(\"{0}\")]"; }
		}
	}
}