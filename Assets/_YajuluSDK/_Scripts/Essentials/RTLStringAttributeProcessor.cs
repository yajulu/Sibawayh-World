using System;
using System.Collections.Generic;
using RTLTMPro;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace _YajuluSDK._Scripts.Essentials
{
	public class RTLStringAttributeProcessor : OdinAttributeProcessor<string>
	{
		private string fixedText;
		public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
		{
			// attributes.Add(new OnValueChangedAttribute("@RTLStringAttributeProcessor.GetFixedText($value)"));
			attributes.Add(new SuffixLabelAttribute("@RTLStringAttributeProcessor.GetFixedText($value)", true));
		}

		public static string GetFixedText(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;
			FastStringBuilder finalText = new FastStringBuilder(input.Length); 
			finalText.Clear();
			RTLSupport.FixRTL(input, finalText, false, false, false);
			// finalText.Reverse();
			// fixedText = finalText;
			return finalText.ToString();
		}
		
	}
}

