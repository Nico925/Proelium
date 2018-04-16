using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlackFox;

public class ShooterBase : MonoBehaviour {

    protected ShooterBaseConfig shooterBaseConfig;

    #region API
    /// <summary>
    /// Spara un proiettile
    /// </summary>
    public virtual void ShootBullet()
    {
        GameObject instantiatedProjectile = Instantiate(shooterBaseConfig.ProjectilePrefab, transform.position, transform.rotation);
        instantiatedProjectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * shooterBaseConfig.BulletSpeed, ForceMode.Impulse);
        instantiatedProjectile.GetComponent<Projectile>().Init(GetComponentInParent<IShooter>());
        Destroy(instantiatedProjectile, shooterBaseConfig.LifeTime);
    }
    #endregion
}

[Serializable]
public class ShooterBaseConfig
{
    public GameObject ProjectilePrefab;
    public float LifeTime;
    public float BulletSpeed;
}
