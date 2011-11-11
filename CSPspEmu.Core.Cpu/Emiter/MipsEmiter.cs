﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace CSPspEmu.Core.Cpu.Emiter
{
	public class MipsEmiter
	{
		static private ulong UniqueCounter = 0;
		internal AssemblyBuilder AssemblyBuilder;
		internal ModuleBuilder ModuleBuilder;

		public MipsEmiter()
		{
			Reset();
		}

		public void Reset()
		{
			UniqueCounter++;
			var CurrentAppDomain = AppDomain.CurrentDomain;
			AssemblyBuilder = CurrentAppDomain.DefineDynamicAssembly(new AssemblyName("assembly" + UniqueCounter), AssemblyBuilderAccess.RunAndSave);
			ModuleBuilder = AssemblyBuilder.DefineDynamicModule("module" + UniqueCounter);
		}
	}
}
