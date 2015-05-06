using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class PoleFlag : Entity
    {
        public bool flagDown = false;
        public bool a = false;

        public PoleFlag(Vector2 pos, Texture2D img)
        {
            this.position = pos;
            this.image = img;
        }

        public void Update()
        {
            if (Player.Instance.onFlag == true)
            {
                if (position.Y <= 16 * 12 - 8 && a == true)
                    position.Y++;
                else
                    flagDown = true;
            }
        }
    }
}
