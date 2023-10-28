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
            new Point(1280, 720),
            new Point(1920, 1080),
            new Point(2560, 1440),
            new Point(3840, 2160)
        };

        private static int _currentResolutionIndex = 0;

        private static bool _fullscreenEnabled = false;

        public SettingsMenuScene() : base("Settings")
        {
            MenuEntry soundEffectVolumeEntry = new("Sound Effects: #");
            MenuEntry musicVolumeEntry = new("Music: #");
            MenuEntry resolutionEntry = new($"Resolution (Window): {_supportedResolutions[_currentResolutionIndex].X}x{_supportedResolutions[_currentResolutionIndex].Y}");
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

        public override void LoadContent()
        {
            base.LoadContent();

            _supportedResolutions = _supportedResolutions.FindAll(res => res.X <= SceneManager.ReturnMaxWidth());
        }

        private void SetResolution(object sender, EventArgs e)
        {
            _currentResolutionIndex++;
            if (_currentResolutionIndex >= _supportedResolutions.Count)
                _currentResolutionIndex = 0;
            _menuEntries[2].Text = $"Resolution (Window): {_supportedResolutions[_currentResolutionIndex].X}x{_supportedResolutions[_currentResolutionIndex].Y}";
            SceneManager.SetResolution(_supportedResolutions[_currentResolutionIndex]);
        }

        private void SetFullScreen(object sender, EventArgs e)
        {
            _fullscreenEnabled = SceneManager.SetFullScreen();
            if (!_fullscreenEnabled) { SceneManager.SetResolution(_supportedResolutions[_currentResolutionIndex]); }
            _menuEntries[3].Text = $"Fullscreen: {(_fullscreenEnabled ? "Enabled" : "Disabled")}";
        }

        private void Back(object sender, EventArgs e)
        {
            SceneManager.SetScene(new MainMenuScene());
        }
    }
}
