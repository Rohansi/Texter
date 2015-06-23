using System;

namespace Texter
{
    public class TextEffect : ITextRenderer
    {
        public delegate Character Func(int x, int y, Character input);

        public uint Width { get; private set; }
        public uint Height { get; private set; }

        private ITextRenderer _renderer;
        private Func _effect;

        internal TextEffect(ITextRenderer renderer, Func effect)
        {
            if (renderer == null)
                throw new ArgumentNullException("renderer");

            if (effect == null)
                throw new ArgumentNullException("effect");

            _renderer = renderer;
            _effect = effect;

            Width = _renderer.Width;
            Height = _renderer.Height;
        }

        public void Set(int x, int y, Character character, bool useBlending = true)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return;

            if (useBlending && character.HasTransparentComponent)
            {
                var glyph = character.Glyph;
                var fore = character.Foreground;
                var back = character.Background;

                var ch = Get(x, y);

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

        public Character Get(int x, int y)
        {
            return _renderer.Get(x, y);
        }
    }
}
