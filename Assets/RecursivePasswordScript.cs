using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Rnd = UnityEngine.Random;
using KModkit;

public class RecursivePasswordScript : MonoBehaviour
{
    public KMBombModule Module;
    public KMBombInfo BombInfo;
    public KMAudio Audio;

    public KMSelectable ToggleSel;
    public KMSelectable[] ArrowSels;
    public TextMesh[] ScreenTexts;

    private int _moduleId;
    private static int _moduleIdCounter = 1;
    private bool _moduleSolved;

    private int[] _currentLetters = new int[5];

    private void Start()
    {
        _moduleId = _moduleIdCounter++;
        ToggleSel.OnInteract += TogglePress;
        for (int i = 0; i < ArrowSels.Length; i++)
            ArrowSels[i].OnInteract += ArrowPress(i);

        _currentLetters = Enumerable.Range(0, 26).ToArray().Shuffle().Take(5).ToArray();
        UpdateScreens();
    }

    private KMSelectable.OnInteractHandler ArrowPress(int i)
    {
        return delegate ()
        {
            ArrowSels[i].AddInteractionPunch(0.25f);
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, ArrowSels[i].transform);
            if (_moduleSolved)
                return false;

            if (i < 5)
                _currentLetters[i % 5] = (_currentLetters[i % 5] + 1) % 26;
            else
                _currentLetters[i % 5] = (_currentLetters[i % 5] + 25) % 26;
            UpdateScreens();
            
            // Do things

            return false;
        };
    }

    private void UpdateScreens()
    {
        for (int i = 0; i < _currentLetters.Length; i++)
            ScreenTexts[i].text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[_currentLetters[i]].ToString();
    }

    private bool TogglePress()
    {
        ToggleSel.AddInteractionPunch(0.5f);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, ToggleSel.transform);
        if (_moduleSolved)
            return false;

        // Do things

        return false;
    }
}
