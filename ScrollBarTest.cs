using Oxide.Game.Rust.Cui;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Scroll Bar Test", "herbs.acab", "1.2.0")]
    public class ScrollBarTest : RustPlugin
    {
        private const string PanelName = "ScrollPanel";
        private const string ScrollViewName = "ScrollView";
        private const string ScrollBarThumbName = "ScrollBarThumb";

        private void ShowUI(BasePlayer player)
        {
            var elements = new CuiElementContainer();

            // Define the base panel
            var mainPanel = elements.Add(new CuiPanel
            {
                Image = { Color = "0.1 0.1 0.1 0.8" },
                RectTransform = { AnchorMin = "0.3 0.3", AnchorMax = "0.7 0.7" },
                CursorEnabled = true
            }, "Overlay", PanelName);

            // Define the scroll view
            elements.Add(new CuiElement
            {
                Parent = PanelName,
                Name = ScrollViewName,
                Components =
                {
                    new CuiImageComponent { Color = "0.2 0.2 0.2 1" },
                    new CuiRectTransformComponent { AnchorMin = "0.0 0.0", AnchorMax = "0.95 1.0" },
                    new CuiScrollViewComponent
                    {
                        ScrollDirection = "Vertical",
                        ScrollSpeed = 0.05f,
                        Fade = 0.3f
                    }
                }
            });

            // Add content to the scroll view
            for (int i = 0; i < 20; i++)
            {
                elements.Add(new CuiLabel
                {
                    Text = { Text = $"Item {i + 1}", FontSize = 18, Align = TextAnchor.MiddleCenter },
                    RectTransform = { AnchorMin = $"0 1-{(i + 1) * 0.05f}", AnchorMax = $"1 1-{i * 0.05f}" }
                }, ScrollViewName);
            }

            // Define the scroll bar background
            elements.Add(new CuiElement
            {
                Parent = PanelName,
                Name = CuiHelper.GetGuid(),
                Components =
                {
                    new CuiImageComponent { Color = "0.1 0.1 0.1 0.8" },
                    new CuiRectTransformComponent { AnchorMin = "0.95 0", AnchorMax = "1 1" }
                }
            });

            // Define the scroll bar thumb
            elements.Add(new CuiElement
            {
                Parent = PanelName,
                Name = ScrollBarThumbName,
                Components =
                {
                    new CuiImageComponent { Color = "0.8 0.8 0.8 0.8" },
                    new CuiRectTransformComponent { AnchorMin = "0.95 0.9", AnchorMax = "1 1" }
                }
            });

            CuiHelper.AddUi(player, elements);

            // Start monitoring the scroll position
            timer.Repeat(0.1f, 0, () => UpdateScrollBarThumb(player));
        }

        private void UpdateScrollBarThumb(BasePlayer player)
        {
            float scrollPos = GetScrollPosition(player);
            float thumbHeight = 0.1f; // Adjust the height of the thumb as needed

            float thumbMin = 1.0f - thumbHeight - scrollPos * (1.0f - thumbHeight);
            float thumbMax = 1.0f - scrollPos * (1.0f - thumbHeight);

            var elements = new CuiElementContainer();

            // Update the scroll bar thumb position
            elements.Add(new CuiElement
            {
                Parent = PanelName,
                Name = ScrollBarThumbName,
                Components =
                {
                    new CuiImageComponent { Color = "0.8 0.8 0.8 0.8" },
                    new CuiRectTransformComponent { AnchorMin = $"0.95 {thumbMin}", AnchorMax = $"1 {thumbMax}" }
                }
            });

            CuiHelper.DestroyUi(player, ScrollBarThumbName);
            CuiHelper.AddUi(player, elements);
        }

        private float GetScrollPosition(BasePlayer player)
        {
            // This function should return the current scroll position as a value between 0 and 1
            // In this example, we simulate it with a random value for demonstration purposes
            // In a real scenario, you would need to calculate it based on the scroll view's state
            return Random.Range(0.0f, 1.0f);
        }

        [ChatCommand("showui")]
        private void ShowUICommand(BasePlayer player, string command, string[] args)
        {
            ShowUI(player);
        }

        [ChatCommand("hideui")]
        private void HideUICommand(BasePlayer player, string command, string[] args)
        {
            CuiHelper.DestroyUi(player, PanelName);
        }
    }
}
