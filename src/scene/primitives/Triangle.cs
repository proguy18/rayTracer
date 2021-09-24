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
            Vector3 v0v1 = v1 - v0;
            Vector3 v0v2 = v2 - v0;
            Vector3 n = v0v1.Cross(v0v2);
            double nDotRayDirection = n.Dot(ray.Direction);
            if (Math.Abs(nDotRayDirection) < Utils.Epsilon) // almost 0 
                return null; // they are parallel so they don't intersect ! 
            Vector3 originv0 = v0 - ray.Origin;
            double t = (n.Dot(originv0)) / nDotRayDirection;
            if (t <= 0) return null; // the triangle is behind 
            Vector3 p = ray.Origin + t * ray.Direction;
            
            // edge 0
            Vector3 edge0 = v1 - v0;
            Vector3 vp0 = p - v0;
            Vector3 c = edge0.Cross(vp0);
            if (n.Dot(c) < 0) return null;

            // edge 1
            Vector3 edge1 = v2 - v1;
            Vector3 vp1 = p - v1;
            c = edge1.Cross(vp1);
            double u = n.Dot(c);
            if ((u < 0)) return null;

            // edge 2
            Vector3 edge2 = v0 - v2;
            Vector3 vp2 = p - v2;
            c = edge2.Cross(vp2);
            double v = n.Dot(c);
            if (v < 0) return null;
            
            Vector3 rayHitPosition = ray.Origin + t * ray.Direction;
            RayHit rayHit = new RayHit(rayHitPosition, n.Normalized(), ray.Direction, null);
            return rayHit;
        }

        /// <summary>
        /// The material of the triangle.
        /// </summary>
        public Material Material { get { return this.material; } }
    }

}
