using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPUGraphicsMaui.GPURenderers
{
    public static class HLSLTriangleShaderSource
    {
        public const string TriangleShaders = @"
struct VSInput {
    float4 Position : POSITION;
    float4 Color : COLOR;
};

struct PSInput {
    float4 Position : SV_POSITION;
    float4 Color : COLOR;
};

PSInput VSMain(VSInput input) {
    PSInput result;
    result.Position = input.Position;
    result.Color = input.Color;
    return result;
}

float4 PSMain(PSInput input) : SV_TARGET{
    return input.Color;
}
";
    }
}
