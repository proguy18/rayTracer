using System;
using System.Collections.Generic;

namespace RayTracer
{
    /// <summary>
    /// Class to represent a ray traced scene, including the objects,
    /// light sources, and associated rendering logic.
    /// </summary>
    public class Scene
    {
        private SceneOptions options;
        private ISet<SceneEntity> entities;
        public ISet<PointLight> Lights { get; }

        /// <summary>
        /// Construct a new scene with provided options.
        /// </summary>
        /// <param name="options">Options data</param>
        public Scene(SceneOptions options = new SceneOptions())
        {
            this.options = options;
            this.entities = new HashSet<SceneEntity>();
            this.Lights = new HashSet<PointLight>();
        }

        /// <summary>
        /// Add an entity to the scene that should be rendered.
        /// </summary>
        /// <param name="entity">Entity object</param>
        public void AddEntity(SceneEntity entity)
        {
            this.entities.Add(entity);
        }

        /// <summary>
        /// Add a point light to the scene that should be computed.
        /// </summary>
        /// <param name="light">Light structure</param>
        public void AddPointLight(PointLight light)
        {
            this.Lights.Add(light);
        }

        /// <summary>
        /// Hits the closest entity, returns the closest entity that has been hit by a ray
        /// </summary>
        /// <param name="closestEntity"></param>
        public RayHit GetClosestEntity(Ray ray)
        {
            double closestDistance = double.MaxValue;
            SceneEntity closestEntity = null;
            RayHit closestHit = null;
            foreach(SceneEntity entity in this.entities) {
                RayHit hit = entity.Intersect(ray);
                if (hit != null) {
                    double distance = (hit.Position - ray.Origin).LengthSq();
                    if(closestDistance > distance) {
                        closestDistance = distance;
                        closestEntity = entity;
                        closestHit = hit;
                        closestHit.ClosestEntity = closestEntity;
                    }
                }
            }

            return closestHit;
        }


        /// </summary>
        /// <param name="outputImage">Image to store render output</param>
        public void Render(Image outputImage)
        {

            Vector3 cameraPos = new Vector3(0, 0, 0);
            double fov = 60;

            
            // Loops through each pixel in the resultant image
            for(int h = 0; h < outputImage.Height; h++) {
                for(int w = 0; w < outputImage.Width; w++) {

                    // Fire rays from each pixel, by converting from pixel notation to world space
                    double imageAspectRatio = outputImage.Width / (double)outputImage.Height; // assuming width > height 
                    double Px = (2 * ((w + 0.5) / outputImage.Width) - 1) * Math.Tan(fov / 2 * Math.PI / 180) * imageAspectRatio; 
                    double Py = (1 - 2 * ((h + 0.5) / outputImage.Height)) * Math.Tan(fov / 2 * Math.PI / 180); 
                    Vector3 rayDirection = new Vector3(Px, Py, 1) - cameraPos; // note that this just equal to Vector3(Px, Py, 1); 
                    rayDirection = rayDirection.Normalized(); 
                    Ray cameraRay = new Ray(cameraPos, rayDirection);

                    // Obtains the closest object that intersects the ray
                    RayHit closestRayHit = GetClosestEntity(cameraRay);
                    Color finalColour = Color.Black;

                    if(closestRayHit != null)
                        finalColour = closestRayHit.ClosestEntity.Material.GetLighting(closestRayHit, this, 0);
                        
                    // Renders the pixel colour
                    outputImage.SetPixel(w, h, finalColour.Clamp()); 
                }
            }
        }


    }
}
