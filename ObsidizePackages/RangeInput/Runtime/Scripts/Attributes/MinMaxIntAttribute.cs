using UnityEngine;

namespace Obsidize.RangeInput
{
	public class MinMaxIntAttribute : PropertyAttribute, IMinMaxAttribute<int>
	{

		private readonly MinMaxRangeInt _bounds;

		public IReadOnlyMinMaxRange<int> Bounds => _bounds;

		public MinMaxIntAttribute(int min, int max)
		{
			_bounds = new MinMaxRangeInt(min, max);
		}
	}
}
