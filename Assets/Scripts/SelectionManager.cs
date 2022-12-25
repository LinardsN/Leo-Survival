//SelectionManager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectionManager : MonoBehaviour
{

    public static SelectionManager Instance { get; set; }
    public Image centerDotIcon;
    public Image handIcon;
    public bool onTarget;
    public GameObject selectedObject;
 
    public GameObject interaction_Info_UI;
    Text interaction_text;
 
    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } 
        else 
        {
            Instance = this;
        }

    }

void Update()
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit))
    {
        var selectionTransform = hit.transform;

        InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
 
        if (interactable && interactable.playerInRange)
        {
            if (selectionTransform.GetComponent<PickableObject>())
            {
                // Object has both InteractableObject and PickableObject scripts
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
                if (interactable.CompareTag("pickable")) {
                    centerDotIcon.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                } else {
                    handIcon.gameObject.SetActive(false);
                    centerDotIcon.gameObject.SetActive(true);
                }
            }
            else
            {
                // Object has InteractableObject script but not PickableObject script
                onTarget = false;
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
            }
        }
        else 
        { 
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            handIcon.gameObject.SetActive(false);
            centerDotIcon.gameObject.SetActive(true);
        }
 
 
    } else {
        onTarget = false;
        interaction_Info_UI.SetActive(false);
        handIcon.gameObject.SetActive(false);
        centerDotIcon.gameObject.SetActive(true);
    }
}


}
