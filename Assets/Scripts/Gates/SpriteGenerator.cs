using UnityEngine;

namespace Gates
{
    public static class SpriteGenerator
    {
        public static Sprite CreateSprite(int width, int height)
        {
            var texture2D = CreateTexture(width, height);
            return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(.5f, .5f), 10);
        }

        private static Texture2D CreateTexture(int width, int height)
        {
            Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false) {filterMode = FilterMode.Point};

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    texture2D.SetPixel(i, j, GetColor(i, j, width, height));
                }
            }
            
            texture2D.Apply();
            return texture2D;
        }

        private static Color GetColor(int x, int y, int width, int height)
        {
            if (x == 0 && y == 0 || x == 0 && y == height - 1 || x == width - 1 && y == 0 || x == width - 1 && y == height - 1)
            {
                return Color.clear;
            }
            
            if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
            {
                return Color.black;
            }

            return Color.white;
        }
    }
}
