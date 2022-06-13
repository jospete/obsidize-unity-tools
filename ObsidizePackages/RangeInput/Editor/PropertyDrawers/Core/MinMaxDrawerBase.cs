using UnityEditor;
using UnityEngine;

namespace Obsidize.RangeInput.Editor
{
	public class MinMaxDrawerBase<FieldType, ValueType> : PropertyDrawer
	{

		private const int minInputRect = 0;
		private const int maxInputRect = 1;
		private const int controlRectCount = 2;
		private const float controlInputPadding = 5f;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{

			Rect controlRect = EditorGUI.PrefixLabel(position, label);
			Rect[] multiControlRects = SplitControlRect(controlRect);
			var previousWidth = EditorGUIUtility.labelWidth;
			var minProperty = property.GetMinMaxRangeMinProperty();
			var maxProperty = property.GetMinMaxRangeMaxProperty();

			EditorGUIUtility.labelWidth = 30f;
			EditorGUI.PropertyField(multiControlRects[minInputRect], minProperty);
			EditorGUI.PropertyField(multiControlRects[maxInputRect], maxProperty);
			EditorGUIUtility.labelWidth = previousWidth;

			// TODO: maybe sanitize the min and max values so min is always less than max
			property.serializedObject.ApplyModifiedProperties();
		}

		private Rect[] SplitControlRect(Rect controlRect)
		{

			var result = controlRect.SplitHorizontally(controlRectCount);

			result[minInputRect].xMax -= controlInputPadding;
			result[maxInputRect].xMax -= controlInputPadding;

			return result;
		}
	}
}