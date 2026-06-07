using OllieAve.FoldOrBeTold.Entities.Constants;

namespace OllieAve.FoldOrBeTold.Entities.Helpers;

internal static class FileSystemHelper
{
    public static string GetAssetPath(string assetFileName)
    {
        string texturePath = Path.Combine(
            AppContext.BaseDirectory,
            EntityConstants.AssetFolderName,
            assetFileName);

        return texturePath;
    }

    public static string GetLevelFilePath(string levelFileName)
    {
        string texturePath = Path.Combine(
            AppContext.BaseDirectory,
            EntityConstants.LevelFileFolderName,
            levelFileName);

        return texturePath;
    }

    public static string GetShaderPath(string shaderFileName)
    {
        string shaderPath = Path.Combine(
            AppContext.BaseDirectory,
            EntityConstants.ShaderFolderName,
            shaderFileName);

        return shaderPath;

    }
}

