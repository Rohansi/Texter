using System;
using SFML.Window;
using SFML.Graphics;
using Texter;

namespace Example
{
    class Example
    {
        const int Width = 80;
        const int Height = 25;

        private TextDisplay _example;
        private RenderWindow _window;

        public Example()
        {
            // Create a TextDisplay to render onto our window
            _example = new TextDisplay(Width, Height);

            // Setup an SFML window
            _window = new RenderWindow(new VideoMode(Width * _example.CharacterWidth, Height * _example.CharacterHeight), "Texter Example", Styles.Close);
            _window.SetFramerateLimit(60);
            _window.Closed += (sender, e) => _window.Close();
        }

        public void Run()
        {
            double time = 0;
            var random = new Random();

            while (_window.IsOpen())
            {
                // Normal SFML stuff
                _window.DispatchEvents();
                _window.Clear(Color.White);

                // Clear the TextDisplay to a Character, this is not required but I do it anyways
                _example.Clear(Character.Blank);

                // Lets leave a border to demo regions
                var region = _example.Region(1, 1, Width - 2, Height - 2);

                // Render our fractal, I think I got this code from Wikipedia and added zooming
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        double posX = (double)x / Width;
                        double posY = (double)y / Height;

                        double x0 = (posX * 3.5) - 2.5;
                        double y0 = (posY * 2.0) - 1;

                        // Zooming
                        x0 /= 1.5f + Math.Sin(time) * 0.5f;
                        y0 /= 1.5f + Math.Sin(time) * 0.5f;

                        double xx = 0;
                        double yy = 0;

                        const int maxIteration = 24;
                        int iteration = 0;

                        while (xx * xx + yy * yy < 2 * 2 && iteration < maxIteration)
                        {
                            double xtemp = xx * xx - yy * yy + x0;
                            yy = 2 * xx * yy + y0;
                            xx = xtemp;

                            iteration++;
                        }

                        double colorPercentage = (double)iteration / maxIteration;
                        byte color = (byte)(colorPercentage * 255);

                        // Modifying the TextDisplay per Character, through a region
                        region.Set(x, y, new Character(random.Next(255), color - 128, color));
                    }
                }

                // Effect that brightens the background color
                var effect = _example.Effect((x, y, c) => new Character(c.Glyph, c.Foreground, Math.Max(c.Background - 128, 0)));

                // Render a box with our effect
                effect.DrawBox(2, 2, 19, 5, TextExtensions.SingleBox, new Character(foreground: 255));

                // And then we render our message onto the box
                _example.DrawText(5, 4, "Hello, world!", new Character(foreground: 255));

                // Render the TextDisplay to the SFML window
                _example.Draw(_window);

                // And finally have SFML display it to us
                _window.Display();

                time += 0.1;
            }
        }
    }
}
