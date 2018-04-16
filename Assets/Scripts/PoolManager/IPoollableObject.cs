using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public interface IPoollableObject
    {
        PoolManager poolManager { get; set; }
        /// <summary>
        /// Indica il gameobject.
        /// </summary>
        GameObject GameObject { get; }
        bool IsActive { get; set; }

        void Activate();
        void Deactivate();
    }
}