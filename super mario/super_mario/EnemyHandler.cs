using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class EnemyHandler
    {
        public List<Enemy> enemyList = new List<Enemy>();
        List<Vector2> spawnPositions;
        ContentManager content;
        InputManager input;

        public void LoadContent(ContentManager Content, InputManager Input)
        {
            this.content = Content;
            this.input = Input;
            spawnPositions = new List<Vector2>();
            spawnPositions.Add(new Vector2(10 * 16, 5 * 16));
        }

        public void Update(GameTime gameTime, Player player)
        {
            for (int i = 0; i < spawnPositions.Count; i++)
            {
                if(player.Position.X - spawnPositions[i].X <= 50)
                {
                    Enemy enemy = new Enemy(new Vector2(48, 208));
                    enemy.LoadContent(content, input);
                    enemyList.Add(enemy);
                    spawnPositions.RemoveAt(i);
                }
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Update(gameTime, player);
                if (enemyList[i].dead == true)
                    enemyList.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Draw(spriteBatch);
            }
        }
    }
}
