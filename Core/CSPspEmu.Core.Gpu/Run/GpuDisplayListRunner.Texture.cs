﻿using System;
using CSPspEmu.Core.Gpu.State;
using CSharpUtils;
using CSPspEmu.Core.Types;

namespace CSPspEmu.Core.Gpu.Run
{
    public sealed unsafe partial class GpuDisplayListRunner
    {
        private TextureStateStruct* TextureState => &GpuState->TextureMappingState.TextureState;

        [GpuInstructionAttribute(GpuOpCodes.TME)]
        public void OP_TME() => GpuState->TextureMappingState.Enabled = Bool1;

        // ReSharper disable once UnusedMember.Global
        [GpuInstructionAttribute(GpuOpCodes.TMS)]
        public void OP_TMS() => GpuState->TextureMappingState.Matrix.Reset();

        // ReSharper disable once UnusedMember.Global
        [GpuInstructionAttribute(GpuOpCodes.TMATRIX)]
        public void OP_TMATRIX() => GpuState->TextureMappingState.Matrix.Write(Float1);

        [GpuInstructionAttribute(GpuOpCodes.TMODE)]
        public void OP_TMODE()
        {
            TextureState->Swizzled = (Param8(0) != 0);
            TextureState->MipmapShareClut = (Param8(8) != 0);
            TextureState->MipmapMaxLevel = (int) Param8(16);
        }

        // Texture Pixel Storage Mode
        // ReSharper disable once UnusedMember.Global
        [GpuInstructionAttribute(GpuOpCodes.TPSM)]
        public void OP_TPSM() => TextureState->PixelFormat = (GuPixelFormats) Extract(0, 4);

        private TextureStateStruct.MipmapState* MipMapState(int index) => &(&TextureState->Mipmap0)[index];

        /// <summary>
        /// TextureMipmap Buffer Pointer.
        /// </summary>
        private void _OP_TBP(int index)
        {
            var mipMap = MipMapState(index);
            mipMap->Address = (mipMap->Address & 0xFF000000) | (Params24 & 0x00FFFFFF);
        }

        /// <summary>
        /// TextureMipmap Buffer Width.
        /// </summary>
        private void _OP_TBW(int index)
        {
            var mipMap = MipMapState(index);
            mipMap->BufferWidth = Param16(0);
            mipMap->Address = (mipMap->Address & 0x00FFFFFF) | ((uint) (Param8(16) << 24) & 0xFF000000);
        }

        [GpuInstructionAttribute(GpuOpCodes.TBP0)]
        public void OP_TBP0() => _OP_TBP(0);

        [GpuInstructionAttribute(GpuOpCodes.TBP1)]
        public void OP_TBP1() => _OP_TBP(1);

        [GpuInstructionAttribute(GpuOpCodes.TBP2)]
        public void OP_TBP2() => _OP_TBP(2);

        [GpuInstructionAttribute(GpuOpCodes.TBP3)]
        public void OP_TBP3() => _OP_TBP(3);

        [GpuInstructionAttribute(GpuOpCodes.TBP4)]
        public void OP_TBP4() => _OP_TBP(4);

        [GpuInstructionAttribute(GpuOpCodes.TBP5)]
        public void OP_TBP5() => _OP_TBP(5);

        [GpuInstructionAttribute(GpuOpCodes.TBP6)]
        public void OP_TBP6() => _OP_TBP(6);

        [GpuInstructionAttribute(GpuOpCodes.TBP7)]
        public void OP_TBP7() => _OP_TBP(7);

        [GpuInstructionAttribute(GpuOpCodes.TBW0)]
        public void OP_TBW0() => _OP_TBW(0);

        [GpuInstructionAttribute(GpuOpCodes.TBW1)]
        public void OP_TBW1() => _OP_TBW(1);

        [GpuInstructionAttribute(GpuOpCodes.TBW2)]
        public void OP_TBW2() => _OP_TBW(2);

        [GpuInstructionAttribute(GpuOpCodes.TBW3)]
        public void OP_TBW3() => _OP_TBW(3);

        [GpuInstructionAttribute(GpuOpCodes.TBW4)]
        public void OP_TBW4() => _OP_TBW(4);

        [GpuInstructionAttribute(GpuOpCodes.TBW5)]
        public void OP_TBW5() => _OP_TBW(5);

        [GpuInstructionAttribute(GpuOpCodes.TBW6)]
        public void OP_TBW6() => _OP_TBW(6);

        [GpuInstructionAttribute(GpuOpCodes.TBW7)]
        public void OP_TBW7() => _OP_TBW(7);

        private void _OP_TSIZE(int index)
        {
            // Astonishia Story is using normalArgument = 0x1804
            // -> use texture_height = 1 << 0x08 (and not 1 << 0x18)
            //        texture_width  = 1 << 0x04
            // The maximum texture size is 512x512: the exponent value must be [0..9]
            // Maybe a bit flag for something?

            var mipMap = MipMapState(index);
            var widthExp = (int) BitUtils.Extract(Params24, 0, 4);
            var heightExp = (int) BitUtils.Extract(Params24, 8, 4);
            var unknownFlag = BitUtils.Extract(Params24, 15, 1) != 0;
            if (unknownFlag)
            {
                Console.Error.WriteLine("_OP_TSIZE UnknownFlag : 0x{0:X}", Params24);
            }
            widthExp = Math.Min(widthExp, 9);
            heightExp = Math.Min(heightExp, 9);

            mipMap->TextureWidth = (ushort) (1 << widthExp);
            mipMap->TextureHeight = (ushort) (1 << heightExp);
        }

        [GpuInstructionAttribute(GpuOpCodes.TSIZE0)]
        public void OP_TSIZE0() => _OP_TSIZE(0);

        [GpuInstructionAttribute(GpuOpCodes.TSIZE1)]
        public void OP_TSIZE1() => _OP_TSIZE(1);

        [GpuInstructionAttribute(GpuOpCodes.TSIZE2)]
        public void OP_TSIZE2() => _OP_TSIZE(2);

        [GpuInstructionAttribute(GpuOpCodes.TSIZE3)]
        public void OP_TSIZE3() => _OP_TSIZE(3);

        [GpuInstructionAttribute(GpuOpCodes.TSIZE4)]
        public void OP_TSIZE4() => _OP_TSIZE(4);

        [GpuInstructionAttribute(GpuOpCodes.TSIZE5)]
        public void OP_TSIZE5() => _OP_TSIZE(5);

        [GpuInstructionAttribute(GpuOpCodes.TSIZE6)]
        public void OP_TSIZE6() => _OP_TSIZE(6);

        [GpuInstructionAttribute(GpuOpCodes.TSIZE7)]
        public void OP_TSIZE7() => _OP_TSIZE(7);

        [GpuInstructionAttribute(GpuOpCodes.TFLUSH)]
        public void OP_TFLUSH() => GpuDisplayList.GpuProcessor.GpuImpl.TextureFlush(GpuState);

        [GpuInstructionAttribute(GpuOpCodes.TSYNC)]
        public void OP_TSYNC() => GpuDisplayList.GpuProcessor.GpuImpl.TextureSync(GpuState);

        [GpuInstructionAttribute(GpuOpCodes.TFLT)]
        public void OP_TFLT()
        {
            TextureState->FilterMinification = (TextureFilter) Param8(0);
            TextureState->FilterMagnification = (TextureFilter) Param8(8);
        }

        [GpuInstructionAttribute(GpuOpCodes.TWRAP)]
        public void OP_TWRAP()
        {
            TextureState->WrapU = (WrapMode) Param8(0);
            TextureState->WrapV = (WrapMode) Param8(8);
        }

        [GpuInstructionAttribute(GpuOpCodes.TFUNC)]
        public void OP_TFUNC()
        {
            TextureState->Effect = (TextureEffect) Param8(0);
            TextureState->ColorComponent = (TextureColorComponent) Param8(8);
            TextureState->Fragment2X = (Param8(16) != 0);

            //Console.WriteLine(TextureState->Effect);
        }

        [GpuInstructionAttribute(GpuOpCodes.USCALE)]
        public void OP_USCALE() => GpuState->TextureMappingState.TextureState.ScaleU = Float1;

        [GpuInstructionAttribute(GpuOpCodes.VSCALE)]
        public void OP_VSCALE() => GpuState->TextureMappingState.TextureState.ScaleV = Float1;

        [GpuInstructionAttribute(GpuOpCodes.UOFFSET)]
        public void OP_UOFFSET() => GpuState->TextureMappingState.TextureState.OffsetU = Float1;

        [GpuInstructionAttribute(GpuOpCodes.VOFFSET)]
        public void OP_VOFFSET() => GpuState->TextureMappingState.TextureState.OffsetV = Float1;

        [GpuInstructionAttribute(GpuOpCodes.TEC)]
        public void OP_TEC() => GpuState->TextureMappingState.TextureEnviromentColor.SetRGB_A1(Params24);

        [GpuInstructionAttribute(GpuOpCodes.TEXTURE_ENV_MAP_MATRIX)]
        public void OP_TEXTURE_ENV_MAP_MATRIX()
        {
            GpuState->TextureMappingState.ShadeU = (short) BitUtils.Extract(Params24, 0, 2);
            GpuState->TextureMappingState.ShadeV = (short) BitUtils.Extract(Params24, 8, 2);
        }

        [GpuInstructionAttribute(GpuOpCodes.TMAP)]
        public void OP_TMAP()
        {
            GpuState->TextureMappingState.TextureMapMode = (TextureMapMode) Param8(0);
            GpuState->TextureMappingState.TextureProjectionMapMode = (TextureProjectionMapMode) Param8(8);
            GpuState->VertexState.Type.NormalCount = GpuState->TextureMappingState.GetTextureComponentsCount();
        }

        [GpuInstructionAttribute(GpuOpCodes.TBIAS)]
        public void OP_TBIAS()
        {
            GpuState->TextureMappingState.LevelMode = (TextureLevelMode) Param8(0);
            GpuState->TextureMappingState.MipmapBias = Param8(16) / 16.0f;
        }

        [GpuInstructionAttribute(GpuOpCodes.TSLOPE)]
        public void OP_TSLOPE() => GpuState->TextureMappingState.SlopeLevel = Float1;
    }
}