using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace BlackFox
{
    public class PowerUpManager : MonoBehaviour
    {

        public float PowerUpLifeTime = 10;
        public float MaxRandomOffSet = 2.5f;
        public float CoreMinDistance = 2;
        public float ZOffSet = 32;
        public float XOffSet = 64;
        List<GameObject> PowerUps = new List<GameObject>();
        bool IsActive = false;
        GameObject container;
        AudioSource audioSurce;

        float MinTimeToSpawn
        {
            get { return GameManager.Instance.LevelMng.CurrentLevel.MinPowerUpRatio; }
        }
        float MaxTimeToSpawn
        {
            get { return GameManager.Instance.LevelMng.CurrentLevel.MaxPowerUpRatio; }
        }
        float timer {
            get
            {
                return Random.Range(MinTimeToSpawn, MaxTimeToSpawn);
            }
        }

        float countdown;

        float PowerupRatioSum;

        private void Update()
        {
            if (IsActive)
            {
                countdown -= Time.deltaTime;
                if (countdown <= 0)
                {
                    SpawnPowerUp();
                    countdown = timer;
                }
            }
        }

        #region API
        public void Init()
        {
            PowerUps = Resources.LoadAll<GameObject>("Prefabs/PowerUps").Where(p => p.GetComponent<PowerUpBase>() != null).ToList();
            powerUpsOptions = GameManager.Instance.LevelMng.CurrentLevel.PowerUpOptions;
            CalculatePercentage();
            container = new GameObject("PowerUpContainer");
            container.transform.parent = transform;
            countdown = timer;
            audioSurce = GetComponent<AudioSource>();
            int random = Random.Range(0, GameManager.Instance.AudioMng.PowerUpAudio.PowerUpSpawn.Count);
            audioSurce.clip = GameManager.Instance.AudioMng.PowerUpAudio.PowerUpSpawn[random].Clip;
            audioSurce.volume = GameManager.Instance.AudioMng.PowerUpAudio.PowerUpSpawn[random].Volume;
        }

        public void Toggle(bool _value)
        {
            IsActive = _value;
        }

        public void CleanSpawned()
        {
            if (container != null)
                Destroy(container); 
        }

        #endregion

        #region PowerUpSpawn

        PowerUpOptions powerUpsOptions;

        List<PowerupsPercentage> powerUpPercentages = new List<PowerupsPercentage>();

        /// <summary>
        /// Calcola la percentuale di probabilità con cui può essere spawnato
        /// </summary>
        void CalculatePercentage()
        {
            List<PowerupsPercentage> tempPowerUpsPercentage = new List<PowerupsPercentage>();
            foreach (PowerupsPercentage powerUpPercentage in powerUpsOptions.Percentages)
            {
                PowerupRatioSum += powerUpPercentage.Percentage;
                tempPowerUpsPercentage.Add(powerUpPercentage);
            }

            for (int i = 0; i < tempPowerUpsPercentage.Count; i++)
            {
                powerUpPercentages.Add(new PowerupsPercentage { PowerUpID = tempPowerUpsPercentage[i].PowerUpID, Percentage = (tempPowerUpsPercentage[i].Percentage * 100) / PowerupRatioSum });
            }
        }


        /// <summary>
        /// Sceglie un pawerup in base alla percentuale di probabilità che abbia di essere spawnato
        /// </summary>
        /// <returns></returns>
        GameObject ChoosePowerUp()
        {
            float randNum = Random.Range(0, PowerupRatioSum);
            float tempMinValue = 0f;

            for (int i = 0; i < powerUpPercentages.Count; i++)
            {
                if(randNum < (powerUpPercentages[i].Percentage + tempMinValue) && randNum >= tempMinValue)
                {
                    foreach (GameObject item in PowerUps)
                    {
                        if (item.GetComponent<PowerUpBase>().ID == powerUpPercentages[i].PowerUpID)
                            return item;
                    } 
                }
                tempMinValue += powerUpPercentages[i].Percentage;
            }
            return null;
        }

        /// <summary>
        /// Spawna un pawerup in una posizione specifica
        /// </summary>
        /// <param name="_position">La posizione che deve avere il powerup.</param>
        void SpawnPowerUp()
        {
            PowerUpBase tempPowerup;
            GameObject tempObj = ChoosePowerUp();
			tempPowerup = Instantiate(tempObj, container.transform).GetComponent<PowerUpBase>();
			// modifica la rotazione del powerup riportandola a 0,0,0
            //tempPowerup = Instantiate(tempObj, GameManager.Instance.LevelMng.Core.transform.position, Quaternion.identity, container.transform).GetComponent<PowerUpBase>();
            //if(tempPowerup.GetComponent<Collider>())
            //    tempPowerup.GetComponent<Collider>().enabled = false;
            DrawParable(tempPowerup.gameObject, ChoosePosition(GameManager.Instance.PlayerMng.Players));
            if (tempPowerup != null)
                tempPowerup.LifeTime = PowerUpLifeTime;
            if (!audioSurce.isPlaying)
                audioSurce.Play();
        }
        
        #endregion

        void DrawParable(GameObject _objToMove, Vector3 _target)
        {
            _objToMove.transform.DOJump(_target, 50, 1, 1f).OnComplete(() => {AddCollider(_objToMove); });
        }

        void AddCollider(GameObject _obj)
        {
            _obj.AddComponent<SphereCollider>().isTrigger = true;
        }

        Vector3 ChoosePosition(List<Player> players)
        {
            float coreX = GameManager.Instance.LevelMng.Core.transform.position.x;
            float coreZ = GameManager.Instance.LevelMng.Core.transform.position.z;
            Vector3 finalPosition = new Vector3();
            for (int i = 0; i < players.Count; i++)
            {
                finalPosition = finalPosition + players[i].Avatar.ship.transform.position;
            }
            finalPosition.y /= players.Count;
            finalPosition.x = (coreX - finalPosition.x) * Random.Range(0f, MaxRandomOffSet) / players.Count; ;
            finalPosition.z = (coreZ - finalPosition.z) * Random.Range(0f, MaxRandomOffSet) / players.Count; ;

            //Check becero per tenere i power Up in scena
            Vector3 coreProjection = new Vector3(coreX, 8, coreZ);
            if (Vector3.Distance(finalPosition, coreProjection) < CoreMinDistance)
            {
                while (Vector3.Distance(finalPosition, coreProjection) < CoreMinDistance)
                {
                    finalPosition = finalPosition + new Vector3(Random.Range(0f, CoreMinDistance), 0, Random.Range(0f, CoreMinDistance));
                }
                if (Random.Range(-1f, 1f) < 0)
                    finalPosition.x *= -1f;
                if (Random.Range(-1f, 1f) < 0)
                    finalPosition.z *= -1f;
            }

            if (finalPosition.x > XOffSet)
                finalPosition.x = XOffSet;
            if (finalPosition.x < -XOffSet)
                finalPosition.x = -XOffSet;

            if (finalPosition.z > ZOffSet)
                finalPosition.z = ZOffSet;
            if (finalPosition.z < -ZOffSet)
                finalPosition.z = -ZOffSet;

            finalPosition.y = 8;
            return finalPosition;
        }

    }

    [System.Serializable]
    public struct PowerupsPercentage
    {
        public PowerUpID PowerUpID;
        public float Percentage;
    }

}