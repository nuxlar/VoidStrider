using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
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

namespace VoidStrider.VoidStriderStates
{
    public abstract class FireVoidBlast : EntityStates.VoidRaidCrab.Weapon.FireMultiBeamFinale
    {

        public override void OnEnter()
        {
            this.duration = this.baseDuration / this.attackSpeedStat;
            Transform modelTransform = this.GetModelTransform();
            this.PlayAnimation(this.animationLayerName, this.animationStateName, this.animationPlaybackRateParam, this.duration);
            if ((bool)this.muzzleEffectPrefab)
                EffectManager.SimpleMuzzleFlash(this.muzzleEffectPrefab, this.gameObject, "ClawMuzzle", false);
            int num = (int)Util.PlayAttackSpeedSound(this.enterSoundString, this.gameObject, this.attackSpeedStat);
            Ray beamRay;
            Vector3 beamEndPos;
            this.CalcBeamPath(out beamRay, out beamEndPos);
            new BlastAttack()
            {
                attacker = this.gameObject,
                inflictor = this.gameObject,
                teamIndex = TeamComponent.GetObjectTeam(this.gameObject),
                baseDamage = (this.damageStat * this.blastDamageCoefficient),
                baseForce = this.blastForceMagnitude,
                position = beamEndPos,
                radius = this.blastRadius,
                falloffModel = BlastAttack.FalloffModel.SweetSpot,
                bonusForce = this.blastBonusForce,
                damageType = DamageType.Generic
            }.Fire();
            if ((bool)modelTransform)
            {
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                if ((bool)component)
                {
                    int childIndex = component.FindChildIndex("ClawMuzzle");
                    if ((bool)this.tracerEffectPrefab)
                    {
                        EffectData effectData = new EffectData()
                        {
                            origin = beamEndPos,
                            start = beamRay.origin,
                            scale = this.blastRadius
                        };
                        effectData.SetChildLocatorTransformReference(this.gameObject, childIndex);
                        EffectManager.SpawnEffect(this.tracerEffectPrefab, effectData, true);
                        EffectManager.SpawnEffect(this.explosionEffectPrefab, effectData, true);
                    }
                }
            }
            this.OnFireBeam(beamRay.origin, beamEndPos);
        }


        public override void FixedUpdate()
        {
            if ((double)this.fixedAge < (double)this.duration)
                return;
            this.outer.SetNextState(this.InstantiateNextState());
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;
    }
}
