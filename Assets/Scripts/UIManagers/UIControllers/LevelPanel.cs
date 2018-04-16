using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    public class LevelPanel : MonoBehaviour
    {

        public int levelNum;

        public int LevelNum
        {
            get { return levelNum; }
            set { levelNum = value; }
        }

        public void ConsoleDebug()
        {
            Debug.Log("Hai cliccato");
        }
    }
}