using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AbbyyLS.ReSharper
{
	public class AddTableNameAttribute : AddAttributeBulbAction<IClassDeclaration>
	{
		public AddTableNameAttribute(IClassDeclaration propertyDeclaration)
			: base(propertyDeclaration)
		{
		}

		protected override string AttributeName
		{
			get { return "BLToolkit.DataAccess.TableNameAttribute"; }
		}

		protected override string DescriptionPattern
		{
			get { return "Add attribute [TableName(\"{0}\")]"; }
		}
	}
}