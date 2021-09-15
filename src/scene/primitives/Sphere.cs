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
        // public RayHit Intersect(Ray ray)
        // {
        //     // Write your code here...
        //     Vector3 co = ray.Origin - center;

        //     // Calculates discriminant
        //     double a = ray.Direction.Dot(ray.Direction);
        //     double b = 2 * ray.Direction.Dot(co);
        //     double c = co.Dot(co) - radius * radius;
        //     double discriminant = b * b - 4 * a * c;

        //     // Ray intersects the sphere in two points
        //     if (discriminant > 0) {
        //         // Calculate t using quadratic formula
        //         double t1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
        //         double t2 = (-b - Math.Sqrt(discriminant)) / (2 * a);

        //         double t = -1.0;

        //         // Uses the smaller t
        //         if(t1 > t2) {
        //             t = t2;
        //         }
        //         else 
        //         {
        //             t = t1;
        //         }

        //         // Return null if sphere is behind the ray
        //         if (t < 0) 
        //         {
        //             return null;
        //         }
        //         else 
        //         {
        //             Vector3 intersection = ray.Origin + ray.Direction * t;

        //             // Calculate the normal to the point of intersection

        //             Vector3 normalHit = (intersection - center).Normalized();

        //             // Calculates the incident ray
        //             //Vector3 incidentRay = ray.Origin + ray.Direction;
        //             Vector3 incidentRay = ray.Direction;

        //             return new RayHit(intersection, normalHit, incidentRay, material, t
        //             );
        //         }
        //     }

        //     // Ray intersects the sphere in one point (tangent)
        //     else if (discriminant == 0) {
        //         // Calculate t using quadratic formula
        //         double t = (-b) / (2 * a);

        //         // Return null if sphere is behind the ray
        //         if (t < 0) 
        //         {
        //             return null;
        //         }
        //         else 
        //         {
        //             Vector3 intersection = ray.Origin + ray.Direction * t;

        //             // Calculate the normal to the point of intersection

        //             Vector3 normalHit = (intersection - center).Normalized();

        //             // Calculates the incident ray
        //             Vector3 incidentRay = ray.Origin + ray.Direction;

        //             return new RayHit(intersection, normalHit, incidentRay, material, t
        //             );
        //         }

        //     }

        //     // Ray does not intersect the sphere
        //     else {
        //         return null;
        //     }
        //     // return null;
        // }
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
            RayHit rayHit = new RayHit(position, normal.Normalized(), ray.Direction, null, t);
            return rayHit;
        }

        /// <summary>
        /// The material of the sphere.
        /// </summary>
        public Material Material { get { return this.material; } }
    }

}
