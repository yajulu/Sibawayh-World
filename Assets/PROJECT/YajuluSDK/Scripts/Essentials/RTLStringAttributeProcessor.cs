#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using RTLTMPro;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Project.YajuluSDK.Scripts.Essentials
{
	public class RTLStringAttributeProcessor : OdinAttributeProcessor<string>
	{
		private string fixedText;
		private static FastStringBuilder finalText = new FastStringBuilder(200); 
		public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
		{
			// attributes.Add(new OnValueChangedAttribute("@RTLStringAttributeProcessor.GetFixedText($value)"));
			attributes.Add(new SuffixLabelAttribute("@RTLStringAttributeProcessor.GetFixedText($value)", true));
		}
		
		public static string GetFixedText(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;
			finalText.Clear();
			RTLSupport.FixRTL(input, finalText, false, false, false);
			// finalText.Reverse();
			// fixedText = finalText;
			return finalText.ToString();
		}
		
	}
}
#endif
