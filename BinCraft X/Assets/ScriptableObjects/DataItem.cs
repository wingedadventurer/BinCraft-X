using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataItem", menuName = "Data/Item", order = 1)]
public class DataItem : ScriptableObject
{
    [Header("UI")]
    new public string name;
    public Sprite spriteInventory;

    [Header("World")]
    public Mesh mesh;
    public Material material;
}
