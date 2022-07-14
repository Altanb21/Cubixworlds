
using System;
using System.Numerics;

namespace Shittopia_Server
{
    internal class Noise
    {
        private double persistence;
        private double frequency;
        private double amplitude;
        private int octaves;
        private int randomSeed;

        public Noise(
          double persistence,
          double frequency,
          double amplitude,
          int octaves,
          int randomSeed)
        {
            this.persistence = persistence;
            this.frequency = frequency;
            this.amplitude = amplitude;
            this.octaves = octaves;
            this.randomSeed = randomSeed;
        }

        public double Get2D(double x, double y)
        {
            double num = 0.0;
            double amplitude = this.amplitude;
            double frequency = this.frequency;
            for (int index = 0; index < this.octaves; ++index)
            {
                num += this.Gradient2D(x * frequency, y * frequency, this.randomSeed) * amplitude;
                amplitude *= this.persistence;
                frequency *= 2.0;
            }
            return num;
        }

        public double Get3D(double x, double y, double z)
        {
            double num = 0.0;
            double amplitude = this.amplitude;
            double frequency = this.frequency;
            for (int index = 0; index < this.octaves; ++index)
            {
                num += this.Gradient3D(x * frequency, y * frequency, z * frequency, this.randomSeed) * amplitude;
                amplitude *= this.persistence;
                frequency *= 2.0;
            }
            return num;
        }

        public bool Get2DTrueFalse(
          Vector2 position,
          int offset,
          float scale,
          float threshold,
          int seed)
        {
            double num1 = ((double)position.X + (double)offset + 0.100000001490116) * (double)scale;
            double num2 = ((double)position.Y + (double)offset + 0.100000001490116) * (double)scale;
            Noise noise = new Noise(1.0, 1.0, 0.600000023841858, 1, seed);
            return (noise.Get2D(num1, num2) + noise.Get2D(num2, num1)) / 2.0 > (double)threshold;
        }

        public double PerlinNoise1D(int x, int seed)
        {
            int num1 = x * WorldGenConst.NOISE_MAGIC_X + WorldGenConst.NOISE_MAGIC_SEED * seed & int.MaxValue;
            int num2 = num1 << 13 ^ num1;
            return 1.0 - (double)(num2 * (num2 * num2 * 15731 + 789221) + 1376312589 & int.MaxValue) / 1073741824.0;
        }

        public double PerlinNoise2D(int x, int y, int seed)
        {
            int num1 = x * WorldGenConst.NOISE_MAGIC_X + y * WorldGenConst.NOISE_MAGIC_Y * WorldGenConst.NOISE_MAGIC_SEED * seed & int.MaxValue;
            int num2 = num1 << 13 ^ num1;
            return 1.0 - (double)(num2 * (num2 * num2 * 15731 + 789221) + 1376312589 & int.MaxValue) / 1073741824.0;
        }

        public double PerlinNoise3D(int x, int y, int z, int seed)
        {
            int num1 = x * WorldGenConst.NOISE_MAGIC_X + y * WorldGenConst.NOISE_MAGIC_Y + z * WorldGenConst.NOISE_MAGIC_Z * WorldGenConst.NOISE_MAGIC_SEED * seed & int.MaxValue;
            int num2 = num1 << 13 ^ num1;
            return 1.0 - (double)(num2 * (num2 * num2 * 15731 + 789221) + 1376312589 & int.MaxValue) / 1073741824.0;
        }

        public double NoiseSeamlessTest2D(int x, int y, int seed) => 0.0;

        public double NoiseSeamless2D(int x, int y, int seed, int w, int h) => (0.0 + this.PerlinNoise2D(x, y, seed) * (double)(w - x) * (double)(h - y) + this.PerlinNoise2D(x - w, y, seed) * (double)x * (double)(h - y) + this.PerlinNoise2D(x - w, y - h, seed) * (double)x * (double)y + this.PerlinNoise2D(x, y - h, seed) * (double)(w - x) * (double)y) / (double)(w * h);

        public double NoiseSeamlessX2D(int x, int y, int seed, int w) => ((double)(w - x) * this.PerlinNoise2D(x, y, seed) + (double)x * this.PerlinNoise2D(x - w, y, seed)) / (double)w;

        public double Gradient2D(double x, double y, int seed)
        {
            int x1 = x > 0.0 ? (int)x : (int)x - 1;
            int y1 = y > 0.0 ? (int)y : (int)y - 1;
            double x2 = x - (double)x1;
            double x3 = y - (double)y1;
            double a1 = this.Smooth2D(x1, y1, seed);
            double b1 = this.Smooth2D(x1 + 1, y1, seed);
            double a2 = this.Smooth2D(x1, y1 + 1, seed);
            double b2 = this.Smooth2D(x1 + 1, y1 + 1, seed);
            return this.InterpolateCosine(this.InterpolateCosine(a1, b1, x2), this.InterpolateCosine(a2, b2, x2), x3);
        }

        public double Gradient3D(double x, double y, double z, int seed)
        {
            int x1 = x > 0.0 ? (int)x : (int)x - 1;
            int y1 = y > 0.0 ? (int)y : (int)y - 1;
            int z1 = z > 0.0 ? (int)z : (int)z - 1;
            double x2 = x - (double)x1;
            double x3 = y - (double)y1;
            double x4 = z - (double)z1;
            double a1 = this.Smooth3D(x1, y1, z1, seed);
            double b1 = this.Smooth3D(x1 + 1, y1, z1, seed);
            double a2 = this.Smooth3D(x1, y1 + 1, z1, seed);
            double b2 = this.Smooth3D(x1 + 1, y1 + 1, z1, seed);
            double a3 = this.Smooth3D(x1, y1, z1 + 1, seed);
            double b3 = this.Smooth3D(x1 + 1, y1, z1 + 1, seed);
            double a4 = this.Smooth3D(x1, y1 + 1, z1 + 1, seed);
            double b4 = this.Smooth3D(x1 + 1, y1 + 1, z1 + 1, seed);
            double a5 = this.InterpolateCosine(a1, b1, x2);
            double b5 = this.InterpolateCosine(a2, b2, x2);
            double a6 = this.InterpolateCosine(a3, b3, x2);
            double b6 = this.InterpolateCosine(a4, b4, x2);
            return this.InterpolateCosine(this.InterpolateCosine(a5, b5, x3), this.InterpolateCosine(a6, b6, x3), x4);
        }

        public double InterpolateLinear(double a, double b, double x) => a * (1.0 - x) + b * x;

        public double InterpolateCosine(double a, double b, double x)
        {
            double num = (1.0 - Math.Cos(x * 3.14159265358979)) * 0.5;
            return a * (1.0 - num) + b * num;
        }

        public double InterpolateCubic(double v0, double v1, double v2, double v3, float x)
        {
            double num1 = v3 - v2 - (v0 - v1);
            double num2 = v0 - v1 - num1;
            double num3 = v2 - v0;
            double num4 = v1;
            return num1 * (double)x * (double)x * (double)x + num2 * (double)x * (double)x + num3 * (double)x + num4;
        }

        public double Smooth1D(int x, int seed) => this.PerlinNoise1D(x, seed) / 2.0 + this.PerlinNoise1D(x - 1, seed) / 4.0 + this.PerlinNoise1D(x + 1, seed) / 4.0;

        public double Smooth2D(int x, int y, int seed)
        {
            double num1 = (this.PerlinNoise2D(x - 1, y - 1, seed) + this.PerlinNoise2D(x + 1, y - 1, seed) + this.PerlinNoise2D(x - 1, y + 1, seed) + this.PerlinNoise2D(x + 1, y + 1, seed)) / 16.0;
            double num2 = (this.PerlinNoise2D(x - 1, y, seed) + this.PerlinNoise2D(x + 1, y, seed) + this.PerlinNoise2D(x, y - 1, seed) + this.PerlinNoise2D(x, y + 1, seed)) / 8.0;
            double num3 = this.PerlinNoise2D(x, y, seed) / 4.0;
            double num4 = num2;
            return num1 + num4 + num3;
        }

        public double Smooth3D(int x, int y, int z, int seed)
        {
            double num1 = (0.0 + (this.PerlinNoise3D(x + 1, y + 1, z, seed) + this.PerlinNoise3D(x - 1, y + 1, z, seed) + this.PerlinNoise3D(x, y + 1, z + 1, seed) + this.PerlinNoise3D(x, y + 1, z - 1, seed)) + (this.PerlinNoise3D(x + 1, y - 1, z, seed) + this.PerlinNoise3D(x - 1, y - 1, z, seed) + this.PerlinNoise3D(x, y - 1, z + 1, seed) + this.PerlinNoise3D(x, y - 1, z - 1, seed)) + (this.PerlinNoise3D(x + 1, y, z + 1, seed) + this.PerlinNoise3D(x + 1, y, z - 1, seed) + this.PerlinNoise3D(x - 1, y, z + 1, seed) + this.PerlinNoise3D(x - 1, y, z - 1, seed))) / 48.0;
            double num2 = (0.0 + (this.PerlinNoise3D(x - 1, y - 1, z - 1, seed) + this.PerlinNoise3D(x - 1, y - 1, z + 1, seed) + this.PerlinNoise3D(x - 1, y + 1, z - 1, seed) + this.PerlinNoise3D(x - 1, y + 1, z + 1, seed)) + (this.PerlinNoise3D(x + 1, y - 1, z - 1, seed) + this.PerlinNoise3D(x + 1, y - 1, z + 1, seed) + this.PerlinNoise3D(x + 1, y + 1, z - 1, seed) + this.PerlinNoise3D(x + 1, y + 1, z + 1, seed))) / 32.0;
            double num3 = 0.0;
            double num4 = this.PerlinNoise3D(x - 1, y, z, seed) + this.PerlinNoise3D(x - 1, y, z, seed) + this.PerlinNoise3D(x, y + 1, z, seed);
            double num5 = (num2 + num4 + (this.PerlinNoise3D(x, y - 1, z, seed) + this.PerlinNoise3D(x, y, z + 1, seed) + this.PerlinNoise3D(x, y, z - 1, seed))) / 16.0;
            double num6 = this.PerlinNoise3D(x, y, z, seed) / 8.0;
            double num7 = num3;
            return num5 + num7 + num6;
        }
    }
}
