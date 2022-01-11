using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct ItemStack
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

    private ItemStack dragStack;

    [HideInInspector] public UnityEvent Changed;
    [HideInInspector] public UnityEvent DragStackChanged;

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

    public void SetStack(int x, int y, DataItem data, int amount)
    {
        if (!data || amount == 0)
        {
            grid[x, y].data = null;
            grid[x, y].amount = 0;
        }
        else
        {
            grid[x, y].data = data;
            grid[x, y].amount = amount;
        }

        Changed.Invoke();
    }

    public void SetDragStack(DataItem data, int amount)
    {
        dragStack.data = data;
        dragStack.amount = amount;
        DragStackChanged.Invoke();
    }

    public ItemStack GetDragStack()
    {
        return dragStack;
    }

    public void ClearDragStack()
    {
        dragStack.data = null;
        dragStack.amount = 0;

        DragStackChanged.Invoke();
    }

    public void SwapItemStacks(int xFrom, int yFrom, int xTo, int yTo)
    {
        ItemStack temp = grid[xTo, yTo];
        grid[xTo, yTo] = grid[xFrom, yFrom];
        grid[xFrom, yFrom] = temp;
        Changed.Invoke();
    }

    public void MergeOrSwapDragStack(int xFrom, int yFrom, int xTo, int yTo)
    {
        // if drag is empty, abort
        if (!dragStack.data)
        {
            return;
        }

        ref ItemStack from = ref grid[xFrom, yFrom];
        ref ItemStack to = ref grid[xTo, yTo];

        // if to is empty, drop
        if (!to.data)
        {
            to.data = dragStack.data;
            to.amount = dragStack.amount;
            dragStack.data = null;
            Changed.Invoke();
            DragStackChanged.Invoke();
            return;
        }

        // if from is empty, merge

        // if same datas, merge
        if (dragStack.data == to.data)
        {
            int maxStackCount = to.data.maxStackCount;
            int available = maxStackCount - dragStack.amount;
            if (dragStack.amount > available)
            {
                // excess, return some to previous stack
                to.amount = maxStackCount;
                dragStack.amount -= available;
                from.data = dragStack.data;
                from.amount += dragStack.amount;
            }
            else
            {
                // empty the drag stack
                to.amount += dragStack.amount;
            }
            ClearDragStack();
            Changed.Invoke();
        }
        // otherwise, if split return to original
        else if (from.data == dragStack.data)
        {
            from.amount += dragStack.amount;

            ClearDragStack();
            Changed.Invoke();
        }
        // otherwise, swap
        else
        {
            from.data = to.data;
            from.amount = to.amount;
            to.data = dragStack.data;
            to.amount = dragStack.amount;

            ClearDragStack();
            Changed.Invoke();
        }
    }

    public void SplitStack(int xFrom, int yFrom, int xTo, int yTo)
    {
        ref ItemStack from = ref grid[xFrom, yFrom];
        ref ItemStack to = ref grid[xTo, yTo];

        // skip splitting from empty stack
        if (!from.data) { return; }
        if (from.amount == 0) { return; }

        // skip splitting to stack of different item
        if (to.data && from.data != to.data) { return; }

        // if target stack is empty, make it of same type
        if (!to.data) { to.data = from.data; }

        int amount = from.amount / 2 + from.amount % 2;
        int free = to.data.maxStackCount - to.amount;

        if (amount > free)
        {
            to.amount = to.data.maxStackCount;
            from.amount -= free;
        }
        else
        {
            to.amount += amount;
            from.amount -= amount;
            if (from.amount == 0)
            {
                from.data = null;
            }
        }

        Changed.Invoke();
    }

    public void ReturnDragStack(int xTo, int yTo)
    {
        if (!dragStack.data) { return; }

        ref ItemStack to = ref grid[xTo, yTo];
        if (to.data && to.data != dragStack.data) { return; }

        to.data = dragStack.data;
        to.amount += dragStack.amount;

        ClearDragStack();
        Changed.Invoke();
    }
}
