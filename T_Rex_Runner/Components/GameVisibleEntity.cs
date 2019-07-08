using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Rex_Runner.Components
{
    public abstract class GameVisibleEntity : GameEntity
    {
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this._model.Draw(gameTime, spriteBatch);
        }

        public virtual bool IsSelected(MouseState ms)
        {
            return this._model.IsSelected(ms);
        }

        public void Select(bool isSelected)
        {
            this._model.Select(isSelected);
        }
    }
}
