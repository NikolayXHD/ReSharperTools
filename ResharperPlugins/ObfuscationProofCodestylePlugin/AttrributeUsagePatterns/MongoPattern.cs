using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	class MongoPattern : IAttributeUsagePattern
	{
		private const string MongoSerializationAttributesNamespace = "MongoDB.Bson.Serialization.Attributes";
		private static readonly HashSet<string> AttributesNotRequiringName = new HashSet<string>
		{
			"MongoDB.Bson.Serialization.Attributes.BsonIdAttribute",
			"MongoDB.Bson.Serialization.Attributes.BsonIgnoreAttribute"
		};

		private const string AttributeReqiuiringName = "MongoDB.Bson.Serialization.Attributes.BsonElementAttribute";

		public bool IsClassAttribute(IAttribute a, out string errorMessage)
		{
			errorMessage = null;

			var type = a.GetAttributeType();

			if (type == null)
				return false;

			var ns = type.GetContainingNamespace();
			return ns.QualifiedName == MongoSerializationAttributesNamespace;
		}

		public bool IsFieldAttribute(IAttribute a, out string errorMessage)
		{
			errorMessage = null;

			var type = a.GetAttributeType();

			if (type == null)
				return false;

			var name = type.GetClrName().FullName;

			if (AttributesNotRequiringName.Contains(name))
				return true;

			if (AttributeReqiuiringName != name)
				return false;

			var attributeParams = a.GetAttributeParams();

			if (attributeParams == null || attributeParams.Count == 0)
				errorMessage = "[BsonElement(\"<missing parameter here>\")]";

			return true;
		}

		public bool MandatoryAttributeOnField { get { return true; } }
		public bool MandatoryAttributeOnClass { get { return false; } }

		public string MissingFieldAttributeErrorMessage
		{
			get { return "Missing mongo field attribute such as [BsonId], [BsonElement(\"...\")], [BsonIgnore]"; }
		}

		public string MissingClassAttributeErrorMessage
		{
			get { throw new InvalidOperationException("mongochsarpdriver doesn't require class to be mapped by an attribute"); }
		}

		public IBulbAction[] GetPropertyFixes(IPropertyDeclaration declaration)
		{
			return new IBulbAction[]
			{
				new AddBsonElementAttribute(declaration),
				new AddBsonIgnoreAttribute(declaration),
				new AddBsonIdAttribute(declaration)
			};
		}

		public IBulbAction[] GetClassFixes(IClassDeclaration declaration)
		{
			return null;
		}
	}
}