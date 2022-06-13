using UnityEditor;

namespace Obsidize.RangeInput.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxRange))]
	public class MinMaxRangeDrawer : MinMaxDrawerBase<MinMaxRange, float>
	{
	}
}
