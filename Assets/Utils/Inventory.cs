using LitJson;
using PixelCrushers.DialogueSystem;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBase
{
    public string name;
    public string displayName;
    public string description;
}
public class ItemInfo: InfoBase
{
    public int amount;
    public int startValue;
}
public class AllItemInfo
{
    public List<ItemInfo> resources;
    public List<ItemInfo> tool; 
}
public class Inventory : Singleton<Inventory>
{
    public Dictionary<string, ItemInfo> itemDict = new Dictionary<string, ItemInfo>();

    public int inventoryUnlockedCellCount = 2;
    public string selectedItemName;

    public void addInventoryUnlockedCell()
    {
        inventoryUnlockedCellCount++;

        EventPool.Trigger("inventoryChanged");
    }
    public bool canAddItem(string itemName, int value = 1)
    {
        return true;
        //if (itemValueDict.ContainsKey(itemName))
        //{
        //    return true;
        //}
        //if (itemValueDict.Count < inventoryUnlockedCellCount)
        //{
        //    return true;
        //}
        //return false;
    }


    public void updateSelectedItem(string name)
    {
        selectedItemName = name;

        DialogueLua.SetVariable("holdingItem", name);
    }

    public void sendGift()
    {
        if (selectedItemName == "")
        {
            Debug.LogError("you send what a you send");
            return;
        }
        //sometimes should not be able to send
        consumeItem(selectedItemName, 1);

        selectedItemName = "";

        EventPool.Trigger("inventoryChanged");
    }

    public void select(string name)
    {
        if(selectedItemName == name)
        {
            updateSelectedItem("");
        }
        else
        {

            updateSelectedItem(name);
        }
        EventPool.Trigger("inventoryChanged");
    }

    public void addItem(string itemName)
    {
        addItem(itemName, 1);
    }
    public void addItem(string itemName, int value)
    {
        if (!canAddItem(itemName, value))
        {
            Debug.LogError("can't add item " + itemName);
        }
        if (itemDict.ContainsKey(itemName))
        {
            itemDict[itemName].amount += value;
        }
        //else if (itemDict.Count < inventoryUnlockedCellCount)
        //{
        //    itemDict[itemName] = value;
        //}
        EventPool.Trigger("inventoryChanged");
    }

    public void consumeItem(string itemName, int value = 1)
    {
        if(!itemDict.ContainsKey(itemName) || itemDict[itemName].amount < value)
        {
            if (CheatManager.Instance.hasUnlimitResource)
            {
                return;
            }
            Debug.LogError("not enough item to consume");
            return;
        }
        itemDict[itemName].amount -= value;

        //if (itemValueDict[itemName] <= 0)
        //{
        //    itemValueDict.Remove(itemName);
        //}
        EventPool.Trigger("inventoryChanged");
    }


    public int itemAmount(string itemName)
    {
        return itemDict.ContainsKey(itemName) ? itemDict[itemName].amount : 0;
    }
    public bool hasItemAmount(string itemName,int amount)
    {
        if (CheatManager.Instance.hasUnlimitResource)
        {
            return true;
        }
        return itemDict.ContainsKey(itemName) && itemDict[itemName].amount >= amount;
    }
    public bool hasItem(string itemName)
    {
        return hasItemAmount(itemName, 1);
    }
    // Start is called before the first frame update
    void Awake()
    {
        string text = Resources.Load<TextAsset>("json/Inventory").text;
        var allNPCs = JsonMapper.ToObject<AllItemInfo>(text);
        foreach (ItemInfo info in allNPCs.resources)
        {
            itemDict[info.name] = info;
            info.amount = info.startValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach(var key in itemDict.Keys)
            {
                //if (!itemDict[key].noItemCollected)
                {
                    itemDict[key].amount += 1;
                }
            }
        }
    }
}
