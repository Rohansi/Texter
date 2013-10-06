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
            _renderer.Set(x, y, _effect(x, y, character), useBlending);
        }

        public override Character Get(int x, int y)
        {
            return _renderer.Get(x, y);
        }
    }
}
