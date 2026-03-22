using BepInEx.Configuration;
using FMOD;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Tools;
using UnityEngine;
using Utility.SerializableCollection;

namespace BOTrueZealMod.Tools
{
    [HarmonyPatch]
    public static class Tools
    {
        public static GameInformationHolder infoHolder;
        public static readonly Dictionary<string, AbilitySO> LoadedBossAbilities = [];

        public static Texture2D LoadTexture(string name)
        {
            if (!TryReadFromResource(name.TryAddExtension("png"), out var ba))
                return null;

            var tex = new Texture2D(1, 1);
            tex.LoadImage(ba);
            tex.filterMode = FilterMode.Point;

            return tex;
        }

        public static string GetID(string id)
        {
            return $"{MOD_PREFIX}_{id}";
        }

        public static bool TryReadFromResource(string resname, out byte[] ba)
        {
            var names = ModAssembly.GetManifestResourceNames().Where(x => x.EndsWith($".{resname}"));
            if (names.Count() <= 0)
            {
                Debug.LogError($"Couldn't load from resource name {resname}, returning an empty byte array.");
                ba = [];
                return false;
            }

            using var strem = ModAssembly.GetManifestResourceStream(names.First());
            ba = new byte[strem.Length];
            strem.Read(ba, 0, ba.Length);
            return true;
        }

        public static Sprite LoadSprite(string name, Vector2? pivot = null, int pixelsperunit = 32)
        {
            var tex = LoadTexture(name);

            if (tex == null)
                return null;

            return Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), pivot ?? new Vector2(0.5f, 0.5f), pixelsperunit);
        }

        public static string ToCodeName(this string orig)
        {
            return orig.Replace("'", "").Replace("-", "").Replace(" ", "");
        }

        public static string FormatEffectText(int dura, int restrict)
        {
            var text = "";

            if (dura > 0)
                text += dura;

            if (restrict > 0)
                text = text + "(" + restrict + ")";

            return text;
        }

        public static string TryAddExtension(this string n, string e)
        {
            if (n.EndsWith($".{e}"))
            {
                return n;
            }
            return n + $".{e}";
        }

        public static void LoadFMODBankFromResource(string resname, bool loadSamples = false)
        {
            if(TryReadFromResource(resname.TryAddExtension("bank"), out var ba))
            {
                LoadFMODBankFromBytes(ba, resname, loadSamples);
            }
        }

        public static void LoadFMODBankFromBytes(byte[] ba, string bankName, bool loadSamples = false)
        {
            if (RuntimeManager.Instance.loadedBanks.ContainsKey(bankName))
            {
                var loadedBank = RuntimeManager.Instance.loadedBanks[bankName];
                loadedBank.RefCount++;
                if (loadSamples)
                {
                    loadedBank.Bank.loadSampleData();
                }
                return;
            }
            RuntimeManager.LoadedBank value = default;
            var res = RuntimeManager.Instance.studioSystem.loadBankMemory(ba, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out value.Bank);
            switch (res)
            {
                case RESULT.OK:
                    value.RefCount = 1;
                    RuntimeManager.Instance.loadedBanks.Add(bankName, value);
                    if (loadSamples)
                    {
                        value.Bank.loadSampleData();
                    }
                    break;
                case RESULT.ERR_EVENT_ALREADY_LOADED:
                    value.RefCount = 2;
                    RuntimeManager.Instance.loadedBanks.Add(bankName, value);
                    break;
                default:
                    throw new BankLoadException(bankName, res);
            }
        }

        public static T CreateScriptable<T>(Action<T> configure = null) where T : ScriptableObject
        {
            var s = ScriptableObject.CreateInstance<T>();
            configure?.Invoke(s);
            return s;
        }

        public static AbilitySO GetAnyAbility(string name)
        {
            var ab = LoadedAssetsHandler.GetCharacterAbility(name);
            if(ab != null)
                return ab;

            ab = LoadedAssetsHandler.GetEnemyAbility(name);
            if(ab != null)
                return ab;

            ab = GetBossAbility(name);
            if(ab != null)
                return ab;

            Debug.LogError($"Failed to get ability with ID {name}");
            return null;
        }

        public static AbilitySO GetBossAbility(string abilityName)
        {
            if (!LoadedBossAbilities.TryGetValue(abilityName, out var value))
            {
                value = LoadBossAbility(abilityName);
                LoadedBossAbilities.Add(abilityName, value);
            }
            return value;
        }

        public static AbilitySO LoadBossAbility(string abilityName)
        {
            return Resources.Load("abilities/boss/" + abilityName) as AbilitySO;
        }

        public static string TrimGuid(this string str)
        {
            if (str.Contains("."))
                return str.Substring(str.LastIndexOf(".") + 1);

            return str;
        }

        public static List<CombatAbility> GetAbilities(this IUnit unit)
        {
            if(unit is CharacterCombat cc)
                return cc.CombatAbilities ?? [];

            else if(unit is EnemyCombat ec)
                return ec.Abilities ?? [];

            return [];
        }
        
        public static void UpdateUIAbilities(this IUnit unit)
        {
            if(unit is CharacterCombat cc)
                CombatManager.Instance.AddUIAction(new CharacterUpdateAllAttacksUIAction(cc.ID, [.. cc.CombatAbilities]));

            else if(unit is EnemyCombat ec)
                CombatManager.Instance.AddUIAction(new EnemyUpdateAllAttacksUIAction(ec.ID, [.. ec.Abilities]));
        }

        public static Vector3 Vector3Divide(Vector3 left, Vector3 right)
        {
            return new(left.x / right.x, left.y / right.y, left.z / right.z);
        }

        public static T AddComponent<T>(this Component comp) where T : Component
        {
            if(comp == null || comp.gameObject == null)
                return null;

            return comp.gameObject.AddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this Component comp) where T : Component
        {
            if(comp == null || comp.gameObject == null)
                return null;

            return comp.gameObject.GetOrAddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if(go == null)
                return null;

            if(go.GetComponent<T>() != null)
                return go.GetComponent<T>();

            return go.AddComponent<T>();
        }

        public static IntentType IntentForDamage(int damage)
        {
            return damage switch
            {
                <= 2 => IntentType.Damage_1_2,
                <= 6 => IntentType.Damage_3_6,
                <= 10 => IntentType.Damage_7_10,
                <= 15 => IntentType.Damage_11_15,
                <= 20 => IntentType.Damage_16_20,

                _ => IntentType.Damage_21
            };
        }

        public static IntentType IntentForHealing(int healing)
        {
            return healing switch
            {
                <= 4 => IntentType.Heal_1_4,
                <= 10 => IntentType.Heal_5_10,
                <= 20 => IntentType.Heal_11_20,

                _ => IntentType.Heal_21,
            };
        }

        public static bool IsOpposing(IUnit first, IUnit second)
        {
            return
                first != null &&
                second != null &&
                first.IsUnitCharacter != second.IsUnitCharacter &&
                    ((second.SlotID >= first.SlotID && second.SlotID <= first.LastSlotId()) ||
                    (second.LastSlotId() >= first.SlotID && second.LastSlotId() <= first.LastSlotId()) ||
                    (first.SlotID >= second.SlotID && first.SlotID <= second.LastSlotId()) ||
                    (first.LastSlotId() >= second.SlotID && first.LastSlotId() <= second.LastSlotId()));
        }

        public static CharacterCombat RealCharacter(this CharacterCombatUIInfo self)
        {
            return CombatManager.Instance._stats.Characters[self.ID];
        }

        public static int LastSlotId(this IUnit u)
        {
            return u.SlotID + u.Size - 1;
        }

        public static IntentTargetInfo TargetIntent(BaseCombatTargettingSO target, params IntentType[] intents)
        {
            return new()
            {
                targets = target,
                targetIntents = intents
            };
        }

        public static List<TargetSlotInfo> GetAllUnitTargetSlotsAsList(this SlotsCombat self, bool getCharacters, bool getAllUnitSlots, int ignoreCastSlot = -1)
        {
            var ret = new List<TargetSlotInfo>();

            if (getCharacters)
            {
                foreach (var combatSlot in self.CharacterSlots)
                {
                    if (combatSlot.HasUnit && combatSlot.Unit.SlotID != ignoreCastSlot)
                        ret.Add(combatSlot.TargetSlotInformation);
                }
            }
            else
            {
                for (var j = 0; j < self.EnemySlots.Length; j++)
                {
                    if (getAllUnitSlots)
                    {
                        if (self.EnemySlots[j].HasUnit && self.EnemySlots[j].Unit.SlotID != ignoreCastSlot)
                            ret.Add(self.EnemySlots[j].TargetSlotInformation);
                    }
                    else if (self.EnemySlots[j].HasUnit && self.EnemySlots[j].Unit.SlotID == j && self.EnemySlots[j].Unit.SlotID != ignoreCastSlot)
                        ret.Add(self.EnemySlots[j].TargetSlotInformation);
                }
            }
            return ret;
        }

        public static TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> create, bool hasCustomParam)
        {
            if (hasCustomParam)
                return create(key);

            if (dict.TryGetValue(key, out TValue value))
                return value;
            return dict[key] = create(key);
        }

        public static void AddOrSet<TKey, TValue>(this SerializableDictionary<TKey, TValue> sdict, TKey key, TValue value)
        {
            if(sdict.ContainsKey(key))
                sdict[key] = value;
            else
                sdict.Add(key, value);
        }

        public static EffectInfo FindEffectInfo<T>(this IEnumerable<EffectInfo> effects) where T : EffectSO
        {
            foreach (var e in effects)
            {
                if (e != null && e.effect is T)
                    return e;
            }

            return null;
        }

        public static T FindEffectSO<T>(this IEnumerable<EffectInfo> effects) where T : EffectSO
        {
            foreach (var e in effects)
            {
                if (e != null && e.effect is T t)
                    return t;
            }

            return null;
        }

        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T> orig)
        {
            var i = 0;

            foreach(var elem in orig)
            {
                yield return (i, elem);
                i++;
            }
        }

        public static StringTrioData GetCustomAchievementData(this InGameLanguage lang, string id, string defaultName, string defaultDescription, string defaultDescription2)
        {
            if (lang._achievementsData.TryGetValue(GetID(id), out var value))
                return value;

            return new StringTrioData(defaultName, defaultDescription, defaultDescription2);
        }
    }
}
