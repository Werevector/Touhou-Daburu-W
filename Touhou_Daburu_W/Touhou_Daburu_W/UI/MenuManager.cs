
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touhou_Daburu_W.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Touhou_Daburu_W
{
    class MenuManager
    {

        SpriteFont font;
        StartMenu mStart;

        public MenuManager()
        {
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("infoFont");
            mStart = new StartMenu(font);
        }

        public void Update(GameTime gameTime)
        {
            mStart.HandleEvents();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mStart.Draw(spriteBatch, font, Color.White);
        }
    }
}
