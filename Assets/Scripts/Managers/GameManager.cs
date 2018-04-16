using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        #region Prefabs
        public GameObject LevelManagerPrefab;
        public GameObject UIManagerPrefab;
        public GameObject PlayerManagerPrefab;
        public GameObject CoinManagerPrefab;
        public GameObject AudioManagerPrefab;
        public GameObject DataManagerPrefab;
        public GameObject SRManagerPrefab;
        public GameObject LoadingControllerPrefab;
        public GameObject ShopRoomPrefab;

        #endregion

        #region Managers
        [HideInInspector]
        public LevelManager LevelMng;
        [HideInInspector]
        public UIManager UiMng;
        [HideInInspector]
        public PlayerManager PlayerMng;
        [HideInInspector]
        public CoinManager CoinMng;
        [HideInInspector]
        public AudioManager AudioMng;
        [HideInInspector]
        public DataManager DataMng;
        [HideInInspector]
        public SRManager SRMng;
        [HideInInspector]
        public LoadingController LoadingCtrl;
        [HideInInspector]
        public SRManager ShopRoomMng;

        #endregion

        [HideInInspector]
        public FlowSM flowSM;

        public Level[] LevelScriptableObjs;
        int LevelSelected;

        private void Awake()
        {
            //Singleton paradigm
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(gameObject);
        }

        void Start()
        {
            flowSM = gameObject.AddComponent<FlowSM>();
        }

        #region API
        public void QuitApplication()
        {
            Application.Quit();
        }

        /// <summary>
        /// Funzione che salva il numero del livello selezionato
        /// </summary>
        /// <param name="_levelNumber"></param>
        public void SelectLevel(int _levelNumber)
        {
            LevelSelected = _levelNumber;
        }

        /// <summary>
        /// Funzione che ritorna lo scriptable del livello da caricare
        /// </summary>
        /// <returns></returns>
        public Level GetSelectedLevel()
        {
            return LevelScriptableObjs[LevelSelected];
        }

        #region Instantiate Managers
        public void InstantiateLevelManager()
        {
            LevelMng = Instantiate(LevelManagerPrefab, transform).GetComponent<LevelManager>();
        }
        public void DestroyLevelManager()
        {
            if(LevelMng)
                Destroy(LevelMng.gameObject);
        }

        public void InstantiateCoinManager()
        {
            CoinMng = Instantiate(CoinManagerPrefab, transform).GetComponent<CoinManager>();
        }

        public void InstantiateUIManager()
        {
            UiMng = Instantiate(UIManagerPrefab, transform).GetComponent<UIManager>();
        }

        public void InstantiatePlayerManager()
        {
            PlayerMng = Instantiate(PlayerManagerPrefab, transform).GetComponent<PlayerManager>();
        }

        public void InstantiateAudioManager()
        {
            AudioMng = Instantiate(AudioManagerPrefab, transform).GetComponent<AudioManager>();
        }

        public void InstantiateDataManager()
        {
            DataMng = Instantiate(DataManagerPrefab, transform).GetComponent<DataManager>();
        }

        public void InstantiateShowRoom()
        {
            SRMng = Instantiate(SRManagerPrefab, transform).GetComponent<SRManager>();
        }

        public void InstantiateShopRoom()
        {
            ShopRoomMng = Instantiate(ShopRoomPrefab, transform).GetComponent<SRManager>();
        }

        public void InstantiateLoadingController() 
        {
            LoadingCtrl = Instantiate(LoadingControllerPrefab, transform).GetComponent<LoadingController>();
        }

        #endregion
        #endregion
               
    }
}


