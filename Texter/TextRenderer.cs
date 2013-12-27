
namespace Texter
{
    public interface ITextRenderer
    {
        uint Width { get; }
        uint Height { get; }

        void Set(int x, int y, Character character, bool useBlending = true);
        Character Get(int x, int y);
    }
}
