using UnityEditor;

namespace Obsidize.RangeInput.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxRangeInt))]
	public class MinMaxRangeIntDrawer : MinMaxDrawerBase<MinMaxRangeInt, int>
	{
	}
}
