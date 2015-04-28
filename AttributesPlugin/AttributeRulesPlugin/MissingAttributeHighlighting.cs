/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using AttributeRulesPlugin;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;

[assembly: RegisterConfigurableSeverity(MissingAttributeHighlighting.SeverityId,
  null,
  HighlightingGroupIds.ConstraintViolation,
  "Missing required attribute",
  "",
  Severity.ERROR,
  false)]

namespace AttributeRulesPlugin
{
	[ConfigurableSeverityHighlighting(SeverityId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING)]
	public class MissingAttributeHighlighting : IHighlighting
	{
		public const string SeverityId = "MissingRequiredAttribute";

		private readonly ITreeNode _expression;
		private readonly string _message;

		public MissingAttributeHighlighting(ITreeNode expression, string message)
		{
			_expression = expression;
			_message = message;
		}

		public string ToolTip
		{
			get { return _message; }
		}

		public string ErrorStripeToolTip
		{
			get { return ToolTip; }
		}

		public int NavigationOffsetPatch
		{
			get { return 0; }
		}

		public bool IsValid()
		{
			return _expression == null || _expression.IsValid();
		}
	}
}