using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Reflection;
using System;

[System.Serializable]
public struct CharacterStats : INetworkSerializable
{
    public int HealthPoints;
    public int ManaPoints;
    public int AttackDamage;
    public int MagicDamage;
    public int Armor;

    public int ResistanceGeneral;
    public int ResistancePhysical;
    public int ResistanceMagical;
    public int ResistanceCC;
    public int ResistanceFire;

    public int HpRegen;
    public int MpRegen;
    public int AttackSpeed;
    public int CastSpeed;
    public int CooldownReduction;
    public int MovementSpeed;
    public int HealReceive;
    public int HealDone;


    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref HealthPoints);
        serializer.SerializeValue(ref ManaPoints);
        serializer.SerializeValue(ref AttackDamage);
        serializer.SerializeValue(ref MagicDamage);
        serializer.SerializeValue(ref Armor);
        serializer.SerializeValue(ref ResistanceGeneral);
        serializer.SerializeValue(ref ResistancePhysical);
        serializer.SerializeValue(ref ResistanceMagical);
        serializer.SerializeValue(ref ResistanceCC);
        serializer.SerializeValue(ref ResistanceFire);
        serializer.SerializeValue(ref HpRegen);
        serializer.SerializeValue(ref MpRegen);
        serializer.SerializeValue(ref AttackSpeed);
        serializer.SerializeValue(ref CastSpeed);
        serializer.SerializeValue(ref CooldownReduction);
        serializer.SerializeValue(ref MovementSpeed);
        serializer.SerializeValue(ref HealReceive);
        serializer.SerializeValue(ref HealDone);
    }

    static string[] FORMATTED_NAMES = new string[]
    {
        "Health Points",
        "Mana Points",
        "Attack Damage",
        "Magic Damage",
        "Armor",
        "General Resistance",
        "Physical Resistance",
        "Magic Resistance",
        "CC Resistance",
        "Fire Resistance",
        "Health Regeneration",
        "Mana Regeneration",
        "Attack Speed",
        "Cast Speed",
        "Cooldown Reduction",
        "Movement Speed",
        "Healing Received",
        "Healing Done"
    };


    public List<string> GetItemStat()
    {
        string[] percentageValues = new string[]
        {
            "ResistanceGeneral",
            "ResistancePhysical",
            "ResistanceMagical",
            "ResistanceCC",
            "ResistanceFire",
            "AttackSpeed",
            "CastSpeed",
            "CooldownReduction",
            "MovementSpeed",
            "HealReceive",
            "HealDone"
        };

        List<string> stats = new List<string>();
        FieldInfo[] fields = this.GetType().GetFields();

        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].FieldType == typeof(int))
            {
                if ((int)fields[i].GetValue(this) > 0)
                {
                    if (Array.IndexOf(percentageValues, fields[i].Name) != -1)
                        stats.Add("+" + fields[i].GetValue(this) + "% " + FORMATTED_NAMES[i]);
                    else
                        stats.Add("+" + fields[i].GetValue(this) + " " + FORMATTED_NAMES[i]);
                }
            }
        }

        return stats;
    }
}

