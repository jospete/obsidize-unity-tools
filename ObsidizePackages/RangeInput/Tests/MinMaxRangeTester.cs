using UnityEngine;

namespace Obsidize.RangeInput.Tests
{
	[CreateAssetMenu(menuName = "Range Tester")]
	public class MinMaxRangeTester : ScriptableObject
	{

		public MinMaxRange testFloatRange;
		public MinMaxRangeInt testIntRange;

		[MinMax(-1f, 1f)]
		public MinMaxRange testAttrFloatRange;

		[MinMaxInt(-50, 100)]
		public MinMaxRangeInt testAttrIntRange;

		/*	[MinMaxInt(-50, 100)]
			public float invalidAttrTest;*/
	}
}
