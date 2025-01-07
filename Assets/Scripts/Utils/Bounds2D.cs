using System;
using UnityEngine;

namespace Alta.Utils
{
    public struct Bounds2D
    {
        private Vector2 center;
        private Vector2 extents;

        public Vector2 Center
        {
            get => center;
            set => center = value;
        }

        public Vector2 Size
        {
            get => extents * 2f;
            set => Extents = value * 0.5f;
        }

        public Vector2 Extents
        {
            get => extents;
            set => extents = new Vector2(MathF.Abs(value.x), MathF.Abs(value.y));
        }

        public Vector2 Min
        {
            get => center - Extents;
            set => Center = value + Extents;
        }

        public Vector2 Max
        {
            get => center + Extents;
            set => Center = value - Extents;
        }

        public Bounds2D(Vector2 center, Vector2 extents)
        {
            this.center = center;
            this.extents = new Vector2(Math.Abs(extents.x), Math.Abs(extents.y));
        }

        public bool Contains(Vector2 point)
        {
            return point.x >= Min.x && point.x <= Max.x &&
                   point.y >= Min.y && point.y <= Max.y;
        }

        public bool Intersects(Bounds2D other)
        {
            return Min.x <= other.Max.x && Max.x >= other.Min.x &&
                   Min.y <= other.Max.y && Max.y >= other.Min.y;
        }

        public override string ToString()
        {
            return $"Bounds2D(Center: {center}, Size: {Size})";
        }
    }
}