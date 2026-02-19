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
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Catalog;
using StardustSandbox.Core.Colors.Palettes;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.States;
using StardustSandbox.Core.InputSystem;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.UI.Elements;
using StardustSandbox.Core.UI.Information;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace StardustSandbox.Core.UI.Common
{
    internal sealed partial class ItemSearchUI : UIBase
    {
        private Action<Item> itemSelectionCallback;

        private Label placeholderLabel, searchQueryLabel;
        private Image panelBackground, shadowBackground;

        private SlotInfo exitButtonSlotInfo;

        private readonly ButtonInfo exitButtonInfo;
        private readonly SlotInfo[] itemButtonSlotInfos;

        private readonly StringBuilder searchQueryStringBuilder = new();

        private readonly List<SearchIndexEntry> searchIndex;
        private readonly List<SearchIndexEntry> searchResults;
        private readonly List<SearchMatch> matchesBuffer;

        private readonly GameWindow gameWindow;
        private readonly TooltipBox tooltipBox;
        private readonly PlayerInputController playerInputController;
        private readonly UIManager uiManager;

        private static readonly StringBuilder normalizeBuilder = new(256);

        internal ItemSearchUI(
            GameWindow gameWindow,
            PlayerInputController playerInputController,
            TooltipBox tooltipBox,
            UIManager uiManager
        ) : base()
        {
            this.gameWindow = gameWindow;
            this.tooltipBox = tooltipBox;
            this.playerInputController = playerInputController;
            this.uiManager = uiManager;

            this.itemButtonSlotInfos = new SlotInfo[UIConstants.ITEM_SEARCH_ITEMS_PER_PAGE];

            this.exitButtonInfo = new(TextureIndex.IconUI, new(224, 0, 32, 32), Localization_Statements.Exit, Localization_GUIs.Button_Exit_Description, uiManager.CloseUI);

            // Preallocate common collections to avoid repeated resizes.
            this.searchIndex = new List<SearchIndexEntry>(UIConstants.ITEM_SERACH_EXPECTED_ITEMS);
            this.searchResults = new List<SearchIndexEntry>(UIConstants.ITEM_SEARCH_ITEMS_PER_PAGE);
            this.matchesBuffer = new List<SearchMatch>(UIConstants.ITEM_SERACH_EXPECTED_ITEMS);

            // Build index once at startup
            foreach (Item item in CatalogDatabase.GetAllItems())
            {
                if (item is not null)
                {
                    this.searchIndex.Add(new(item));
                }
            }
        }

        internal void Setup(Action<Item> itemSelectionCallback)
        {
            this.itemSelectionCallback = itemSelectionCallback;
        }

        private static void PlayTypingSound()
        {
            SoundEngine.Play((SoundEffectIndex)Randomness.Random.Range((int)SoundEffectIndex.GUI_Typing_1, (int)SoundEffectIndex.GUI_Typing_5));
        }

        // Normalize + remove diacritics + lower invariant
        // Uses a shared StringBuilder to reduce GC
        private static string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            string formD = value.Normalize(NormalizationForm.FormD);
            _ = normalizeBuilder.Clear();

            for (int i = 0; i < formD.Length; i++)
            {
                char c = formD[i];

                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    _ = normalizeBuilder.Append(c);
                }
            }

            return normalizeBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC)
                .ToLowerInvariant();
        }

        // Main search routine with tokenization and scoring
        // Results are cached in searchResults
        private void SearchItems()
        {
            string rawQuery = this.searchQueryStringBuilder.ToString();

            // Trim locally without allocating a new string if possible
            int start = 0;
            int end = rawQuery.Length - 1;

            while (start <= end && char.IsWhiteSpace(rawQuery[start]))
            {
                start++;
            }

            while (end >= start && char.IsWhiteSpace(rawQuery[end]))
            {
                end--;
            }

            if (start > end)
            {
                this.searchResults.Clear();
                return;
            }

            string trimmed = rawQuery.Substring(start, end - start + 1);
            string normalizedQuery = Normalize(trimmed);

            // Tokenize query (split on spaces)
            // Keep small arrays
            string[] tokens = normalizedQuery.Split([' '], StringSplitOptions.RemoveEmptyEntries);

            // Reuse matches buffer to collect scored candidates.
            this.matchesBuffer.Clear();

            for (int i = 0; i < this.searchIndex.Count; i++)
            {
                SearchIndexEntry entry = this.searchIndex[i];

                int totalScore = 0;
                bool allTokensMatch = true;

                // For each token, check presence and contribution to score
                for (int t = 0; t < tokens.Length; t++)
                {
                    string token = tokens[t];

                    if (entry.NormalizedName.StartsWith(token, StringComparison.Ordinal))
                    {
                        totalScore += 100;
                        continue;
                    }

                    if (entry.NormalizedName.Contains(token, StringComparison.Ordinal))
                    {
                        totalScore += 50;
                        continue;
                    }

                    if (entry.NormalizedDescription.Contains(token, StringComparison.Ordinal))
                    {
                        totalScore += 10;
                        continue;
                    }

                    allTokensMatch = false;
                    break;
                }

                if (allTokensMatch && totalScore > 0)
                {
                    this.matchesBuffer.Add(new(entry, totalScore));
                }
            }

            // No matches -> clear results (UI will show default items later)
            if (this.matchesBuffer.Count == 0)
            {
                this.searchResults.Clear();
                return;
            }

            // Sort matches by score descending
            // Matches count will be small (<= items count)
            this.matchesBuffer.Sort((a, b) => b.Score.CompareTo(a.Score));

            // Fill searchResults with top N without reallocating list internals
            this.searchResults.Clear();

            for (int i = 0, take = Math.Min(UIConstants.ITEM_SEARCH_ITEMS_PER_PAGE, this.matchesBuffer.Count); i < take; i++)
            {
                this.searchResults.Add(this.matchesBuffer[i].Entry);
            }
        }

        // Populate/refresh UI slots from searchResults
        // Minimizes property sets when item unchanged
        private void RefreshItemCatalog()
        {
            // If there are no search results (empty query), show first page of items as default.
            if (this.searchResults.Count == 0)
            {
                int count = Math.Min(UIConstants.ITEM_SEARCH_ITEMS_PER_PAGE, this.searchIndex.Count);
                for (int i = 0; i < count; i++)
                {
                    this.searchResults.Add(this.searchIndex[i]);
                }
            }

            for (int i = 0; i < this.itemButtonSlotInfos.Length; i++)
            {
                SlotInfo itemSlot = this.itemButtonSlotInfos[i];

                if (i < this.searchResults.Count)
                {
                    itemSlot.Background.CanDraw = true;

                    Item item = this.searchResults[i].Item;

                    if (item is null)
                    {
                        continue;
                    }

                    itemSlot.Icon.TextureIndex = item.TextureIndex;
                    itemSlot.Icon.SourceRectangle = item.SourceRectangle;
                }
                else
                {
                    itemSlot.Background.CanDraw = false;
                }
            }
        }

        protected override void OnBuild(Container root)
        {
            BuildBackground(root);
            BuildItemSlots();
            BuildLabels();
            BuildExitButton();

            root.AddChild(this.tooltipBox);
        }

        private void BuildBackground(Container root)
        {
            this.shadowBackground = new()
            {
                TextureIndex = TextureIndex.Pixel,
                Scale = GameScreen.GetViewport(),
                Color = new(AAP64ColorPalette.DarkGray, 160),
                Size = Vector2.One,
            };

            this.panelBackground = new()
            {
                TextureIndex = TextureIndex.UIBackgroundItemSearch,
                Size = new(542.0f, 540.0f),
                Alignment = UIDirection.Center,
            };

            root.AddChild(this.shadowBackground);
            root.AddChild(this.panelBackground);
        }

        private void BuildItemSlots()
        {
            Vector2 margin = new(38.0f, 114.0f);

            int index = 0;

            for (byte row = 0; row < UIConstants.ITEM_SEARCH_ITEMS_PER_ROW; row++)
            {
                for (byte col = 0; col < UIConstants.ITEM_SEARCH_ITEMS_PER_COLUMN; col++)
                {
                    SlotInfo slot = new(
                        new()
                        {
                            TextureIndex = TextureIndex.UIButtons,
                            SourceRectangle = new(320, 140, 32, 32),
                            Alignment = UIDirection.Northwest,
                            Scale = new(2.0f),
                            Size = new(32.0f),
                            Margin = margin
                        },

                        new()
                        {
                            Alignment = UIDirection.Center,
                            TextureIndex = TextureIndex.IconElements,
                            SourceRectangle = new(0, 0, 32, 32),
                            Scale = new(1.5f),
                            Size = new(32.0f)
                        }
                    );

                    // Position
                    this.panelBackground.AddChild(slot.Background);
                    slot.Background.AddChild(slot.Icon);

                    // Spacing
                    margin.X += 80.0f;

                    this.itemButtonSlotInfos[index] = slot;
                    index++;
                }

                margin.X = 38.0f;
                margin.Y += 80.0f;
            }
        }

        private void BuildLabels()
        {
            Vector2 margin = new(96.0f, 16.0f);

            this.placeholderLabel = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.1f),
                TextContent = "Search for an item...",
                Color = AAP64ColorPalette.White,
                Alignment = UIDirection.Northwest,
                Margin = margin,
            };

            this.searchQueryLabel = new()
            {
                SpriteFontIndex = SpriteFontIndex.BigApple3pm,
                Scale = new(0.1f),
                Color = AAP64ColorPalette.White,
                Alignment = UIDirection.Northwest,
                Margin = margin,
            };

            this.panelBackground.AddChild(this.placeholderLabel);
            this.panelBackground.AddChild(this.searchQueryLabel);
        }

        private void BuildExitButton()
        {
            SlotInfo slot = UIBuilderUtility.BuildButtonSlot(new(-32.0f, -72.0f), this.exitButtonInfo);

            slot.Background.Alignment = UIDirection.Northeast;
            slot.Icon.Alignment = UIDirection.Center;

            this.panelBackground.AddChild(slot.Background);
            slot.Background.AddChild(slot.Icon);

            this.exitButtonSlotInfo = slot;
        }

        protected override void OnResize(Vector2 newSize)
        {
            this.shadowBackground.Scale = newSize;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            this.tooltipBox.CanDraw = false;

            UpdateExitButton();
            UpdateItemCatalog();
        }

        private void UpdateExitButton()
        {
            if (Interaction.OnMouseEnter(this.exitButtonSlotInfo.Background))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Hover);
            }

            if (Interaction.OnMouseLeftClick(this.exitButtonSlotInfo.Background))
            {
                SoundEngine.Play(SoundEffectIndex.GUI_Click);
                this.exitButtonInfo.ClickAction?.Invoke();
            }

            if (Interaction.OnMouseOver(this.exitButtonSlotInfo.Background))
            {
                this.tooltipBox.CanDraw = true;

                TooltipBoxContent.SetTitle(this.exitButtonInfo.Name);
                TooltipBoxContent.SetDescription(this.exitButtonInfo.Description);

                this.exitButtonSlotInfo.Background.Color = AAP64ColorPalette.HoverColor;
            }
            else
            {
                this.exitButtonSlotInfo.Background.Color = AAP64ColorPalette.White;
            }
        }

        private void UpdateItemCatalog()
        {
            for (int i = 0; i < this.itemButtonSlotInfos.Length; i++)
            {
                SlotInfo slot = this.itemButtonSlotInfos[i];

                if (!slot.Background.CanDraw)
                {
                    break;
                }

                Item item = this.searchResults[i].Item;

                if (Interaction.OnMouseEnter(slot.Background))
                {
                    SoundEngine.Play(SoundEffectIndex.GUI_Hover);
                }

                if (Interaction.OnMouseLeftClick(slot.Background))
                {
                    this.uiManager.CloseUI();
                    this.itemSelectionCallback?.Invoke(item);
                    break;
                }

                if (Interaction.OnMouseOver(slot.Background))
                {
                    this.tooltipBox.CanDraw = true;

                    TooltipBoxContent.SetTitle(item.Name);
                    TooltipBoxContent.SetDescription(item.Description);

                    slot.Background.Color = AAP64ColorPalette.HoverColor;
                }
                else
                {
                    slot.Background.Color = AAP64ColorPalette.White;
                }
            }
        }

        protected override void OnOpened()
        {
            GameHandler.SetState(GameStates.IsCriticalMenuOpen);

            this.searchResults.Clear();
            this.searchQueryStringBuilder.Clear();
            this.searchQueryLabel.TextContent = string.Empty;

            this.playerInputController.Disable();

            this.gameWindow.KeyDown += OnKeyDown;
            this.gameWindow.TextInput += OnTextInput;

            UpdateDisplayedText();
            RefreshItemCatalog();
        }

        protected override void OnClosed()
        {
            GameHandler.RemoveState(GameStates.IsCriticalMenuOpen);

            this.playerInputController.Enable();

            this.gameWindow.KeyDown -= OnKeyDown;
            this.gameWindow.TextInput -= OnTextInput;
        }

        private void OnKeyDown(object sender, InputKeyEventArgs inputKeyEventArgs)
        {
            if (!IsSpecialKey(inputKeyEventArgs.Key, out Action specialKeyAction))
            {
                return;
            }

            PlayTypingSound();
            specialKeyAction?.Invoke();

            UpdateDisplayedText();
            SearchItems();
            RefreshItemCatalog();
        }

        private void OnTextInput(object sender, TextInputEventArgs textInputEventArgs)
        {
            if (IsSpecialKey(textInputEventArgs.Key, out _))
            {
                return;
            }

            PlayTypingSound();
            AddCharacter(textInputEventArgs.Character);

            UpdateDisplayedText();
            SearchItems();
            RefreshItemCatalog();
        }

        private bool IsSpecialKey(Keys key, out Action action)
        {
            action = key switch
            {
                Keys.Space => HandleSpaceKey,
                Keys.Back => HandleBackspaceKey,
                _ => null,
            };

            return action != null;
        }

        private void HandleBackspaceKey()
        {
            if (this.searchQueryStringBuilder.Length == 0)
            {
                return;
            }

            _ = this.searchQueryStringBuilder.Remove(this.searchQueryStringBuilder.Length - 1, 1);
        }

        private void HandleSpaceKey()
        {
            _ = this.searchQueryStringBuilder.Append(' ');
        }

        private void AddCharacter(char character)
        {
            if (char.IsControl(character))
            {
                return;
            }

            _ = this.searchQueryStringBuilder.Append(character);
        }

        private void UpdateDisplayedText()
        {
            this.searchQueryLabel.TextContent = this.searchQueryStringBuilder.ToString();

            if (this.searchQueryStringBuilder.Length == 0)
            {
                this.placeholderLabel.CanDraw = true;
                this.searchQueryLabel.CanDraw = false;
            }
            else
            {
                this.placeholderLabel.CanDraw = false;
                this.searchQueryLabel.CanDraw = true;
            }
        }
    }
}
