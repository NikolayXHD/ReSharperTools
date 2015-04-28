using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	public class AddBsonElementAttribute : AddAttributeBulbAction<IPropertyDeclaration>
	{
		public AddBsonElementAttribute(IPropertyDeclaration propertyDeclaration)
			:base(propertyDeclaration)
		{
		}

		protected override string AttributeName
		{
			get { return "MongoDB.Bson.Serialization.Attributes.BsonElementAttribute"; }
		}

		protected override string DescriptionPattern
		{
			get { return "Add attribute [BsonElement(\"{0}\")]"; }
		}
	}
}