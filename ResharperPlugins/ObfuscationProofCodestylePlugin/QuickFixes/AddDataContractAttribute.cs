using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	public class AddDataContractAttribute : AddAttributeBulbAction<IClassDeclaration>
	{
		public AddDataContractAttribute(IClassDeclaration propertyDeclaration)
			:base(propertyDeclaration)
		{
		}

		protected override string AttributeName
		{
			get { return "System.Runtime.Serialization.DataContractAttribute"; }
		}

		protected override string DescriptionPattern
		{
			get { return "Add attribute [DataContract(Name=\"{0}\")]"; }
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
			get { return PropertyName; }
		}
	}
}