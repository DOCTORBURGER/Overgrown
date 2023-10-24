using Microsoft.Xna.Framework;
using Overgrown.UI;
using System;
using System.Collections.Generic;

namespace Overgrown.Scenes
{
    public class SettingsMenuScene : MenuScene
    {
        private List<Point> _supportedResolutions = new List<Point>
        {
            new Point(800, 480),
            new Point(1600, 960),
            new Point(2400, 1440)
        };

        private static int _currentResolutionIndex = 0;

        private static bool _fullscreenEnabled = false;

        public SettingsMenuScene() : base("Settings")
        {
            MenuEntry soundEffectVolumeEntry = new("Sound Effects: #");
            MenuEntry musicVolumeEntry = new("Music: #");
            MenuEntry resolutionEntry = new($"Resolution: {_supportedResolutions[_currentResolutionIndex].X}x{_supportedResolutions[_currentResolutionIndex].Y}");
            MenuEntry fullscreenEntry = new($"Fullscreen: {(_fullscreenEnabled ? "Enabled" : "Disabled")}");
            MenuEntry backEntry = new("Back");

            resolutionEntry.Selected += SetResolution;
            fullscreenEntry.Selected += SetFullScreen;
            backEntry.Selected += Back;

            _menuEntries.Add(soundEffectVolumeEntry);
            _menuEntries.Add(musicVolumeEntry);
            _menuEntries.Add(resolutionEntry);
            _menuEntries.Add(fullscreenEntry);
            _menuEntries.Add(backEntry);
        }

        private void SetResolution(object sender, EventArgs e)
        {
            _currentResolutionIndex++;
            if (_currentResolutionIndex >= _supportedResolutions.Count)
                _currentResolutionIndex = 0;
            _menuEntries[2].Text = $"Resolution: {_supportedResolutions[_currentResolutionIndex].X}x{_supportedResolutions[_currentResolutionIndex].Y}";
            SceneManager.SetResolution(_supportedResolutions[_currentResolutionIndex]);
        }

        private void SetFullScreen(object sender, EventArgs e)
        {
            _fullscreenEnabled = SceneManager.SetFullScreen();
        }

        private void Back(object sender, EventArgs e)
        {
            SceneManager.SetScene(new MainMenuScene());
        }
    }
}
