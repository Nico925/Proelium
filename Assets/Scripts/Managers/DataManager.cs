using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BlackFox
{
    public class DataManager : MonoBehaviour
    {
        [HideInInspector]
        public List<AvatarData> AvatarDatasInstances { get { return DataSavedToAvatarData(modelsDatas); } }

        List<ColorSetData> colorData { get { return DataSavedToColorSetData(colorsDatas); } }

        List<ModelsSaved> modelsDatas = new List<ModelsSaved>();
        List<ColorsSaved> colorsDatas = new List<ColorsSaved>();

        public void Init()
        {
            GetPlayerPrefsForColors(LoadColorSets());
            InstantiateAvatarDatas(LoadAvatarDatas());
        }

        /// <summary>
        /// Setta la variabile in PlayerPref corrispondente al dato che viene passato, sostituisce la struttura contenuta in datas con una nuova con valori aggiornati
        /// </summary>
        /// <param name="_dataIndex"></param>
        /// <param name="_data">L'avatar data che deve essere modificato</param>
        public void PurchaseAvatar(AvatarData _data)
        {
            PlayerPrefs.SetInt(_data.DataName, 1);

            for (int i = 0; i < modelsDatas.Count; i++)
            {
                if (_data.DataName == modelsDatas[i].avatar.DataName)
                {
                    AvatarData tempData = modelsDatas[i].avatar;                                  // Riutilizzo l'avatarData già contenuta all'interno di datas.avatar
                    modelsDatas[i] = new ModelsSaved() { avatar = tempData, isPurchased = 1 };
                }
            }

            //avatarDatas[_dataIndex].IsPurchased = true;
            //_data.IsPurchased = true;
        }

        public void PurchaseColorSet(ColorSetData _color)
        {
            PlayerPrefs.SetInt(_color.ColorName, 1);

            for (int i = 0; i < colorsDatas.Count; i++)
            {
                if (_color.ColorName == colorsDatas[i].color.ColorName)
                {
                    ColorSetData tempData = colorsDatas[i].color;                                  // Riutilizzo l'avatarData già contenuta all'interno di datas.avatar
                    colorsDatas[i] = new ColorsSaved() { color = tempData, isPurchased = 1 };
                }
            }

        }

        /// <summary>
        /// Carica l'array di avatar data da Resources
        /// </summary>
        List<AvatarData> LoadAvatarDatas()
        {
            return Resources.LoadAll<AvatarData>("ShipModels").ToList();
            // controlla chi è purchase e chi no
        }

        /// <summary>
        /// Carica l'array di color set data da Resources
        /// </summary>
        List<ColorSetData> LoadColorSets()
        {
            return Resources.LoadAll<ColorSetData>("ShipModels/ColorSets").ToList();
        }

        /// <summary>
        /// Istanzia gli avatar data
        /// </summary>
        void InstantiateAvatarDatas(List<AvatarData> _datas)
        {
            foreach (AvatarData data in _datas)
            {
                ModelsSaved tempData = new ModelsSaved();
                tempData.avatar = Instantiate(data);
                if (PlayerPrefs.HasKey(data.DataName)) 
                    tempData.isPurchased = PlayerPrefs.GetInt(data.DataName);
                else
                    tempData.isPurchased = data.IsPurchased == true ? 1 : 0;

                modelsDatas.Add(tempData);
            }
        }

        void GetPlayerPrefsForColors(List<ColorSetData> _datas)
        {
            foreach (ColorSetData data in _datas)
            {
                ColorsSaved tempData = new ColorsSaved();
                tempData.color = data;
                if (PlayerPrefs.HasKey(data.ColorName))
                    tempData.isPurchased = PlayerPrefs.GetInt(data.ColorName);
                else
                    tempData.isPurchased = data.IsPurchased == true ? 1 : 0;

                colorsDatas.Add(tempData);
            }
        }

        List<AvatarData> DataSavedToAvatarData(List<ModelsSaved> _data)
        {
            List<AvatarData> DataToReturn = new List<AvatarData>();
            foreach (ModelsSaved data in _data)
            {
                DataToReturn.Add(data.avatar);
            }
            return DataToReturn;
        }

        List<ColorSetData> DataSavedToColorSetData(List<ColorsSaved> _data)
        {
            List<ColorSetData> DataToReturn = new List<ColorSetData>();
            foreach (ColorsSaved data in colorsDatas)
            {
                DataToReturn.Add(data.color);
            }
            return DataToReturn;
        }

        /// <summary>
        /// Resetta i modelli ed i colori
        /// </summary>
        public void DataReset()
        {
            ResetModelsPurchased();
            ResetColorsPurchased();
        }

        /// <summary>
        /// Resetta tutti i modelli rendendo disponibile solo il gufo
        /// </summary>
        void ResetModelsPurchased()
        {
            for (int i = 0; i < modelsDatas.Count; i++)
            {
                if (modelsDatas[i].avatar.DataName != "Owl")
                {
                    PlayerPrefs.SetInt(modelsDatas[i].avatar.DataName, 0);
                    modelsDatas[i].avatar.IsPurchased = false;
                }
                else
                {
                    PlayerPrefs.SetInt(modelsDatas[i].avatar.DataName, 1);
                    modelsDatas[i].avatar.IsPurchased = true;
                }
            }
        }

        /// <summary>
        /// Resetta tutti i colori tranne i 4 principali
        /// </summary>
        void ResetColorsPurchased()
        {
            for (int i = 0; i < colorsDatas.Count; i++)
            {
                if(colorsDatas[i].color.ColorName != "Blue" && colorsDatas[i].color.ColorName != "Green" && colorsDatas[i].color.ColorName != "LightBlue" && colorsDatas[i].color.ColorName != "Orange")
                {
                    PlayerPrefs.SetInt(colorsDatas[i].color.ColorName, 0);
                    colorsDatas[i].color.IsPurchased = false;
                }
                else
                {
                    PlayerPrefs.SetInt(colorsDatas[i].color.ColorName, 1);
                    colorsDatas[i].color.IsPurchased = true;
                }
            }
        }

        struct ModelsSaved
        {
            public AvatarData avatar;
            public int isPurchased { set { avatar.IsPurchased = value == 0 ?  false : true; } }  // 1: Purchased, 0: non Purchased
        }

        struct ColorsSaved
        {
            public ColorSetData color;
            public int isPurchased { set { color.IsPurchased = value == 0 ? false : true; } }  // 1: Purchased, 0: non Purchased
        }
    }

    public class ConstrainedAvatarData
    {
        public Player player;
        public int SelectedDataIndex;
        public int SelectedDataColorId;

        public ConstrainedAvatarData(Player _player, int _selectedDataColorId = 0, int _selectedDataIndex = 0)
        {
            player = _player;
            SelectedDataIndex = _selectedDataIndex;
            SelectedDataColorId = _selectedDataColorId;
        }
    }
}