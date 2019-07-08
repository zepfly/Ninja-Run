using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Rex_Runner
{
    public class Sprite2D
    {
        int _millisecondsPerFrame = 40;

        List<Texture2D> _images;
        Vector2 _position;
        List<Point> _sheetSize;
        List<int> _colideOffset;
        float _layerDepth;

        List<Vector2> _frameSize;
        Point _currentFrame;

        int _timeSinceLastFrame = 0;
        int _speed = 10;
        int _numberOfJump = 2;
        GLOBAL.SpriteState _state = GLOBAL.SpriteState.Idle;

        public Vector2 Position { get => _position; set => _position = value; }
        public int CountOfSheet { get => _sheetSize[(int)_state].X * _sheetSize[(int)_state].Y; }
        public void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }
        public List<int> ColideOffset { get => _colideOffset; }
        public List<Vector2> FrameSize { get => _frameSize; }
        public GLOBAL.SpriteState State { get => _state; set => _state = value; }
        public int Speed { get => _speed; set => _speed = value; }

        // Constructor character
        public Sprite2D(List<Texture2D> list, Vector2 position, List<Point> sheetSize, List<int> colideOffset, float layerDepth = 0)
        {
            _images = list;
            _position = position;
            _sheetSize = sheetSize;
            _colideOffset = colideOffset;
            _layerDepth = layerDepth;

            _currentFrame = Point.Zero;
            _frameSize = new List<Vector2>();
            for (int i = 0; i < _images.Count; i++)
                _frameSize.Add(new Vector2((float)_images[i].Width / _sheetSize[i].X, (float)_images[i].Height / _sheetSize[i].Y));
        }

        // Constructor sprite which have 1 image
        public Sprite2D(Texture2D texture, Vector2 position, Point sheetSize, int colideOffset, float layerDepth = 0, int speed = 10)
        {
            _images = new List<Texture2D>();
            _images.Add(texture);
            _position = position;
            _sheetSize = new List<Point>();
            _sheetSize.Add(sheetSize);
            _colideOffset = new List<int>();
            _colideOffset.Add(colideOffset);
            _layerDepth = layerDepth;

            _frameSize = new List<Vector2>();
            _frameSize.Add(new Vector2(_images[0].Width, _images[0].Height));
            _currentFrame = Point.Zero;
            _speed = speed;
        }

        public void LoadContent(Game game, string[] imagesPath)
        {
            for (int i = 0; i < imagesPath.Length; i++)
            {
                var tex = game.Content.Load<Texture2D>(imagesPath[i]);
                this._images.Add(tex);
            }
        }

        public void UnloadContent()
        {
            this._images.Clear();
        }

        const float g = 9.8f;
        const float v0 = 45;
        const float characterGround = 335;
        const float characterSlidePosition = 30;
        float t = 0;
        float v;
        float y0;
        bool isFalling;
        public void Update(GameTime gameTime)
        {
            if (_images.Count > 1)
            {
                _timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (_timeSinceLastFrame > _millisecondsPerFrame)
                {
                    _timeSinceLastFrame = 0;
                    if ((_state != GLOBAL.SpriteState.Jump && _state != GLOBAL.SpriteState.Dead && _state != GLOBAL.SpriteState.Slide)
                        || (_currentFrame.X != _sheetSize[(int)_state].X - 1 || _currentFrame.Y != _sheetSize[(int)_state].Y - 1))
                    {
                        ++_currentFrame.X;
                        if (_currentFrame.X >= _sheetSize[(int)_state].X)
                        {
                            _currentFrame.X = 0;
                            ++_currentFrame.Y;
                            if (_currentFrame.Y >= _sheetSize[(int)_state].Y)
                            {
                                _currentFrame.Y = 0;
                            }
                        }
                    }
                }

                // When character slide
                if (_state == GLOBAL.SpriteState.Slide)
                {
                    t += (float)gameTime.ElapsedGameTime.Milliseconds;
                    if (t >= 500)
                    {
                        ChangeState(GLOBAL.SpriteState.Run);
                        _position.Y -= characterSlidePosition;
                    }
                }

                // Change position character when jumping
                if (_state == GLOBAL.SpriteState.Jump)
                {
                    t += (float) gameTime.ElapsedGameTime.Milliseconds / 80;
                    v = (!isFalling) ? v0 - g * t : g * t;
                    _position.Y = (!isFalling) ? y0 - ((characterGround - y0) + v0 * t - g * t * t / 2) : y0 + (g * t * t / 2);
                    if (v == 0)
                    {
                        isFalling = true;
                        t = 0;
                    }

                    if (_position.Y >= characterGround)
                    {
                        ChangeState(GLOBAL.SpriteState.Run);
                        _position.Y = characterGround;
                    }
                }
            }
            else
            {
                _position.X -= _speed;
                if (_position.X + _frameSize[0].X < 0)
                    _position.X += _frameSize[0].X * 2;
            }
        }
        public void ChangeState(GLOBAL.SpriteState state)
        {
            //if (state == GLOBAL.SpriteState.Jump)
            //{
            //    if (_numberOfJump == 0)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        t = 0;
            //        y0 = _position.Y;
            //        isFalling = false;
            //        _numberOfJump--;
            //    }
            //}
            //else _numberOfJump = 2;

            if (((state == GLOBAL.SpriteState.Jump || state == GLOBAL.SpriteState.Slide) && _state == GLOBAL.SpriteState.Jump) 
                || (_state == GLOBAL.SpriteState.Slide && state == GLOBAL.SpriteState.Slide))
                return;

            if (state == GLOBAL.SpriteState.Slide)
            {
                t = 0;
                _position.Y += characterSlidePosition;
            }
            if (state == GLOBAL.SpriteState.Jump)
            {
                t = 0;
                y0 = _position.Y;
                isFalling = false;
            }

            _state = state;
            _currentFrame.X = 0;
            _currentFrame.Y = 0;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_images[(int)_state], _position, 
                new Rectangle(_currentFrame.X * (int)_frameSize[(int)_state].X, _currentFrame.Y * (int)_frameSize[(int)_state].Y, (int)_frameSize[(int)_state].X, (int)_frameSize[(int)_state].Y),
                Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, _layerDepth);
        }

        public bool IsSelected(MouseState mouse)
        {
            float x = mouse.X;
            float y = mouse.Y;
            if (x >= _position.X && x <= _position.X + _frameSize[(int)_state].X &&
                y >= _position.Y && y <= _position.Y + _frameSize[(int)_state].Y)
                return true;
            return false;
        }

        public void Select(bool isSelected)
        {
            //if (isSelected)
            //    this.state = 1;
            //else
            //    this.state = 0;
        }

        public bool IsIntersect(Sprite2D other)
        {
            Rectangle currentBound = this.CollisionRect;
            Rectangle otherBound = other.CollisionRect;
            return currentBound.Intersects(otherBound);
        }

        public Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)_position.X + _colideOffset[(int)_state], (int)_position.Y + _colideOffset[(int)_state], 
                    (int)_frameSize[(int)_state].X - _colideOffset[(int)_state], (int)_frameSize[(int)_state].Y - _colideOffset[(int)_state]);
            }
        }

        public void IncreaseSpeed()
        {
            _speed *= 2;
            _millisecondsPerFrame -= 10;
        }

        public void Reset()
        {
            _state = GLOBAL.SpriteState.Idle;
            _position.Y = characterGround;
        }
    }
}
