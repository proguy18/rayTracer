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
            // Write your code here...
            Vector3 co = ray.Origin - center;

            // Calculates discriminant
            double a = ray.Direction.Dot(ray.Direction);
            double b = 2 * ray.Direction.Dot(co);
            double c = co.Dot(co) - radius * radius;
            double discriminant = b * b - 4 * a * c;

            // Ray intersects the sphere in two points
            if (discriminant > 0) {
                // Calculate t using quadratic formula
                double t1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                double t2 = (-b - Math.Sqrt(discriminant)) / (2 * a);

                double t;

                // Uses the smaller t
                if(t1 > t2) {
                    t = t2;
                }
                else 
                {
                    t = t1;
                }

                Vector3 intersection = ray.Origin + ray.Direction * t;

                // Calculate the normal to the point of intersection

                Vector3 normalHit1 = (intersection - center).Normalized();

                // Calculates the incident ray
                Vector3 incidentRay = ray.Origin + ray.Direction;

                return new RayHit(intersection, normalHit1, incidentRay, material

                );

            }

            // Ray intersects the sphere in one point (tangent)
            else if (discriminant == 0) {
                // Calculate t using quadratic formula
                double t = (-b) / (2 * a);

                Vector3 intersection = ray.Origin + ray.Direction * t;

                // Calculate the normal to the point of intersection

                Vector3 normalHit = (intersection - center).Normalized();

                // Calculates the incident ray
                Vector3 incidentRay = ray.Origin + ray.Direction;
                return new RayHit(intersection, normalHit, incidentRay, material
                );

            }

            // Ray does not intersect the sphere
            else {
                return null;
            }
            // return null;
        }

        /// <summary>
        /// The material of the sphere.
        /// </summary>
        public Material Material { get { return this.material; } }
    }

}
