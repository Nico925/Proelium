using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox {

    public class Laser : MonoBehaviour {

        public Transform NE, NW, SE, SW;
        public string lastPoint = "NE";
        public float speed = 1.0f;
        float CoolDowntime = 10.0f;
        float prectime;

        public enum States {
            Idle,
            Moving,
            Alert,
            Shoot,
        }

        private States _currentState = States.Idle;
        /// <summary>
        /// Stato attuale.
        /// </summary>
        public States CurrentState {
            get { return _currentState; }
            set {
                if (_currentState != value) {
                    // è cambiato lo stato
                    onStateChanged(_currentState, value);
                }
                _currentState = value;
            }
        }

        // Use this for initialization

        void Start() {
            transform.position = NE.position;
            prectime = -CoolDowntime;

            
        }

        /// <summary>
        /// Accade ogni volta che cambia stato.
        /// </summary>
        void onStateChanged(States _oldState, States _newState) {
            switch (_newState) {
                case States.Idle:
                    break;
                case States.Moving:
                    Timer = Random.Range(5, 15);
                    break;
                case States.Alert:
                    Timer = Random.Range(0.5f, 2.5f);
                    break;
                case States.Shoot:
                    break;
                default:
                    break;
            }
        }

        float Timer = 0;

        void Update() {

            Debug.Log("State: " + CurrentState);
            switch (CurrentState) {
                case States.Idle:
                    // Non fa null
                    CurrentState = States.Moving;
                    break;
                case States.Moving:
                    Timer -= Time.deltaTime;
                    MoveLaser();
                    if (Timer < 0)
                        CurrentState = States.Alert;
                    break;
                case States.Alert:
                    Timer -= Time.deltaTime;
                    // Fai alert
                    if (Timer < 0)
                        CurrentState = States.Shoot;
                    break;
                case States.Shoot:
                    // shoot
                    CurrentState = States.Moving;
                    break;
                default:
                    // Stato non valido
                    break;
            }
            
        }



        void MoveLaser() {
            //funzione predisposta al movimento del laser

            if (lastPoint == "NE") {

                transform.position = Vector3.MoveTowards(transform.position, SE.position, speed);

                if (Vector3.Distance(transform.position, SE.position) == 0) {
                    lastPoint = "SE";
                }

            } else if (lastPoint == "SE") {

                transform.position = Vector3.MoveTowards(transform.position, SW.position, speed);

                if (Vector3.Distance(transform.position, SW.position) == 0) {
                    lastPoint = "SW";
                }

            } else if (lastPoint == "SW") {

                transform.position = Vector3.MoveTowards(transform.position, NW.position, speed);

                if (Vector3.Distance(transform.position, NW.position) == 0) {
                    lastPoint = "NW";
                }
            } else if (lastPoint == "NW") {

                transform.position = Vector3.MoveTowards(transform.position, NE.position, speed);

                if (Vector3.Distance(transform.position, NE.position) == 0) {
                    lastPoint = "NE";
                }

            }

        }


        void TimeLaser() {
            if (Time.time >= prectime + CoolDowntime == true)


                prectime = Time.time;
        }

    }
}