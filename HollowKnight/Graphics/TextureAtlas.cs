using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Graphics
{
	public class TextureAtlas
	{
		private Dictionary<string, TextureRegion> _regions;

		public Texture2D Texture { get; set; }

		public TextureAtlas()
		{
			_regions = new Dictionary<string, TextureRegion>();
		}

		public TextureAtlas(Texture2D texture)
		{
			Texture = texture;
			_regions = new Dictionary<string, TextureRegion>();
		}

		public void AddRegion(string name, int x, int y, int width, int height)
		{
			TextureRegion region = new TextureRegion(Texture, x, y, width, height);
			_regions.Add(name, region);
		}

		public TextureRegion GetRegion(string name)
		{
			return _regions[name];
		}

		public bool RemoveRegion(string name)
		{
			return _regions.Remove(name);
		}

		public void Clear()
		{
			_regions.Clear();
		}

		/// <summary>
		/// Gets all animation frames matching a prefix (e.g. "Walking" finds "Walking_0", "Walking_1", ...).
		/// Returns the source rectangles as an array for use with AnimatedSprite.
		/// </summary>
		public Rectangle[] GetAnimationFrames(string prefix)
		{
			List<Rectangle> frames = new List<Rectangle>();
			int i = 0;
			while (_regions.ContainsKey($"{prefix}_{i}"))
			{
				frames.Add(_regions[$"{prefix}_{i}"].SourceRectangle);
				i++;
			}
			return frames.ToArray();
		}

		public static TextureAtlas FromFile(ContentManager content, string fileName)
		{
			TextureAtlas atlas = new TextureAtlas();

			string filePath = Path.Combine(content.RootDirectory, fileName);

			using (Stream stream = TitleContainer.OpenStream(filePath))
			{
				using (XmlReader reader = XmlReader.Create(stream))
				{
					XDocument doc = XDocument.Load(reader);
					XElement root = doc.Root;

					string texturePath = root.Element("Texture").Value;
					atlas.Texture = content.Load<Texture2D>(texturePath);

					var regions = root.Element("Regions")?.Elements("Region");

					if (regions != null)
					{
						foreach (var region in regions)
						{
							string name = region.Attribute("name")?.Value;
							int x = int.Parse(region.Attribute("x")?.Value ?? "0");
							int y = int.Parse(region.Attribute("y")?.Value ?? "0");
							int width = int.Parse(region.Attribute("width")?.Value ??
									"0");
							int height = int.Parse(region.Attribute("height")?.Value ??
									"0");

							if (!string.IsNullOrEmpty(name))
							{
								atlas.AddRegion(name, x, y, width, height);
							}
						}
					}

					return atlas;
				}
			}
		}
	}
}
