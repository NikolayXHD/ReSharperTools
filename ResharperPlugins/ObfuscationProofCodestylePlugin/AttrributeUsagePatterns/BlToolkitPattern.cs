using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	class BlToolkitPattern : IAttributeUsagePattern
	{
		private const string TableNameAttribute = "BLToolkit.DataAccess.TableNameAttribute";
		private const string MapAttribute = "BLToolkit.Mapping.MapFieldAttribute";
		private const string IgnoreAttribute = "BLToolkit.Mapping.MapIgnoreAttribute";

		public bool IsClassAttribute(IAttribute a, out string errorMessage, out string warningMessage)
		{
			errorMessage = null;
			warningMessage = null;

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

		public bool IsFieldAttribute(IAttribute a, out string errorMessage, out string warningMessage)
		{
			errorMessage = null;
			warningMessage = null;

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
			get { return "Missing attribute such as [MapField(\"...\")], [MapIgnore]"; }
		}

		public string MissingClassAttributeErrorMessage
		{
			get { return "Missing attribute [TableName(\"...\")] "; }
		}

		public IBulbAction[] GetPropertyFixes(IPropertyDeclaration declaration)
		{
			var map = new AddMapFiedAttribute(declaration);
			var ignore = new AddMapIgnoreAttribute(declaration);

			if (declaration.AccessorDeclarations.Count < 2)
			{
				return new IBulbAction[]
				{
					ignore,
					map
				};
			}

			return new IBulbAction[]
			{
				map,
				ignore
			};
		}

		public IBulbAction[] GetClassFixes(IClassDeclaration declaration)
		{
			return new IBulbAction[] { new AddTableNameAttribute(declaration) };
		}

		public bool ShouldFollowPattern(IClassDeclaration declaration)
		{
			return false;
		}
	}
}