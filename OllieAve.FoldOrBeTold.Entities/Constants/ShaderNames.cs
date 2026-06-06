using System.Reflection;

namespace OllieAve.FoldOrBeTold.Entities.Constants;

public class ShaderNames
{
    public static string WhiteOutlineShaderName => "WhiteOutline.fs";

    private static readonly Lazy<List<string>> _all = new(() =>
        [.. typeof(ShaderNames)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(p => p.PropertyType == typeof(string))
            .Select(p => (string)p.GetValue(null)!)]
    );

    public static List<string> All => _all.Value;
}
