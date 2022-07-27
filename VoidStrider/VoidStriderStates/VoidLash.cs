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

namespace VoidStrider.VoidStriderStates
{
    class VoidLash : EntityStates.VoidJailer.Weapon.ChargeCapture
    {
    }
}
