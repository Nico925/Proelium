using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BlackFox
{
    public class ShowRoomController : MonoBehaviour
    {
        List<GameObject> avatars = new List<GameObject>();
        List<AvatarData> datas { get { return manager.datas; } }
        public Player player;
        SRManager manager;

        Vector3 _corridorVector;
        public Vector3 CorridorVector
        {
            get
            {
                if (_corridorVector == Vector3.zero)
                    EvaluateDirection();
                return _corridorVector;
            }
            protected set
            {
                _corridorVector = value;
            }
        }

        int _indexOfCurrent;
        public int IndexOfCurrent {
            get { return _indexOfCurrent; }
            set {
                _indexOfCurrent = value;
                if(EventManager.OnShowRoomValueUpdate != null)
                    EventManager.OnShowRoomValueUpdate(manager.datas[IndexOfCurrent].SelectionParameters[IndexOfCurrent], player);
            }
        }
        public int colorIndex { get; private set; }
        public Transform currentModel;
        public Transform nextModel;
        Transform prevModel;
        GameObject modelContainer;

        #region API
        /// <summary>
        /// Reorder the direction to follow and the Istance of the Avatar Models
        /// </summary>
        /// <param name="_data"></param>
        public void Init(Player myPlayer, SRManager _manager)
        {
            player = myPlayer;
            manager = _manager;
            InstanceModels(datas.ToArray());       
        }

        /// <summary>
        /// Richiamata nello stato di avatar selection perchè lo show Room viene creato nei menu, ma ancora non sono presenti le slider, di conseguenza la prima volta le slider non hanno valore
        /// </summary>
        public void SetSliderValue()
        {
            if (EventManager.OnShowRoomValueUpdate != null)
                EventManager.OnShowRoomValueUpdate(manager.datas[IndexOfCurrent].SelectionParameters[IndexOfCurrent], player);
        }

        /// <summary>
        /// Dislay next Model
        /// </summary>
        public void ShowNextModel(bool _isInShop = false)
        {
            for (int i = IndexOfCurrent + 1; i < datas.Count; i++)
            {
                if (_isInShop)
                {
                    IndexOfCurrent = i;
                    modelContainer.transform.DOMove(-CorridorVector * IndexOfCurrent, 0.5f);
                    break;
                }
                else if (datas[i].IsPurchased)
                {
                    IndexOfCurrent = i;
                    modelContainer.transform.DOMove(-CorridorVector * IndexOfCurrent, 0.5f);
                    break;
                }
            }
        }

        /// <summary>
        /// Display previous Model
        /// </summary>
        public void ShowPreviousModel(bool _isInShop = false)
        {
            if (IndexOfCurrent > 0)
            {
                for (int i = IndexOfCurrent - 1; i >= 0; i--)
                {
                    if (_isInShop)
                    {
                        IndexOfCurrent = i;
                        modelContainer.transform.DOMove(-CorridorVector * IndexOfCurrent, 0.5f);
                        break;
                    }
                    else if (datas[i].IsPurchased)
                    {
                        IndexOfCurrent = i;
                        modelContainer.transform.DOMove(-CorridorVector * IndexOfCurrent, 0.5f);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Show next color of the ColorSet list of the current AvatarData
        /// </summary>
        public void ShowNextColor(bool _isInShop = false)
        {
            if (!_isInShop)
            {
                colorIndex = manager.GetNextColorID(SRManager.ColorSelectDirection.Up, this, colorIndex, IndexOfCurrent);
                foreach (GameObject avatar in avatars)
                {
                    foreach (MeshRenderer renderer in avatar.GetComponentsInChildren<MeshRenderer>())
                    {
                        renderer.materials = new Material[] { datas[IndexOfCurrent].ColorSets[colorIndex].ShipMaterialMain };
                    }
                } 
            }
            else
            {
                colorIndex++;
                if (colorIndex >= datas[IndexOfCurrent].ColorSets.Count || colorIndex < 0)
                    colorIndex--;
                foreach (GameObject avatar in avatars)
                {
                    foreach (MeshRenderer renderer in avatar.GetComponentsInChildren<MeshRenderer>())
                    {
                        renderer.materials = new Material[] { datas[IndexOfCurrent].ColorSets[colorIndex].ShipMaterialMain };
                    }
                }
            }
        }

        /// <summary>
        /// Show previous color of the ColorSet list of the current AvatarData
        /// </summary>
        public void ShowPreviousColor(bool _isInShop = false)
        {
            if (!_isInShop)
            {
                colorIndex = manager.GetNextColorID(SRManager.ColorSelectDirection.Down, this, colorIndex, IndexOfCurrent);
                foreach (GameObject avatar in avatars)
                {
                    foreach (MeshRenderer renderer in avatar.GetComponentsInChildren<MeshRenderer>())
                    {
                        renderer.materials = new Material[] { datas[IndexOfCurrent].ColorSets[colorIndex].ShipMaterialMain };
                    }
                } 
            }
            else
            {
                colorIndex--;
                if (colorIndex >= datas[IndexOfCurrent].ColorSets.Count || colorIndex < 0)
                    colorIndex++;
                foreach (GameObject avatar in avatars)
                {
                    foreach (MeshRenderer renderer in avatar.GetComponentsInChildren<MeshRenderer>())
                    {
                        renderer.materials = new Material[] { datas[IndexOfCurrent].ColorSets[colorIndex].ShipMaterialMain };
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Used to evaluate the positive direction of the ShowRoom
        /// It also istance prevModel
        /// </summary>
        void EvaluateDirection()
        {
            CorridorVector = nextModel.position - currentModel.position;

            if (prevModel != null)
                DestroyImmediate(prevModel.gameObject);

            prevModel = new GameObject("prevModelPosition").transform;
            prevModel.transform.parent = transform;
            prevModel.position = -CorridorVector;
            prevModel.rotation = nextModel.rotation;
        }

        /// <summary>
        /// Place all the required Models in scene along the corridor of the ShowRoom
        /// </summary>
        /// <param name="_data">AvatarDatas of the models</param>
        void InstanceModels(AvatarData[] _data)
        {
            if (modelContainer)
                DestroyImmediate(modelContainer);
            modelContainer = new GameObject("ModelContainer");
            modelContainer.transform.parent = transform;

            for (int i = 0; i < _data.Length; i++)
            {
                avatars.Add(Instantiate(_data[i].ModelPrefab, currentModel.position + CorridorVector * i, nextModel.rotation, modelContainer.transform));
                avatars[i].AddComponent<RotateOnPosition>();
                foreach(MeshRenderer mesh in avatars[i].GetComponentsInChildren<MeshRenderer>())
                {
                    if(player.ID == PlayerLabel.One)
                    {
                        colorIndex = 0;
                    }
                    else
                    {
                        colorIndex = manager.CheckColorAvailability(this, (int)player.ID - 1, i);
                    }
                    mesh.materials = new Material[] { datas[i].ColorSets[colorIndex].ShipMaterialMain };
                }
            }

            ReSetFirstShowRoom();
        }

        public void ReSetFirstShowRoom()
        {
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].IsPurchased)
                {
                    IndexOfCurrent = i;
                    currentModel = avatars[IndexOfCurrent].transform;
                    modelContainer.transform.DOMove(-CorridorVector * IndexOfCurrent, 0.5f);
                    if (avatars.Count > i + 1)
                        nextModel = avatars[IndexOfCurrent + 1].transform;
                    break;
                }
            }
        }
    }
}
