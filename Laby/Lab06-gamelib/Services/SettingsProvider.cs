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
        private const string SettingsPath = "settings.json";

        public GameSettings Settings => _settings;

        public SettingsProvider()
        {
            if (!File.Exists(SettingsPath))
            {
                throw new FileNotFoundException($"Critical Error: {SettingsPath} not found!");
            }

            string json = File.ReadAllText(SettingsPath);
            _settings = JsonSerializer.Deserialize<GameSettings>(json)
                        ?? throw new Exception("Failed to parse settings.json");
        }
    }
}

