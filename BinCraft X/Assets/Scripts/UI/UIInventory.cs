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

    private UIInventorySlot slotHovered;
    private UIInventorySlot slotPressed;
    private UIInventorySlot slotRightPressed;

    private bool willClearPressed;

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

        if (willClearPressed)
        {
            willClearPressed = false;
            slotHovered = null;
            slotPressed = null;
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
        if (slotRightPressed)
        {
            slotHand.gameObject.SetActive(true);
            ItemStack itemStackHand = inventory.GetItemStack(slotRightPressed.x, slotRightPressed.y);
            slotHand.SetSprite(itemStackHand.data ? itemStackHand.data.spriteInventory : null);
            slotHand.SetAmount(itemStackHand.amount);
        }
        else if (slotPressed)
        {
            slotHand.gameObject.SetActive(true);
            ItemStack itemStackHand = inventory.GetItemStack(slotPressed.x, slotPressed.y);
            slotHand.SetSprite(itemStackHand.data ? itemStackHand.data.spriteInventory : null);
            slotHand.SetAmount(itemStackHand.amount);
        }
        else
        {
            slotHand.gameObject.SetActive(false);
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
            slotPressed = slot;
            UpdateDragSlot();
        }
    }

    public void OnSlotReleased(UIInventorySlot slot)
    {
        if (slotPressed && slotHovered && slotHovered != slot)
        {
            Inventory.instance.MergeOrSwapItemStacks(slotPressed.x, slotPressed.y, slotHovered.x, slotHovered.y);
        }

        willClearPressed = true;
        UpdateDragSlot();
    }

    public void OnSlotRightPressed(UIInventorySlot slot)
    {

    }

    public void OnSlotRightReleased(UIInventorySlot slot)
    {
        
    }

    public void OnOutsideDropped()
    {
        if (slotPressed)
        {
            ItemStack stack = Inventory.instance.GetItemStack(slotPressed.x, slotPressed.y);
            Game.instance.SpawnItem(stack.data, stack.amount);

            Inventory.instance.ClearItemStack(slotPressed.x, slotPressed.y);
            willClearPressed = true;
            UpdateDragSlot();
        }
    }
}
