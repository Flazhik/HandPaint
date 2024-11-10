using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

namespace HandPaint.Components
{
    [ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
    public class SkittlesPox: MonoSingleton<SkittlesPox>
    {
        private static readonly List<string> Tantrums = new List<string>
        {
            $"<color=red>YOU SHOULDN'T HAVE TASTED THE RAINBOW FROM AN UNRELIABLE DEALER</color>\nYou are now infected with {Skittles} POX",
            $"<color=red>HA-HA, YOU WISH!</color>\nYOU CAN ONLY CURE {Skittles} POX BY RESTARTING!",
            $"Maybe I didn't make myself clear.\nYou can only cure {Skittles} pox by restarting the scene.",
            "Hello? Nothing happens until you restart the scene!",
            "You know what? You're right, I shouldn't have used such a demanding tone with you. It's just...",
            "...it would be really cool if you just restarted the scene instead. How does that sound?",
            "...okay, let's take another approach.\n¡Solo puedes curar la viruela de Skittles reiniciando la escena!",
            "¿No hablas español? All right, let's see...",
            "Вылечить скитлстрянку можно только перезагрузив сцену!",
            "Skittles poxu se můžeš zbavit pouze restartováním scény!",
            "Okay, you're either really dumb or really curious",
            "And I'm, like, 95% positive you're just dumb",
            "Maybe 85%, but the point stands",
            "All I know you're 100% getting on my nerves!",
            "Look, I'm not even telling you to accept the consequences of your actions. I've literally just told you how to fix this! RESTART THE SCENE!",
            "<color=red>TYPE IT IN ONE MORE TIME, I DARE YOU! THEN WE WILL SEE HOW MUCH BLOOD THIS LITTLE BLUE BODY OF YOURS CAN FIT!</color>",
            "..."
        };
        private const string Skittles = "<color=red>S</color><color=orange>K</color><color=yellow>I</color><color=green>T</color><color=#00ffff>T</color><color=blue>L</color><color=purple>E</color><color=red>S</color>";
        
        private List<KeyControl> _easterSequence;
        
        public bool activated;
        public float hue;
        private int _sequenceIndex;
        private int _tantrumIndex;

        protected IEnumerator Start()
        {
            SceneManager.sceneLoaded += (s, m) =>
            {
                activated = false;
                HandPaintConfig.TriggerValueChangeEvent();
            };

            yield return StartCoroutine(WaitForTheKeyboard());
            _easterSequence = new List<KeyControl>
            {
                Keyboard.current.digit0Key,
                Keyboard.current.digit4Key,
                Keyboard.current.digit5Key,
                Keyboard.current.digit1Key,
            };
        }

        private void Update()
        {
            if (Keyboard.current == null || !Keyboard.current.anyKey.wasPressedThisFrame)
                return;

            if (_easterSequence != null && _easterSequence[_sequenceIndex].wasPressedThisFrame)
            {
                _sequenceIndex++;
                if (_sequenceIndex != _easterSequence.Count)
                    return;

                _sequenceIndex = 0;
                
                if (_tantrumIndex < Tantrums.Count) {
                    if (_tantrumIndex == 0)
                    {
                        activated = true;
                        HandPaintConfig.TriggerValueChangeEvent();
                    }
                    DisplayMessage(Tantrums[_tantrumIndex]);
                    _tantrumIndex++;
                }
                else
                {
                    activated = false;
                    HandPaintConfig.TriggerValueChangeEvent();
                    DisplayMessage("Fine, FINE! IT'S OFF! HAPPY NOW?!\nGET LOST!!!");
                    _tantrumIndex = 0;
                }
            }
            else
                _sequenceIndex = 0;
        }

        private void FixedUpdate()
        {
            if (!activated)
                return;
            
            hue = (hue + Time.fixedDeltaTime * HandPaintConfig.EasterEggConfig.Frequency.value / 60f) % 1f;
        }
        
        private static IEnumerator WaitForTheKeyboard()
        {
            yield return new WaitUntil(() => Keyboard.current != null);
        }

        private static void DisplayMessage(string msg) => MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage(msg);
    }
}