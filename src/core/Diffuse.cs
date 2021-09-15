using System.IO;
using System;
using StbImageWriteSharp;

namespace RayTracer
{
    /// <summary>
    /// Class to represent an image in memory and allow for I/O operations 
    /// relating to that image.
    /// </summary>
    public class Diffuse : Material
    {
        public Diffuse(Color color)
        :base(MaterialType.Diffuse, color)
        {
        }

        public override Color GetLighting(RayHit closestRayHit, Scene scene, int recursionDepth)
        {
            Color finalColour = Color.Black;
            foreach (PointLight pointLight in scene.Lights) {
                Color colourVector = new Color(0, 0 ,0);
                Vector3 l = (pointLight.Position - closestRayHit.Position).Normalized();
                Vector3 n = closestRayHit.Normal;
                Vector3 offset = Utils.Epsilon * l;
                Ray shadowRay = new Ray(closestRayHit.Position + offset, l);

                RayHit blockingEntityRayHit = scene.GetClosestEntity(shadowRay);

                // Blocking entity is intersecting (blocking) current entity
                if (blockingEntityRayHit != null) {
                    double distanceToShadowEntity = (blockingEntityRayHit.Position - closestRayHit.Position).LengthSq();
                    double distanceToLight = (pointLight.Position - closestRayHit.Position).LengthSq();

                    // Blocking entity is between light and current entity, so there is a shadow
                    if(distanceToLight > distanceToShadowEntity) {
                        continue;
                    }
                }
                colourVector = (closestRayHit.ClosestEntity.Material.Color * pointLight.Color) * Math.Abs(n.Dot(l));
                finalColour += colourVector;
            }

            return finalColour;
        }
    }
}
