Shader "Unlit/ColorCycle" {
Properties {
    //[IntRange] index ("Color Range Index", Range(0, 7)) = 0
    alpha ("Opacity", Range(0.0, 1.0)) = 1.0
    speed ("Speed Multiplier", Range(0.0, 10.0)) = 1.0
}
SubShader {
Blend SrcAlpha OneMinusSrcAlpha
Tags {
    "RenderType"="Transparent"
    "Queue"="Transparent"
}
Pass { CGPROGRAM
#include "UnityCG.cginc"
#define time _Time.x

#pragma vertex VSMain
#pragma fragment FSMain

uniform int index;
uniform float alpha, speed;

// RGBA Color Model
static const float4 white = { 1.0, 1.0, 1.0, alpha };
static const float4 red = { 1.0, 0.0, 0.0, alpha };
static const float4 green = { 0.0, 1.0, 0.0, alpha };
static const float4 blue = { 0.0, 0.0, 1.0, alpha };

// CMYK Color Model
static const float4 cyan = { 0.0, 1.0, 1.0, alpha };
static const float4 magenta = { 1.0, 0.0, 1.0, alpha };
static const float4 yellow = { 1.0, 1.0, 0.0, alpha };
static const float4 key = { 0.0, 0.0, 0.0, alpha };

static const int length = 8;

static const float4 colors[length] = {
    white, red, green, blue,
    cyan, magenta, yellow, key
};

v2f_img VSMain(appdata_img i) {
    v2f_img o = {
        UnityObjectToClipPos(i.vertex),
        i.texcoord
    };
    return o;
}

float4 FSMain(v2f_img i) : SV_TARGET0 {
    float t = (time * length) * speed;

    int curind = int(floor(t) % length);
    int nexind = int(ceil(t) % length);

    return lerp(
        colors[curind],
        colors[nexind],
        frac(t)
    );
}
ENDCG }
}}
