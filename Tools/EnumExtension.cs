using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BOTrueZealMod.Tools
{
    [HarmonyPatch]
    public static class EnumExtension
    {
		private readonly static Dictionary<Type, Dictionary<string, int>> extendedEnums = new();
		private readonly static Dictionary<object, Dictionary<string, ulong>> enumsToAdd = new();

		public static T ExtendEnum<T>(string name) where T : Enum
		{
			return (T)ExtendEnum(name, typeof(T));
		}

		public static string RemoveUnacceptableCharactersForEnum(this string str)
		{
			if (str == null)
				return null;

			return str.Replace("\"", "").Replace("\\", "").Replace(" ", "").Replace("-", "_").Replace("\n", "");
		}

		public static object ExtendEnum(string name, Type t)
		{
			if (!t.IsEnum)
				return 0;

			var guid = MOD_GUID;
			name = name.RemoveUnacceptableCharactersForEnum().Replace(".", "");
			guid = guid.RemoveUnacceptableCharactersForEnum();

			var guidandname = $"{guid}.{name}";
			object value = null;

            try
            {
				value = Enum.Parse(t, guidandname);
            }
            catch { }

            if (value != null)
				return value;

            var max = 0;
            try
            {
                max = Enum.GetValues(t).Cast<int>().Max();
            }
            catch
            {
            }

            int val;
            if (t.IsDefined(typeof(FlagsAttribute), false))
                val = max == 0 ? 1 : max * 2;
            else
                val = max + 1;

            if (!extendedEnums.TryGetValue(t, out var valuesForEnum))
                extendedEnums[t] = valuesForEnum = [];
            valuesForEnum.Add(guidandname, val);

            if (!enumsToAdd.TryGetValue(t, out var valuesForEnum2))
                enumsToAdd[t] = valuesForEnum2 = [];
            valuesForEnum2.Add(guidandname, (ulong)val);

            return val;
        }

		[HarmonyPatch(typeof(Enum), "GetCachedValuesAndNames")]
		[HarmonyPostfix]
		public static void AddStuff(object __result, object enumType, bool getNames)
        {
            if (!enumsToAdd.ContainsKey(enumType) || enumsToAdd[enumType].Count <= 0)
                return;

            var names = ((string[])valuesandnamesNames.GetValue(__result)).Concat(enumsToAdd[enumType].Keys).ToArray();
            var values = ((ulong[])valuesandnamesValues.GetValue(__result)).Concat(enumsToAdd[enumType].Values).ToArray();
            Array.Sort(values, names, Comparer<ulong>.Default);
            valuesandnamesNames.SetValue(__result, names);
            valuesandnamesValues.SetValue(__result, values);
            enumsToAdd[enumType].Clear();
        }

        public static FieldInfo valuesandnamesNames = AccessTools.Field(AccessTools.TypeByName("System.Enum+ValuesAndNames"), "Names");
		public static FieldInfo valuesandnamesValues = AccessTools.Field(AccessTools.TypeByName("System.Enum+ValuesAndNames"), "Values");
	}
}
