using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefabUtily {

    #region API
    /// <summary> 
    /// Load ALL resources from the specific path inside the Resources Folder
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_path"></param>
    /// <returns></returns>
    public static List<GameObject> LoadAllPrefabsWithComponentOfType<T>(string _path)
    {
        List<GameObject> listGameObj = new List<GameObject>();
        GameObject[] prefabs = Resources.LoadAll<GameObject>(_path);
        foreach (var item in prefabs)
        {
            if (item.GetComponent<T>() != null)
            {
                listGameObj.Add(item);
            }
        }
        return listGameObj;
    }

    /*************************************************/
    //public static List<T> LoadAllPrefabsWithComponentOfType<T>(string _path) {
    //    List<T> listGameObj = new List<T>();
    //    GameObject[] prefabs = Resources.LoadAll<GameObject>(_path);
    //    foreach (var item in prefabs) {
    //        if (item.GetComponent<T>() != null) {
    //            listGameObj.Add(item.GetComponent<T>());
    //        }
    //    }
    //    return listGameObj;
    //}
    /*************************************************/


    /// <summary>
    /// Load ALL resources from the specific path inside the Resources Folder ignoring the specific parameter GameObject 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_path"></param>
    /// <param name="_itemToIgnore"></param>
    /// <returns></returns>
    public static List<GameObject> LoadAllPrefabsWithComponentOfType<T>(string _path, GameObject _itemToIgnore)
    {
        List<GameObject> listGameObj = LoadAllPrefabsWithComponentOfType<T>(_path);

        return RemoveItemFromList(listGameObj, _itemToIgnore);
    }

    /// <summary>
    /// Load ALL resources from the specific path inside the Resources Folder ignoring the specific parameter GameObject 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_path"></param>
    /// <param name="_itemsToIgnore"></param>
    /// <returns></returns>
    public static List<GameObject> LoadAllPrefabsWithComponentOfType<T>(string _path, List<GameObject> _itemsToIgnore)
    {
        //Potrebbe non funzionare -- da collaudare !
        List<GameObject> listGameObj = LoadAllPrefabsWithComponentOfType<T>(_path);

        for (int i = 0; i < _itemsToIgnore.Count; i++)
        {
            RemoveItemFromList(listGameObj, _itemsToIgnore[i]);
        }

        return listGameObj;
    }
    #endregion

    /// <summary>
    /// Delete the specific GameObject from the List
    /// </summary>
    /// <param name="_listGameObj"></param>
    /// <param name="_itemToIgnore"></param>
    /// <returns></returns>
    static List<GameObject> RemoveItemFromList(List<GameObject> _listGameObj, GameObject _itemToIgnore)
    {
        GameObject itemToRemove = null;

        foreach (GameObject item in _listGameObj)
        {
            if (item.name == _itemToIgnore.name)
            {
                itemToRemove = item;
            }
        }

        if (itemToRemove != null && _listGameObj.Contains(itemToRemove))
        {
            _listGameObj.Remove(itemToRemove);
        }

        return _listGameObj;
    }
}
