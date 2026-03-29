using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;
using HollowKnight.Environment;
using HollowKnight.Enemies;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HollowKnight.Levels
{
    public class LevelLoader
    {
        public List<IEnemy> Enemies     { get; } = new();
        public List<IObject> Platforms   { get; } = new();
        public Vector2 KnightSpawn { get; private set; } = Vector2.Zero;

        private readonly Dictionary<string, Func<Vector2, IObject>> _platformMap;
        private readonly Dictionary<string, Func<Vector2, IEnemy>>  _enemyMap;
        private readonly Dictionary<string, Action<string, Vector2>> _spawnMap;

        public LevelLoader()
        {
            _platformMap = new Dictionary<string, Func<Vector2, IObject>>
            {
                ["Tutorial_Platform_1"]  = pos => new TutorialPlatform(1, pos, 146, 147),
                ["Tutorial_Platform_2"]  = pos => new TutorialPlatform(2, pos, 149, 194),
                ["Tutorial_Platform_3"]  = pos => new TutorialPlatform(3, pos, 264, 79),
                ["Tutorial_Platform_4"]  = pos => new TutorialPlatform(4, pos, 89, 56),
                ["Tutorial_Platform_5"]  = pos => new TutorialPlatform(5, pos, 168, 93),
                ["Tutorial_Platform_6"]  = pos => new TutorialPlatform(6, pos, 138, 141),
                ["Tutorial_Platform_7"]  = pos => new TutorialPlatform(7, pos, 138, 228),
                ["Tutorial_Platform_8"]  = pos => new TutorialPlatform(8, pos, 105, 62),
                ["Tutorial_Platform_9"]  = pos => new TutorialPlatform(9, pos, 110, 62),
                ["Tutorial_Platform_10"] = pos => new TutorialPlatform(10, pos, 174, 70),
                ["Path_1"]               = pos => new Path(1, pos, 1050, 32, hitOffsetY: 10),
                ["Path_2"]               = pos => new Path(2, pos, 940, 32, hitOffsetY: 40),
                ["Path_ledge"]           = pos => new PathLedge(pos),
                ["Spike_Floor_1"]        = pos => new Spike(SpikeVariant.Floor1, pos),
                ["Spike_Floor_2"]        = pos => new Spike(SpikeVariant.Floor2, pos),
                ["Spike_Ceiling"]        = pos => new Spike(SpikeVariant.Ceiling, pos),
            };

            _enemyMap = new Dictionary<string, Func<Vector2, IEnemy>>
            {
                ["Crawlid"]  = pos => new Crawlid(pos),
                ["Vengefly"] = pos => new Vengefly(pos),
            };

            _spawnMap = new Dictionary<string, Action<string, Vector2>>
            {
                ["Knight"]   = (name, pos) => KnightSpawn = pos,
                ["Platform"] = SpawnPlatform,
                ["Enemy"]    = SpawnEnemy,
            };
        }

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

        private static Vector2 ParseVector2(string s)
        {
            string[] parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
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
