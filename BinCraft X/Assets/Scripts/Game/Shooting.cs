using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject prefabBullet;
    public Transform transformBulletSpawn;

    public float bulletSpeed;
    public float bulletDamage;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        float xScreen = Screen.width / 2;
        float yScreen = Screen.height / 2;

        float xAcc = Random.Range(xScreen, xScreen);
        float yAcc = Random.Range(yScreen, yScreen);

        var ray = Camera.main.ScreenPointToRay(new Vector3(xAcc, yAcc, 0));

        GameObject bullet = Instantiate(prefabBullet, transformBulletSpawn.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().damage = bulletDamage;
        bullet.GetComponent<Rigidbody>().velocity += ray.direction * bulletSpeed;
    }
}
