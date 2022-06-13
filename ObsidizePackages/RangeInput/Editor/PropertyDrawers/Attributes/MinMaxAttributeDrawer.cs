using UnityEditor;
using UnityEngine;

namespace Obsidize.RangeInput.Editor
{

	[CustomPropertyDrawer(typeof(MinMaxAttribute))]
	public class MinMaxAttributeDrawer : MinMaxAttributeDrawerBase<MinMaxAttribute, MinMaxRange, float>
	{

		private static float TruncateDecimal(float v)
		{
			return Mathf.Round(v * 100f) / 100f;
		}

		protected override string GetTooltipText(float min, float max)
		{
			return $"{TruncateDecimal(min)} to {TruncateDecimal(max)}";
		}

		protected override float ShowValueEditorField(Rect rect, float inputValue)
		{
			return EditorGUI.FloatField(rect, TruncateDecimal(inputValue));
		}

		protected override float GetSerializedValue(SerializedProperty property)
		{
			return property.floatValue;
		}

		protected override void SetSerializedValue(SerializedProperty property, float value)
		{
			property.floatValue = value;
		}

		protected override float GetValueAsFloat(float value)
		{
			return value;
		}

		protected override float GetFloatAsValue(float value)
		{
			return value;
		}
	}
}
