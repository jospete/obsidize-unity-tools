using UnityEditor;
using UnityEngine;

namespace Obsidize.RangeInput.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxIntAttribute))]
	public class MinMaxIntAttributeDrawer : MinMaxAttributeDrawerBase<MinMaxIntAttribute, MinMaxRangeInt, int>
	{

		protected override string GetTooltipText(int min, int max)
		{
			return $"{min} to {max}";
		}

		protected override int ShowValueEditorField(Rect rect, int inputValue)
		{
			return EditorGUI.IntField(rect, inputValue);
		}

		protected override int GetSerializedValue(SerializedProperty property)
		{
			return property.intValue;
		}

		protected override void SetSerializedValue(SerializedProperty property, int value)
		{
			property.intValue = value;
		}

		protected override float GetValueAsFloat(int value)
		{
			return value;
		}

		protected override int GetFloatAsValue(float value)
		{
			return Mathf.RoundToInt(value);
		}
	}
}
