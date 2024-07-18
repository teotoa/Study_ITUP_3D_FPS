using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject projectilePrefab;
    public float projectileAngle = 30f;
    public float projectileForce = 10f;
    public float projectileTime = 5f;

    protected override void Fire()
    {
        ProjectileFire();
    }


    void ProjectileFire()
    {
        Camera cam = Camera.main;


        Vector3 direction = cam.transform.forward;
        direction = Quaternion.AngleAxis(-projectileAngle, transform.right) * direction;
        direction.Normalize();
        direction *= projectileForce;

        GameObject obj = Instantiate(projectilePrefab);
        obj.transform.position = firingPosition.position;

        obj.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        obj.GetComponent<Bomb>().time = projectileTime;
    }
}
