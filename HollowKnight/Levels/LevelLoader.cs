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
        public List<IEnemy> Enemies        { get; } = new();
        public List<IObject> Backgrounds   { get; } = new();
        public List<IObject> Platforms     { get; } = new();
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
                ["Background_1"]  = pos => new Background(1, pos),
                ["Background_2"] = pos => new Background(2, pos),
                // <Region name="Path_1" x="265" y="223" width="1060" height="83" />
                ["Path_1"]               = pos => new Path(1, pos, 1050, 32, hitOffsetY: 10),
                ["Path_2"]               = pos => new Path(2, pos, 940, 32, hitOffsetY: 40),
                ["Path_ledge"]           = pos => new PathLedge(pos),
                ["Spike_Floor_1"]        = pos => new Spike(SpikeVariant.Floor1, pos),
                ["Spike_Floor_2"]        = pos => new Spike(SpikeVariant.Floor2, pos),
                ["Spike_Ceiling"]        = pos => new Spike(SpikeVariant.Ceiling, pos),
                ["Bench"]               = pos => new Bench(pos),
                ["Plant1_Idle"]        = pos => new Grass(1,pos, 100, 5, hitOffsetY: 0),
                ["Plant2_Idle"]        = pos => new Grass(2,pos, 100, 5, hitOffsetY: 0),
                ["Wall_0"]             = pos => new Wall(0, pos, 80, 200),
                ["Wall_1"]             = pos => new Wall(1, pos, 80, 200),
                ["Wall_2"]             = pos => new Wall(2, pos, 80, 200),
                ["Door_0"]             = pos => new Door(pos, 60, 150),
                ["Brick_1"]            = pos => new Brick(1,pos,272,62),
                ["Brick_2"] = pos => new Brick(2, pos, 122, 39),
                ["Brick_3"] = pos => new Brick(3, pos, 274, 132),
                ["Brick_4"] = pos => new Brick(4, pos, 227, 61),
                ["Brick_5"] = pos => new Brick(5, pos, 345, 139),
                ["Brick_6"] = pos => new Brick(6, pos, 197, 127),
                ["Brick_7"] = pos => new Brick(7, pos, 81, 302),
                ["Brick_8"] = pos => new Brick(8, pos, 78, 280),

                ["MantisThrone_1"] = pos => new MantisThrone(1, pos, 119, 613),
                ["MantisThrone_2"] = pos => new MantisThrone(2, pos, 112, 374),

                ["Floor_1"] = pos => new Floor(1, pos, 249, 141),
                ["Floor_2"] = pos => new Floor(2, pos, 249, 141),
                ["Floor_3"] = pos => new Floor(3, pos, 625, 141),

                ["Flag_1"] = pos => new Flag(1, pos, 119, 622),
                ["Flag_2"] = pos => new Flag(2, pos, 102, 622),
                ["Flag_3"] = pos => new Flag(3, pos, 121, 621),
                ["Flag_4"] = pos => new Flag(4, pos, 104, 621),

                ["Village_1"] = pos => new Village(1, pos, 243, 507),
                ["Village_2"] = pos => new Village(2, pos, 262, 511),
                ["Village_3"] = pos => new Village(3, pos, 161, 520),
            };

            _enemyMap = new Dictionary<string, Func<Vector2, IEnemy>>
            {
                ["Crawlid"]    = pos => new Crawlid(pos),
                ["Vengefly"]   = pos => new Vengefly(pos),
                ["MantisLord"] = pos => new MantisLord(pos),
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

            AssignPlatformsToCrawlid();
            Console.WriteLine($"[LevelLoader] Loaded: {Enemies.Count} enemies, " +
                              $"{Platforms.Count} platforms. Knight spawns at {KnightSpawn}.");
        }

        private void AssignPlatformsToCrawlid()
        {
            foreach (var enemy in Enemies)
            {
                IObject assignedPlatform = null;
                float minVerticalDist = float.MaxValue;
                
                // Get bounds of the enemy (first hitbox)
                Rectangle[] crawlidHitboxes = enemy.GetBounds();
                if (crawlidHitboxes.Length == 0) continue;
                Rectangle crawlidBounds = crawlidHitboxes[0];
                float crawlidCenterX = crawlidBounds.Center.X;

                foreach (var platform in Platforms)
                {
                    Rectangle[] pHitboxes = platform.GetBounds();
                    if (pHitboxes.Length == 0) continue;
                    Rectangle pBounds = pHitboxes[0];

                    // Check if enemy is horizontally within platform
                    if (crawlidCenterX >= pBounds.Left && crawlidCenterX <= pBounds.Right)
                    {
                        float verticalDist = pBounds.Top - crawlidBounds.Bottom;
                        
                        // We want the platform to be BELOW or very close to the enemy
                        // Tolerance of 10 pixels for existing overlap/float
                        if (verticalDist >= -10 && verticalDist < minVerticalDist)
                        {
                            minVerticalDist = verticalDist;
                            assignedPlatform = platform;
                        }
                    }
                }
                
                if (assignedPlatform != null)
                {
                    enemy.SetPlatform(assignedPlatform);
                }
            }
        }

        private static readonly HashSet<string> BackgroundNames = new()
        {
            "Background_1", "Background_2"
        };

        private void SpawnPlatform(string name, Vector2 position)
        {
            if (!_platformMap.TryGetValue(name, out var create))
            {
                Console.WriteLine($"[LevelLoader] Unknown platform '{name}' at {position}");
                return;
            }

            IObject obj = create(position);
            if (BackgroundNames.Contains(name))
                Backgrounds.Add(obj);
            else
                Platforms.Add(obj);
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
