using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class InventorySystem : MonoBehaviour
{
 
   public static InventorySystem Instance { get; set; }
 
    public GameObject inventoryScreenUI;
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;
    public bool isOpen;
    // public bool isFull;
    public int inventorySize;
 
 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
 
 
    void Start()
    {
        isOpen = false;
        PopulateSlotList();
    }
 
    private void PopulateSlotList() {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot")) {
                slotList.Add(child.gameObject);
            }
            inventorySize = slotList.Count;


        }
    }
 
    void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
 
            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
 
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen) {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;
        }
    }

    public void AddToInventory(string itemName) {
            whatSlotToEquip = FindNextEmptySlot();
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            itemList.Add(itemName);
        
    }

    private GameObject FindNextEmptySlot() {
        foreach(GameObject slot in slotList) {
            if(slot.transform.childCount == 0) {
                return slot;
            }
        }
        return new GameObject();

    }

    public bool CheckIfFull() {
        int counter = 0;

        foreach (GameObject slot in slotList) {
            if (slot.transform.childCount > 0) {
                counter += 1;
            }
        }
        if (counter == inventorySize) {
                return true;
            } else {
                return false;
            }
    }
    
    public void RemoveItem(string nameToRemove, int amountToRemove) {
    var itemsToRemove = slotList
        .Where(s => s.transform.childCount > 0 && s.transform.GetChild(0).name == nameToRemove + "(Clone)")
        .Take(amountToRemove);
        
    foreach (var item in itemsToRemove) {
        Destroy(item.transform.GetChild(0).gameObject);
    }
}

    public void ReCalculateList() {
        itemList.Clear();
        foreach (GameObject slot in slotList) {
            if (slot.transform.childCount > 0) {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                itemList.Add(result);
            }
        }
    }

}