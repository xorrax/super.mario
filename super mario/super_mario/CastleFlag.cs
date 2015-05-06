using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class CastleFlag : Entity
    {
        public bool a = true;
        public bool flagUp = false;

        public CastleFlag(Vector2 pos, Texture2D img)
        {
            this.position = pos;
            this.image = img;
        }

        public void Update()
        {
            if (position.Y >= 16 * 8 && a == true)
                position.Y--;
            else if(Player.Instance.InCastle == true)
                flagUp = true;
        }
            
         
    }
}
