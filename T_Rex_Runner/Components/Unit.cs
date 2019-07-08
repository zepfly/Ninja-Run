using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_Rex_Runner.Components
{
    public class Unit : GameVisibleEntity
    {
        protected Game _game;

        public Unit(Game game, Sprite2D model)
        {
            this._game = game;
            this._model = model;
        }
    }
}
