using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {
    public class AmmoCleaner : PowerUpBase
    {
        protected override void Init()
        {
            base.Init();
            //SpawnRatio = GameManager.Instance.LevelMng.CurrentLevel.RatioAmmoCleaner;
        }
        public override void UsePowerUp()
        {
            foreach (IPowerUpCollector enemy in enemyCollectors)
            {
                (enemy as Avatar).ship.shooter.Ammo = 0;
            }
        }
    }
}
