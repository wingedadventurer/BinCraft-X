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
    [SerializeField] private RectTransform rectTransformSlotHand;

    private UIInventorySlot[,] slots;
    [SerializeField] private UIInventorySlot slotHand;
    
    private UIInventorySlot slotDragged;

    private bool willClearDrag;

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
        rectTransformSlotHand.position = Input.mousePosition;
        // move to bottom-right
        rectTransformSlotHand.position += new Vector3(rectTransformSlotHand.rect.size.x, -rectTransformSlotHand.rect.size.y, 0) * 0.5f;
        // apply offset
        rectTransformSlotHand.position += new Vector3(40, 0, 0);

        if (willClearDrag)
        {
            willClearDrag = false;
            slotDragged = null;
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
                //slot.gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnSlotPressed(slot); });
                slot.Pressed.AddListener(delegate { OnSlotPressed(slot); } );
                slot.Released.AddListener(delegate { OnSlotReleased(slot); } );
                slot.Dragged.AddListener(delegate { OnSlotDragged(slot); } );
                slot.Dropped.AddListener(delegate { OnSlotDropped(slot); } );
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
        if (slotDragged)
        {
            slotHand.gameObject.SetActive(true);
            ItemStack itemStackHand = inventory.GetItemStack(slotDragged.x, slotDragged.y);
            slotHand.SetSprite(itemStackHand.data ? itemStackHand.data.spriteInventory : null);
            slotHand.SetAmount(itemStackHand.amount);
        }
        else
        {
            slotHand.gameObject.SetActive(false);
        }
    }

    public void OnSlotPressed(UIInventorySlot slot)
    {
        
    }

    public void OnSlotReleased(UIInventorySlot slot)
    {
        willClearDrag = true;
    }
    
    public void OnSlotDragged(UIInventorySlot slot)
    {
        if (Inventory.instance.GetItemStack(slot.x, slot.y).amount > 0)
        {
            slotDragged = slot;
            UpdateDragSlot();
        }
    }

    public void OnSlotDropped(UIInventorySlot slot)
    {
        if (slotDragged)
        {
            if (slotDragged != slot)
            {
                Inventory.instance.MergeOrSwapItemStacks(slotDragged.x, slotDragged.y, slot.x, slot.y);
            }
            slotDragged = null;
            UpdateDragSlot();
        }
    }

    public void OnOutsideDropped()
    {
        if (slotDragged)
        {
            // TODO: spawn items in world

            Inventory.instance.ClearItemStack(slotDragged.x, slotDragged.y);
            slotDragged = null;
            UpdateDragSlot();
        }
    }
}
