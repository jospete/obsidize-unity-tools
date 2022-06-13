using UnityEngine;

namespace Obsidize.RangeInput
{
	public class MinMaxAttribute : PropertyAttribute, IMinMaxAttribute<float>
	{

		private readonly MinMaxRange _bounds;

		public IReadOnlyMinMaxRange<float> Bounds => _bounds;

		public MinMaxAttribute(float min, float max)
		{
			_bounds = new MinMaxRange(min, max);
		}
	}
}
