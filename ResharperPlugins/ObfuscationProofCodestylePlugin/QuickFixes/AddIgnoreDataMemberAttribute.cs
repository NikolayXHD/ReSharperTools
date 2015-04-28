using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	public class AddIgnoreDataMemberAttribute : AddAttributeBulbAction<IPropertyDeclaration>
	{
		public AddIgnoreDataMemberAttribute(IPropertyDeclaration propertyDeclaration)
			: base(propertyDeclaration)
		{
		}

		protected override string AttributeName
		{
			get { return "System.Runtime.Serialization.IgnoreDataMemberAttribute"; }
		}

		protected override string DescriptionPattern
		{
			get { return "Add attribute [IgnoreDataMember]"; }
		}

		protected override string FixedParamValue
		{
			get { return null; }
		}
	}
}