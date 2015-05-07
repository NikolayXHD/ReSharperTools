using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	class DataContractPattern : IAttributeUsagePattern
	{
		private const string DataContractAttribute = "System.Runtime.Serialization.DataContractAttribute";
		private const string DataMemberAttribute = "System.Runtime.Serialization.DataMemberAttribute";
		private const string IngnoreDataMemberAttribute = "System.Runtime.Serialization.IgnoreDataMemberAttribute";

		public bool IsClassAttribute(IAttribute a, out string errorMessage, out string warningMessage)
		{
			errorMessage = null;
			warningMessage = null;

			var type = a.GetAttributeType();

			if (type == null)
				return false;

			var name = type.GetClrName().FullName;
			if (name != DataContractAttribute)
				return false;

			
			var namePropertyAssignment = a.PropertyAssignments.FirstOrDefault(ass => ass.PropertyNameIdentifier.Name == "Name");
			var className = a.GetContainingTypeDeclaration().DeclaredName;

			if (className.EndsWith("Config") || className.EndsWith("ConfigSection"))
			{
				const string missingParameterHere = "[DataContract(Name=\"<missing parameter here>\")]";

				if (namePropertyAssignment == null)
					errorMessage = missingParameterHere;
				else
				{
					var value = namePropertyAssignment.Source.ConstantValue.Value as string;
					if (string.IsNullOrEmpty(value))
						errorMessage = missingParameterHere;
					else
					{
						var firstChar = value.Substring(0, 1);
						if (firstChar.ToLowerInvariant() == firstChar)
							warningMessage = "[DataContract(Name=\"<ExpectingUpperCaseHere>\")]";
					}
				}
			}
			else
			{
				if (namePropertyAssignment != null)
					warningMessage = "Name is only required for NConfiguration config classes";
			}

			return true;
		}

		public bool IsFieldAttribute(IAttribute a, out string errorMessage, out string warningMessage)
		{
			errorMessage = null;
			warningMessage = null;

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
				errorMessage = "[DataMember(Name=\"<missing parameter here>\")]";

			else if (a.GetContainingTypeDeclaration().DeclaredName.EndsWith("Model"))
			{
				var memberName = field.DeclaredName;
				var propertyValue = (string)namePropertyAssignment.Source.ConstantValue.Value;

				if (memberName == propertyValue)
				{
					// warn if not attribute value is not in lower camel case
					var memberNameFirstChar = memberName.Substring(0, 1);
					if (memberNameFirstChar.ToUpperInvariant() == memberNameFirstChar)
						warningMessage = "[DataMember(Name=\"<expectingLowerCamelCase>\")]";
				}

				// value was intentionally changed from default
			}

			return true;
		}

		public bool MandatoryAttributeOnField { get { return true; } }
		public bool MandatoryAttributeOnClass { get { return true; } }

		public string MissingFieldAttributeErrorMessage
		{
			get { return "Missing attribute such as [DataMember(Name=\"...\")], [IngnoreDataMember]"; }
		}

		public string MissingClassAttributeErrorMessage
		{
			get { return "Missing attribute [DataContract(Name=\"...\")]"; }
		}

		public IBulbAction[] GetPropertyFixes(IPropertyDeclaration declaration)
		{
			var map = new AddDataMemberAttribute(declaration);
			var ignore = new AddIgnoreDataMemberAttribute(declaration);

			return new IBulbAction[]
			{
				map,
				ignore
			};
		}

		public IBulbAction[] GetClassFixes(IClassDeclaration declaration)
		{
			return new IBulbAction[] { new AddDataContractAttribute(declaration) };
		}

		public bool ShouldFollowPattern(IClassDeclaration declaration)
		{
			var name = declaration.DeclaredName;

			return name.EndsWith("Model") || name.EndsWith("Config") || name.EndsWith("ConfigSection");
		}
	}
}