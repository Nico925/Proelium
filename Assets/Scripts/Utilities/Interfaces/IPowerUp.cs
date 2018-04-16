using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {
    public interface IPowerUp {
        void UsePowerUp();        
    }
    
    public interface IPowerUpCollector {
        void CollectPowerUp(IPowerUp _powerUp);
    }
}
