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
            Camera camera = new Camera(outputImage, options);
            // Loops through each pixel in the resultant image
            for(int h = 0; h < outputImage.Height; h++) {
                for(int w = 0; w < outputImage.Width; w++) {
                    Color finalColour = Color.Black;
                    for(int sampleIndex = 0; sampleIndex < camera.NumberOfSamples; sampleIndex++) {
                        Ray cameraRay = camera.CreateRay(w, h, sampleIndex);
                        
                        // Obtains the closest object that intersects the ray
                        RayHit closestRayHit = GetClosestEntity(cameraRay);
                        if(closestRayHit != null)
                            finalColour += closestRayHit.ClosestEntity.Material.GetLighting(closestRayHit, this, 0);     
                    }

                    // Renders the pixel colour
                    finalColour /= camera.NumberOfSamples;
                    outputImage.SetPixel(w, h, finalColour.Clamp());   
                }
            }
        }
    }
}
