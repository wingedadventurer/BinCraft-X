using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataItem", menuName = "Data/Item", order = 1)]
public class DataItem : ScriptableObject
{
    new public string name;

    [Header("HP (leave 0 for invincibility)")]
    public int hp;

    [Header("Inventory sprite")]
    public Sprite spriteInventory;

    [Header("World representation")]
    public Mesh mesh;
    public Material material;
}
