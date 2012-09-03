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

		TextDisplay example; 
		
		bool running = true;
		RenderWindow window;

		public Example()
		{
			// Initialize Texter
			TextDisplay.Initialize();

			// Create a TextDisplay to render onto our window
			example = new TextDisplay(Width, Height);

			// Setup an SFML window
			window = new RenderWindow(new VideoMode(Width * TextDisplay.CharacterWidth, Height * TextDisplay.CharacterHeight), "Texter Example", Styles.Close);
			window.SetFramerateLimit(30);
			window.Closed += (sender, e) =>
			{
				running = false;
			};
		}

		public void Run()
		{
			double time = 0;

			while (running)
			{
				// Normal SFML stuff
				window.DispatchEvents();
				window.Clear(Color.White);

				// Clear the TextDisplay to a Character, this is not required but I do it anyways
				example.Clear(Character.Blank);

				// Render our fractal, I think I got this code from Wikipedia and added zooming
				for (int y = 0; y < Height; y++)
				{
					for (int x = 0; x < Width; x++)
					{
						double x0 = (((double)x / Width) * 3.5) - 2.5;
						double y0 = (((double)y / Height) * 2.0) - 1;

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

						byte color = (byte)(((double)iteration / maxIteration) * 255);

						// Modifying the TextDisplay per Character
						example.Set(x, y, Character.Create(background: color));
					}
				}

				// Render a rectangle to contain our message
				example.DrawRectangle(22, 10, 19, 5, Character.Create(background: 128), Character.Create('@', foreground: 0));

				// And then we render our message onto the rectangle
				example.DrawText(25, 12, "Hello, world!", Character.Create(foreground: 0));

				// Render the TextDisplay to the SFML window
				example.Draw(window, new Vector2f(0, 0));

				// And finally have SFML display it to us
				window.Display();

				time += 0.1;
			}
		}
	}
}
