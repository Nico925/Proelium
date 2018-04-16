using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackFox
{
    /// <summary>
    /// Spawn a random Setup between 1 and the higher IDSetup value
    /// </summary>
    public class ArrowsSpawer : SpawnerBase
    {
        new public ArrowsSpawerOptions Options;
        List<Arrow> arrows = new List<Arrow>();
        List<GameObject> activeArrows = new List<GameObject>();
        int maxRandSetup = 0;
        int randomSetup = 1;

        public override SpawnerBase OptionInit(SpawnerOptions options)
        {
            Options = options as ArrowsSpawerOptions;
            return this;
        }

        void Start()
        {
            foreach (var spawn in FindObjectsOfType<SpawnPoint>())
                foreach (var item in spawn.ValidAs)
                    if (item == SpawnPoint.SpawnType.ArrowSpawner)
                    {
                        Arrow newArrow;
                        newArrow.transf = spawn.transform;
                        newArrow.IDsetup = spawn.IDSetup;
                        arrows.Add(newArrow);
                    }

            
            //Find the higher IDSetup value
            foreach (Arrow arr in arrows)
                if (maxRandSetup < arr.IDsetup)
                    maxRandSetup = arr.IDsetup;
            //Pick a random Setup
            randomSetup = Random.Range(1, maxRandSetup);
            //Instanciate arrows at selected spawnpoint
            foreach (Arrow arr in arrows)
                if (arr.IDsetup == randomSetup)
                    InstanciateNewArrow(arr.transf);
        }

        void OnDisable()
        {   
            //Destroy all arrow in Scene
            foreach (GameObject arr in activeArrows)
            {
                activeArrows.Remove(arr);
                Destroy(arr);
            }
                

        }

        /// <summary>
        /// Instanciate a new Arrow in _transform and set it as child of thi GameObj
        /// </summary>
        /// <param name="_transf"></param>
        void InstanciateNewArrow(Transform _transf)
        {
            activeArrows.Add(Instantiate(Options.ArrowPrefab, _transf.position, _transf.rotation, transform));
        }

        struct Arrow
        {
            public Transform transf;
            public int IDsetup;
        }
    }

    [System.Serializable]
    public class ArrowsSpawerOptions : SpawnerOptions {
        public GameObject ArrowPrefab;
    }
}
