using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PixelDust.Core.Sprites;
using PixelDust.Core.Engine;
using PixelDust.Core.Worlding;

using System;
using System.Collections.Generic;

namespace PixelDust.Core.Elements
{
    public sealed class PElementRender
    {
        internal const int SpriteSize = PWorld.Scale;

        public bool EnableAnimation { get; set; }
        public float AnimationDelay { get; set; }

        // Animation
        private float _currentAnimationDelay;
        private int _currentAnimationIndex;
        private readonly List<Rectangle> frames = new();

        // Dependencies
        private readonly PElementContext _context;

        public PElementRender(PElementContext context)
        {
            _context = context;
        }

        public void AddFrame(Vector2 pos)
        {
            frames.Add(new(new((int)pos.X * SpriteSize, (int)pos.Y * SpriteSize), new(SpriteSize)));
        }

        internal void Update()
        {
            if (EnableAnimation) { UpdateAnimation(); }
        }
        private void UpdateAnimation()
        {
            if (_currentAnimationDelay < AnimationDelay)
            {
                _currentAnimationDelay++;
            }
            else
            {
                _currentAnimationDelay = 0;
                _currentAnimationIndex = _currentAnimationIndex < frames.Count ? _currentAnimationIndex + 1 : 0;
            }
        }

        internal void Draw()
        {
            Vector2 pos = new(_context.Position.X * PWorld.Scale, _context.Position.Y * PWorld.Scale);
            Rectangle rectangle = frames[_currentAnimationIndex];

            PGraphics.SpriteBatch.Draw(PTextures.Elements, pos, rectangle, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }
    }
}