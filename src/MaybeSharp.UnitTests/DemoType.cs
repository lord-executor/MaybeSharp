namespace MaybeSharp.UnitTests
{
	public class DemoType
	{
		public string Name { get; } = "DemoType";
		public int Index { get; }
		public object Optional { get; }

		public DemoType() { }

		public DemoType(string name)
		{
			Name = name;
		}

		public DemoType(string name, int index)
			: this(name)
		{
			Index = index;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
