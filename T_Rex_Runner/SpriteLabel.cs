using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Rex_Runner
{
    public class SpriteLabel
    {
        SpriteFont _spriteFont;
        String _text;
        Vector2 _position;
        Color _txtColor;
        float _layerDepth;

        public String Text { get => _text; set => _text = value; }
        public Vector2 Position { get => _position; }
        public void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }
        public Color TxtColor { get => _txtColor; set => _txtColor = value; }

        public SpriteLabel(SpriteFont spriteFont, String text, Vector2 position, Color color, float layerDepth = 1)
        {
            _spriteFont = spriteFont;
            _text = text;
            _position = position;
            _txtColor = color;
            _layerDepth = layerDepth;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_spriteFont, _text, _position, _txtColor, 0, Vector2.Zero, 1, SpriteEffects.None, _layerDepth);
        }
    }
}
