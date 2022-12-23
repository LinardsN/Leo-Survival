using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

 
public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
 
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
 
    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;
 
 
 
    private void Awake()
{
    rectTransform = GetComponent<RectTransform>();
    canvasGroup = GetComponent<CanvasGroup>();
    canvas = GameObject.FindObjectOfType<Canvas>();
}

 
 
public GameObject dragParent;

public void OnBeginDrag(PointerEventData eventData)
{
    Debug.Log("OnBeginDrag");

    canvasGroup.alpha = .6f;
    canvasGroup.blocksRaycasts = false;
    startPosition = transform.position;
    startParent = transform.parent;

    // Check if the dragParent field has been set
    if (dragParent == null)
    {
        // If it hasn't been set, create a new, empty game object to be the parent of the item being dragged
        dragParent = new GameObject("Drag Parent");
        dragParent.transform.SetParent(canvas.transform);
    }

    // Set the parent of the item being dragged to the dragParent game object, preserving its position in the world space
    transform.SetParent(dragParent.transform, worldPositionStays: true);
    itemBeingDragged = gameObject;
}




 
    public void OnDrag(PointerEventData eventData)
    {
        //So the item will move with our mouse (at same speed)  and so it will be consistant if the canvas has a different scale (other then 1);
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
 
    }
 
 
 
public void OnEndDrag(PointerEventData eventData)
{
    itemBeingDragged = null;

    // Check if the item was dropped in a slot
    if (eventData.pointerEnter != null && eventData.pointerEnter.tag == "Slot")
    {
        // Set the parent of the item being dragged to the slot object
        transform.SetParent(eventData.pointerEnter.transform);

        // Update the position of the item to the position of the slot
        transform.position = eventData.pointerEnter.transform.position;
    }
    else
    {
        // Set the parent of the item being dragged to the original parent object
        transform.SetParent(startParent);

        // Reset the position of the item to the original position
        transform.position = startPosition;
    }

    Debug.Log("OnEndDrag");
    canvasGroup.alpha = 1f;
    canvasGroup.blocksRaycasts = true;
}






 
 
 
}