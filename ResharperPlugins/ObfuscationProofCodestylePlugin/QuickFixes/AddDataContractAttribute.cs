using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	public class AddDataContractAttribute : AddAttributeBulbAction<IClassDeclaration>
	{
		public AddDataContractAttribute(IClassDeclaration propertyDeclaration)
			: base(propertyDeclaration)
		{
		}

		protected override string AttributeName
		{
			get { return "System.Runtime.Serialization.DataContractAttribute"; }
		}

		protected override string DescriptionPattern
		{
			get
			{
				var declaredName = PropertyDeclaration.DeclaredName;

				if (!declaredName.EndsWith("Config") && !declaredName.EndsWith("ConfigSection"))
					return "Add attribute [DataContract]";

				return "Add attribute [DataContract(Name=\"{0}\")]";
			}
		}

		protected override string FixedParamValue
		{
			get { return null; }
		}

		protected override string NamedParamName
		{
			get
			{
				var declaredName = PropertyDeclaration.DeclaredName;

				if (!declaredName.EndsWith("Config") && !declaredName.EndsWith("ConfigSection"))
					return null;

				return "Name";
			}
		}

		protected override string NamedParamValue
		{
			get
			{
				var declaredName = PropertyDeclaration.DeclaredName;

				if (!declaredName.EndsWith("Config") && !declaredName.EndsWith("ConfigSection"))
					return null;

				var propertyName = PropertyDeclaration.DeclaredName;

				if (propertyName.EndsWith("Config"))
					return propertyName.Substring(0, propertyName.Length - "Config".Length);

				return propertyName;
			}
		}

		protected override string PropertyAssignmentNameToErase
		{
			get
			{
				var declaredName = PropertyDeclaration.DeclaredName;
				if (!declaredName.EndsWith("Config") && !declaredName.EndsWith("ConfigSection"))
					return "Name";

				return null;
			}
		}
	}
}