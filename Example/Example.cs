using System;
using SFML.Window;
using SFML.Graphics;
using Texter;

namespace Example
{
	class Example
	{
		const int Width = 132;
		const int Height = 60;

		bool running = true;
		RenderWindow window;
		TextDisplay example;

		Random r;

		public Example()
		{
			window = new RenderWindow(new VideoMode(Width * TextDisplay.CharacterWidth,
				Height * TextDisplay.CharacterHeight), "", Styles.Close);

			window.SetFramerateLimit(30);

			window.Closed += (sender, e) =>
			{
				running = false;
			};

			example = new TextDisplay(Width, Height);

			r = new Random();
		}

		public void Run()
		{
			double t = 0;

			while (running)
			{
				window.DispatchEvents();
				window.Clear(Color.White);
				example.Clear(Character.Create(0, 0, 0));

				for (int y = 0; y < Height; y++)
				{
					for (int x = 0; x < Width; x++)
					{
						double x0 = (((double)x / Width) * 3.5) - 2.5;
						double y0 = (((double)y / Height) * 2.0) - 1;

						// zooming
						x0 /= 1 + Math.Sin(t) * 1;
						y0 /= 1 + Math.Sin(t) * 1;

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

						example.Set(x, y, Character.Create(32, 0, (byte)(((double)iteration / maxIteration) * 255)));
					}
				}

				example.Draw(window, new Vector2f(0, 0));
				window.Display();

				t += 0.1;
			}
		}
	}
}
