using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AttributeRulesPlugin
{
	class DataContractAttributePattern : IAttributeUsagePattern
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

			if (!hasNameProperty(a))
				errorMessage = "[DataContract(Name=\"<missing parameter here>\")]";

			return true;
		}

		private static bool hasNameProperty(IAttribute a)
		{
			return a.PropertyAssignments.Any(ass => ass.PropertyNameIdentifier.Name == "Name");
		}

		public bool IsFieldAttribute(IAttribute a, out string errorMessage)
		{
			errorMessage = null;

			var type = a.GetAttributeType();

			if (type == null)
				return false;

			var name = type.GetClrName().FullName;

			if (IngnoreDataMemberAttribute == name)
				return true;

			if (DataMemberAttribute != name)
				return false;

			if (!hasNameProperty(a))
				errorMessage = "[DataContract(Name=\"<missing parameter here>\")]";

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
	}
}