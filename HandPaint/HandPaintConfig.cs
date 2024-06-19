using System.IO;
using System.Reflection;
using HandPaint.Components;
using HandPaint.Scripts;
using PluginConfig.API;
using PluginConfig.API.Fields;
using UnityEngine;

namespace HandPaint
{
    public static class HandPaintConfig
    {
        public static readonly BoolField EnableRepaint;
        public static readonly BoolField PaintFeedbackerSeparately;
        
        public static readonly ColorAlphaField[] WhiplashColors = new ColorAlphaField[3];
        public static ColorAlphaField WhiplashRopeColorField;
        
        public static readonly ColorAlphaField[] KnuckleblasterColors = new ColorAlphaField[3];
        public static ColorAlphaField KnuckleblasterShellColorField;
        
        public static readonly ColorAlphaField[] FeedbackerColors = new ColorAlphaField[3];
        public static readonly ColorAlphaField[] RightFeedbackerColors = new ColorAlphaField[3];
        
        public static readonly EasterEggConfig EasterEggConfig;

        private static readonly ConfigPanel FeedbackerPanel;
        private static readonly ConfigPanel RightFeedbackerPanel;
        private static readonly ConfigPanel KnuckleblasterPanel;
        private static readonly ConfigPanel WhiplashPanel;

        private static readonly Color[] BaseFeedbackerColors =
        {
            new Color(0.251f, 0.439f, 0.933f, 1f),
            new Color(0.392f, 0.392f, 0.392f, 1f),
            new Color(0.251f, 0.439f, 0.933f, 1f)
        };        
        private static readonly Color[] BaseKnuckleblasterColors =
        {
            new Color(0.859f, 0.090f, 0.090f, 1f),
            new Color(0.498f, 0.314f, 0.251f, 1f),
            new Color(0.878f, 0.686f, 0.514f, 1f)
        };
        private static readonly Color[] BaseWhiplashColors =
        {
            new Color(0.486f, 0.612f, 0.153f, 1f),
            new Color(0.843f, 0.686f, 0.498f, 1f),
            new Color(0.409f, 0.345f, 0.282f, 1f)
        };
        private static readonly Color BaseWhiplashRopeColor = new Color(0.25f, 0.25f, 0.25f);
        private static readonly Color BaseKnuckleblasterShellColor = new Color(1, 0, 0, 1);

        static HandPaintConfig()
        {
            var config = PluginConfigurator.Create(PluginInfo.NAME, PluginInfo.GUID);
            EnableRepaint = new BoolField(config.rootPanel, "Repaint arms", "handpaint.enabled", true);
            PaintFeedbackerSeparately = new BoolField(config.rootPanel, "Paint feedbacker arms separately", "handpaint.paint-feedbacker-separately", false);
            FeedbackerPanel = new ConfigPanel(config.rootPanel, "Feedbacker colors", "handpaint.feedbacker-colors");
            RightFeedbackerPanel = new ConfigPanel(config.rootPanel, "Feedbacker right arm colors", "handpaint.feedbacker-r-colors");
            KnuckleblasterPanel = new ConfigPanel(config.rootPanel, "Knuckleblaster colors", "handpaint.knuckleblaster-colors");
            WhiplashPanel = new ConfigPanel(config.rootPanel, "Whiplash colors", "handpaint.whiplash-colors");
            EasterEggConfig = new EasterEggConfig(config);
            
            config.SetIconWithURL($"file://{Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "icon.png")}");
        }

        public static void Init()
        {
            for (var i = 0; i < 3; i++)
            {
                WhiplashColors[i] = new ColorAlphaField(WhiplashPanel, $"Whiplash color {i + 1}", $"handpaint.whiplash-color-{i + 1}", BaseWhiplashColors[i]);
                KnuckleblasterColors[i] = new ColorAlphaField(KnuckleblasterPanel, $"Knuckleblaster color {i + 1}", $"handpaint.knuckleblaster-color-{i + 1}", BaseKnuckleblasterColors[i]);
                FeedbackerColors[i] = new ColorAlphaField(FeedbackerPanel, $"Feedbacker color {i + 1}", $"handpaint.feedbacker-l-color-{i + 1}", BaseFeedbackerColors[i]);
                RightFeedbackerColors[i] = new ColorAlphaField(RightFeedbackerPanel, $"Feedbacker right arm color {i + 1}", $"handpaint.feedbacker-r-color-{i + 1}", BaseFeedbackerColors[i]);
            }
            WhiplashRopeColorField = new ColorAlphaField(WhiplashPanel, "Whiplash rope color", "handpaint.whiplash-rope-color", BaseWhiplashRopeColor);
            KnuckleblasterShellColorField = new ColorAlphaField(KnuckleblasterPanel, "Knuckleblaster shell color", "handpaint.knuckleblaster-shell-color", BaseKnuckleblasterShellColor);
            EnableRepaint.onValueChange += v =>
            {
                var hidden = !v.value || SkittlePoxIsActive();
                EnableRepaint.hidden = SkittlePoxIsActive();
                EasterEggConfig.Panel.hidden = !SkittlePoxIsActive();
                PaintFeedbackerSeparately.hidden = hidden;
                FeedbackerPanel.hidden = hidden;
                RightFeedbackerPanel.hidden = hidden || !PaintFeedbackerSeparately.value;
                KnuckleblasterPanel.hidden = hidden;
                WhiplashPanel.hidden = hidden;
            };
            PaintFeedbackerSeparately.onValueChange +=
                v => RightFeedbackerPanel.hidden = !(v.value && EnableRepaint.value);
            EnableRepaint.TriggerValueChangeEvent();
        }

        private static bool SkittlePoxIsActive() => SkittlesPox.Instance.activated;
    }
}