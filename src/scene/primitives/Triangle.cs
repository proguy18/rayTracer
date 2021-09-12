using System;

namespace RayTracer
{
    /// <summary>
    /// Class to represent a triangle in a scene represented by three vertices.
    /// </summary>
    public class Triangle : SceneEntity
    {
        private Vector3 v0, v1, v2;
        private Material material;

        /// <summary>
        /// Construct a triangle object given three vertices.
        /// </summary>
        /// <param name="v0">First vertex position</param>
        /// <param name="v1">Second vertex position</param>
        /// <param name="v2">Third vertex position</param>
        /// <param name="material">Material assigned to the triangle</param>
        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2, Material material)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            this.material = material;
        }

        /// <summary>
        /// Determine if a ray intersects with the triangle, and if so, return hit data.
        /// </summary>
        /// <param name="ray">Ray to check</param>
        /// <returns>Hit data (or null if no intersection)</returns>
        public RayHit Intersect(Ray ray)
        {
            // Write your code here...

            // Obtain variables for the plane which the triangle resides in
            Vector3 side0 = v1 - v0;
            Vector3 side1 = v2 - v0;

            // Normal of the plane
            Vector3 normal = side0.Cross(side1);

            double det = -ray.Direction.Dot(normal);
            double invdet = 1.0/det;
            Vector3 AO  = ray.Origin - v0;
            Vector3 DAO = AO.Cross(ray.Direction);
            double u =  side1.Dot(DAO) * invdet;
            double v = -side0.Dot(DAO) * invdet;
            double t =  AO.Dot(normal)  * invdet; 
            if(det >= double.Epsilon && t >= 0.0 && u >= 0.0 && v >= 0.0 && (u+v) <= 1.0) {
                Vector3 intersection = ray.Origin + ray.Direction * t;

                // Calculate the normal to the point of intersection
                Vector3 normalHit = (intersection + normal).Normalized();

                // Calculates the incident ray
                Vector3 incidentRay = ray.Origin + ray.Direction;

                return new RayHit(intersection, normalHit, incidentRay, material);
            }
            return null;
        }

        /// <summary>
        /// The material of the triangle.
        /// </summary>
        public Material Material { get { return this.material; } }
    }

}
