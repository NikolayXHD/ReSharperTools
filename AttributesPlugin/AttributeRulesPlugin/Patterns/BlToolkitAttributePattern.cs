using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AttributeRulesPlugin
{
	class BlToolkitAttributePattern : IAttributeUsagePattern
	{
		private const string TableNameAttribute = "BLToolkit.DataAccess.TableNameAttribute";
		private const string MapAttribute = "BLToolkit.Mapping.MapFieldAttribute";
		private const string IgnoreAttribute = "BLToolkit.Mapping.MapIgnoreAttribute";

		public bool IsClassAttribute(IAttribute a, out string errorMessage)
		{
			errorMessage = null;

			var type = a.GetAttributeType();

			if (type == null)
				return false;

			if (type.GetClrName().FullName != TableNameAttribute)
				return false;

			var parameters = a.GetAttributeParams();

			if (parameters == null || parameters.Count == 0)
				errorMessage = "[TableName(\"<missing parameter here>\")]";

			return true;
		}

		public bool IsFieldAttribute(IAttribute a, out string errorMessage)
		{
			errorMessage = null;

			var type = a.GetAttributeType();

			if (type == null)
				return false;

			var name = type.GetClrName().FullName;

			if (name == IgnoreAttribute)
				return true;

			if (name != MapAttribute)
				return false;

			var parameters = a.GetAttributeParams();
			if (parameters == null || parameters.Count == 0)
				errorMessage = "[MapField(\"<missing parameter here>\")]";

			return true;
		}

		public bool MandatoryAttributeOnField { get { return true; } }
		public bool MandatoryAttributeOnClass { get { return true; } }

		public string MissingFieldAttributeErrorMessage
		{
			get { return "Missing BlToolkit attribute such as [MapField(\"...\")], [MapIgnore]"; }
		}

		public string MissingClassAttributeErrorMessage
		{
			get { return "Missing [TableName(\"...\")] attribute"; }
		}
	}
}