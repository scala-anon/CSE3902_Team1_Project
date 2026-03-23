using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HollowKnight
{
    public class LevelLoader
    {
        // lists for all the enemies and platforms 
        public List<IEnemy> Enemies     { get; } = new();
        public List<IObject> Platforms   { get; } = new();
        public Vector2 KnightSpawn { get; private set; } = Vector2.Zero;

        // maps the object name to a factory method 
        private readonly Dictionary<string, Func<Vector2, IObject>> _platformMap;
        private readonly Dictionary<string, Func<Vector2, IEnemy>>  _enemyMap;

        // maps object type to
        private readonly Dictionary<string, Action<string, Vector2>> _spawnMap;

        // constructor, and builds dictionaries
        public LevelLoader()
        {
            _platformMap = new Dictionary<string, Func<Vector2, IObject>>
            {
                // reflection if want to make better 
                ["Tutorial_Platform_1"]  = pos => new Tutorial_Platform_1(pos),
                ["Tutorial_Platform_2"]  = pos => new Tutorial_Platform_2(pos),
                ["Tutorial_Platform_3"]  = pos => new Tutorial_Platform_3(pos),
                ["Tutorial_Platform_4"]  = pos => new Tutorial_Platform_4(pos),
                ["Tutorial_Platform_5"]  = pos => new Tutorial_Platform_5(pos),
                ["Tutorial_Platform_6"]  = pos => new Tutorial_Platform_6(pos),
                ["Tutorial_Platform_7"]  = pos => new Tutorial_Platform_7(pos),
                ["Tutorial_Platform_8"]  = pos => new Tutorial_Platform_8(pos),
                ["Tutorial_Platform_9"]  = pos => new Tutorial_Platform_9(pos),
                ["Tutorial_Platform_10"] = pos => new Tutorial_Platform_10(pos),
                ["Path_1"]               = pos => new Path_1(pos),
                ["Path_2"]               = pos => new Path_2(pos),
                ["Path_ledge"]           = pos => new Path_ledge(pos),
                ["Spike_Floor_1"]        = pos => new Spike(pos),
                ["Spike_Floor_2"]        = pos => new FloorSpike(pos),
                ["Spike_Ceiling"]        = pos => new CeilingSpike(pos),
            };

            _enemyMap = new Dictionary<string, Func<Vector2, IEnemy>>
            {
                ["Crawlid"]  = pos => new Crawlid(pos),
                ["Vengefly"] = pos => new Vengefly(pos),
            };

            // head dictionary, decides where things go
            _spawnMap = new Dictionary<string, Action<string, Vector2>>
            {
                ["Knight"]   = (name, pos) => KnightSpawn = pos,
                ["Platform"] = SpawnPlatform,
                ["Enemy"]    = SpawnEnemy,
            };
        }

        // parse then spawn 
        public void Load(string xmlFilePath)
        {
            XDocument doc  = XDocument.Load(xmlFilePath);
            XElement  root = doc.Root
                ?? throw new Exception($"[LevelLoader] Bad XML root in {xmlFilePath}");

            foreach (XElement item in root.Elements("Item"))
            {
                string  objectType = item.Element("ObjectType")?.Value?.Trim() ?? "";
                string  objectName = item.Element("ObjectName")?.Value?.Trim() ?? "";
                string  locStr     = item.Element("Location")?.Value?.Trim()   ?? "0 0";
                Vector2 position   = ParseVector2(locStr);

                if (_spawnMap.TryGetValue(objectType, out var spawn))
                    spawn(objectName, position);
                else
                    Console.WriteLine($"[LevelLoader] Unknown ObjectType '{objectType}'");
            }

            Console.WriteLine($"[LevelLoader] Loaded: {Enemies.Count} enemies, " +
                              $"{Platforms.Count} platforms. Knight spawns at {KnightSpawn}.");
        }

        // Spawn functions
        private void SpawnPlatform(string name, Vector2 position)
        {
            if (!_platformMap.TryGetValue(name, out var create))
            {
                Console.WriteLine($"[LevelLoader] Unknown platform '{name}' at {position}");
                return;
            }
            Platforms.Add(create(position));
        }

        private void SpawnEnemy(string name, Vector2 position)
        {
            if (!_enemyMap.TryGetValue(name, out var create))
            {
                Console.WriteLine($"[LevelLoader] Unknown enemy '{name}' at {position}");
                return;
            }
            Enemies.Add(create(position));
        }

        // Parses the x and y from xml
        private static Vector2 ParseVector2(string s)
        {
            string[] parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            // if everything goes well
            if (parts.Length >= 2 &&
                float.TryParse(parts[0], out float x) &&
                float.TryParse(parts[1], out float y))
            {
                return new Vector2(x, y);
            }

            Console.WriteLine($"[LevelLoader] Could not parse Vector2 from '{s}', defaulting to zero.");
            return Vector2.Zero;
        }
    }
}
