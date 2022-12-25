using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{

    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;
    private bool isCrafting = false;

    public List<string> inventoryItemList = new List<string>();

    //Category buttons
    Button toolsBTN;

    //Craft buttons
    Button craftAxeBTN;

    //Requirement Text
    Text axeReq1, axeReq2;

    public bool isOpen;

    //All Blueprints
    public Blueprint AxeBLP = new Blueprint("Axe", 2, "Stone", 1, "Stick", 1);

    public static CraftingSystem Instance {get; set;}

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } 
        else 
        {
            Instance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate{OpenToolsCategory();});

        //Axe
        axeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("Req1").GetComponent<Text>();
        axeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("Req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate{
            if (!isCrafting) {
            CraftAnyItem(AxeBLP);
            }
        });


    }

    void CraftAnyItem(Blueprint blueprintToCraft) {
    Debug.Log("Crafting item: " + blueprintToCraft.itemName);

    // Set the crafting flag to true
    isCrafting = true;

    // Remove required items from the inventory
    if (blueprintToCraft.numOfRequirements == 1) {
        InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
    } else if (blueprintToCraft.numOfRequirements == 2) {
        InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
    } else if (blueprintToCraft.numOfRequirements == 3) {

    } else if (blueprintToCraft.numOfRequirements == 4) {

    }

    // Add crafted item to the inventory
    InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);

    // Recalculate the inventory list
    StartCoroutine(calculate(0));

    // Refresh the needed items display
    RefreshNeededItems();

    // Wait for a few seconds before setting the crafting flag to false
    StartCoroutine(WaitForCrafting(2f));
}

private IEnumerator WaitForCrafting(float delay) {
    yield return new WaitForSeconds(delay);
    isCrafting = false;
}


    public IEnumerator calculate(float delay) {
        yield return new WaitForSeconds(delay);
        InventorySystem.Instance.ReCalculateList();
    }


    void OpenToolsCategory() {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        RefreshNeededItems();
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
 
            
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
 
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen) {
                Cursor.lockState = CursorLockMode.Locked;
            }
            
            isOpen = false;
        }
    }
    public void RefreshNeededItems() {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList) {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }

        }
        axeReq1.text = "1 Stone [" + stone_count + "]";
    axeReq2.text = "1 Stick [" + stick_count + "]";
    
    if (stone_count >= 1 && stick_count >=1) {
        craftAxeBTN.gameObject.SetActive(true);
    } else {
        craftAxeBTN.gameObject.SetActive(false);
    }
    }
    
}
