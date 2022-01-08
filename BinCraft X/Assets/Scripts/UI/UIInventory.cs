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
    [SerializeField] private UIInventorySlot slotMouse;
    [SerializeField] private RectTransform rectTransformSlotMouse;

    private UIInventorySlot[,] slots;

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
        rectTransformSlotMouse.position = Input.mousePosition;
        // move to bottom-right
        rectTransformSlotMouse.position += new Vector3(rectTransformSlotMouse.rect.size.x, -rectTransformSlotMouse.rect.size.y, 0) * 0.5f;
        // apply offset
        rectTransformSlotMouse.position += new Vector3(40, 0, 0);
    }

    public void AddSlots()
    {
        Inventory inventory = Inventory.instance;
        slots = new UIInventorySlot[inventory.width, inventory.height];

        containerGridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        containerGridLayoutGroup.constraintCount = inventory.width;

        for (int x = 0; x < inventory.width; x++)
        {
            for (int y = 0; y < inventory.height; y++)
            {
                GameObject goSlot = Instantiate(prefabInventorySlot, containerInventorySlot.transform);
                UIInventorySlot slot = goSlot.GetComponent<UIInventorySlot>();
                slots[x, y] = slot;
                slot.x = x;
                slot.y = y;
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
}
