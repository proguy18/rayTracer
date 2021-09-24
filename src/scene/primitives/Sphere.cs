using System;

namespace RayTracer
{
    /// <summary>
    /// Class to represent an (infinite) plane in a scene.
    /// </summary>
    public class Sphere : SceneEntity
    {
        private Vector3 center;
        private double radius;
        private Material material;

        /// <summary>
        /// Construct a sphere given its center point and a radius.
        /// </summary>
        /// <param name="center">Center of the sphere</param>
        /// <param name="radius">Radius of the spher</param>
        /// <param name="material">Material assigned to the sphere</param>
        public Sphere(Vector3 center, double radius, Material material)
        {
            this.center = center;
            this.radius = radius;
            this.material = material;
        }

        /// <summary>
        /// Determine if a ray intersects with the sphere, and if so, return hit data.
        /// </summary>
        /// <param name="ray">Ray to check</param>
        /// <returns>Hit data (or null if no intersection)</returns>
        public RayHit Intersect(Ray ray)
        {
            Vector3 oc = ray.Origin - center;
            Vector3 rayDir = ray.Direction;
            double a = rayDir.LengthSq();
            double b = oc.Dot(rayDir);
            double c = oc.LengthSq() - (radius * radius);
            double discriminant = b * b - a * c;
            if (discriminant > 0)
            {
                double tmp = Math.Sqrt(discriminant);
                double t = (-b - tmp) / a;
                if (t > Utils.Epsilon)
                    return ComputeRayHit(ray, t);
                t = (-b + tmp) / a;
                if (t > Utils.Epsilon)
                    return ComputeRayHit(ray, t);
            }
            return null;
        }

        private RayHit ComputeRayHit(Ray ray, double t)
        {
            Vector3 position = ray.Origin + t * ray.Direction;
            Vector3 normal = (position - center) / radius;
            RayHit rayHit = new RayHit(position, normal.Normalized(), ray.Direction, null);
            return rayHit;
        }

        /// <summary>
        /// The material of the sphere.
        /// </summary>
        public Material Material { get { return this.material; } }
    }

}
