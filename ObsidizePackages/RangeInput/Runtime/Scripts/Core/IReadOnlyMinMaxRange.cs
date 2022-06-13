namespace Obsidize.RangeInput
{
	public interface IReadOnlyMinMaxRange<T>
	{
		public T Min { get; }
		public T Max { get; }
		public T Clamp(T value);
		public bool Contains(T value);
		public T Lerp(float t);
		public float InverseLerp(T value);
	}
}
