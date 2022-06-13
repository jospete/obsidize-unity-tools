using UnityEditor;
using UnityEngine;

namespace Obsidize.RangeInput.Editor
{
	public abstract class MinMaxAttributeDrawerBase<AttributeType, FieldType, ValueType> : PropertyDrawer
		where AttributeType : PropertyAttribute, IMinMaxAttribute<ValueType>
	{

		public static bool autoExpandDefaultValues = true;

		private const int minInputRect = 0;
		private const int sliderInputRect = 1;
		private const int maxInputRect = 2;
		private const int controlRectCount = 3;
		private const int controlRectPadding = 5;

		private bool _hasCustomDrawer;
		private bool _didWarnInvalidPropertyType;

		protected abstract float GetValueAsFloat(ValueType value);
		protected abstract ValueType GetFloatAsValue(float value);
		protected abstract ValueType GetSerializedValue(SerializedProperty property);
		protected abstract void SetSerializedValue(SerializedProperty property, ValueType value);
		protected abstract ValueType ShowValueEditorField(Rect rect, ValueType inputValue);
		protected abstract string GetTooltipText(ValueType min, ValueType max);

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{

			var isMinMaxStruct = fieldInfo.FieldType == typeof(FieldType);
			var minMaxAttribute = attribute as AttributeType;
			_hasCustomDrawer = isMinMaxStruct && minMaxAttribute != null;

			if (!_hasCustomDrawer)
			{
				WarnInvalidPropertyTypeIfNeeded(property.name);
				EditorGUI.PropertyField(position, property, label, true);
				return;
			}

			var minMaxBounds = minMaxAttribute.Bounds;
			var minProperty = property.GetMinMaxRangeMinProperty();
			var maxProperty = property.GetMinMaxRangeMaxProperty();

			var minVal = GetSerializedValue(minProperty);
			var maxVal = GetSerializedValue(maxProperty);
			ValueType defaultVal = default;

			// When both values are defaulted, expand them to the entire range automatically
			if (autoExpandDefaultValues && minVal.Equals(defaultVal) && maxVal.Equals(defaultVal))
			{
				minVal = minMaxBounds.Min;
				maxVal = minMaxBounds.Max;
			}

			Rect controlRect = EditorGUI.PrefixLabel(position, label);
			Rect[] multiControlRects = SplitControlRect(controlRect);

			label.tooltip = GetTooltipText(minMaxBounds.Min, minMaxBounds.Max);

			EditorGUI.BeginChangeCheck();

			minVal = ShowValueEditorField(multiControlRects[minInputRect], minVal);
			maxVal = ShowValueEditorField(multiControlRects[maxInputRect], maxVal);

			var minValFloat = GetValueAsFloat(minVal);
			var maxValFloat = GetValueAsFloat(maxVal);

			EditorGUI.MinMaxSlider(
				multiControlRects[sliderInputRect],
				ref minValFloat,
				ref maxValFloat,
				GetValueAsFloat(minMaxBounds.Min),
				GetValueAsFloat(minMaxBounds.Max)
			);

			minVal = minMaxBounds.Clamp(GetFloatAsValue(minValFloat));
			maxVal = minMaxBounds.Clamp(GetFloatAsValue(maxValFloat));

			if (!EditorGUI.EndChangeCheck())
			{
				return;
			}

			SetSerializedValue(minProperty, minVal);
			SetSerializedValue(maxProperty, maxVal);

			property.serializedObject.ApplyModifiedProperties();
		}

		private void WarnInvalidPropertyTypeIfNeeded(string propertyName)
		{

			if (_didWarnInvalidPropertyType)
			{
				return;
			}

			Debug.LogWarning(
				$"Invalid attribute usage of {typeof(AttributeType).Name}" +
				$" - only {typeof(ValueType).Name} fields are allowed" +
				$", but received {fieldInfo.FieldType.Name}" +
				$" (target property -> {propertyName})"
			);

			_didWarnInvalidPropertyType = true;
		}

		private Rect[] SplitControlRect(Rect controlRect)
		{

			Rect[] rects = controlRect.SplitHorizontally(controlRectCount);
			int spacing = (int)rects[minInputRect].width - 50;

			rects[minInputRect].width -= spacing + controlRectPadding;
			rects[maxInputRect].width -= spacing + controlRectPadding;
			rects[sliderInputRect].x -= spacing;
			rects[sliderInputRect].width += spacing * 2;
			rects[maxInputRect].x += spacing + controlRectPadding;

			return rects;
		}
	}
}
