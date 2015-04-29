using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	class DataContractPattern : IAttributeUsagePattern
	{
		private const string DataContractAttribute = "System.Runtime.Serialization.DataContractAttribute";
		private const string DataMemberAttribute = "System.Runtime.Serialization.DataMemberAttribute";
		private const string IngnoreDataMemberAttribute = "System.Runtime.Serialization.IgnoreDataMemberAttribute";

		public bool IsClassAttribute(IAttribute a, out string errorMessage)
		{
			errorMessage = null;

			var type = a.GetAttributeType();

			if (type == null)
				return false;

			var name = type.GetClrName().FullName;
			if (name != DataContractAttribute)
				return false;

			var namePropertyAssignment = a.PropertyAssignments.FirstOrDefault(ass => ass.PropertyNameIdentifier.Name == "Name");
			if (!(namePropertyAssignment != null))
				errorMessage = "[DataContract(Name=\"<missing parameter here>\")]";

			return true;
		}

		public bool IsFieldAttribute(IAttribute a, out string errorMessage)
		{
			errorMessage = null;

			var field = a.GetContainingTypeMemberDeclaration();
			
			if (field == null || field.NameIdentifier == null)
				return false;
			
			var type = a.GetAttributeType();

			if (type == null)
				return false;

			var attributeTypeName = type.GetClrName().FullName;

			if (attributeTypeName == IngnoreDataMemberAttribute)
				return true;

			if (attributeTypeName != DataMemberAttribute)
				return false;

			var namePropertyAssignment = a.PropertyAssignments.FirstOrDefault(ass => ass.PropertyNameIdentifier.Name == "Name");

			if (namePropertyAssignment == null || namePropertyAssignment.Source == null || !namePropertyAssignment.Source.ConstantValue.IsString())
				errorMessage = "[DataContract(Name=\"<missing parameter here>\")]";
			else if (a.GetContainingTypeDeclaration().NameIdentifier.Name.EndsWith("Model"))
			{
				var memberName = field.NameIdentifier.Name;
				var propertyValue = (string) namePropertyAssignment.Source.ConstantValue.Value;

				if (memberName == propertyValue)
				{
					// warn if not attribute value is not in lower camel case
					var memberNameFirstChar = memberName.Substring(0, 1);
					if (memberNameFirstChar.ToUpperInvariant() == memberNameFirstChar)
						errorMessage = "[DataContract(Name=\"<expectingLowerCamelCase>\")]";
				}

				// value was intentionally changed from default
			}

			return true;
		}

		public bool MandatoryAttributeOnField { get { return true; } }
		public bool MandatoryAttributeOnClass { get { return true; } }

		public string MissingFieldAttributeErrorMessage
		{
			get { return "Missing field serialization attribute such as [DataMember(Name=\"...\")], [IngnoreDataMember]"; }
		}

		public string MissingClassAttributeErrorMessage
		{
			get { return "Missing serialization attribute [DataContract(Name=\"...\")]"; }
		}

		public IBulbAction[] GetPropertyFixes(IPropertyDeclaration declaration)
		{
			bool lowerCamelCase = declaration.GetContainingTypeDeclaration().NameIdentifier.Name.EndsWith("Model");

			return new IBulbAction[]
			{
				new AddDataMemberAttribute(declaration, lowerCamelCase),
				new AddIgnoreDataMemberAttribute(declaration)
			};
		}

		public IBulbAction[] GetClassFixes(IClassDeclaration declaration)
		{
			return new IBulbAction[] { new AddDataContractAttribute(declaration) };
		}
	}
}