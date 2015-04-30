using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.Util;

namespace AbbyyLS.ReSharper
{
	[QuickFix]
	public sealed class AddAttributeQuickFix : IQuickFix
	{
		private readonly IBulbAction[] _items;

		/// <summary>
		/// For languages other than C# any inheritor of <see cref="IContextActionDataProvider"/> can 
		/// be injected in this constructor.
		/// </summary>
		public AddAttributeQuickFix(AttributeErrorHighlighting provider)
		{
			_items = provider.Fixes;
		}

		public AddAttributeQuickFix(AttributeWarningHighlighting provider)
		{
			_items = provider.Fixes;
		}

		public IEnumerable<IntentionAction> CreateBulbItems()
		{
			return _items != null ? _items.ToQuickFixAction() : Enumerable.Empty<IntentionAction>();
		}

		public bool IsAvailable(IUserDataHolder cache)
		{
			return _items != null && _items.Length > 0;
		}
	}
}
