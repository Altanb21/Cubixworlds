
using System.Numerics;

namespace Shittopia_Server
{
    internal class WorldGenerator
    {
        private Noise perlinNoise;

        public WorldGenerator() => this.perlinNoise = new Noise(1.0, 0.023, 256.0, 1, 123);

        public byte BlockCategoryAtPosition(Vector3 blockPos)
        {
            double num = this.perlinNoise.Get3D((double)blockPos.X, (double)blockPos.Y, (double)blockPos.Z);
            return (double)blockPos.Y >= num ? (byte)0 : (byte)1;
        }
    }
}
