using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private int count;

    private List<GameObject> bullets = new List<GameObject>();
    private int index;

    private void Start()
    {
        // make sure at least one bullet is available
        if (count < 1) { count = 1; }

        for (int i = 0; i < count; i++)
        {
            GameObject bullet = Instantiate(prefabBullet, transform);
            bullets.Add(bullet);
            bullet.SetActive(false);
        }
    }

    public GameObject Get()
    {
        GameObject bullet = bullets[index++];
        if (index >= bullets.Count) { index = 0; }
        bullet.SetActive(true);
        return bullet;
    }

    public void SetBulletsEnabled(bool value)
    {
        foreach (GameObject gameObject in bullets)
        {
            gameObject.SetActive(value);
        }
    }
}
