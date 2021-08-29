using UnityEngine;
using UnityEngine.EventSystems;

namespace Interface
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("DropEvent");
            if (eventData.pointerDrag != null)
            {
                eventData.pointerDrag.GetComponent<Transform>().SetParent(transform);
            }
        }
    }
}
