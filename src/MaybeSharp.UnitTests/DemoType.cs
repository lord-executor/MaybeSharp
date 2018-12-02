using System;
using System.Collections.Generic;
using System.Text;

namespace MaybeSharp.UnitTests
{
	class DemoType
	{
		public string Name { get; } = "DemoType";
		public object Optional { get; }

		public DemoType() { }

		public DemoType(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
