using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	public class AddDataMemberAttribute : AddAttributeBulbAction<IPropertyDeclaration>
	{
		private readonly bool _lowerCamelCase;

		public AddDataMemberAttribute(IPropertyDeclaration propertyDeclaration, bool lowerCamelCase)
			:base(propertyDeclaration)
		{
			_lowerCamelCase = lowerCamelCase;
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
				var name = PropertyName;
				if (!_lowerCamelCase)
					return name;

				return toLowerCamelCase(PropertyName);
			}
		}

		private static string toLowerCamelCase(string identifier)
		{
			if (identifier.Length == 1)
				return identifier.Substring(0, 1).ToLowerInvariant();

			return identifier.Substring(0, 1).ToLowerInvariant() + identifier.Substring(1);
		}
	}
}