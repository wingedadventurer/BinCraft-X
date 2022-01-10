using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public static UIInventory instance;

    [SerializeField] private GameObject prefabInventorySlot;
    [SerializeField] private GameObject containerInventorySlot;
    [SerializeField] private GridLayoutGroup containerGridLayoutGroup;
    [SerializeField] private RectTransform rectTransformSlotDrag;

    private UIInventorySlot[,] slots;
    [SerializeField] private UIInventorySlot slotDrag;

    private UIInventorySlot slotHovered;
    private UIInventorySlot slotDragged;

    private ItemStack itemStackDrag;

    private bool willClearDragged;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AddSlots();
        UpdateSlots();
        UpdateDragSlot();

        Inventory.instance.Changed.AddListener(UpdateSlots);
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
            slotHovered = null;
            slotDragged = null;
            itemStackDrag.data = null;
            UpdateDragSlot();
        }
    }

    public void AddSlots()
    {
        Inventory inventory = Inventory.instance;
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
        Inventory inventory = Inventory.instance;
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
        Inventory inventory = Inventory.instance;
        if (itemStackDrag.data)
        {
            slotDrag.gameObject.SetActive(true);
            slotDrag.SetSprite(itemStackDrag.data.spriteInventory);
            slotDrag.SetAmount(itemStackDrag.amount);
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
        ItemStack stack = Inventory.instance.GetItemStack(slot.x, slot.y);
        if (stack.data)
        {
            slotDragged = slot;
            itemStackDrag = stack;
            UpdateDragSlot();
        }
    }

    public void OnSlotReleased(UIInventorySlot slot)
    {
        if (slotDragged && slotHovered && slotHovered != slot)
        {
            Inventory.instance.MergeOrSwapItemStacks(slotDragged.x, slotDragged.y, slotHovered.x, slotHovered.y);
        }

        willClearDragged = true;
        UpdateDragSlot();
    }

    public void OnSlotRightPressed(UIInventorySlot slot)
    {
        ItemStack stack = Inventory.instance.GetItemStack(slot.x, slot.y);
        if (stack.data)
        {
            slotDragged = slot;
            itemStackDrag = stack;

            itemStackDrag.amount = itemStackDrag.amount / 2 + (itemStackDrag.amount % 2);
            UpdateDragSlot();
        }
    }

    public void OnSlotRightReleased(UIInventorySlot slot)
    {
        Debug.Log("x");

        if (slotDragged && slotHovered && slotHovered != slot)
        {
            Inventory.instance.SplitStack(slotDragged.x, slotDragged.y, slotHovered.x, slotHovered.y);
        }

        willClearDragged = true;
        UpdateDragSlot();
    }

    public void OnOutsideDropped()
    {
        if (slotDragged)
        {
            ItemStack stack = Inventory.instance.GetItemStack(slotDragged.x, slotDragged.y);
            Game.instance.SpawnItem(stack.data, stack.amount);

            Inventory.instance.ClearItemStack(slotDragged.x, slotDragged.y);
            willClearDragged = true;
            UpdateDragSlot();
        }
    }
}
