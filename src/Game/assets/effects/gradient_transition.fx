// Effect responsible for generating a vertical gradient (with two colors)
// that changes based on a numerical instance of time to another chosen
// gradient that also has two colors.

#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

extern float4 StartColor1;
extern float4 StartColor2;
extern float4 EndColor1;
extern float4 EndColor2;
extern float TimeNormalized;

sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    // Sample the texture color
    float4 pixelColor = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

    // Calculate the gradient factor using the y-coordinate of the texture
    float gradientFactor = input.TextureCoordinates.y;

    // Interpolate between InitialColor and FinalColor based on the gradient factor
    float4 interpolatedColor1 = lerp(StartColor1, EndColor1, gradientFactor);
    float4 interpolatedColor2 = lerp(StartColor2, EndColor2, gradientFactor);

    // Builds the final color by applying the transition between the two gradients.
    float4 finalColor = lerp(interpolatedColor1, interpolatedColor2, TimeNormalized);
    
    // Combine the pixel color with the gradient color
    return pixelColor * finalColor;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
