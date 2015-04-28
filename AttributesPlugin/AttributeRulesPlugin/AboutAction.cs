using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace AttributeRulesPlugin
{
	[ActionHandler("AttributeRulesPlugin.About")]
	public class AboutAction : IActionHandler
	{
		public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
		{
			// return true or false to enable/disable this action
			return true;
		}

		public void Execute(IDataContext context, DelegateExecute nextExecute)
		{
			MessageBox.Show(
			  "Attribute inspections\nAbbyy Language Services\n\nChecks code against attribute usage rules to prevent obfuscation from breaking functionality.",
			  "About Attribute inspections",
			  MessageBoxButtons.OK,
			  MessageBoxIcon.Information);
		}
	}
}