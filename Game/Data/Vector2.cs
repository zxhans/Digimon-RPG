using System;
using System.Diagnostics;

namespace Digimon_Project.Game.Data
{
    public class Vector2
    {
        public float X;
        public float Y;
        public readonly float Length;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
            Length = (float)Math.Sqrt(x * x + y * y);
        }

        public int Distance(float x2, float y2)
        {
            float x1 = X;
            float y1 = Y;
            float dX = x1 - x2;
            float dY = y1 - y2;

            int dist = (int)(dX + dY);

            return dist;
        }

        public int Distance(Vector2 vec)
        {
            float x1 = X;
            float y1 = Y;
            float x2 = vec.X;
            float y2 = vec.Y;
            float dX = Math.Abs(x1 - x2);
            float dY = Math.Abs(y1 - y2);

            int dist = (int)(dX + dY);

            return dist;
        }

        public float Dot(Vector2 vec)
        {
            return X * vec.X + Y * vec.Y;
        }

        public Vector2 Normalize()
        {
            return new Vector2(X / Length, Y / Length);
        }

        public Vector2 Rotate(float angle)
        {
            double rad = ((Math.PI / 180) * angle);
            double cos = Math.Cos(rad);
            double sin = Math.Sin(rad);

            return new Vector2( (float)(X * cos - Y * sin), (float)(X* sin + Y * cos));
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator /(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X / v2.X, v1.Y / v2.Y);
        }

        public static Vector2 operator *(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X * v2.X, v1.Y * v2.Y);
        }
    }
}
