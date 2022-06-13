namespace Obsidize.RangeInput
{
	public interface IMinMaxAttribute<T>
	{
		public IReadOnlyMinMaxRange<T> Bounds { get; }
	}
}
