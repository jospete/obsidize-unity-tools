using UnityEditor;
using UnityEngine;

namespace Obsidize.RangeInput.Editor
{
	public static class MinMaxEditorUtility
	{

		private const string minPropertyName = "_min";
		private const string maxPropertyName = "_max";

		public static SerializedProperty GetMinMaxRangeMinProperty(this SerializedProperty property)
		{
			return property.FindPropertyRelative(minPropertyName);
		}

		public static SerializedProperty GetMinMaxRangeMaxProperty(this SerializedProperty property)
		{
			return property.FindPropertyRelative(maxPropertyName);
		}

		public static Rect[] SplitHorizontally(this Rect r, int count)
		{

			Rect[] rects = new Rect[count];

			for (int i = 0; i < count; i++)
			{
				rects[i] = new Rect(
					r.position.x + (i * r.width / count),
					r.position.y,
					r.width / count,
					r.height
				);
			}

			return rects;
		}
	}
}
