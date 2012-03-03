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

			example = new TextDisplay(132, 60);

			r = new Random();
		}

		public void Run()
		{
			while (running)
			{
				window.DispatchEvents();
				window.Clear(Color.White);
				example.Clear(Character.Create(0, 0, 0));



				example.Draw(window, new Vector2f(0, 0));
				window.Display();
			}
		}
	}
}
