using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace VoidStrider
{
    [BepInPlugin("com.zorp.VoidStrider", "VoidStrider", "1.0.0")]
    [BepInDependency("com.bepis.r2api")]
    [R2APISubmoduleDependency(new string[]
    {
        "LanguageAPI",
        "PrefabAPI",
        "ContentAddition"
    })]
    public class VoidStrider : BaseUnityPlugin
    {

        public void Awake()
        {
            LanguageAPI.Add("VOID_STRIDER_NAME", "Void Strider");
            LanguageAPI.Add("VOID_STRIDER_DESCRIPTION", "Something sinister lurks deep within the void");

            // Load assets
            AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) "RoR2/DLC1/VoidJailer/VoidJailerBody.prefab");
            GameObject voidStriderPrefab = PrefabAPI.InstantiateClone(asyncOperationHandle.WaitForCompletion(), "VoidStriderBody");
            asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) "RoR2/DLC1/VoidJailer/VoidJailerBody.prefab");
            GameObject voidStriderDisplayPrefab = asyncOperationHandle.WaitForCompletion();

            // Defining the survivor (to test)
            SurvivorDef blegh = new SurvivorDef(){
            bodyPrefab = voidStriderPrefab,
            displayPrefab = voidStriderDisplayPrefab,
            descriptionToken = "VOID_STRIDER_DESCRIPTION",
            primaryColor = new Color(0.5f, 0.5f, 0.5f),
            displayNameToken = "VOID_STRIDER_NAME",
            desiredSortPosition = 99f
            };

            // Adding Content
            ContentAddition.AddBody(voidStriderPrefab);
            ContentAddition.AddSurvivorDef(blegh);
        }
    }
}