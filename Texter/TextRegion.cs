using System;

namespace Texter
{
    public class TextRegion : ITextRenderer
    {
        public uint Width { get; private set; }
        public uint Height { get; private set; }

        private ITextRenderer _renderer;
        private int _startX;
        private int _startY;

        internal TextRegion(ITextRenderer renderer, int x, int y, uint w, uint h)
        {
            if (renderer == null)
                throw new ArgumentNullException("renderer");

            _renderer = renderer;
            _startX = x;
            _startY = y;

            Width = w;
            Height = h;
        }

        public void Set(int x, int y, Character character, bool useBlending = true)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return;

            _renderer.Set(_startX + x, _startY + y, character, useBlending);
        }

        public Character Get(int x, int y)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return Character.Blank;

            return _renderer.Get(_startX + x, _startY + y);
        }
    }
}
