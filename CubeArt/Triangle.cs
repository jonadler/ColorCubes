﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing.Imaging;
using System.Drawing;
using System;
using System.IO;

namespace TriangleRender
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Triangle : Game
    {
        GraphicsDeviceManager graphics;
        bool saveToImage;

        //Camera
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;
        RenderTarget2D screenshot;

        //BasicEffect for rendering
        BasicEffect basicEffect;

        //Geometric info
        Helpers.VertexPositionColorNormal[] triangleVertices;
        VertexBuffer vertexBuffer;

        

        public Triangle(bool save)
        {
            saveToImage = save;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 720;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            float width;
            float height;
            float depth;
            float adjust;
            width = 40f;
            adjust = 0.125f;
            height = width*(1-adjust) + (adjust)*(width * ((float) System.Math.Sqrt(3.0f)) / 2f);
            depth = 80f;
            //Setup Camera

            worldMatrix = Matrix.Identity;
            viewMatrix = Matrix.CreateLookAt(new Vector3(-width/2,-width/2, -width/ 2), new Vector3(0,0, 0), new Vector3(0, 0, -1));



            projectionMatrix = Matrix.CreateOrthographic(width, height, 0, depth);

            //BasicEffect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;

            // Want to see the colors of the vertices, this needs to be on
            basicEffect.VertexColorEnabled = true;

            //Lighting requires normal information which VertexPositionColor does not have
            //If you want to use lighting and VPC you need to create a custom def
            basicEffect.LightingEnabled = true;

            //vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(
            //   Helpers.VertexPositionColorNormal), 36, BufferUsage.
            //   WriteOnly);
            var cube = Generate.Shapes.cubeToTriangles(Generate.Shapes.colorCube(3, Generate.Shapes.createColor(0.5, 0.5, 0.5)));
            triangleVertices = new Helpers.VertexPositionColorNormal[cube.Length];
            for(int i = 0; i < cube.Length; i++){
                triangleVertices[i] = new Helpers.VertexPositionColorNormal(
                    new Vector3((float) cube[i].PositionX, (float) cube[i].PositionY, (float) cube[i].PositionZ), 
                    new Microsoft.Xna.Framework.Color((float)cube[i].ColorR, (float)cube[i].ColorG, (float)cube[i].ColorB), 
                    new Vector3((float)cube[i].NormalX, (float)cube[i].NormalY, (float)cube[i].NormalZ));
            }
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(Helpers.VertexPositionColorNormal), cube.Length, BufferUsage.WriteOnly);
            ////Geometry  - a simple triangle about the origin
            //triangleVertices = new Helpers.VertexPositionColorNormal[36];

            //triangleVertices[0 ] = new Helpers.VertexPositionColorNormal(new Vector3( 0, 0, 0), new Microsoft.Xna.Framework.Color(0.5F, 0.0F, 0.0F), new Vector3(0, 0, -1));
            //triangleVertices[1 ] = new Helpers.VertexPositionColorNormal(new Vector3( 0, 1, 0), new Microsoft.Xna.Framework.Color(0.5F, 0.0F, 0.0F), new Vector3(0, 0, -1));
            //triangleVertices[2 ] = new Helpers.VertexPositionColorNormal(new Vector3( 1, 1, 0), new Microsoft.Xna.Framework.Color(0.5F, 0.0F, 0.0F), new Vector3(0, 0, -1));
            //triangleVertices[3 ] = new Helpers.VertexPositionColorNormal(new Vector3( 0, 0, 0), new Microsoft.Xna.Framework.Color(1.0F, 0.0F, 0.0F), new Vector3(0, 0, -1));
            //triangleVertices[4 ] = new Helpers.VertexPositionColorNormal(new Vector3( 1, 0, 0), new Microsoft.Xna.Framework.Color(1.0F, 0.0F, 0.0F), new Vector3(0, 0, -1));
            //triangleVertices[5 ] = new Helpers.VertexPositionColorNormal(new Vector3( 1, 1, 0), new Microsoft.Xna.Framework.Color(1.0F, 0.0F, 0.0F), new Vector3(0, 0, -1));

            //triangleVertices[6 ] = new Helpers.VertexPositionColorNormal(new Vector3( 0, 0, 0), new Microsoft.Xna.Framework.Color(0.0F, 0.5F, 0.0F), new Vector3(-1,0,0));
            //triangleVertices[7 ] = new Helpers.VertexPositionColorNormal(new Vector3( 0, 1, 0), new Microsoft.Xna.Framework.Color(0.0F, 0.5F, 0.0F), new Vector3(-1,0,0));
            //triangleVertices[8 ] = new Helpers.VertexPositionColorNormal(new Vector3( 0, 1, 1), new Microsoft.Xna.Framework.Color(0.0F, 0.5F, 0.0F), new Vector3(-1,0,0));
            //triangleVertices[9 ] = new Helpers.VertexPositionColorNormal(new Vector3( 0, 0, 0), new Microsoft.Xna.Framework.Color(0.0F, 1.0F, 0.0F), new Vector3(-1,0,0));
            //triangleVertices[10] = new Helpers.VertexPositionColorNormal(new Vector3( 0, 0, 1), new Microsoft.Xna.Framework.Color(0.0F, 1.0F, 0.0F), new Vector3(-1,0,0));
            //triangleVertices[11] = new Helpers.VertexPositionColorNormal(new Vector3( 0, 1, 1), new Microsoft.Xna.Framework.Color(0.0F, 1.0F, 0.0F), new Vector3(-1,0,0));

            //triangleVertices[12] = new Helpers.VertexPositionColorNormal(new Vector3(0, 0, 0), new Microsoft.Xna.Framework.Color(0.0F, 0.0F, 0.5F), new Vector3(0, -1,0));
            //triangleVertices[13] = new Helpers.VertexPositionColorNormal(new Vector3(1, 0, 0), new Microsoft.Xna.Framework.Color(0.0F, 0.0F, 0.5F), new Vector3(0, -1,0));
            //triangleVertices[14] = new Helpers.VertexPositionColorNormal(new Vector3(1, 0, 1), new Microsoft.Xna.Framework.Color(0.0F, 0.0F, 0.5F), new Vector3(0, -1,0));
            //triangleVertices[15] = new Helpers.VertexPositionColorNormal(new Vector3(0, 0, 0), new Microsoft.Xna.Framework.Color(0.0F, 0.0F, 1.0F), new Vector3(0, -1,0));
            //triangleVertices[16] = new Helpers.VertexPositionColorNormal(new Vector3(0, 0, 1), new Microsoft.Xna.Framework.Color(0.0F, 0.0F, 1.0F), new Vector3(0, -1,0));
            //triangleVertices[17] = new Helpers.VertexPositionColorNormal(new Vector3(1, 0, 1), new Microsoft.Xna.Framework.Color(0.0F, 0.0F, 1.0F), new Vector3(0, -1,0));

            //triangleVertices[18] = new Helpers.VertexPositionColorNormal(new Vector3(1, 1, 1), new Microsoft.Xna.Framework.Color(0.0F, 0.5F, 0.5F), new Vector3(0,1,0));
            //triangleVertices[19] = new Helpers.VertexPositionColorNormal(new Vector3(1, 0, 0), new Microsoft.Xna.Framework.Color(0.0F, 0.5F, 0.5F), new Vector3(0,1,0));
            //triangleVertices[20] = new Helpers.VertexPositionColorNormal(new Vector3(1, 0, 1), new Microsoft.Xna.Framework.Color(0.0F, 0.5F, 0.5F), new Vector3(0,1,0));
            //triangleVertices[21] = new Helpers.VertexPositionColorNormal(new Vector3(1, 1, 1), new Microsoft.Xna.Framework.Color(0.0F, 1.0F, 1.0F), new Vector3(0,1,0));
            //triangleVertices[22] = new Helpers.VertexPositionColorNormal(new Vector3(0, 0, 1), new Microsoft.Xna.Framework.Color(0.0F, 1.0F, 1.0F), new Vector3(0,1,0));
            //triangleVertices[23] = new Helpers.VertexPositionColorNormal(new Vector3(1, 0, 1), new Microsoft.Xna.Framework.Color(0.0F, 1.0F, 1.0F), new Vector3(0,1,0));

            //triangleVertices[24] = new Helpers.VertexPositionColorNormal(new Vector3(1, 1, 1), new Microsoft.Xna.Framework.Color(0.5F, 0.5F, 0.0F), new Vector3(0,0,1));
            //triangleVertices[25] = new Helpers.VertexPositionColorNormal(new Vector3(1, 0, 0), new Microsoft.Xna.Framework.Color(0.5F, 0.5F, 0.0F), new Vector3(0,0,1));
            //triangleVertices[26] = new Helpers.VertexPositionColorNormal(new Vector3(1, 1, 0), new Microsoft.Xna.Framework.Color(0.5F, 0.5F, 0.0F), new Vector3(0,0,1));
            //triangleVertices[27] = new Helpers.VertexPositionColorNormal(new Vector3(1, 1, 1), new Microsoft.Xna.Framework.Color(1.0F, 1.0F, 0.0F), new Vector3(0,0,1));
            //triangleVertices[28] = new Helpers.VertexPositionColorNormal(new Vector3(0, 1, 0), new Microsoft.Xna.Framework.Color(1.0F, 1.0F, 0.0F), new Vector3(0,0,1));
            //triangleVertices[29] = new Helpers.VertexPositionColorNormal(new Vector3(1, 1, 0), new Microsoft.Xna.Framework.Color(1.0F, 1.0F, 0.0F), new Vector3(0,0,1));

            //triangleVertices[30] = new Helpers.VertexPositionColorNormal(new Vector3(1, 1, 1), new Microsoft.Xna.Framework.Color(0.5F, 0.0F, 0.5F), new Vector3(1,0,0));
            //triangleVertices[31] = new Helpers.VertexPositionColorNormal(new Vector3(0, 1, 0), new Microsoft.Xna.Framework.Color(0.5F, 0.0F, 0.5F), new Vector3(1,0,0));
            //triangleVertices[32] = new Helpers.VertexPositionColorNormal(new Vector3(0, 1, 1), new Microsoft.Xna.Framework.Color(0.5F, 0.0F, 0.5F), new Vector3(1,0,0));
            //triangleVertices[33] = new Helpers.VertexPositionColorNormal(new Vector3(1, 1, 1), new Microsoft.Xna.Framework.Color(1.0F, 0.0F, 1.0F), new Vector3(1,0,0));
            //triangleVertices[34] = new Helpers.VertexPositionColorNormal(new Vector3(0, 0, 1), new Microsoft.Xna.Framework.Color(1.0F, 0.0F, 1.0F), new Vector3(1,0,0));
            //triangleVertices[35] = new Helpers.VertexPositionColorNormal(new Vector3(0, 1, 1), new Microsoft.Xna.Framework.Color(1.0F, 0.0F, 1.0F), new Vector3(1,0,0));

            vertexBuffer.SetData<Helpers.VertexPositionColorNormal>(triangleVertices);


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }


        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            if (saveToImage)
            {
                screenshot = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Bgra32, DepthFormat.None);
                GraphicsDevice.SetRenderTarget(screenshot);
            }

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            basicEffect.EnableDefaultLighting();
            basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0, 0);
            basicEffect.DirectionalLight0.Direction = new Vector3(1, 0, 0);
            basicEffect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0);
            basicEffect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
            basicEffect.EmissiveColor = new Vector3(1, 0, 0);
            GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(32,32,32));
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            //Turn off culling so we see both sides of our rendered triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.
                    Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, triangleVertices.Length);
            }

            base.Draw(gameTime);
            if(saveToImage) {
                GraphicsDevice.Present();
                GraphicsDevice.SetRenderTarget(null);

                using (Bitmap bitmap = new Bitmap(screenshot.Width, screenshot.Height, PixelFormat.Format32bppArgb))
                {
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, screenshot.Width, screenshot.Height);

                    Microsoft.Xna.Framework.Color[] rawData = new Microsoft.Xna.Framework.Color[screenshot.Width * screenshot.Height];
                    screenshot.GetData<Microsoft.Xna.Framework.Color>(rawData);
                    Microsoft.Xna.Framework.Color tempColor;
                    for (int row = 0; row < screenshot.Height; row++)
                    {
                        for (int column = 0; column < screenshot.Width; column++)
                        {
                            // Assumes row major ordering of the array.
                            tempColor = rawData[row * screenshot.Width + column];
                            bitmap.SetPixel(column, row, System.Drawing.Color.FromArgb((int)tempColor.B, (int)tempColor.G, (int)tempColor.R));
                        }
                    }


                    bitmap.Save("test.png", ImageFormat.Png);
                }
            }
        }
    }
}