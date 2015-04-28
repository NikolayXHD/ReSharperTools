using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	public class AddMapIgnoreAttribute : AddAttributeBulbAction<IPropertyDeclaration>
	{
		public AddMapIgnoreAttribute(IPropertyDeclaration propertyDeclaration)
			:base(propertyDeclaration)
		{
		}

		protected override string AttributeName
		{
			get { return "BLToolkit.Mapping.MapIgnoreAttribute"; }
		}

		protected override string DescriptionPattern
		{
			get { return "Add attribute [MapIgnore]"; }
		}

		protected override string FixedParamValue
		{
			get { return null; }
		}
	}
}