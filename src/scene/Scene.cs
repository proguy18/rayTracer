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
        private ISet<PointLight> lights;

        /// <summary>
        /// Construct a new scene with provided options.
        /// </summary>
        /// <param name="options">Options data</param>
        public Scene(SceneOptions options = new SceneOptions())
        {
            this.options = options;
            this.entities = new HashSet<SceneEntity>();
            this.lights = new HashSet<PointLight>();
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
            this.lights.Add(light);
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
                    bool isShadow = false;
                    // Fire rays from each pixel, by converting from pixel notation to world space
                    double imageAspectRatio = outputImage.Width / (double)outputImage.Height; // assuming width > height 
                    double Px = (2 * ((w + 0.5) / outputImage.Width) - 1) * Math.Tan(fov / 2 * Math.PI / 180) * imageAspectRatio; 
                    double Py = (1 - 2 * ((h + 0.5) / outputImage.Height)) * Math.Tan(fov / 2 * Math.PI / 180); 
                    Vector3 rayDirection = new Vector3(Px, Py, 1) - cameraPos; // note that this just equal to Vector3(Px, Py, 1); 
                    rayDirection = rayDirection.Normalized(); // it's a direction so don't forget to normalize 
                    Ray primaryRay = new Ray(cameraPos, rayDirection);

                    foreach (SceneEntity entity in this.entities) {
                        SceneEntity closestEntity = null;
                        RayHit hit = entity.Intersect(primaryRay);
                        // There is an intersection
                        if (hit != null)
                        {
                            Vector3 closestPos = entity.Intersect(primaryRay).Position; 
                            closestEntity = entity;
                            // Finds which object is closer to the camera visual studio 2017 or 2019
                            foreach (SceneEntity entityTie in this.entities) {
                                RayHit tie = entityTie.Intersect(primaryRay);

                                // Obtains the closest entity that intersects with the ray
                                if (tie != null) {
                                    if(closestPos.Z > tie.Position.Z) 
                                    {
                                        closestPos = entityTie.Intersect(primaryRay).Position;
                                        closestEntity = entityTie;
                                        // break;
                                    }
                                }
         
                            }
                            
                            // Renders the closest object
                            RayHit intersected = closestEntity.Intersect(primaryRay);
                            Color finalColour = new Color(0, 0, 0);
                            if(closestEntity.Material.Type.ToString() == "Diffuse") {
                                {
                                    foreach (PointLight pointLight in this.lights) {
                                        Color colourVector = new Color(0, 0 ,0);
                                        Vector3 l = (pointLight.Position - intersected.Position).Normalized();
                                        Vector3 n = intersected.Normal;
                                        Vector3 offset = 0.05*l;
                                        Ray shadow = new Ray(intersected.Position + offset, l);

                                        colourVector = (closestEntity.Material.Color * pointLight.Color) * Math.Abs(n.Dot(l));
                                        // Checks if the shadow ray intersects other objects
                                        foreach (SceneEntity blockingEntity in this.entities) {
                                            if(blockingEntity != closestEntity) {
                                                if(blockingEntity.Intersect(shadow) != null) {
                                                    if(blockingEntity.Intersect(shadow).Time < (pointLight.Position - intersected.Position).Length()) {
                                                        isShadow = true;
                                                        // Console.WriteLine("shadow point " + h.ToString() + ", " + w.ToString());
                                                        break;
                                                    }
                                                }
                                            }
                                            
                                        }
                                        if(isShadow) {
                                            finalColour = new Color(0,0,0);
                                            break;
                                        }
                                        else {
                                            finalColour += colourVector;
                                            
                                        }
                                    }   
                                }
                            }
                            outputImage.SetPixel(w, h, finalColour);
                            // break;
                        }
                    }
                }
            }
        }


    }
}
