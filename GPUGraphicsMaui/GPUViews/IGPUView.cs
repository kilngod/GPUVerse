// ---------------------------------------------------------------------------------------
//                                        ILGPU
//                           Copyright (c) 2023 ILGPU Project
//                                    www.ilgpu.net
//
// File: .cs
//
// This file is part of ILGPU and is distributed under the University of Illinois Open
// Source License. See LICENSE.txt for details.
// ---------------------------------------------------------------------------------------

using Microsoft.Maui.Controls;

using GPUGraphicsMaui.GPUHandlers;


namespace GPUGraphicsMaui.GPUViews
{
    public interface IGPUView : IView
    {
        GPUEngine Engine { get; }
        GPUPlatform Platform { get; }

        void Invalidate();
    }
}
