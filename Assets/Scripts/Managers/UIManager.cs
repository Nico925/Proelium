using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace BlackFox {
    public class UIManager : MonoBehaviour
    {
        [HideInInspector]
        public MainMenuController canvasMenu;
        [HideInInspector]
        public LevelSelectionController canvasLevelSelection;
        [HideInInspector]
        public CanvasGameController canvasGame;
        [HideInInspector]
        public AvatarSelectionManager avatarSelectionManager;
        [HideInInspector]
        public CreditsMenuController creditsMenuController;
        [HideInInspector]
        public ManualController manualController;
        [HideInInspector]
        public StoreController storeController;

        public GameObject AvatarUI;

        [HideInInspector]
        public BaseMenu CurrentMenu;

        public List<Sprite> RoundsImage = new List<Sprite>();

        #region API
        public void Init()
        {
            GameObject cam = GameObject.Find("CameraUI");
            cam.SetActive(false);
            cam.SetActive(true);
            LoadButtonImage();
        }

        #region Menu Actions

        public void GoUpInMenu(Player _player)
        {
            CurrentMenu.GoUpInMenu(_player);
        }

        public void GoDownInMenu(Player _player)
        {
            CurrentMenu.GoDownInMenu(_player);
        }

        public void GoLeftInMenu(Player _player)
        {
            CurrentMenu.GoLeftInMenu(_player);
        }

        public void GoRightInMenu(Player _player)
        {
            CurrentMenu.GoRightInMenu(_player);
        }

        public void GoBackInMenu(Player _player)
        {
            CurrentMenu.GoBack(_player);
        }

        public void SelectInMenu(Player _player)
        {
            CurrentMenu.Selection(_player);
        }

        #endregion

        #region Menu Instantiate And Destroy
        #region Main Menu
        /// <summary>
        /// Crea il CanvasMenu non appena subentra il MainMenuState
        /// </summary>
        public void CreateMainMenu()
        {
            canvasMenu = Instantiate(Resources.Load("Prefabs/UI/CanvasMenu") as GameObject, transform).GetComponent<MainMenuController>();
            canvasMenu.Init();
        }

        /// <summary>
        /// Distrugge il CanvasMenu non appena subentra il MainMenuState
        /// </summary>
        public void DestroyMainMenu()
        {
            Destroy(canvasMenu.gameObject);
        }
        #endregion

        #region Store Menu
        /// <summary>
        /// Crea il CanvasMenu non appena subentra il MainMenuState
        /// </summary>
        public void CreateStoreMenu()
        {
            storeController = Instantiate(Resources.Load("Prefabs/UI/ShopCanvas") as GameObject, transform).GetComponent<StoreController>();
            storeController.Init();
        }

        /// <summary>
        /// Distrugge il CanvasMenu non appena subentra il MainMenuState
        /// </summary>
        public void DestroyStoreMenu()
        {
            Destroy(storeController.gameObject);
        }
        #endregion

        #region LevelSelection Menu
        /// <summary>
        /// Crea il CanvasLevelSelection non appena subentra il MainMenuState
        /// </summary>
        public void CreateLevelSelectionMenu()
        {
            canvasLevelSelection = Instantiate(Resources.Load("Prefabs/UI/CanvasLevelSelection") as GameObject, transform).GetComponent<LevelSelectionController>();
        }

        /// <summary>
        /// Distrugge il CanvasLevelSelection non appena subentra il MainMenuState
        /// </summary>
        public void DestroyLevelSelectionMenu()
        {
            Destroy(canvasLevelSelection.gameObject);
        }
        #endregion

        #region Manual Menu

        /// <summary>
        /// Crea il CanvasManual
        /// </summary>
        public void CreateManualCanvas()
        {
            manualController = Instantiate(Resources.Load("Prefabs/UI/CanvasManual") as GameObject, transform).GetComponent<ManualController>();
        }

        /// <summary>
        /// Distrugge il CanvasManual
        /// </summary>
        public void DestroyManualCanvas()
        {
            Destroy(manualController.gameObject);
        }
        
        #endregion

        #region Credits Menu
        /// <summary>
        /// Crea il CreditsMenuController
        /// </summary>
        public void CreateCreditsMenu()
        {
            creditsMenuController = Instantiate(Resources.Load("Prefabs/UI/CanvasCredits") as GameObject, transform).GetComponent<CreditsMenuController>();
        }

        /// <summary>
        /// Distrugge il CreditsMenuController
        /// </summary>
        public void DestroyCreditsMenu()
        {
            Destroy(creditsMenuController.gameObject);
        }
        #endregion

        #region AvatarSelection Menu

        public void CreateAvatarSelectionMenu() {
            avatarSelectionManager = Instantiate(Resources.Load("Prefabs/UI/AvatarSelectionCanvas") as GameObject, transform).GetComponent<AvatarSelectionManager>();
        }

        /// <summary>
        /// Distrugge il CanvasMenu non appena subentra il MainMenuState
        /// </summary>
        public void DestroyAvatarSelectionMenu() {
            Destroy(avatarSelectionManager.gameObject);
        }
        #endregion

        #region Game Menu
        /// <summary>
        /// Crea il Canvas Game Menu
        /// </summary>
        public void CreateGameMenu()
        {
            canvasGame = Instantiate(Resources.Load("Prefabs/UI/CanvasGame") as GameObject, transform).GetComponent<CanvasGameController>();
        }

        /// <summary>
        /// Distrugge il Canvas Game Menu
        /// </summary>
        public void DestroyGameMenu()
        {
            Destroy(canvasGame.gameObject);
        }
        #endregion

        #region AvatarUI

        /// <summary>
        /// Crea l'avatarUI
        /// </summary>
        /// <param name="_target">l'oggetto a cui attaccare la UI</param>
        public AvatarUI CreateAvatarUI(GameObject _target)
        {
            return Instantiate(AvatarUI, _target.transform.position, _target.transform.rotation, _target.transform).GetComponentInChildren<AvatarUI>();
        }

        #endregion

        #region Load Button Image
        [HideInInspector]
        public Sprite SelectedButton;
        [HideInInspector]
        public Sprite DeselectionButton;

        void LoadButtonImage()
        {
            SelectedButton = Resources.Load("UI/MenuUI/tasto_acceso", typeof(Sprite)) as Sprite;
            DeselectionButton = Resources.Load("UI/MenuUI/tasto", typeof(Sprite)) as Sprite;
        }

        #endregion
        #endregion

        public void SetRoundImage(int _roundNumber)
        {
            canvasGame.Counter.RoundNumber = RoundsImage[_roundNumber - 1];
        }
        #endregion

    }
}