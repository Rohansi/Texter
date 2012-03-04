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

				int o = (int)(Math.Sin(t) * 10);
				Rectangle(20 + o, 20 + o, 60 - o, 40 - o);
				Rectangle(70 - o, 20 - o, 110 + o, 40 + o);

				Rectangle(41, 20, 89, 40);

				example.DrawString(2, 2, "Hello, world!");

				example.Draw(window, new Vector2f(0, 0));
				window.Display();

				t += 0.35;
			}
		}

		void Rectangle(int x1, int y1, int x2, int y2)
		{
			for (int y = y1; y < y2; y++)
				for (int x = x1; x < x2; x++)
					example.Set(x, y, Character.Create((byte)r.Next(32, 126), (byte)r.Next(15), 0));
		}
	}
}
