using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace super_mario
{
    public class Camera
    {
        static Camera instance;
        Vector2 position;
        Matrix viewMatrix;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public static Camera Instance
        {
            get
            {
                if (instance == null)
                    instance = new Camera();
                return instance;
            }
        }

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }
        public void SetCameraPoint(Vector2 cameraPosition)
        {
            position = new Vector2(cameraPosition.X - ScreenManager.Instance.Dimensions.X / 2, cameraPosition.Y - ScreenManager.Instance.Dimensions.Y / 2);

            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
        }

        public void Update()
        {
            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }

    }
}
