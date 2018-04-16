using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace BlackFox {

    public class AvatarUI : MonoBehaviour {
        public GameObject KillToview;
        public Slider LifeSlider;
        public Slider AmmoSlider;
        public Image PinCountDown;

        
        private void Start()
        {
            LifeSlider.value = 0.45f;
            AmmoSlider.value = 0;
        }
        
        /// <summary>
        /// Setta il valore della slider che mostra la vita
        /// </summary>
        /// <param name="_avatar"></param>
        void SetLifeSliderValue(Avatar _avatar) {
            // Aggiorno la UI se l'avatar che gli viene passato è uguale al componente che ha come padre
            if (_avatar == GetComponentInParent<Avatar>())
                LifeSlider.value =  (0.45f * _avatar.ship.Life) / _avatar.ship.Config.MaxLife;

            //Logica per cambiare il colore della barra della vita
            //if (Ring.fillAmount < 0.3f) {
            //    Ring.color = Color.red;
            //} else if (Ring.fillAmount > 0.7f) {
            //    Ring.color = Color.green;
            //} else {
            //    Ring.color = Color.yellow;
            //}
                
        }

        /// <summary>
        /// Setta il valore della slider che mostra la quantità di proiettili
        /// </summary>
        /// <param name="_avatar"></param>
        void SetAmmoSlider(Avatar _avatar)
        {
            if (_avatar == GetComponentInParent<Avatar>())
                AmmoSlider.value = (0.87f * _avatar.ship.shooter.Ammo) / _avatar.AvatarData.shipConfig.shooterConfig.MaxAmmo;
            if (AmmoSlider.value > 0.87f)
                AmmoSlider.value = 0.87f;
        }
        
        /// <summary>
        /// Fa comparire l'immagine +1
        /// </summary>
        public void KillView()
        {
            Instantiate(KillToview, transform.position , Quaternion.identity);            
        }

        #region Events
        private void OnEnable()
        {
            EventManager.OnLifeValueChange += SetLifeSliderValue;
            EventManager.OnAmmoValueChange += SetAmmoSlider;
        }

        private void OnDisable()
        {
            EventManager.OnLifeValueChange -= SetLifeSliderValue;
            EventManager.OnAmmoValueChange -= SetAmmoSlider;
        }
        #endregion
    }
}