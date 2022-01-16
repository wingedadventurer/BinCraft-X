using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionShaker : MonoBehaviour
{
    [SerializeField] private float amount;
    [SerializeField] private float period;
    private Vector3 posOriginal;

    void Start()
    {
        posOriginal = transform.position;
        InvokeRepeating("Shake", 0, period);
    }

    private void Shake()
    {
        transform.position = posOriginal + new Vector3(Random.Range(-amount, amount), Random.Range(-amount, amount), Random.Range(-amount, amount));
    }
}
