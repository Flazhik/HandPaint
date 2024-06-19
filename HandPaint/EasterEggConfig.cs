using System;
using PluginConfig.API;
using PluginConfig.API.Fields;

namespace HandPaint
{
    public class EasterEggConfig
    {
        public readonly FloatSliderField Frequency;
        public readonly FloatSliderField[] Phases = new FloatSliderField[3];
        public readonly ConfigPanel Panel;

        private static readonly float[] PhasesDefaultValues = { 0.41f, 0.6f, 0.79f };
        
        public EasterEggConfig(PluginConfigurator config)
        {
            Panel = new ConfigPanel(config.rootPanel, "SKITTLES POX", "handpaint.easter");
            Frequency = new FloatSliderField(Panel, "Hue shift frequency (cycles per minute)", "handpaint.easter.frequency",
                new Tuple<float, float>(0, 120), 30, 0);
            for (var i = 0; i < Phases.Length; i++) {
                Phases[i] = new FloatSliderField(Panel, $"Phase of color {i + 1}", $"handpaint.easter.frequency.color-{i + 1}-phase",
                    new Tuple<float, float>(0, 1), PhasesDefaultValues[i], 2);
            }

            Panel.hidden = true;
        }
    }
}