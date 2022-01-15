using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Data/Item", order = 1)]
[ExecuteInEditMode]
public class DataItem : ScriptableObject
{
    new public string name;

    [TextArea(3, 10)]
    public string description;

    public int maxStackCount = 1;

    [Header("HP (leave 0 for invincibility)")]
    public float hp;

    [Header("Inventory sprite")]
    public Sprite spriteInventory;

    [Header("World representation")]
    public GameObject prefabItem;
}
