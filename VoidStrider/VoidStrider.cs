using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using EntityStates;
using EntityStates.VoidJailer;
using EntityStates.VoidJailer.Weapon;
using RoR2.CharacterAI;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using VoidStrider.VoidStriderStates;

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
            AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidJailer/VoidJailerBody.prefab");
            GameObject voidStriderPrefab = PrefabAPI.InstantiateClone(asyncOperationHandle.WaitForCompletion(), "VoidStriderBody");
            asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidJailer/VoidJailerBody.prefab");
            GameObject voidStriderDisplayPrefab = asyncOperationHandle.WaitForCompletion();
            asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase2.prefab");
            GameObject voidRaidCrabPrefab = asyncOperationHandle.WaitForCompletion(); 
            // Adjusting Stats
            CharacterBody voidStriderBody = voidStriderPrefab.GetComponent<CharacterBody>();
            CharacterDirection voidStriderDirection = voidStriderPrefab.GetComponent<CharacterDirection>();
            CharacterMotor voidStriderMotor = voidStriderPrefab.GetComponent<CharacterMotor>();

            voidStriderMotor.mass = 10000;
            voidStriderMotor.airControl = 1.5f;
            voidStriderMotor.jumpCount = 1;

            voidStriderBody.baseMaxHealth = 1500;
            voidStriderBody.levelMaxHealth = 325;
            voidStriderBody.baseDamage = 18;
            voidStriderBody.levelDamage = 4;

            voidStriderBody.baseAttackSpeed = 1.25f;
            voidStriderBody.baseMoveSpeed = 20;
            voidStriderBody.baseAcceleration = 420;
            voidStriderBody.baseJumpPower = 50;
            voidStriderDirection.turnSpeed = 500;

            voidStriderBody.baseArmor = 35;

            // Adjusting Skills
            foreach (GenericSkill componentsInChild in voidStriderPrefab.GetComponentsInChildren<GenericSkill>())
                UnityEngine.Object.DestroyImmediate((UnityEngine.Object) componentsInChild);
            SkillLocator skillLocator = voidStriderPrefab.GetComponent<SkillLocator>();
            Reflection.SetFieldValue<GenericSkill[]>(skillLocator, "allSkills", new GenericSkill[0]);
            
            SkillDef instance1 = ScriptableObject.CreateInstance<SkillDef>();
            skillLocator.primary = voidStriderPrefab.GetComponent<GenericSkill>();
            // Replacing Primary
            skillLocator.primary = voidStriderPrefab.AddComponent<GenericSkill>();
            SkillFamily instance228 = ScriptableObject.CreateInstance<SkillFamily>();
            instance228.variants = new SkillFamily.Variant[1];
            ContentAddition.AddSkillFamily(instance228);
            Reflection.SetFieldValue<SkillFamily>((object) skillLocator.primary, "_skillFamily", instance228);

            skillLocator.secondary = voidStriderPrefab.GetComponent<GenericSkill>();
            // Replacing Secondary
            skillLocator.secondary = voidStriderPrefab.AddComponent<GenericSkill>();
            SkillFamily instance229 = ScriptableObject.CreateInstance<SkillFamily>();
            instance229.variants = new SkillFamily.Variant[1];
            ContentAddition.AddSkillFamily(instance229);
            Reflection.SetFieldValue<SkillFamily>((object) skillLocator.secondary, "_skillFamily", instance229);

            skillLocator.utility = voidStriderPrefab.GetComponent<GenericSkill>();
            // Replacing Utility
            skillLocator.utility = voidStriderPrefab.AddComponent<GenericSkill>();
            SkillFamily instance230 = ScriptableObject.CreateInstance<SkillFamily>();
            instance230.variants = new SkillFamily.Variant[1];
            ContentAddition.AddSkillFamily(instance230);
            Reflection.SetFieldValue<SkillFamily>((object) skillLocator.utility, "_skillFamily", instance230);

            skillLocator.special = voidStriderPrefab.GetComponent<GenericSkill>();
            // Replacing Special
            skillLocator.special = voidStriderPrefab.AddComponent<GenericSkill>();
            SkillFamily instance231 = ScriptableObject.CreateInstance<SkillFamily>();
            instance231.variants = new SkillFamily.Variant[1];
            ContentAddition.AddSkillFamily(instance231);
            Reflection.SetFieldValue<SkillFamily>((object) skillLocator.special, "_skillFamily", instance231);

            SkillDef voidBlast = ScriptableObject.CreateInstance<SkillDef>();
            voidBlast.activationState = new SerializableEntityStateType(typeof (FireVoidBlast));
            voidBlast.activationStateMachineName = "Weapon";
            voidBlast.baseMaxStock = 3;
            voidBlast.baseRechargeInterval = 4f;
            voidBlast.beginSkillCooldownOnSkillEnd = true;
            voidBlast.canceledFromSprinting = false;
            voidBlast.cancelSprintingOnActivation = true;
            voidBlast.fullRestockOnAssign = true;
            voidBlast.interruptPriority = InterruptPriority.Any;
            voidBlast.isCombatSkill = true;
            voidBlast.mustKeyPress = false;
            voidBlast.rechargeStock = 1;
            voidBlast.requiredStock = 1;
            voidBlast.stockToConsume = 1;
            voidBlast.skillDescriptionToken = "";
            voidBlast.skillName = "VoidJailer_Primary";
            voidBlast.skillNameToken = "Void Blast";

            SkillDef voidLash = ScriptableObject.CreateInstance<SkillDef>();
            voidLash.activationState = new SerializableEntityStateType(typeof (VoidLash));
            voidLash.activationStateMachineName = "Weapon";
            voidLash.baseMaxStock = 1;
            voidLash.baseRechargeInterval = 10f;
            voidLash.beginSkillCooldownOnSkillEnd = false;
            voidLash.canceledFromSprinting = false;
            voidLash.cancelSprintingOnActivation = true;
            voidLash.fullRestockOnAssign = true;
            voidLash.interruptPriority = InterruptPriority.Any;
            voidLash.isCombatSkill = true;
            voidLash.mustKeyPress = false;
            voidLash.rechargeStock = 1;
            voidLash.requiredStock = 1;
            voidLash.stockToConsume = 1;
            voidLash.skillDescriptionToken = "";
            voidLash.skillName = "VoidJailer_Secondary";
            voidLash.skillNameToken = "Void Lash";

            SkillDef voidBlink = ScriptableObject.CreateInstance<SkillDef>();
            voidBlink.activationState = new SerializableEntityStateType(typeof (VoidBlink));
            voidBlink.activationStateMachineName = "Weapon";
            voidBlink.baseMaxStock = 4;
            voidBlink.baseRechargeInterval = 3f;
            voidBlink.beginSkillCooldownOnSkillEnd = true;
            voidBlink.canceledFromSprinting = false;
            voidBlink.cancelSprintingOnActivation = false;
            voidBlink.fullRestockOnAssign = true;
            voidBlink.interruptPriority = InterruptPriority.Any;
            voidBlink.isCombatSkill = true;
            voidBlink.mustKeyPress = false;
            voidBlink.rechargeStock = 1;
            voidBlink.requiredStock = 1;
            voidBlink.stockToConsume = 1;
            voidBlink.skillDescriptionToken = "";
            voidBlink.skillName = "VoidJailer_Utility";
            voidBlink.skillNameToken = "Void Blink";

            SkillDef voidSingularity = ScriptableObject.CreateInstance<SkillDef>();
            voidSingularity.activationState = new SerializableEntityStateType(typeof (VoidSingularity));
            voidSingularity.activationStateMachineName = "Weapon";
            voidSingularity.baseMaxStock = 1;
            voidSingularity.baseRechargeInterval = 30f;
            voidSingularity.beginSkillCooldownOnSkillEnd = true;
            voidSingularity.canceledFromSprinting = true;
            voidSingularity.cancelSprintingOnActivation = true;
            voidSingularity.fullRestockOnAssign = true;
            voidSingularity.interruptPriority = InterruptPriority.Any;
            voidSingularity.isCombatSkill = true;
            voidSingularity.mustKeyPress = false;
            voidSingularity.rechargeStock = 1;
            voidSingularity.requiredStock = 1;
            voidSingularity.stockToConsume = 1;
            voidSingularity.skillDescriptionToken = "";
            voidSingularity.skillName = "VoidJailer_Special";
            voidSingularity.skillNameToken = "Void Singularity";

            SkillFamily voidBlastFamily = skillLocator.primary.skillFamily;
            SkillFamily voidLashFamily = skillLocator.secondary.skillFamily;
            SkillFamily voidBlinkFamily = skillLocator.utility.skillFamily;
            SkillFamily voidSingularityFamily = skillLocator.special.skillFamily;
            SkillFamily.Variant[] variants154 = voidBlastFamily.variants;
            SkillFamily.Variant variant1 = new();
            variant1.skillDef = voidBlast;
            variant1.viewableNode = new ViewablesCatalog.Node(voidBlast.skillNameToken, false);
            SkillFamily.Variant variant155 = variant1;
            variants154[0] = variant155;
            SkillFamily.Variant[] variants155 = voidLashFamily.variants;
            variant1 = new SkillFamily.Variant();
            variant1.skillDef = voidLash;
            variant1.viewableNode = new ViewablesCatalog.Node(voidLash.skillNameToken, false);
            SkillFamily.Variant variant156 = variant1;
            variants155[0] = variant156;
            SkillFamily.Variant[] variants156 = voidBlinkFamily.variants;
            variant1 = new SkillFamily.Variant();
            variant1.skillDef = voidBlink;
            variant1.viewableNode = new ViewablesCatalog.Node(voidBlink.skillNameToken, false);
            SkillFamily.Variant variant157 = variant1;
            variants156[0] = variant157;
            SkillFamily.Variant[] variants157 = voidSingularityFamily.variants;
            variant1 = new SkillFamily.Variant();
            variant1.skillDef = voidSingularity;
            variant1.viewableNode = new ViewablesCatalog.Node(voidSingularity.skillNameToken, false);
            SkillFamily.Variant variant158 = variant1;
            variants157[0] = variant158;

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
            ContentAddition.AddSkillDef(voidBlast);
            ContentAddition.AddSkillDef(voidLash);
            ContentAddition.AddSkillDef(voidBlink);
            ContentAddition.AddSkillDef(voidSingularity);
            ContentAddition.AddBody(voidStriderPrefab);
            ContentAddition.AddSurvivorDef(blegh);
        }
    }
}