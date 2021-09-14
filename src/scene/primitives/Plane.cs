using System;

namespace RayTracer
{
    /// <summary>
    /// Class to represent an (infinite) plane in a scene.
    /// </summary>
    public class Plane : SceneEntity
    {
        private Vector3 center;
        private Vector3 normal;
        private Material material;

        /// <summary>
        /// Construct an infinite plane object.
        /// </summary>
        /// <param name="center">Position of the center of the plane</param>
        /// <param name="normal">Direction that the plane faces</param>
        /// <param name="material">Material assigned to the plane</param>
        public Plane(Vector3 center, Vector3 normal, Material material)
        {
            this.center = center;
            this.normal = normal.Normalized();
            this.material = material;
        }

        /// <summary>
        /// Determine if a ray intersects with the plane, and if so, return hit data.
        /// </summary>
        /// <param name="ray">Ray to check</param>
        /// <returns>Hit data (or null if no intersection)</returns>
        public RayHit Intersect(Ray ray)
        {
            // Write your code here...
            
            double t = 0;
            double denominator = ray.Direction.Dot(normal);
            Vector3 p010 = new Vector3(0, 0, 0);

            // If ray is not parallel to the plane, intersection occurs.
            if (Math.Abs(denominator) < double.Epsilon) {
                return null;
            }

            // compute d parameter using equation 2
            // double d = normal.Dot(center); 
            p010 = center - ray.Origin;
        
            // compute t (equation 3)
            t = (normal.Dot(p010)) / denominator; 
            // check if the triangle is in behind the ray
            if (t < 0) {
                return null;
            } // the triangle is behind 
        
            Vector3 intersection = ray.Origin + ray.Direction * t;

            // Calculate the normal to the point of intersection
            Vector3 normalHit = (intersection + normal).Normalized();

            // Calculates the incident ray
            Vector3 incidentRay = ray.Origin + ray.Direction;

            return new RayHit(intersection, normalHit, incidentRay, material, t);
        }

        /// <summary>
        /// The material of the plane.
        /// </summary>
        public Material Material { get { return this.material; } }
    }

}
