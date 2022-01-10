using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct ItemStack
{
    public DataItem data;
    public int amount;
}

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public UnityEvent Changed;

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

        bool changed = false;

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
                            changed = true;
                        }
                        else
                        {
                            stack.amount += amount;
                            Changed.Invoke();
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
                        changed = true;
                    }
                    else
                    {
                        stack.amount += amount;
                        Changed.Invoke();
                        return 0;
                    }
                }
            }
        }

        // if items still remain (amount > 0), return that amount
        Debug.Log(string.Format("<color=orange>Cannot pickup {0} {1}; inventory full!</color>", amount, data.name));
        if (changed)
        {
            Changed.Invoke();
        }
        return amount;
    }

    public bool HasItem(DataItem data)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                ref ItemStack stack = ref grid[x, y];
                if (stack.data == data && stack.amount > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void RemoveItem(DataItem data)
    {
        for (int x = width - 1; x >= 0; x--)
        {
            for (int y = height - 1; y >= 0; y--)
            {
                ref ItemStack stack = ref grid[x, y];
                if (stack.data == data && stack.amount > 0)
                {
                    stack.amount--;
                    if (stack.amount == 0)
                    {
                        stack.data = null;
                    }
                    Changed.Invoke();
                    return;
                }
            }
        }
    }

    public int GetItemCount(DataItem data)
    {
        int count = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                ref ItemStack stack = ref grid[x, y];
                if (stack.data == data)
                {
                    count += stack.amount;
                }
            }
        }

        return count;
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

    public ItemStack GetItemStack(int x, int y)
    {
        return grid[x, y];
    }

    public void ClearItemStack(int x, int y)
    {
        grid[x, y].data = null;
        grid[x, y].amount = 0;
        Changed.Invoke();
    }

    public void SwapItemStacks(int xFrom, int yFrom, int xTo, int yTo)
    {
        ItemStack temp = grid[xTo, yTo];
        grid[xTo, yTo] = grid[xFrom, yFrom];
        grid[xFrom, yFrom] = temp;
        Changed.Invoke();
    }

    public void MergeOrSwapItemStacks(int xFrom, int yFrom, int xTo, int yTo)
    {
        ref ItemStack from = ref grid[xFrom, yFrom];
        ref ItemStack to = ref grid[xTo, yTo];

        // if from is empty, abort
        if (!from.data)
        {
            return;
        }

        // if to is empty, swap
        if (!to.data)
        {
            SwapItemStacks(xFrom, yFrom, xTo, yTo);
            return;
        }

        // if same datas, merge, otherwise swap
        if (from.data == to.data)
        {
            // merge stacks

            int maxStackCount = from.data.maxStackCount;
            int available = maxStackCount - to.amount;
            if (from.amount > available)
            {
                to.amount = maxStackCount;
                from.amount -= available;
            }
            else
            {
                to.amount += from.amount;
                from.amount = 0;
                from.data = null;
            }
            Changed.Invoke();
        }
        else
        {
            // swap stacks
            SwapItemStacks(xFrom, yFrom, xTo, yTo);
        }
    }
}
