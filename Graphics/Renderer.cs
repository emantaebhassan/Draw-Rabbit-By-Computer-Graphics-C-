using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;


using System.IO;
using System.Diagnostics;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        
        uint triangleBufferID;
        uint xyzAxesBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        
        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX=0, 
                     translationY=0, 
                     translationZ=0;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 triangleCenter;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            Gl.glClearColor(1, 1, 1f, 1);
            
            float[] triangleVertices= { 
		        //left ear
		       -27.66f,27.51f,0.0f,
                0.63f,0.52f,0.42f,
               -24.96f,25.02f,0.0f,
                0.63f,0.52f,0.42f,
               -18.81f,24.12f,0.0f,
                0.63f,0.52f,0.42f,

               -18.81f,24.12f,0.0f,
               0.72f,0.57f,0.49f,
               -24.96f,25.01f,0.0f,
               0.72f,0.57f,0.49f,
               -11.25f,14.67f,0.0f,
               0.72f,0.57f,0.49f,

               -11.25f,14.94f,0.0f,
                0.65f,0.53f,0.43f,
               -10.35f,10.02f,0.0f,
               0.65f,0.53f,0.43f,
               -18.24f,20.37f,0.0f,
               0.65f,0.53f,0.43f,

               -18.48f,20.82f,0.0f,
               0.83f,0.7f,0.69f,
               -18.99f,13.86f,0.0f,
               0.83f,0.7f,0.69f,
               -10.35f,10.02f,0.0f,
               0.83f,0.7f,0.69f,

               -10.41f,10.02f,0.0f,
               0.72f,0.57f,0.49f,
               -18.99f,13.83f,0.0f,
               0.72f,0.57f,0.49f,
               -14.46f,7.98f,0.0f,
               0.72f,0.57f,0.49f,

               -18.48f,20.82f,0.0f,
               0.59f,0.46f,0.38f,
               -18.99f,13.95f,0.0f,
               0.59f,0.46f,0.38f,
               -25.05f,25.2f,0.0f,
               0.59f,0.46f,0.38f,

               //right ear
               4.2f,29.1f,0.0f,
               0.58f,0.47f,0.38f,
               1.68f,29.1f,0.0f,
               0.58f,0.47f,0.38f,
               4.86f,20.1f,0.0f,
               0.58f,0.47f,0.38f,

               4.86f,20.1f,0.0f,
               0.83f,0.7f,0.7f,
               1.68f,29.1f,0.0f,
               0.83f,0.7f,0.7f,
               -6.21f,21.36f,0.0f,
               0.83f,0.7f,0.7f,

               -6.21f,21.36f,0.0f,
               0.78f,0.62f,0.6f,
               4.86f,20.1f,0.0f,
               0.78f,0.62f,0.6f,
               -3.12f,11.73f,0.0f,
               0.78f,0.62f,0.6f,

               -3.12f,11.73f,0.0f,
               0.65f,0.52f,0.42f,
               4.86f,20.1f,0.0f,
               0.65f,0.52f,0.42f,
               0.93f,13.14f,0.0f,
               0.65f,0.52f,0.42f,

               0.93f,13.14f,0.0f,
               0.63f,0.52f,0.42f,
               -3.12f,11.73f,0.0f,
               0.63f,0.52f,0.42f,
               -1.68f,7.44f,0.0f,
               0.63f,0.52f,0.42f,

               -1.68f,7.44f,0.0f,
               0.65f,0.52f,0.42f,
               -6.21f,21.36f,0.0f,
               0.65f,0.52f,0.42f,
               -8.31f,9.84f,0.0f,
               0.65f,0.52f,0.42f,


               //face
               //1
               -14.46f,7.98f,0.0f,
               0.65f,0.52f,0.42f,
               -10.41f,10.02f,0.0f,
               0.65f,0.52f,0.42f,
               -7.83f,3.09f,0.0f,
               0.65f,0.52f,0.42f,

               //2
               -7.83f,3.09f,0.0f,
               0.58f,0.47f,0.38f,
               -10.41f,10.02f,0.0f,
               0.58f,0.47f,0.38f,
               -8.31f,9.84f,0.0f,
               0.58f,0.47f,0.38f,

               //3
               -8.31f,9.84f,0.0f,
               0.51f,0.4f,0.32f,
               -7.83f,3.09f,0.0f,
               0.51f,0.4f,0.32f,
               -1.44f,7.35f,0.0f,
               0.51f,0.4f,0.32f,

               //4
               -1.86f,7.35f,0.0f,
               0.58f,0.47f,0.38f,
               -7.83f,3.09f,0.0f,
              0.58f,0.47f,0.38f,
               -7.38f,0.3f,0.0f,
               0.58f,0.47f,0.38f,
               -0.51f,5.67f,0.0f,
               0.58f,0.47f,0.38f,

                //5
               -0.51f,5.67f,0.0f,
                0.65f,0.52f,0.42f,
               -7.38f,0.3f,0.0f,
                0.65f,0.52f,0.42f,
               0.18f,-0.39f,0.0f,
                0.65f,0.52f,0.42f,

               //6
               0.18f,-0.39f,0.0f,
                0.58f,0.47f,0.38f,
               -7.38f,0.3f,0.0f,
                0.58f,0.47f,0.38f,
               -5.04f,-6.81f,0.0f,
                0.58f,0.47f,0.38f,

               //7
               -5.04f,-6.81f,0.0f,
              0.65f,0.52f,0.42f,
               -7.38f,0.3f,0.0f,
              0.65f,0.52f,0.42f,
              -13.53f,-10.47f,0.0f,
              0.65f,0.52f,0.42f,

              //8
              -13.53f,-10.47f,0.0f,
               0.58f,0.47f,0.38f,
              -7.38f,0.3f,0.0f,
               0.58f,0.47f,0.38f,
              -9.66f,-0.93f,0.0f,
               0.58f,0.47f,0.38f,
              -15.3f,-6.81f,0.0f,
               0.58f,0.47f,0.38f,

              //9
              -15.3f,-6.81f,0.0f,
              0.72f,0.58f,0.49f,
              -19.65f,-6.12f,0.0f,
              0.72f,0.58f,0.49f,
              -18.57f,-2.37f,0.0f,
              0.72f,0.58f,0.49f,
              -9.66f,-0.93f,0.0f,
              0.72f,0.58f,0.49f,

              //10
              -9.66f,-0.93f,0.0f,
              0.65f,0.52f,0.42f,
              -18.57f,-2.37f,0.0f,
              0.65f,0.52f,0.42f,
              -18.06f,2.1f,0.0f,
              0.65f,0.52f,0.42f,
              -9.99f,1.2f,0.0f,
              0.65f,0.52f,0.42f,

              //11
              -9.99f,1.2f,0.0f,
              0.72f,0.58f,0.49f,
              -18.06f,2.1f,0.0f,
              0.72f,0.58f,0.49f,
               -14.46f,7.98f,0.0f,
               0.72f,0.58f,0.49f,
               -7.83f,3.09f,0.0f,
               0.72f,0.58f,0.49f,

               //eye
               -7.83f,3.09f,0.0f,
               0,0,0,
               -7.38f,0.3f,0.0f,
               0,0,0,
              -9.66f,-0.93f,0.0f,
               0,0,0,
              -9.99f,1.2f,0.0f,
              0,0,0,

              //mouse
              -13.53f,-10.47f,0.0f,
              0.74f,0.75f,0.76f,
              -15.3f,-6.81f,0.0f,
              0.74f,0.75f,0.76f,
              -16.71f,-8.88f,0.0f,
              0.74f,0.75f,0.76f,

              -16.71f,-8.88f,0.0f,
              0.83f,0.7f,0.7f,
              -15.3f,-6.81f,0.0f,
              0.83f,0.7f,0.7f,
              -19.65f,-6.12f,0.0f,
              0.83f,0.7f,0.7f,

              -19.65f,-6.12f,0.0f,
             0.69f,0.69f,0.69f,
              -16.71f,-8.88f,0.0f,
              0.69f,0.69f,0.69f,
              -18.06f,-9.57f,0.0f,
             0.69f,0.69f,0.69f,

              -18.06f,-9.57f,0.0f,
              0.63f,0.62f,0.62f,
              -16.71f,-8.88f,0.0f,
              0.63f,0.62f,0.62f,
              -13.53f,-10.47f,0.0f,
              0.63f,0.62f,0.62f,


              //1

               -0.51f,5.67f,0.0f,
               0.51f,0.4f,0.32f,
               0.18f,-0.39f,0.0f,
               0.51f,0.4f,0.32f,
               10.08f,9.24f,0.0f,
               0.51f,0.4f,0.32f,
               
               //2
                10.08f,9.24f,0.0f,
                0.58f,0.47f,0.38f,
                0.18f,-0.39f,0.0f,
                0.58f,0.47f,0.38f,
                22.17f,6.3f,0.0f,
                0.58f,0.47f,0.38f,

               //3
                22.17f,6.3f,0.0f,
                0.65f,0.52f,0.42f,
                0.18f,-0.39f,0.0f,
                0.65f,0.52f,0.42f,
                7.83f,-10.56f,0.0f,
                0.65f,0.52f,0.42f,

                //4
                7.83f,-10.56f,0.0f,
                0.58f,0.47f,0.38f,
                22.17f,6.3f,0.0f,
                0.58f,0.47f,0.38f,
                15.21f,-12.87f,0.0f,
                0.58f,0.47f,0.38f,

                //5
                15.21f,-12.87f,0.0f,
                0.72f,0.58f,0.49f,
                22.17f,6.3f,0.0f,
                0.72f,0.58f,0.49f,
                27.99f,-5.76f,0.0f,
                0.72f,0.58f,0.49f,

                //6
                27.99f,-5.76f,0.0f,
                0.65f,0.52f,0.42f,
                15.21f,-12.87f,0.0f,
                0.65f,0.52f,0.42f,
                17.64f,-20.1f,0.0f,
                0.65f,0.52f,0.42f,

                //7
                17.64f,-20.1f,0.0f,
                0.72f,0.58f,0.49f,
                27.99f,-5.76f,0.0f,
                0.72f,0.58f,0.49f,
                26.04f,-18.78f,0.0f,
                0.72f,0.58f,0.49f,

                //8
                26.04f,-18.78f,0.0f,
                0.58f,0.47f,0.38f,
                17.64f,-20.1f,0.0f,
                0.58f,0.47f,0.38f,
                26.22f,-23.4f,0.0f,
                0.58f,0.47f,0.38f,

                //9
                26.22f,-23.4f,0.0f,
                0.72f,0.58f,0.49f,
                17.64f,-20.1f,0.0f,
                0.72f,0.58f,0.49f,
                19.92f,-26.34f,0.0f,
                0.72f,0.58f,0.49f,

                //10
                19.92f,-26.34f,0.0f,
                0.58f,0.47f,0.38f,
                17.64f,-20.1f,0.0f,
                0.58f,0.47f,0.38f,
                14.88f,-27.51f,0.0f,
                0.58f,0.47f,0.38f,

                //11
                14.88f,-27.51f,0.0f,
                0.65f,0.52f,0.42f,
                17.64f,-20.1f,0.0f,
                0.65f,0.52f,0.42f,
                12.93f,-22.59f,0.0f,
                0.65f,0.52f,0.42f,

                //12
                12.93f,-22.59f,0.0f,
                0.72f,0.6f,0.52f,
                11.43f,-25.71f,0.0f,
                0.72f,0.6f,0.52f,
                14.88f,-27.51f,0.0f,
                0.72f,0.6f,0.52f,

                //13
                17.64f,-20.1f,0.0f,
                0.51f,0.4f,0.32f,
                15.21f,-12.87f,0.0f,
                0.51f,0.4f,0.32f,
                7.83f,-10.56f,0.0f,
                0.51f,0.4f,0.32f,

                //14
                7.83f,-10.56f,0.0f,
                0.58f,0.47f,0.38f,
                17.64f,-20.1f,0.0f,
                0.58f,0.47f,0.38f,
                12.93f,-22.59f,0.0f,
                0.58f,0.47f,0.38f,

                 //15
                12.93f,-22.59f,0.0f,
                0.51f,0.4f,0.32f,
                7.83f,-10.56f,0.0f,
                0.51f,0.4f,0.32f,
                3.6f,-21.18f,0.0f,
                0.51f,0.4f,0.32f,

                //16
               0.18f,-0.39f,0.0f,
               0.51f,0.4f,0.32f,
                7.83f,-10.56f,0.0f,
                0.51f,0.4f,0.32f,
               -5.04f,-6.81f,0.0f,
               0.51f,0.4f,0.32f,

               //17
                -5.04f,-6.81f,0.0f,
                0.65f,0.52f,0.42f,
                8.83f,-10.56f,0.0f,
                0.65f,0.52f,0.42f,
                -3.87f,-20.7f,0.0f,
                0.65f,0.52f,0.42f,

                //18
                -3.87f,-20.73f,0.0f,             
                0.72f,0.58f,0.49f,
                7.83f,-10.56f,0.0f,
                0.72f,0.58f,0.49f,
                0.66f,-28.83f,0.0f,
                0.72f,0.58f,0.49f,

                //19
                -3.87f,-20.73f,0.0f,
                0.65f,0.52f,0.42f,
                0.66f,-28.83f,0.0f,
                0.65f,0.52f,0.42f,
                -4.11f,-24.57f,0.0f,
                0.65f,0.52f,0.42f,

                //20
                -4.11f,-24.57f,0.0f,
                0.72f,0.58f,0.49f,
                0.66f,-28.83f,0.0f,
                0.72f,0.58f,0.49f,
                -4.95f,-28.74f,0.0f,
                0.72f,0.58f,0.49f,

                //21
                -4.95f,-28.74f,0.0f,
                0.65f,0.53f,0.43f,
                -4.11f,-24.57f,0.0f,
                0.65f,0.53f,0.43f,
                -6.48f,-27.06f,0.0f,
                0.65f,0.53f,0.43f,

                //22
              -5.04f,-6.81f,0.0f,
              0.51f,0.4f,0.32f,
              -13.53f,-10.47f,0.0f,
              0.51f,0.4f,0.32f,
              -10.5f,-17.25f,0.0f,
              0.51f,0.4f,0.32f,

              //23
              -10.5f,-17.25f,0.0f,
              0.58f,0.47f,0.38f,
              -5.04f,-6.81f,0.0f,
              0.58f,0.47f,0.38f,
              -3.87f,-20.73f,0.0f,
              0.58f,0.47f,0.38f,

              //24
              -3.87f,-20.73f,0.0f,
              0.65f,0.52f,0.42f,
              -10.5f,-17.25f,0.0f,
              0.65f,0.52f,0.42f,
              -7.32f,-24.66f,0.0f,
              0.65f,0.52f,0.42f,

              //25
              -10.5f,-17.25f,0.0f,
              0.51f,0.4f,0.32f,
              -7.32f,-24.66f,0.0f,
              0.51f,0.4f,0.32f,
              -9.99f,-20.28f,0.0f,
              0.51f,0.4f,0.32f,

              //26
              -9.99f,-20.28f,0.0f,
              0.58f,0.46f,0.37f,
              -7.32f,-24.66f,0.0f,
              0.58f,0.46f,0.37f,
              -11.61f,-20.73f,0.0f,
              0.58f,0.46f,0.37f,

              //27
              -11.61f,-20.73f,0.0f,
              0.65f,0.52f,0.42f,
              -7.32f,-24.66f,0.0f,
              0.65f,0.52f,0.42f,
              -13.44f,-25.02f,0.0f,
              0.65f,0.52f,0.42f,

              //28
              -13.44f,-25.02f,0.0f,
              0.72f,0.58f,0.49f,
              -11.61f,-20.73f,0.0f,
              0.72f,0.58f,0.49f,
              -15.03f,-23.22f,0.0f,
              0.72f,0.58f,0.49f,

            }; // Triangle Center = (10, 7, -5)
            
           triangleCenter = new vec3(10, 7, -5);

            float[] xyzAxesVertices = {
		        //x
		        0.0f, 0.0f, 0.0f,
                2.0f, 0.0f, 0.0f, 
		        100.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        //y
	            1.0f, 0.0f, 0.0f,
                2.0f,1.0f, 0.0f, 
		        0.0f, 100.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 
		        //z
	            0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 1.0f,  
		        0.0f, 0.0f, -100.0f,
                0.0f, 0.0f, 1.0f,  
            };

            triangleBufferID = GPU.GenerateBuffer(triangleVertices);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);

            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(50, 50, 50), // Camera is at (0,5,5), in World Space
                        new vec3(0, 0, 0), // and looks at the origin
                        new vec3(0, 1, 0)  // Head is up (set to 0,-1,0 to look upside-down)
                );
            // Model Matrix Initialization
            ModelMatrix = new mat4(1);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
            
            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            #region XYZ axis

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
             
            //Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            
            #endregion

            #region Animated Triangle
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, triangleBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 3, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 6, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 9, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 12, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 15, 3);

            Gl.glDrawArrays(Gl.GL_POLYGON, 18, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 21, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 24, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 27, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 30, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 33, 3);


            Gl.glDrawArrays(Gl.GL_POLYGON, 36, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 39, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 42, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 45, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 49, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 52, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 55, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 58, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 62, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 66, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 70, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 74, 4);

            Gl.glDrawArrays(Gl.GL_POLYGON, 78, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 81, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 84, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 87, 3);

            Gl.glDrawArrays(Gl.GL_POLYGON, 90, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 93, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 96, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 99, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 102, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 105, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 108, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 111, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 114, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 117, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 120, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 123, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 126, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 129, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 132, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 135, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 138, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 141, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 144, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 147, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 150, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 153, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 156, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 159, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 162, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 165, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 168, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 171, 3);
            Gl.glDrawArrays(Gl.GL_POLYGON, 174, 3);


            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            #endregion
        }
        

        public void Update()
        {

            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds/950.0f;

            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * triangleCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 0, 1)));
            transformations.Add(glm.translate(new mat4(1),  triangleCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix =  MathHelper.MultiplyMatrices(transformations);
            
            timer.Reset();
            timer.Start();
        }
        
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
