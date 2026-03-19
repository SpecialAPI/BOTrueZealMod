using System;
using System.Collections.Generic;
using System.Text;

namespace BOTrueZealMod.Tools
{
    public static class Magic
    {
        public static void ExtendAllEnums()
        {
            foreach (var type in ModAssembly.GetTypes())
            {
                var custom = type.GetCustomAttributes(false);
                if (custom == null)
                    continue;

                var extension = custom.OfType<EnumExtensionAttribute>().FirstOrDefault();
                if (extension == null || extension.type == null || !extension.type.IsEnum)
                    continue;

                foreach (var f in type.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    f.SetValue(null, ExtendEnum(f.Name, extension.type));
                }
            }
        }
    }

    public class EnumExtensionAttribute(Type extensiontype) : Attribute
    {
        public Type type = extensiontype;
    }
}
