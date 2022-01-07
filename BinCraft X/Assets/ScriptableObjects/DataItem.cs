using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Data/Item", order = 1)]
public class DataItem : ScriptableObject
{
    new public string name;

    public int maxStackCount = 1;

    [Header("HP (leave 0 for invincibility)")]
    public float hp;

    [Header("Inventory sprite")]
    public Sprite spriteInventory;

    [Header("World representation")]
    public Mesh mesh;
    public Material material;
}
