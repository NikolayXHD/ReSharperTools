using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	public class AddDataMemberAttribute : AddAttributeBulbAction<IPropertyDeclaration>
	{
		public AddDataMemberAttribute(IPropertyDeclaration propertyDeclaration)
			:base(propertyDeclaration)
		{
		}

		protected override string AttributeName
		{
			get { return "System.Runtime.Serialization.DataMemberAttribute"; }
		}

		protected override string DescriptionPattern
		{
			get { return "Add attribute [DataMember(Name=\"{0}\")]"; }
		}

		protected override string FixedParamValue
		{
			get { return null; }
		}

		protected override string NamedParamName
		{
			get { return "Name"; }
		}

		protected override string NamedParamValue
		{
			get
			{
				var typeName = PropertyDeclaration.GetContainingTypeDeclaration().DeclaredName;
				var propertyName = PropertyDeclaration.DeclaredName;

				if (typeName.EndsWith("Model"))
					return IdentifierUtil.ToLowerCamelCase(propertyName);
				
				return propertyName;
			}
		}
	}
}