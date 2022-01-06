using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Data/Enemy", order = 1)]
public class DataEnemy : ScriptableObject
{
    [Header("HP")]
    public float hp;

    [Header("World representation")]
    public Mesh mesh;
    public Material material;
}
