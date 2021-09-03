using UnityEngine;
using UnityEngine.Serialization;

namespace CardControllers
{
    [CreateAssetMenu(menuName = "Card", order = 1)]
    public class Card : ScriptableObject
    {
        public new string name;
        public string description;
        public Sprite artwork;
        public int quantity;
        public Color color;
        public int maxValue;
        public int minValue;
        public int maxQuantityInGame;
    }
}