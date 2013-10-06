
namespace Texter
{
    public class TextRegion : TextRenderer
    {
        private TextRenderer _renderer;
        private int _startX;
        private int _startY;

        internal TextRegion(TextRenderer renderer, int x, int y, uint w, uint h)
        {
            _renderer = renderer;
            _startX = x;
            _startY = y;

            Width = w;
            Height = h;
        }

        public override void Set(int x, int y, Character character)
        {
            if (x < 0 || x > Width - 1 | y < 0 | y > Height - 1)
                return;

            _renderer.Set(_startX + x, _startY + y, character);
        }

        public override Character Get(int x, int y)
        {
            if (x < 0 || x > Width - 1 | y < 0 | y > Height - 1)
                return Character.Blank;

            return _renderer.Get(_startX + x, _startY + y);
        }
    }
}
