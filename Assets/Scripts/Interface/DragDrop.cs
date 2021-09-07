using UnityEngine;
using UnityEngine.EventSystems;

namespace Interface
{
    public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Canvas canvas;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        internal Vector3 originalPos;

        private void Awake()
        {
            canvas = GameManager.Instance.canvas;
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originalPos = gameObject.transform.position;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = .6f;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    
        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
    
        public void OnPointerDown(PointerEventData eventData)
        {
        }
    }
}