using System.IO;
using System;
using StbImageWriteSharp;

namespace RayTracer
{
    /// <summary>
    /// Class to represent an image in memory and allow for I/O operations 
    /// relating to that image.
    /// </summary>
    public class Refractive : Reflective
    {
        public Refractive(double refractiveIndex)
        :base(MaterialType.Refractive, refractiveIndex)
        {
        }

        /// <summary>
        /// Gets a refracted ray
        /// </summary>
        /// <param name="refractedColor">Refracted structure</param>
        private Ray GetRefractedRay(RayHit refractedRayHit) 
        {
            double cosi = Math.Clamp(refractedRayHit.Incident.Dot(refractedRayHit.Normal), -1, 1);
            double etai = 1;
            double etat = refractiveIndex;

            Vector3 n = refractedRayHit.Normal;
            if (cosi < 0)
                cosi = -cosi;
            else
            {
                double tmp = etai;
                etai = etat;
                etat = tmp;
                
                n = -refractedRayHit.Normal;
            }

            double eta = etai / etat;
            double k = 1 - eta * eta * (1 - cosi * cosi);
            Vector3 dirRefraction = eta * refractedRayHit.Incident + (eta * cosi - Math.Sqrt(k)) * n;
            dirRefraction = dirRefraction.Normalized();
            Vector3 refractOrigin = refractedRayHit.Position + Utils.Epsilon * (-n);
            Ray refractionRay = new Ray(refractOrigin, dirRefraction);
            return refractionRay;
        }

        private double GetFresnel(RayHit closestHit)
        {
            double cosi = Math.Clamp(closestHit.Incident.Dot(closestHit.Normal),-1, 1);
            double etai = 1;
            double etat = refractiveIndex; 
            if (cosi > 0) 
            {
                double tmp = etai;
                etai = etat;
                etat = tmp;
            } 
            double sint = etai / etat * Math.Sqrt(Math.Max(0.0, 1 - cosi * cosi));
            if (sint >= 1) 
                return 1;
            double cost = Math.Sqrt(Math.Max(0.0, 1 - sint * sint)); 
            cosi = Math.Abs(cosi); 
            double rs = (etat * cosi - etai * cost) / (etat * cosi + etai * cost); 
            double rp = (etai * cosi - etat * cost) / (etai * cosi + etat * cost); 
            return (rs * rs + rp * rp) / 2;
        }


        /// <summary>
        /// Computes color of pixel in the case the object is refractive
        /// </summary>
        /// <param name="refractedColor">Refract structure</param>
        public override Color GetLighting(RayHit closestRayHit, Scene scene, int recursionDepth)
        {
            if(recursionDepth > Utils.MaxDepth)
                return Color.Black; 

            Color reflectiveColor = base.GetLighting(closestRayHit, scene, recursionDepth);
            Ray refractedRay = GetRefractedRay(closestRayHit);
            RayHit refractedHit = scene.GetClosestEntity(refractedRay);
            Color refractiveColor = Color.Black;
            if (refractedHit != null)
                refractiveColor = refractedHit.ClosestEntity.Material.GetLighting(refractedHit, scene, recursionDepth + 1);
            double fresnel = GetFresnel(closestRayHit);
            Color fresnelColor = reflectiveColor * fresnel + refractiveColor * (1 - fresnel);
            return fresnelColor.Clamp();
        }
    }
}
