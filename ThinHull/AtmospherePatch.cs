using CG.Game;
using CG.Ship.Repair;
using Gameplay.Atmosphere;
using HarmonyLib;
using Photon.Pun;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ThinHull
{
    [HarmonyPatch(typeof(Atmosphere))]
    internal class AtmospherePatch
    {
        internal static Dictionary<HullBreach, Room> nearestRoom;
        private static readonly FieldInfo breachesField = AccessTools.Field(typeof(HullDamageController), "breaches");
        private const float airLeakRate = 0.4f;
        private const float heatLeakRate = 0.3f;

        [HarmonyPostfix]
        [HarmonyPatch("RoomsInitialize")]
        static void RoomsInitialize()
        {
            CreateDictionary();
        }

        [HarmonyPostfix]
        [HarmonyPatch("RoomsShutdown")]
        static void RoomsShutdown()
        {
            nearestRoom = null;
        }

        [HarmonyPrefix]
        [HarmonyPatch("LateUpdate")]
        static void LateUpdate(Atmosphere __instance)
        {
            if (!PhotonNetwork.IsMasterClient || nearestRoom == null) return;

            foreach (HullBreach breach in nearestRoom.Keys)
            {
                if (breach.State.condition == BreachCondition.Intact) continue;
                if (breach.State.condition == BreachCondition.Minor && !Configs.minorLeak.Value) continue;

                Room room = nearestRoom[breach];
                AtmosphereValues values = __instance.Atmospheres.GetElementAt(room.RoomIndex);
                values.Oxygen *= 1 - airLeakRate / room.Volume * (int)breach.State.condition;
                values.Pressure = values.Oxygen;
                float tempK = values.Temperature + 273f;
                tempK *= 1 - heatLeakRate / room.Volume * (int)breach.State.condition;
                values.Temperature = tempK - 273f;
                __instance.Atmospheres.SetElement(room, values);
            }
        }

        internal static void CreateDictionary()
        {
            nearestRoom = new();
            HullDamageController controller = ClientGame.Current.PlayerShip?.GetComponentInChildren<HullDamageController>();
            if (controller == null) return;

            foreach (HullBreach breach in (List<HullBreach>)breachesField.GetValue(controller))
            {
                float num = float.MaxValue;
                Room cachedRoom = null;
                foreach (RoomCollection collection in RoomCollection.s_roomCollections)
                {
                    foreach (Room room in collection.Rooms)
                    {
                        collection.RoomSphereIntersect(room, breach.transform.position, 1000f, out Vector3 b);
                        float sqrMagnitude = (breach.transform.position - b).sqrMagnitude;
                        if (sqrMagnitude < num)
                        {
                            num = sqrMagnitude;
                            cachedRoom = room;
                        }
                    }
                }
                nearestRoom.Add(breach, cachedRoom);
            }
        }
    }
}
