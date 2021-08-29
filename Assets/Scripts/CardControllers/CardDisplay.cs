using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardControllers
{
    public class CardDisplay : MonoBehaviour
    {
        public Card card;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Image artworkImage;
        [SerializeField] private Image quantityArtwork;
        [SerializeField] private Transform quantityParent;
        [SerializeField] private Image background;

        private void Awake()
        {
            //show all card references
            nameText.text = card.name;
            descriptionText.text = card.description;
            background.color = card.color;
            if (artworkImage != null)
                artworkImage.sprite = card.artwork;
            InstantiateQuantityReference();
        }

        private void InstantiateQuantityReference()
        {
            for (var i = 0; i < card.quantity; i++)
            {
                quantityArtwork.sprite = artworkImage.sprite;
                Instantiate(quantityArtwork, quantityParent);
            }
        }
    }
}