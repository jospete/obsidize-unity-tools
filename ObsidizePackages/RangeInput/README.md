# Obsidize Range Input

Tools for registering and editing both dynamic and fixed range values.

## Installation

This git repo is directly installable via the unity package manager.
Simply paste the repo url into the package manager "add" menu and unity will do the rest.

## Usage

```csharp
using Obsidize.RangeInput;
using UnityEngine;

[CreateAssetMenu(menuName = "Range Tester")]
public class MinMaxRangeTester : ScriptableObject
{

	// A simple float range input, similar to a Vector2
	public MinMaxRange testFloatRange;

	// A simple int range input, similar to a Vector2Int
	public MinMaxRangeInt testIntRange;

	// A fixed float range input that will show a dual-knob slider
	[MinMax(-1f, 1f)]
	public MinMaxRange testAttrFloatRange;

	// A fixed int range input that will show a dual-knob slider
	[MinMaxInt(-50, 100)]
	public MinMaxRangeInt testAttrIntRange;

	// Will generate a warning log, since MinMax*** attributes are 
	// only valid on MinMax constructs
	/*	[MinMaxInt(-50, 100)]
		public float invalidAttrTest;*/
}
```