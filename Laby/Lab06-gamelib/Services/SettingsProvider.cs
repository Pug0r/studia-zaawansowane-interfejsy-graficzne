using Lab06_gamelib;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.Json;

namespace Lab06_gamelib.Services
{
    public class SettingsProvider
    {
        private readonly GameSettings _settings;
        private const string SettingsFileName = "settings.json";

        public GameSettings Settings => _settings;

        public SettingsProvider()
        {
            var basePath = AppContext.BaseDirectory;
            var settingsPath = Path.Combine(basePath, SettingsFileName);
            if (!File.Exists(settingsPath))
            {
                throw new FileNotFoundException($"Critical Error: {SettingsFileName} not found!");
            }

            string json = File.ReadAllText(settingsPath);
            _settings = JsonSerializer.Deserialize<GameSettings>(json)
                        ?? throw new Exception("Failed to parse settings.json");
        }
    }
}

