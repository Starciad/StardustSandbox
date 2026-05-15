/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Cameras;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Backgrounds;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.Enums.Simulation;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Common;
using StardustSandbox.Core.WorldSystem;

using System;

namespace StardustSandbox.Core
{
    internal sealed class GameHandler : IResettable
    {
        internal SimulationSpeed SimulationSpeed => this.simulationSpeed;
        internal bool HasSaveFileLoaded => !string.IsNullOrWhiteSpace(this.loadedSaveFileName);
        internal string LoadedSaveFileName => this.loadedSaveFileName;

        private GameStates states;
        private SimulationSpeed simulationSpeed;
        private string loadedSaveFileName;

        private readonly ActorManager actorManager;
        private readonly AmbientManager ambientManager;
        private readonly Camera2D camera;
        private readonly GameWindow gameWindow;
        private readonly PlayerInputController playerInputController;
        private readonly SongManager songManager;
        private readonly UIDatabase uiDatabase;
        private readonly UIManager uiManager;
        private readonly World world;

        internal GameHandler(
            ActorManager actorManager,
            AmbientManager ambientManager,
            Camera2D camera,
            GameWindow gameWindow,
            PlayerInputController playerInputController,
            SongManager songManager,
            UIDatabase uiDatabase,
            UIManager uiManager,
            World world
        )
        {
            this.actorManager = actorManager;
            this.ambientManager = ambientManager;
            this.camera = camera;
            this.gameWindow = gameWindow;
            this.playerInputController = playerInputController;
            this.songManager = songManager;
            this.uiDatabase = uiDatabase;
            this.uiManager = uiManager;
            this.world = world;
        }

        internal void DefineLoadedSaveFile(string saveFileName)
        {
            this.loadedSaveFileName = saveFileName;

            this.gameWindow.Title = string.IsNullOrWhiteSpace(saveFileName)
                ? GameConstants.GetTitleAndVersionString()
                : string.Concat(saveFileName, " — ", GameConstants.GetTitleAndVersionString());
        }

        internal void LoadSaveFile(string saveFileName)
        {
            if (string.IsNullOrWhiteSpace(saveFileName))
            {
                throw new ArgumentException("Save file name cannot be null or whitespace.", nameof(saveFileName));
            }

            this.actorManager.Deserialize(saveFileName);
            this.world.Deserialize(saveFileName);

            DefineLoadedSaveFile(saveFileName);
        }

        internal void UnloadSaveFile()
        {
            DefineLoadedSaveFile(string.Empty);
        }

        public void Reset()
        {
            UnloadSaveFile();

            this.actorManager.Reset();
            this.world.Reset();
        }

        internal void StartGame()
        {
            this.camera.Reset();

            this.songManager.StopGameplayMusicCycle();
            this.songManager.StartGameplayMusicCycle();

            ((HudUI)this.uiDatabase.GetUI(UIIndex.Hud)).Setup();
            ((ItemExplorerUI)this.uiDatabase.GetUI(UIIndex.ItemExplorer)).Setup();

            this.uiManager.OpenUI(UIIndex.Hud);

            this.ambientManager.BackgroundHandler.SetBackground(BackgroundIndex.Ocean);

            Reset();
            this.world.StartNew(WorldConstants.WORLD_SIZES_TEMPLATE[0]);

            this.world.CanUpdate = true;
            this.world.CanDraw = true;

            this.actorManager.CanDraw = true;
            this.actorManager.CanUpdate = true;

            SetSpeed(SimulationSpeed.Normal);

            this.camera.SetPosition(this.camera.WorldToScreen(Vector2.Zero));

            this.playerInputController.Pen.Tool = PenTool.Pencil;
            this.playerInputController.Enable();
        }

        internal void StopGame()
        {
            UnloadSaveFile();

            this.songManager.StopGameplayMusicCycle();

            this.playerInputController.Pen.Tool = PenTool.Visualization;
            this.playerInputController.Disable();

            this.world.CanDraw = false;
            this.world.CanUpdate = false;

            this.actorManager.CanDraw = false;
            this.actorManager.CanUpdate = false;

            SetSpeed(SimulationSpeed.Normal);

            RemoveState(GameStates.IsPaused);
            RemoveState(GameStates.IsSimulationPaused);
            RemoveState(GameStates.IsCriticalMenuOpen);

            this.world.Time.InGameSecondsPerRealSecond = TimeConstants.DEFAULT_VERY_FAST_SECONDS_PER_FRAMES;
            this.world.Time.IsFrozen = false;
        }

        internal void SetSpeed(SimulationSpeed speed)
        {
            this.simulationSpeed = speed;

            this.world.SetSpeed(speed);
            this.actorManager.SetSpeed(speed);
        }

        internal void ReloadSaveFile()
        {
            this.actorManager.Reload();
            this.world.Reload();
        }

        internal bool HasState(GameStates value)
        {
            return this.states.HasFlag(value);
        }

        internal void SetState(GameStates value)
        {
            this.states |= value;
        }

        internal void RemoveState(GameStates value)
        {
            this.states &= ~value;
        }

        internal void ToggleState(GameStates value)
        {
            this.states ^= value;
        }
    }
}

