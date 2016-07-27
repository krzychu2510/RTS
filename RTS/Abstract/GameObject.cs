﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RTS.Concrete;

namespace RTS.Abstract
{
    public enum Job { DONE, START, BUILD, DESTROY, ATTACK, WALK, MINE}
    public abstract class GameObject
    {
        public GameObject target;
        public int id;
        public string name;
        public Color texture;
        protected int speed;
        public ObjectContainer Container;
        public bool PlatformCollision { get; set; }
        public int OwnerID { get; set; }
        public Player Owner { get; set; }
        public Dictionary<string,int> properties = new Dictionary<string, int>();
        public Job CurrentJob { get; set; }
        public Vector2 Coords { get; set; }
        public Vector2 targetCoords { get; set; }
        public ActionControl actionControl = new ActionControl();
        public bool isSelected { get; set; }
        public GameObject()
        {
            properties["Health"] = 100;
            properties["Damage"] = 0;
            properties["Armor"] = 10;
            properties["SightLine"] = 20;
            isSelected = false;
        }
        public virtual void move(Vector2 coords, int speed)
        {
            this.targetCoords = coords;
            this.speed = speed;
        }

        public virtual void Update()
        {
            if (Vector2.Distance(this.targetCoords, this.Coords) > 10)
            {
                float xMove = targetCoords.X - Coords.X;
                float yMove = targetCoords.Y - Coords.Y;

                Coords = new Vector2(xMove*speed/10000 + Coords.X, yMove*speed/10000 + Coords.Y);
            }
            else
            {
                Point targetPoint = actionControl.InvokeAction();
                if (targetPoint != Point.Zero)
                {
                    this.targetCoords = new Vector2(targetPoint.X, targetPoint.Y);
                }
            }
            if (properties["Health"] < 0)
            {
                Container.DeleteObject(this);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, SpriteFont font)
        {
            var rect = new Texture2D(graphicsDevice, 1, 1);
            rect.SetData(new[] { texture });
            int i = 0;
            foreach (var prop in properties)
            {
                spriteBatch.DrawString(font, prop.Key+":"+prop.Value, new Vector2((int)Coords.X, (int)Coords.Y - 100+i), Color.Black);
                i += 10;
            }
            if (isSelected)
            {
                spriteBatch.Draw(rect, new Rectangle(new Point((int)(Coords.X-5), (int)(Coords.Y-5)), new Point(20, 20)), Color.DarkRed);
            }
            spriteBatch.Draw(rect, new Rectangle(new Point((int) Coords.X,(int) Coords.Y),new Point(10,10)), texture);
        }
    }
}
