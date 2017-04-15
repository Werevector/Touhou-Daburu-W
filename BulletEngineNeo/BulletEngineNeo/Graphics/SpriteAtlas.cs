using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BulletEngineNeo.Graphics
{
    class SpriteAtlas
    {
        private Texture2D mAtlas;
        private Dictionary<string, List<Rectangle>> mClipMap;
        private Dictionary<string, int> mClipOriginAngles;

        public SpriteAtlas() { }

        public SpriteAtlas(Texture2D texture, Dictionary<String, List<Rectangle>> clips)
        {
            mAtlas = texture;
            mClipMap = clips;
        }

        public void Draw(SpriteBatch sb, String clipSet, int index, Vector2 position, float alpha = 1.0f)
        {
            Rectangle source = mClipMap[clipSet][index];
            Rectangle destination = new Rectangle((int)position.X - source.Width / 2, (int)position.Y - source.Height / 2, source.Width, source.Height);
            sb.Draw(mAtlas, destination, source, Color.White * alpha);
        }

        public void Draw(SpriteBatch sb, String clipSet, int index, Vector2 position, SpriteEffects effect)
        {
            Rectangle source = mClipMap[clipSet][index];
            Rectangle destination = new Rectangle((int)position.X - source.Width / 2, (int)position.Y - source.Height / 2, source.Width, source.Height);
            sb.Draw(mAtlas, destination, source, Color.White, 0.0f, new Vector2(), effect, 0.0f);
        }


        public void Draw(SpriteBatch sb, string clipSet, int index, Vector2 position, float angle, float scale, SpriteEffects effect, float depth, float alpha)
        {
            Rectangle source = mClipMap[clipSet][index];
            Vector2 origin = new Vector2(source.Width / 2, source.Height / 2);
            angle += (float)(mClipOriginAngles[clipSet] * Math.PI / 180);
            sb.Draw(mAtlas, position, source, Color.White * alpha, angle, origin, scale, effect, depth);
        }

        public Rectangle GetClip(string clipSet, int index)
        {
            return mClipMap[clipSet][index];
        }

        public Texture2D Texture2DCopy(Rectangle clip)
        {
            Texture2D copyTexture = new Texture2D(mAtlas.GraphicsDevice, clip.Width, clip.Height);
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

        public void SetImage(Texture2D image)
        {
            mAtlas = image;
        }
        public void SetClipMap(Dictionary<string, List<Rectangle>> map, Dictionary<string, int> origins)
        {
            mClipMap = map;
            mClipOriginAngles = origins;
        }
    }
}
