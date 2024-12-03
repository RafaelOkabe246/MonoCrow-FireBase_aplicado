using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : RhythmEffect
{
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform firePoint;

    public void ShootBullet() 
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        bullet.SetupBullet(transform.right, bulletSpeed);
    }
}
