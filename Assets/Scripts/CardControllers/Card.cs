using UnityEngine;
using UnityEngine.Serialization;

namespace CardControllers
{
    [CreateAssetMenu(menuName = "Card", order = 1)]
    public class Card : ScriptableObject
    {
        [SerializeField] private new string name;
        public string description;
        public Sprite artwork;
        public int quantity;
        public Color color;
        public int[] power;
        public int maxQuantityInGame;
    }
}