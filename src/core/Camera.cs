using System.IO;
using System;
using StbImageWriteSharp;

namespace RayTracer
{
    /// <summary>
    /// Class to represent an image in memory and allow for I/O operations 
    /// relating to that image.
    /// </summary>
    public class Camera
    {
        private int aaMultiplier;
        private int numberOfSamples;

        private int width;

        private int height;
        private double imageAspectRatio;

        public int NumberOfSamples => numberOfSamples;
        private int fov;

        private Vector3 cameraPos;

        public Camera(Image outputImage, SceneOptions options) 
        {
            this.aaMultiplier = options.AAMultiplier;
            this.numberOfSamples = aaMultiplier * aaMultiplier;
            this.width = outputImage.Width;
            this.height = outputImage.Height;
            this.imageAspectRatio = width / (double)height; // assuming width > height 
            this.fov = 60;
            this.cameraPos = options.CameraPosition;
        }

        public Ray CreateRay(int w, int h, int sampleIndex) 
        {
            int xOffsetIndex = sampleIndex % aaMultiplier;
            int yOffsetIndex = sampleIndex / aaMultiplier;
            // Fire rays from each pixel, by converting from pixel notation to world space
            double Px = (2 * ((w + (xOffsetIndex + 0.5) / aaMultiplier) / width) - 1) * Math.Tan(fov / 2 * Math.PI / 180) * imageAspectRatio; 
            double Py = (1 - 2 * ((h + (yOffsetIndex + 0.5) / aaMultiplier) / height)) * Math.Tan(fov / 2 * Math.PI / 180); 
            Vector3 rayDirection = new Vector3(Px, Py, 1) - cameraPos; // note that this just equal to Vector3(Px, Py, 1); 
            rayDirection = rayDirection.Normalized(); 
            return new Ray(cameraPos, rayDirection);
        }

    }
}
