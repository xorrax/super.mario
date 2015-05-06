using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace super_mario
{
    public class Layer
    {
        public List<List<Tile>> tiles;
        List<List<string>> attributes, contents;
        List<string> motion, solid, special;
        FileManager fileManager;
        ContentManager content;
        Texture2D tileSheet;
        string[] getMotion;
        string TileMoveSpeed = "";
        Layer layer;
        InputManager input;

        bool posUpd;

        //Sounds
        SoundEffect breakBlock, powerupAppears, coinSound;

        static public Vector2 TileDimensions
        {
            get { return new Vector2(16, 16); }
        }

        public void LoadContent(Map map, string layerID)
        {
            tiles = new List<List<Tile>>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            motion = new List<string>();
            solid = new List<string>();
            special = new List<string>();
            fileManager = new FileManager();
            layer = this;
            input = new InputManager();
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            breakBlock = this.content.Load<SoundEffect>("Sound/breakblock");
            powerupAppears = this.content.Load<SoundEffect>("Sound/powerupappears");
            coinSound = this.content.Load<SoundEffect>("Sound/coin");
            posUpd = false;
            fileManager.LoadContent("Load/Maps/" + map.ID + ".cme", attributes, contents, layerID);

            int indexY = 0;

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "TileSet":
                            tileSheet = content.Load<Texture2D>("TileSets/" + contents[i][j]);
                            break;
                        case "Solid":
                            solid.Add(contents[i][j]);
                            break;
                        case "Motion":
                            motion.Add(contents[i][j]);
                            break;
                        case"Special":
                            special.Add(contents[i][j]);
                            break;
                        case"MoveSpeed":
                            TileMoveSpeed = contents[i][j];
                            break;
                        case "StartLayer":
                            List<Tile> tempTiles = new List<Tile>();
                            Tile.Motion tempMotion = Tile.Motion.Static;
                            Tile.State tempState;
                            Tile.Specials tempSpecials = Tile.Specials.Normal;

                            for (int k = 0; k < contents[i].Count; k++)
                            {
                                string[] split = contents[i][k].Split(',');
                                tempTiles.Add(new Tile());

                                if (solid.Contains(contents[i][k]))
                                    tempState = Tile.State.Solid;
                                else
                                    tempState = Tile.State.Passive;

                                foreach (string m in motion)
                                {
                                    getMotion = m.Split(':');
                                    if (getMotion[0] == contents[i][k])
                                    {
                                        tempMotion = (Tile.Motion)Enum.Parse(typeof(Tile.Motion), getMotion[1]);
                                        break;
                                    }
                                }

                                foreach (string s in special)
                                {
                                    getMotion = s.Split(':');
                                    if (getMotion[0] == contents[i][k])
                                    {
                                        tempSpecials = (Tile.Specials)Enum.Parse(typeof(Tile.Specials), getMotion[1]);
                                        break;
                                    }
                                }
                                float tempSpeed;
                                tempSpeed = float.Parse(TileMoveSpeed);

                                tempTiles[k].SetTile(tempState, tempMotion, tempSpecials, new Vector2(k * 16, indexY * 16), tileSheet,
                                    new Rectangle(int.Parse(split[0]) * 16, int.Parse(split[1]) * 16, 16, 16), tempSpeed);
                            }

                            tiles.Add(tempTiles);
                            indexY++;
                            break;
                    }
                }
            }
        }

        public void Update(GameTime gameTime, Player player)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                {
                    if (tiles[i][j].destroyed == true)
                    {
                        if (tiles[i][j].specials == Tile.Specials.Destroyable)
                        {
                            breakBlock.Play();
                        }
                        else if (tiles[i][j].specials == Tile.Specials.Coin)
                        {
                            coinSound.Play();
                        }
                        tiles[i].RemoveAt(j);
                    }

                    if(tiles[i][j].activated == true)
                    {
                        if(tiles[i][j].specials == Tile.Specials.Mushroom)
                        {
                            ObjectHandler.instance.MushroomPos = tiles[i][j].Position;
                            ObjectHandler.instance.SpawnMushroom = true;
                            tiles[i][j].SetTile(Tile.State.Solid, Tile.Motion.Static, Tile.Specials.Normal, tiles[i][j].Position, tileSheet, new Rectangle(4 * 16, 0, 16, 16), 0);
                            powerupAppears.Play();
                        }
                        else if (tiles[i][j].specials == Tile.Specials.Star)
                        {
                            ObjectHandler.instance.StarPos = tiles[i][j].Position;
                            ObjectHandler.instance.SpawnStar = true;
                            tiles[i][j].SetTile(Tile.State.Solid, Tile.Motion.Static, Tile.Specials.Normal, tiles[i][j].Position, tileSheet, new Rectangle(4 * 16, 0, 16, 16), 0);
                            powerupAppears.Play();
                        }
                    }

                    if (tiles[i][j].setPos == true)
                    {
                        foreach (Tile t in tiles[i])
                            t.setPos = true;
                    }

                    if (tiles[i][j].specials == Tile.Specials.Flagtop && posUpd == false)
                    {
                        ObjectHandler.instance.PFPos = new Vector2(tiles[i][j].Position.X - 8, tiles[i][j].Position.Y);
                        posUpd = true;
                    }

                    if (tiles[i][j].specials == Tile.Specials.CastleFlag)
                        ObjectHandler.instance.CFPos = new Vector2(tiles[i][j].Position.X - 8, tiles[i][j].Position.Y);

                    tiles[i][j].Update(gameTime, player, layer, ObjectHandler.instance);
                }
            }

            for (int i = 0; i < ObjectHandler.instance.mushroomList.Count; i++)
            {
                if (ObjectHandler.instance.mushroomList[i].Removed == true)
                {
                    ObjectHandler.instance.mushroomList.RemoveAt(i);
                }
            }
            for (int i = 0; i < ObjectHandler.instance.starList.Count; i++)
            {
                if (ObjectHandler.instance.starList[i].Removed == true)
                {
                    ObjectHandler.instance.starList.RemoveAt(i);
                }
            }
            if(posUpd == true)
                ObjectHandler.instance.Update(gameTime, player, content, input);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                    tiles[i][j].Draw(spriteBatch);
            }
        }
    }
}
