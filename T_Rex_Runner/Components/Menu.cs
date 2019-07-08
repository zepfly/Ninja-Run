using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Rex_Runner.Components
{
    public class Menu : GameControl
    {
        List<MenuItem> items = new List<MenuItem>();

        public Menu(Game game, Sprite2D model) : base(game, model)
        {
            //var mnuItem = CreateItems(1);
            //mnuItem.Click += MnuItem_Click;
            //mnuItem.Hover += MnuItem_Hover;
            //items.Add(mnuItem);
            //items.Add(CreateItems(2));
        }

        private void MnuItem_Hover(object sender, EventArgs e)
        {
            this._game.Window.Title = "HOVER HOVER HOVERINGGGG YEAHHH~~~";
        }

        private void MnuItem_Click(object sender, EventArgs e)
        {
            this._game.Window.Title = "You clicked on First Menu Button.";
        }

        private MenuItem CreateItems(Vector2 position)
        {
            //float itemHeight = 80f;
            //float x = this._model.Left + 20;
            //float y = this._model.Top + 20 + index * itemHeight;
            var tex = _game.Content.Load<Texture2D>(@"Menu/Play");
            var sprite = new Sprite2D(tex, position, new Point(1, 1), 0, 1, 0);
            return new MenuItem(_game, sprite);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Draw(gameTime, spriteBatch);
            }

            base.Draw(gameTime, spriteBatch);
        }
    }
}
