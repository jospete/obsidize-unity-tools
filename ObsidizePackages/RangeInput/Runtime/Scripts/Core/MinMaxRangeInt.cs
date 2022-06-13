using System;
using UnityEngine;

namespace Obsidize.RangeInput
{

	[Serializable]
	public struct MinMaxRangeInt : IMinMaxRange<int>
	{

		[SerializeField] private int _min;
		[SerializeField] private int _max;
		public int Min
		{
			get => _min;
			set => _min = Mathf.Min(value, _max);
		}
		public int Max
		{
			get => _max;
			set => _max = Mathf.Max(value, _min);
		}

		public MinMaxRangeInt(int min, int max)
		{
			_min = Mathf.Min(min, max);
			_max = Mathf.Max(min, max);
		}

		public int Clamp(int value)
		{
			return Mathf.Clamp(value, Min, Max);
		}

		public bool Contains(int value)
		{
			return Min <= value && value <= Max;
		}

		public float InverseLerp(int value)
		{
			return Mathf.InverseLerp(Min, Max, value);
		}

		public int Lerp(float t)
		{
			return Lerp(t, Mathf.RoundToInt);
		}

		public int LerpFloor(float t)
		{
			return Lerp(t, Mathf.FloorToInt);
		}

		public int LerpCeil(float t)
		{
			return Lerp(t, Mathf.CeilToInt);
		}

		public float LerpFloat(float t)
		{
			return Mathf.Lerp(Min, Max, t);
		}

		public int Lerp(float t, Func<float, int> disambiguate)
		{
			return disambiguate(LerpFloat(t));
		}
	}
}
