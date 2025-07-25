﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.Pool
{
    public class LeanPoolManager : MonoBehaviour
    {
        public List<LeanGameObjectPool> LeanPools;

        private void Awake()
        {
           if(LeanPools.Count == 0)
               GetAllLeanPools();
               
               
            InitializeLeanPools();
        }
        
        [Button]
        private void GetAllLeanPools()
        {
            LeanPools = FindObjectsOfType<LeanGameObjectPool>().ToList();
        }



        public void InitializeLeanPools()
        {
            foreach (var pool in LeanPools)
            {
                
                if(LeanPool.Links.ContainsKey(pool.Prefab))
                    continue;
                //Add to instances
                
                
                LeanPool.Links.Add(pool.Prefab, pool);
            }
        }
    }
}
