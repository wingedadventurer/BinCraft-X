using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct ItemStack
{
    public DataItem data;
    public int amount;
}

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public int width;
    public int height;

    private ItemStack[,] grid;

    private void Awake()
    {
        instance = this;

        grid = new ItemStack[width, height];
    }

    // returns remaining item count if it didn't fit the inventory
    public int AddItem(DataItem data, int amount)
    {
        if (amount == 0) { return 0; }

        // if stacks of this item already exist, fill them until depleted
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                ref ItemStack stack = ref grid[x, y];
                if (stack.data == data)
                {
                    int amountRemaining = stack.data.maxStackCount - stack.amount;
                    if (amountRemaining > 0)
                    {
                        if (amount > amountRemaining)
                        {
                            stack.amount += amountRemaining;
                            amount -= amountRemaining;
                        }
                        else
                        {
                            stack.amount += amount;
                            return 0;
                        }
                    }
                }
            }
        }

        // if items still remain, create new stacks
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                ref ItemStack stack = ref grid[x, y];
                if (stack.data == null)
                {
                    stack.data = data;

                    if (amount > stack.data.maxStackCount)
                    {
                        stack.amount += stack.data.maxStackCount;
                        amount -= stack.data.maxStackCount;
                    }
                    else
                    {
                        stack.amount += amount;
                        return 0;
                    }
                }
            }
        }

        // if items still remain (amount > 0), return that amount
        Debug.Log(string.Format("<color=orange>Cannot pickup {0} {1}; inventory full!</color>", amount, data.name));
        return amount;
    }

    public void Print()
    {
        string s = "inventory:\n";

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                ref ItemStack stack = ref grid[x, y];
                s += string.Format("[{0}][{1}] ", x, y);

                s += stack.data ? stack.data.name : "X";
                s += " (" + stack.amount + ")";
                s += "\n";
            }
        }
        s += "\n";

        Debug.Log(s);
    }
}
