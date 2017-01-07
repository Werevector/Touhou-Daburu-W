using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Touhou_Daburu
{
    class TextureAtlas
    {
        private Texture2D mAtlas;
        private Dictionary<String, List<Rectangle>> mClipMap;

        public TextureAtlas(Texture2D texture, Dictionary<String, List<Rectangle>> clips)
        {
            mAtlas = texture;
            mClipMap = clips;
        }

        public void Draw(SpriteBatch sb, String clipSet, int index, Vector2 position )
        {
            Rectangle source = mClipMap[clipSet][index];
            Rectangle destination = new Rectangle((int)position.X-source.Width/2, (int)position.Y-source.Height/2, source.Width, source.Height);
            sb.Draw(mAtlas, destination, source, Color.White);
        }

        public void Draw(SpriteBatch sb, string clipSet, int index, Vector2 position, float angle, float scale, SpriteEffects effect, float depth, float alpha)
        {
            Rectangle source = mClipMap[clipSet][index];
            Vector2 origin = new Vector2(source.Width / 2, source.Height / 2);
            sb.Draw(mAtlas, position, source, Color.White * alpha, angle, origin, scale, effect, depth);
        }

        public Rectangle GetClip(string clipSet, int index)
        {
            return mClipMap[clipSet][index];
        }
        
        public Texture2D Texture2DCopy(Rectangle clip)
        {
            Texture2D copyTexture = new Texture2D(mAtlas.GraphicsDevice, clip.Width, clip.Height );
            Color[] data = new Color[clip.Width * clip.Height];
            mAtlas.GetData(0, clip, data, 0, data.Length);
            copyTexture.SetData(data);
            return copyTexture;
        }

        public Dictionary<string, List<Texture2D>> Texture2DCopyAll()
        {
            Dictionary<string, List<Texture2D>> textureMap = new Dictionary<string, List<Texture2D>>();
            foreach (var clipset in mClipMap)
            {
                List<Texture2D> texSet = new List<Texture2D>();
                foreach (var clip in clipset.Value)
                {
                    texSet.Add(Texture2DCopy(clip));
                }
                textureMap.Add(clipset.Key, texSet);
            }
            return textureMap;
        }

        public Texture2D GenerateDebugTexture(Color c)
        {
            Texture2D t = new Texture2D(mAtlas.GraphicsDevice, 1, 1);
            t.SetData<Color>(
                new Color[] { c });
            return t;
        }
    }
}
