using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDamage;

    [SerializeField] private Transform transformBulletSpawn;
    [SerializeField] private DataItem dataBullet;

    private BulletPool bulletPool;

    private void Awake()
    {
        bulletPool = FindObjectOfType<BulletPool>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Game.instance.playerControllable && Inventory.instance.HasItem(dataBullet))
        {
            Shoot();
            Inventory.instance.RemoveItem(dataBullet);
        }
    }

    private void Shoot()
    {
        float xScreen = Screen.width / 2;
        float yScreen = Screen.height / 2;

        float xAcc = Random.Range(xScreen, xScreen);
        float yAcc = Random.Range(yScreen, yScreen);

        var ray = Camera.main.ScreenPointToRay(new Vector3(xAcc, yAcc, 0));

        GameObject bullet = bulletPool.Get();
        bullet.transform.position = transformBulletSpawn.transform.position;
        bullet.GetComponent<Bullet>().damage = bulletDamage;
        bullet.GetComponent<Rigidbody>().velocity = ray.direction * bulletSpeed;
    }
}
