using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Rex_Runner.Components
{
    public class Dinosaur : Unit
    {
        public delegate void CollideHandler(object sender, EventIntersectArgs e);
        public event CollideHandler Collide;

        public Dinosaur(Game game, Sprite2D model) : base(game, model)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        //public void MoveLeft()
        //{
        //    if (this._model.Left > 0)
        //        this._model.Left--;
        //}

        //public void MoveRight()
        //{
        //    if (this._model.Left <= 700)
        //        this._model.Left++;
        //}

        public bool IsIntersect(Dinosaur other)
        {
            bool collide = this._model.IsIntersect(other.Model);
            if (collide && Collide != null)
            {
                this.Collide(this, new EventIntersectArgs(this, other));
            }

            return collide;
        }
    }
}
