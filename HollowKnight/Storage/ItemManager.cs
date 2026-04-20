using System.Collections.Generic;

namespace HollowKnight.Storage
{
    public static class ItemManager
    {
        public const string VengefulSpiritItemName = "Vengeful Spirit";

        private static int _currentIndex = 0;

        // Placeholder list — replace with real items later
        private static List<string> _items = new List<string>()
        {
            VengefulSpiritItemName,
            "Boomerang",
            "Bomb"
        };

        public static IReadOnlyList<string> Items => _items;
        public static int CurrentIndex => _currentIndex;
        public static string CurrentItem => _items[_currentIndex];

        public static void NextItem()
        {
            _currentIndex = (_currentIndex + 1) % _items.Count;
        }

        public static void PreviousItem()
        {
            _currentIndex = (_currentIndex - 1 + _items.Count) % _items.Count;
        }

        public static void SelectItem(int index)
        {
            if (index < 0 || index >= _items.Count)
            {
                return;
            }

            _currentIndex = index;
        }

        public static void Reset()
        {
            _currentIndex = 0;
        }
    }
}
