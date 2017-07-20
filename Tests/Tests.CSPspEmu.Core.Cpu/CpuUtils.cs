﻿using CSPspEmu.Core.Cpu;
using CSPspEmu.Core.Memory;

namespace Tests.CSPspEmu.Core.Cpu.Cpu
{
    public class TestConnector : ICpuConnector
    {
        public void Yield(CpuThreadState CpuThreadState)
        {
        }
    }

    public class TestInterruptManager : IInterruptManager
    {
        void IInterruptManager.Interrupt(CpuThreadState CpuThreadState)
        {
        }
    }

    public static class CpuUtils
    {
        static LazyPspMemory LazyPspMemory = new LazyPspMemory();

        public static CpuProcessor CreateCpuProcessor(PspMemory Memory = null)
        {
            if (Memory == null) Memory = LazyPspMemory;
            var InjectContext = new InjectContext();
            InjectContext.SetInstance<PspMemory>(Memory);
            InjectContext.SetInstance<ICpuConnector>(new TestConnector());
            InjectContext.SetInstance<IInterruptManager>(new TestInterruptManager());
            return InjectContext.GetInstance<CpuProcessor>();
        }
    }
}