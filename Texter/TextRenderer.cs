
namespace Texter
{
    public interface ITextRenderer
    {
        /// <summary>
        /// Width of the renderer.
        /// </summary>
        uint Width { get; }

        /// <summary>
        /// Height of the renderer.
        /// </summary>
        uint Height { get; }

        /// <summary>
        /// Set a character in the renderer.
        /// </summary>
        void Set(int x, int y, Character character, bool useBlending = true);

        /// <summary>
        /// Get a character from the renderer.
        /// </summary>
        Character Get(int x, int y);
    }
}
