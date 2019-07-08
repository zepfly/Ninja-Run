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
    public class MenuItem : GameControl
    {
        public delegate void ClickHandler(object sender, EventArgs e);
        public event ClickHandler Click;
        bool isClicked = false;

        public delegate void HoverHandler(object sender, EventArgs e);
        public event HoverHandler Hover;
        bool isHover = false;

        public MenuItem(Game game, Sprite2D model) : base(game, model)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.AddClickListener();
            this.AddHoverListener();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public void AddClickListener()
        {
            MouseState ms = Mouse.GetState();
            if (!isClicked && ms.LeftButton == ButtonState.Pressed && this.IsSelected(ms))
            {
                isClicked = true;
                if (this.Click != null)
                {
                    this.Click(this, new EventArgs());
                }
            }
            if (ms.LeftButton == ButtonState.Released)
            {
                isClicked = false;
            }
        }

        public void AddHoverListener()
        {
            MouseState ms = Mouse.GetState();
            if (!isHover && this.IsSelected(ms))
            {
                isHover = true;
                if (this.Hover != null)
                {
                    this.Hover(this, new EventArgs());
                }
            }
            else
            {
                isHover = false;
            }
        }
    }
}
