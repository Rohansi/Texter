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
				example.Clear(Character.Create(' ', 0, 0));

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

						int iteration = 0;
						int maxIteration = 24;

						while (xx * xx + yy * yy < 2 * 2 && iteration < maxIteration)
						{
							double xtemp = xx * xx - yy * yy + x0;
							yy = 2 * xx * yy + y0;
							xx = xtemp;

							iteration++;
						}

						byte color = (byte)(((double)iteration / maxIteration) * 255);

						// Modifying the TextDisplay per Character
						example.Set(x, y, Character.Create(' ', 0, color));
					}
				}

				// And modifying the TextDisplay with DrawText, black with a transparent background
				example.DrawText(25, 12, "Hello, world!", 0);

				// Render the TextDisplay to the SFML window
				example.Draw(window, new Vector2f(0, 0));

				// And finally have SFML display it to us
				window.Display();

				time += 0.1;
			}
		}
	}
}
