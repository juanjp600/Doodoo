using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FZH;

internal readonly record struct Configuration(
    ImmutableArray<Configuration.GhRepo> Repos,
    ImmutableArray<Configuration.ZhWorkspace> Workspaces,
    Configuration.ZhGraphQlKey UserZhGraphQlKey)
{
    public readonly record struct GhRepo(ulong Id);
    public readonly record struct ZhWorkspace(string Id);
    public readonly record struct ZhGraphQlKey(string Value)
    {
        public bool Validate() => Value.StartsWith("zh_");
    }

    private static readonly Configuration DefaultConfig
        = new Configuration(
            Repos: new[] { new GhRepo(Id: 0) }.ToImmutableArray(),
            Workspaces: new[] { new ZhWorkspace("?????") }.ToImmutableArray(),
            UserZhGraphQlKey: new ZhGraphQlKey("Get a GraphQL key at https://app.zenhub.com/settings/tokens"));

    private static readonly Configuration BlankConfig
        = new Configuration(
            Repos: ImmutableArray<GhRepo>.Empty,
            Workspaces: ImmutableArray<ZhWorkspace>.Empty,
            UserZhGraphQlKey: new ZhGraphQlKey(""));
    
    private const string ConfigDir = "Cfg";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        IgnoreReadOnlyFields = false,
        IgnoreReadOnlyProperties = false,
        IncludeFields = true,
        WriteIndented = true
    };
    
    private static string ZhAuthFilePath
        => Path.Combine(ConfigDir, "ZhAuth.json");

    public static void EnsureDirExists()
    {
        Directory.CreateDirectory(ConfigDir);
        if (!File.Exists(ZhAuthFilePath))
        {
            File.WriteAllText(ZhAuthFilePath,
                JsonSerializer.Serialize(
                    value: DefaultConfig,
                    options: JsonSerializerOptions));
        }
    }

    public static Configuration LoadConfig()
    {
        if (!File.Exists(ZhAuthFilePath)) { return BlankConfig; }

        string str = File.ReadAllText(ZhAuthFilePath).Trim();
        if (JsonSerializer.Deserialize(str, typeof(Configuration), JsonSerializerOptions)
                is Configuration loadedConfig && loadedConfig.Validate())
        {
            return loadedConfig;
        }

        return BlankConfig;
    }

    private bool Validate()
    {
        return UserZhGraphQlKey.Validate();
    }
}