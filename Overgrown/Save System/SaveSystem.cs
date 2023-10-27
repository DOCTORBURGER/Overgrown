using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using Overgrown.Entities;
using Overgrown.Scenes;
using Microsoft.Xna.Framework;

namespace Overgrown.Save_System
{
    public static class SaveSystem
    {
        private static string _saveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Overgrown");
        private static string _saveFile = Path.Combine(_saveDirectory, "savegame.json");

        public static void Save(Player player)
        {
            SaveData saveData = new SaveData()
            {
                PlayerX = (int)player.Position.X,
                PlayerY = (int)player.Position.Y,
            };

            Directory.CreateDirectory(_saveDirectory);

            string json = JsonSerializer.Serialize(saveData);
            File.WriteAllText(_saveFile, json);
        }

        public static Vector2? Load()
        {
            if (!File.Exists(_saveFile))
            {
                return null;
            }

            string json = File.ReadAllText(_saveFile);
            SaveData loadedData = JsonSerializer.Deserialize<SaveData>(json);

            return new Vector2(loadedData.PlayerX, loadedData.PlayerY);
        }
    }
}
