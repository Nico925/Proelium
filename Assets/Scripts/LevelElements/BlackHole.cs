using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BlackFox
{
    /// IMPORTANTE IL MESH RENDERER DEL GAMEOBJECT A CUI E' ATTACCATO DEVE ESSERE DISATTIVATO \\\
    public class BlackHole : MonoBehaviour
    {

        public float RangeToDestroyAgents;
        public float RangeToDestroyPins;
        int Size = 1;
        
        /// <summary>
        /// La scala a cui arriva una volta aumentata la taglia
        /// </summary>
        public int ScaleSize2 = 2;
        public int ScaleSize3 = 4;


        float Timer = 5;
        States currentState;
        float CheckTimer = 1f;
        // la velocità con cui viene spostato verso il centro
        public float Attraction = 15;

        enum States
        {
            CheckZoneOfSpawn,
            Active,
        }

        private void Start()
        {
            currentState = States.CheckZoneOfSpawn;
        }

        private void Update()
        {
            switch (currentState)
            {
                case States.CheckZoneOfSpawn:
                    CheckTimer -= Time.deltaTime;
                    if (CheckTimer <= 0)
                    {
                        GetComponent<MeshRenderer>().enabled = true;
                        currentState = States.Active;
                    }
                    break;
                case States.Active:
                        ChangeSize(Size);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Ad ogni dimensione del buco nero, applica una forza di attrazione differente.
        /// </summary>
        /// <param name="_size">La dimensione del buco nero</param>
        void ChangeSize(int _size)
        {
            switch (_size)
            {
                case 1:
                    Attraction = 0.1f;
                    Timer -= Time.deltaTime;
                    if (Timer <= 0)
                    {
                        transform.DOScale(new Vector3(ScaleSize2, ScaleSize2, ScaleSize2), 0.5f);
                        Size = 2;
                        Timer = 5;
                    }
                    break;
                case 2:
                    Attraction = 0.3f;
                    Timer -= Time.deltaTime;
                    if (Timer <= 0)
                    {
                        transform.DOScale(new Vector3(ScaleSize3, ScaleSize3, ScaleSize3), 0.5f);
                        Size = 3;
                        Timer = 5;
                    }
                    break;
                case 3:
                    Attraction = 0.7f;
                    Timer -= Time.deltaTime;
                    if (Timer <= 0)
                    {
                        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => { Destroy(gameObject); });
                    }
                    break;
                default:
                    break;
            }
        }


        private void OnTriggerStay(Collider other)
        {
            GameObject tempObj = other.gameObject;
            
            if (tempObj.tag == "Pin")
            {
                if (Vector3.Distance(tempObj.transform.position, transform.position) <= RangeToDestroyPins)
                {
                    Destroy(tempObj);
                }
            }

            if (tempObj.GetComponent<Avatar>() != null)
            {
                tempObj.transform.position = Vector3.MoveTowards(tempObj.transform.position, transform.localPosition, Attraction);
                if (Vector3.Distance(tempObj.transform.position, transform.position) <= RangeToDestroyAgents)
                {
                    Destroy(tempObj);
                }
            }

            if (tempObj.tag == "Core")
            {
                Destroy(gameObject);
            }

            if (tempObj.tag == "BlackHole")
            {
                Destroy(gameObject);
            }
        }
    }
}