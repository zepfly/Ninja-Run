using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Rex_Runner.Components
{
    public abstract class GameEntity
    {
        protected Sprite2D _model;

        public Sprite2D Model { get => _model; set => _model = value; }


        public virtual void Update(GameTime gameTime)
        {
            this._model.Update(gameTime);
        }

        public virtual void UnloadContent()
        {
            this._model.UnloadContent();
        }
    }
}
