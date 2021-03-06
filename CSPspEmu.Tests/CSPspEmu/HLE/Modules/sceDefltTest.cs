﻿using CSPspEmu.Hle.Modules._unknownPrx;
using System.IO;
using Xunit;

namespace CSPspEmu.Hle.Modules.Tests
{
    
    public unsafe class SceDefltTest : BaseModuleTest
    {
        [Inject] sceDeflt sceDeflt = null;

        [Fact(Skip = "file not found")]
        public void TestMethod1()
        {
            //global::packageName.Properties.Resources.ThatFileName
            

            var inflated = ReadResourceBytes("sample.inflated");
            var deflated = ReadResourceBytes("sample.deflated");
            var buffer = new byte[inflated.Length];
            uint crc32;
            fixed (byte* bufferPtr = buffer)
            fixed (byte* deflatedPtr = deflated)
            {
                sceDeflt.sceZlibDecompress(bufferPtr, buffer.Length, deflatedPtr, &crc32);
            }

            Assert.Equal(inflated, buffer);

            //Assert.Equal((uint)0x3496193C, (uint)crc32);
            //Assert.Inconclusive("Missing CRC check! We should check this is correct.");

            File.WriteAllBytes("../../../TestOutput/sample.inflated.again", buffer);
        }
    }
}