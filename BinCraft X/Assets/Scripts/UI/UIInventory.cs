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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AddSlots();
        UpdateSlots();

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

        // hand slot
        //ItemStack itemStackHand = inventory.GetHand();
        //slotHand.gameObject.SetActive(itemStackHand.data); // hide if hand is empty
        //slotHand.SetSprite(itemStackHand.data ? itemStackHand.data.spriteInventory : null);
        //slotHand.SetAmount(itemStackHand.amount);
    }

    public void OnSlotPressed(UIInventorySlot slot)
    {
        Debug.Log("pressed slot " + slot.x + " " + slot.y);

        // TODO: do stuff
    }

    public void OnSlotReleased(UIInventorySlot slot)
    {
        Debug.Log("released slot " + slot.x + " " + slot.y);

        // TODO: do stuff
    }

    public void OnSlotDropped(UIInventorySlot slot)
    {
        Debug.Log("dropped slot " + slot.x + " " + slot.y);

        // TODO: do stuff
    }
}
