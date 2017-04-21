using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace BulletEngineNeo
{
    public class Vec2
    {
        public float x;
        public float y;

        public Vec2() { x = 0; y = 0; }
        public Vec2(float a, float b) { x = a;  y = b; }

        [MoonSharpHidden]
        public Vector2 ToMono() { return new Vector2(x, y); }

        [MoonSharpHidden]
        public void FromMono(Vector2 v) { x = v.X; y = v.Y; }

        public static Vec2 operator +(Vec2 v, Vec2 v2) { return new Vec2(v.x + v2.x, v.y + v2.y); }
    }
}
