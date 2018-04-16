using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    /// <summary>
    /// Interfaccia per tutti coloro che hanno la capacità di sparare.
    /// </summary>
    public interface IShooter
    {
        List<IDamageable> GetDamageable();

        GameObject GetOwner();
    }
}
