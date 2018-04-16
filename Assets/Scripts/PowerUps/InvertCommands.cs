using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class InvertCommands : PowerUpBase
    {
        protected override void Init()
        {
            base.Init();
            //SpawnRatio = GameManager.Instance.LevelMng.CurrentLevel.RatioInvertCommand;
        }
        public override void UsePowerUp()
        {
            foreach (IPowerUpCollector enemy in enemyCollectors)
            {
                (enemy as Avatar).InvertCommands(PowerUpDuration, collector as Avatar);
            }            
        }
    }
}