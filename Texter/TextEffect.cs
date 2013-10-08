using System;

namespace Texter
{
    public class TextEffect : TextRenderer
    {
        public delegate Character Func(int x, int y, Character input);
        
        private TextRenderer _renderer;
        private Func _effect;

        internal TextEffect(TextRenderer renderer, Func effect)
        {
            _renderer = renderer;
            _effect = effect;

            if (_effect == null)
                throw new ArgumentNullException("effect");

            Width = _renderer.Width;
            Height = _renderer.Height;
        }

        public override void Set(int x, int y, Character character, bool useBlending = true)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return;

            if (useBlending && character.HasTransparentComponent)
            {
                int glyph = character.Glyph;
                int fore = character.Foreground;
                int back = character.Background;

                Character ch = Get(x, y);

                if (glyph == -1)
                    glyph = ch.Glyph;

                if (fore == -1)
                    fore = ch.Foreground;

                if (back == -1)
                    back = ch.Background;

                character = new Character(glyph, fore, back);
            }

            _renderer.Set(x, y, _effect(x, y, character), useBlending);
        }

        public override Character Get(int x, int y)
        {
            return _renderer.Get(x, y);
        }
    }
}
