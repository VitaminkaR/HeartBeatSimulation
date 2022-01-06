using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HeartBitSimulation
{
    class Graphics
    {
        private VertexBuffer vertexBuffer;
        private List<VertexPositionColor> vertices;
        private BasicEffect effect;

        private Matrix viewMatrix;
        private Matrix projectionMatrix;
        private Matrix worldMatrix;

        private GraphicsDevice graphicsDevice;

        public void Init(GraphicsDeviceManager manager)
        {
            graphicsDevice = manager.GraphicsDevice;
            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
MathHelper.PiOver4,
HBSimGame.WIDTH / HBSimGame.HEIGHT,
1, 100);
            worldMatrix = Matrix.Identity;

            vertices = new List<VertexPositionColor>();
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), 1, BufferUsage.None);

            effect = new BasicEffect(graphicsDevice);
            effect.VertexColorEnabled = true;
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            effect.World = worldMatrix;
        }

        public void Draw()
        {
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                try
                {
                    graphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices.ToArray(), 0, vertices.Count - 1);
                }
                catch
                {
                } 
            }
        }

        // добавляет новую вершину в массив
        public void AddVertex(Vector3 pos, Color color)
        {
            vertices.Add(new VertexPositionColor(pos, color));
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Count, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColor>(vertices.ToArray());
        }

        // убирает вершину по id
        public void DelVertex(int id)
        {
            vertices.RemoveAt(id);
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Count, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColor>(vertices.ToArray());
        }

        // убирает все вершины
        public void DelAllVertex()
        {
            vertices = new List<VertexPositionColor>();
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), 1, BufferUsage.None);
        }

        // вернуть вершину
        public VertexPositionColor GetVertex(int id) => vertices[id];

        // вернуть последнюю вершину
        public VertexPositionColor GetLastVertex() => vertices[vertices.Count - 1];

        // есть ли веришны
        public bool VNotNull() => vertices.Count > 0;
    }
}
