using System;
using SFML.Window;
using SFML.Graphics;
using Texter;

namespace Example
{
    class Example
    {
        const int Width = 160;
        const int Height = 60;

        TextDisplay example;
        RenderWindow window;

        public Example()
        {
            // Create a TextDisplay to render onto our window
            example = new TextDisplay(Width, Height);

            // Setup an SFML window
            window = new RenderWindow(new VideoMode(Width * example.CharacterWidth, Height * example.CharacterHeight), "Texter Example", Styles.Close);
            window.SetFramerateLimit(60);
            window.Closed += (sender, e) => window.Close();
        }

        public void Run()
        {
            double time = 0;
            Random random = new Random();

            while (window.IsOpen())
            {
                // Normal SFML stuff
                window.DispatchEvents();
                window.Clear(Color.White);

                // Clear the TextDisplay to a Character, this is not required but I do it anyways
                example.Clear(Character.Blank);

                // Lets leave a border to demo regions
                var region = example.Region(1, 1, Width - 2, Height - 2);

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
                        x0 /= 1 + Math.Sin(time) * 1;
                        y0 /= 1 + Math.Sin(time) * 1;

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

                // Render a rectangle to contain our message
                example.DrawRectangle(22, 10, 19, 5, new Character(' ', 128, 128), new Character('@', foreground: 255));

                // And then we render our message onto the rectangle
                example.DrawText(25, 12, "Hello, world!", new Character(foreground: 255));

                // Render the TextDisplay to the SFML window
                example.Draw(window, new Vector2f(0, 0));

                // And finally have SFML display it to us
                window.Display();

                time += 0.1;
            }
        }
    }
}
