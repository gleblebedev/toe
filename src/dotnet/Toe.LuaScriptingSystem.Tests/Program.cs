using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AluminumLua;

namespace Toe.LuaScriptingSystem.Tests
{
	class Program
	{
		static void Main(string[] args)
		{
			var t = new Test();
			t.SetUp();
			t.Test1();
			t.TearDown();
		}
	}
}
