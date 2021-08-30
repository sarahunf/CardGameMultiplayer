using UnityEngine;
using UnityEngine.EventSystems;

namespace Interface
{
    public class DropSlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("DropEvent");
            if (eventData.pointerDrag != null && transform.childCount < GameSetup.Instance.players.Count)
                eventData.pointerDrag.GetComponent<Transform>().SetParent(transform);
            else
                eventData.pointerDrag.GetComponent<Transform>().position = eventData.pointerDrag.GetComponent<DragDrop>().originalPos ;
        }
    }
}