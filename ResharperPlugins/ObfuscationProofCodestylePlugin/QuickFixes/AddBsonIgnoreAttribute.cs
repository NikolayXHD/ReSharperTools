using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	public class AddBsonIgnoreAttribute : AddAttributeBulbAction<IPropertyDeclaration>
	{
		public AddBsonIgnoreAttribute(IPropertyDeclaration propertyDeclaration)
			:base(propertyDeclaration)
		{
		}

		protected override string AttributeName
		{
			get { return "MongoDB.Bson.Serialization.Attributes.BsonIgnoreAttribute"; }
		}

		protected override string DescriptionPattern
		{
			get { return "Add attribute [BsonIgnore]"; }
		}

		protected override string FixedParamValue
		{
			get { return null; }
		}
	}
}