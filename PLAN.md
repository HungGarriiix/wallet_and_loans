# Refactor Configuration Loading Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use bp:subagent-driven-development (recommended) or bp:executing-plans to implement this plan task-by-task. Steps use checkbox syntax for tracking.

**Goal:** Refactor the configuration loading logic from `Program.cs` into `ConfigJSON.cs` to improve modularity and provide a clean `ReadConfig` (or similar) method for the application to use.

**Architecture:** Move configuration-specific logic (JSON parsing, environment variable overrides, file discovery) from the main application entry point to the `ConfigJSON` class.

**Tech Stack:** .NET, DSharpPlus, Newtonsoft.Json

---

### Task 1: Refactor `ConfigJSON.cs` to include loading logic

**Files:**
- Modify: `yuuka-chan/ConfigJSON.cs`

- [ ] **Step 1: Add helper methods and `ReadConfig` to `ConfigJSON.cs`**

Move the logic from `Program.cs` (`FindExistingFile`, `LoadDotEnv`, `GetConfigValue`, and the core of `LoadConfigAsync`) into `ConfigJSON.cs` as static methods.

```csharp
public static async Task<ConfigJSON> ReadConfig()
{
    ConfigJSON configJson = new ConfigJSON();
    string? configPath = FindExistingFile("config.json", Path.Combine("yuuka-chan", "config.json"));
    if (configPath != null)
    {
        var json = await File.ReadAllTextAsync(configPath);
        configJson = JsonConvert.DeserializeObject<ConfigJSON>(json) ?? new ConfigJSON();
    }

    string? envPath = FindExistingFile(".env", Path.Combine("yuuka-chan", ".env"));
    var envMap = LoadDotEnv(envPath);
    configJson.Token = GetConfigValue(new[] { "TOKEN" }, envMap, configJson.Token);
    configJson.Prefix = GetConfigValue(new[] { "PREFIX" }, envMap, configJson.Prefix);
    configJson.URL = GetConfigValue(new[] { "URL" }, envMap, configJson.URL);

    string guildId = GetConfigValue(new[] { "GUILD_ID", "GUILDID" }, envMap, configJson.GuildID.ToString());
    if (!string.IsNullOrWhiteSpace(guildId))
        configJson.GuildID = ulong.Parse(guildId);

    // Validation
    if (string.IsNullOrWhiteSpace(configJson.Token))
        throw new Exception("Missing bot token. Set TOKEN in .env or token in config.json.");
    if (string.IsNullOrWhiteSpace(configJson.URL))
        throw new Exception("Missing API URL. Set URL in .env or url in config.json.");
    if (configJson.GuildID == 0)
        throw new Exception("Missing guild id. Set GUILD_ID in .env or guildId in config.json.");

    return configJson;
}
```

- [ ] **Step 2: Add private helper methods to `ConfigJSON.cs`**

Include `FindExistingFile`, `LoadDotEnv`, and `GetConfigValue` as private static methods in `ConfigJSON.cs`.

---

### Task 2: Clean up `Program.cs`

**Files:**
- Modify: `yuuka-chan/Program.cs`

- [ ] **Step 1: Update `Main` to use `ConfigJSON.ReadConfig()`**

Replace `await LoadConfigAsync()` with `await ConfigJSON.ReadConfig()`.

- [ ] **Step 2: Remove redundant methods from `Program.cs`**

Delete `LoadConfigAsync`, `FindExistingFile`, `LoadDotEnv`, and `GetConfigValue` from `Program.cs`.

---

### Task 3: Verification

- [ ] **Step 1: Build the project**

Run: `dotnet build yuuka-chan/yuuka-chan.csproj`
Expected: Build successful.

- [ ] **Step 2: Dry run (optional)**

If possible, run the bot to ensure it still reads configuration correctly (will likely fail at connection if tokens are invalid, but loading should work).
