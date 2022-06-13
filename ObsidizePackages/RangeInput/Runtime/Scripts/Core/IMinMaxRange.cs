namespace Obsidize.RangeInput
{
	public interface IMinMaxRange<T> : IReadOnlyMinMaxRange<T>
	{
		public new T Min { get; set; }
		public new T Max { get; set; }
	}
}
