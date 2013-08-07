using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Texter
{
    public class TextRegion : TextRenderer
    {
        private TextRenderer renderer;
        private int xx;
        private int yy;

        internal TextRegion(TextRenderer renderer, int x, int y, uint w, uint h)
        {
            this.renderer = renderer;
            xx = x;
            yy = y;

            Width = w;
            Height = h;
        }

        public override void Set(int x, int y, Character character, bool blend = true)
        {
            if (x < 0 || x > Width - 1 | y < 0 | y > Height - 1)
                return;

            renderer.Set(xx + x, yy + y, character, blend);
        }

        public override Character Get(int x, int y)
        {
            if (x < 0 || x > Width - 1 | y < 0 | y > Height - 1)
                return Character.Blank;

            return renderer.Get(xx + x, yy + y);
        }
    }
}
