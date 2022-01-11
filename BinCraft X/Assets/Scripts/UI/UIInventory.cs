using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public static UIInventory instance;

    public GameObject panel;

    [SerializeField] private GameObject prefabInventorySlot;
    [SerializeField] private GameObject containerInventorySlot;
    [SerializeField] private GridLayoutGroup containerGridLayoutGroup;
    [SerializeField] private RectTransform rectTransformSlotDrag;

    private UIInventorySlot[,] slots;
    [SerializeField] private UIInventorySlot slotDrag;

    private UIInventorySlot slotHovered;
    private UIInventorySlot slotDragged;

    private Inventory inventory;

    private bool willClearDragged;

    private bool shiftDragged;

    private void Awake()
    {
        instance = this;
        inventory = Inventory.instance;

        inventory.Changed.AddListener(UpdateSlots);
        inventory.DragStackChanged.AddListener(UpdateDragSlot);

        AddSlots();
        UpdateSlots();
        UpdateDragSlot();
    }

    private void Update()
    {
        // move mouse slot to mouse + add offset
        rectTransformSlotDrag.position = Input.mousePosition;
        // move to bottom-right
        rectTransformSlotDrag.position += new Vector3(rectTransformSlotDrag.rect.size.x, -rectTransformSlotDrag.rect.size.y, 0) * 0.5f;
        // apply offset
        rectTransformSlotDrag.position += new Vector3(40, 0, 0);

        if (willClearDragged)
        {
            willClearDragged = false;
            if (slotDragged)
            {
                inventory.ReturnDragStack(slotDragged.x, slotDragged.y);
            }
            slotDragged = null;
            shiftDragged = false;
        }
    }

    public void SetPanelVisible(bool value)
    {
        panel.SetActive(value);

        // if we were dragging, cleanup
        if (!value && slotDragged)
        {
            inventory.ReturnDragStack(slotDragged.x, slotDragged.y);
            slotDragged = null;
            shiftDragged = false;
            slotDrag.gameObject.SetActive(false);
        }
    }

    public bool GetPanelVisible()
    {
        return panel.activeSelf;
    }

    public void AddSlots()
    {
        slots = new UIInventorySlot[inventory.width, inventory.height];

        containerGridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        containerGridLayoutGroup.constraintCount = inventory.width;

        for (int y = 0; y < inventory.height; y++)
        {
            for (int x = 0; x < inventory.width; x++)
            {
                GameObject goSlot = Instantiate(prefabInventorySlot, containerInventorySlot.transform);
                UIInventorySlot slot = goSlot.GetComponent<UIInventorySlot>();
                slots[x, y] = slot;
                slot.x = x;
                slot.y = y;

                slot.Entered.AddListener(delegate { OnSlotEntered(slot); } );
                slot.Exited.AddListener(delegate { OnSlotExited(slot); } );
                slot.Pressed.AddListener(delegate { OnSlotPressed(slot); } );
                slot.Released.AddListener(delegate { OnSlotReleased(slot); } );
                slot.RightPressed.AddListener(delegate { OnSlotRightPressed(slot); } );
                slot.RightReleased.AddListener(delegate { OnSlotRightReleased(slot); } );
            }
        }
    }

    public void UpdateSlots()
    {
        for (int x = 0; x < inventory.width; x++)
        {
            for (int y = 0; y < inventory.height; y++)
            {
                ItemStack itemStack = inventory.GetItemStack(x, y);
                UIInventorySlot slot = slots[x, y];
                slot.SetSprite(itemStack.data ? itemStack.data.spriteInventory : null);
                slot.SetAmount(itemStack.amount);
            }
        }
    }

    public void UpdateDragSlot()
    {
        ItemStack dragStack = inventory.GetDragStack();
        if (dragStack.data)
        {
            slotDrag.gameObject.SetActive(true);
            slotDrag.SetSprite(dragStack.data.spriteInventory);
            slotDrag.SetAmount(dragStack.amount);
        }
        else
        {
            slotDrag.gameObject.SetActive(false);
        }
    }

    public void OnSlotEntered(UIInventorySlot slot)
    {
        slotHovered = slot;
    }

    public void OnSlotExited(UIInventorySlot slot)
    {
        slotHovered = null;
    }

    public void OnSlotPressed(UIInventorySlot slot)
    {
        ItemStack stack = inventory.GetItemStack(slot.x, slot.y);
        if (stack.data)
        {
            slotDragged = slot;
            inventory.SetDragStack(stack.data, stack.amount);
            inventory.ClearItemStack(slot.x, slot.y);
        }
    }

    public void OnSlotReleased(UIInventorySlot slot)
    {
        if (slotDragged && slotHovered)
        {
            inventory.MergeOrSwapDragStack(slotDragged.x, slotDragged.y, slotHovered.x, slotHovered.y);
        }
        willClearDragged = true;
    }

    public void OnSlotRightPressed(UIInventorySlot slot)
    {
        ItemStack stack = inventory.GetItemStack(slot.x, slot.y);
        if (stack.data)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // shift drag -> split
                slotDragged = slot;
                int splitAmount = stack.amount / 2 + stack.amount % 2;
                inventory.SetStack(slot.x, slot.y, stack.data, stack.amount - splitAmount);
                inventory.SetDragStack(stack.data, splitAmount);
                shiftDragged = true;
            }
            else
            {
                // normal drag -> 1
                slotDragged = slot;
                int splitAmount = 1;
                inventory.SetStack(slot.x, slot.y, stack.data, stack.amount - splitAmount);
                inventory.SetDragStack(stack.data, splitAmount);
                shiftDragged = true;
            }
        }
    }

    public void OnSlotRightReleased(UIInventorySlot slot)
    {
        if (slotDragged && slotHovered)
        {
            if (shiftDragged)
            {
                inventory.MergeOrSwapDragStack(slotDragged.x, slotDragged.y, slotHovered.x, slotHovered.y);
            }
        }
        willClearDragged = true;
    }
    //{
    //    // RMB + Shift
    //    if (slotDragged && slotHovered && shiftDragged)
    //    {
    //        inventory.SplitStack(slotDragged.x, slotDragged.y, slotHovered.x, slotHovered.y);
    //    }

    //    willClearDragged = true;
    //}

    public void OnOutsideDropped()
    {
        if (slotDragged)
        {
            ItemStack stack = inventory.GetDragStack();
            Debug.Log(stack.data);
            Game.instance.SpawnItem(stack.data, stack.amount);
            inventory.ClearDragStack();
            inventory.ClearItemStack(slotDragged.x, slotDragged.y);
            willClearDragged = true;
        }
    }
}
