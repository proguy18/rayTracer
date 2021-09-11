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
            Vector3 side1 = v2 - v1;

            // Normal of the plane
            Vector3 normal = side0.Cross(side1);
            
            double t = 0;
            double denominator = ray.Direction.Dot(normal);
            Vector3 p010 = new Vector3(0, 0, 0);
            // If ray is not parallel to the plane, intersection occurs.
            if (denominator > 1e-6) {
                // Vector from origin to a point
                Vector3 p0l0 = v0 - ray.Origin;
                
                t = p010.Dot(normal) / denominator;

                // Intersection
                if (t >= 0) 
                {
                    
                    Vector3 intersection = ray.Origin + ray.Direction * t;

                    // Checks whether the point of intersection is inside the triangle
                    Vector3 C = new Vector3(0, 0, 0);

                    // Edge 0 
                    Vector3 edge0 = v1 - v0; 
                    Vector3 vp0 = intersection - v0; 
                    C = edge0.Cross(vp0);
                    if (normal.Dot(C) < 0) 
                    {
                        return null;
                    } 

                    // Edge 1
                    Vector3 edge1 = v2 - v1; 
                    Vector3 vp1 = intersection - v1; 
                    C = edge1.Cross(vp1);
                    if (normal.Dot(C) < 0) 
                    {
                        return null;
                    } 

                    // Edge 2
                    Vector3 edge2 = v0 - v2; 
                    Vector3 vp2 = intersection - v2; 
                    C = edge2.Cross(vp2); 
                    if (normal.Dot(C) < 0) 
                    {
                        return null;
                    } 

                    // Calculate the normal to the point of intersection
                    Vector3 normalHit = (intersection + normal).Normalized();

                    // Calculates the incident ray
                    Vector3 incidentRay = ray.Origin + ray.Direction;

                    return new RayHit(intersection, normalHit, incidentRay, material);
                }
                // Intersection behind the ray
                else 
                {
                    return null;
                }
            }
            return null;

            


            // return null;
        }

        /// <summary>
        /// The material of the triangle.
        /// </summary>
        public Material Material { get { return this.material; } }
    }

}
