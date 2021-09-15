using System.IO;
using System;
using StbImageWriteSharp;

namespace RayTracer
{
    /// <summary>
    /// Class to represent an image in memory and allow for I/O operations 
    /// relating to that image.
    /// </summary>
    public class Reflective : Material
    {
        public Reflective()
        :base(MaterialType.Reflective, Color.Black)
        {
        }
        public Reflective(MaterialType materialType, double refractiveIndex)
        :base(materialType, Color.Black, refractiveIndex)
        {
        }

        /// <summary>
        /// Gets a reflected ray
        /// </summary>
        /// <param name="reflectedColor">Reflect structure</param>
        protected Ray GetReflectedRay(RayHit reflectionRayHit)
        {
            Vector3 reflectedDir = (-2 * (reflectionRayHit.Incident.Dot(reflectionRayHit.Normal)) * reflectionRayHit.Normal) + reflectionRayHit.Incident;
            Ray reflectedRay = new Ray(reflectionRayHit.Position + Utils.Epsilon * reflectedDir, reflectedDir);
            return reflectedRay;
        }

        /// <summary>
        /// Computes color of pixel in the case the object is reflective
        /// </summary>
        /// <param name="reflectedColor">Reflect structure</param>
        public override Color GetLighting(RayHit closestRayHit, Scene scene, int recursionDepth)
        {
            if(recursionDepth > Utils.MaxDepth)
                return Color.Black; 

            Ray reflectedRay = GetReflectedRay(closestRayHit);
            RayHit reflectedRayHit = scene.GetClosestEntity(reflectedRay);
            // Intersection occurs
            if (reflectedRayHit != null) 
                return reflectedRayHit.ClosestEntity.Material.GetLighting(reflectedRayHit, scene, recursionDepth + 1);
            return Color.Black;
        }
    }
}
