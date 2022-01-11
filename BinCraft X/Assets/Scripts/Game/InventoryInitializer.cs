using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInitializer : MonoBehaviour
{
    public List<ItemStack> stacks = new List<ItemStack>();

    void Start()
    {
        Inventory inventory = Inventory.instance;
        foreach (ItemStack itemStack in stacks)
        {
            if (itemStack.data && itemStack.amount > 0)
            {
                inventory.AddItem(itemStack.data, itemStack.amount);
            }
        }

        Destroy(gameObject);
    }
}
