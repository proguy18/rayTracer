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

        /// <summary>
        /// Fire a ray for each pixel on the image
        /// </summary>
        /// <param name="Ray">Ray structure</param>
        // public void FireRay(Image outputImage)
        // {
        //     for(int h = 0; h < outputImage.Height; h++) {
        //         for(int w = 0; w < outputImage.Width; w++) {
        //             camera_origin = new Vector3(0,0,0);
        //             direction = new Vector3()
        //             Ray a = new Ray(origin, direction)

        //         }
        //     }
        // }

        /// <summary>
        /// Render the scene to an output image. This is where the bulk
        /// of your ray tracing logic should go... though you may wish to
        /// break it down into multiple functions as it gets more complex!
        /// </summary>
        /// <param name="outputImage">Image to store render output</param>
        public void Render(Image outputImage)
        {
            // // Renders a sphere
            // Sphere sphere = new Sphere(
            //     new Vector3(0, 0, 5), 
            //     1, 
            //     new Material(
            //             Material.MaterialType.Glossy, 
            //             new Color(255, 255, 255)
            //     )
            // );

            // Plane plane = new Plane(
            //     new Vector3(50, 50, 100),
            //     new Vector3(50, 5, 10),
            //     new Material(
            //         Material.MaterialType.Glossy,
            //         new Color(255, 255, 255)
            //     )
            // );

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
                    rayDirection = rayDirection.Normalized(); // it's a direction so don't forget to normalize 
                    Ray ray = new Ray(cameraPos, rayDirection);

                    foreach (SceneEntity entity in this.entities) {
                        // if(entity.GetType().ToString() == "RayTracer.Plane") {
                        //     continue;
                        // }
                        RayHit hit = entity.Intersect(ray);
                        if (hit != null)
                        {
                            // We got a hit with this entity!
                            // The colour of the entity is entity.Material.Color
                            outputImage.SetPixel(w, h, entity.Material.Color);
                        }
                    }


                    // RayHit rayHit1 = plane.Intersect(ray);

                    // if(rayHit1 != null) {
                    //     outputImage.SetPixel(w, h, new Color(255, 0, 0));
                    // }
                    // else {
                    //     outputImage.SetPixel(w, h, new Color(0, 0, 0));
                    // }



                    // RayHit rayHit = sphere.Intersect(ray);
                    // // outputImage.SetPixel(w, h, new Color(255, 255, 0));


                    // // Console.WriteLine(rayHit);
                    // if(rayHit != null) {
                    //     outputImage.SetPixel(w, h, new Color(255, 255, 255));

                    // }
                    // else {
                    //     outputImage.SetPixel(w, h, new Color(0, 0, 0));
                    // }



                }
            }
            // double Testx = (2 * ((0 + 0.5) / outputImage.Width) - 1) * (Math.Tan(fov / 2 * Math.PI / 180)); 
            // double Testy = (1 - 2 * ((0 + 0.5) / outputImage.Height)) * (Math.Tan(fov / 2 * Math.PI / 180));
            // Console.WriteLine("Testx = " + Testx + " Testy = " + Testy);
            // Console.WriteLine(Math.Tan(fov / 2 * Math.PI / 180));
            // Begin writing your code here...
            // Vector3 a = new Vector3(1, 1, 1);
            // Vector3 b = new Vector3(1, 1, 100);
            // Console.WriteLine("Length Squared of the vector is " + a.LengthSq());
            // Console.WriteLine("Length of the vector is " + a.Length());
            // Console.WriteLine("Normalized of the vector is " + a.Normalized());
            // Console.WriteLine("Dot product of the vectors is "+ a.Dot(b));
            // Console.WriteLine("Cross product of the vector is "+ a.Cross(b));
            // Console.WriteLine("Vector addition of the vector is " + (a+b));
            // Console.WriteLine("Vector negation of the vector is " + -(a));
            // Console.WriteLine("Vector subtraction of the vector is " + (a-b));
            // Console.WriteLine("Scalar multiplication of the vector is " + (a*4));
            // Console.WriteLine("Scalar division of the vector is " + (a/4));
        }

    }
}
