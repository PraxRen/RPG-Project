using RPG.Core;
using RPG.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private TimeOfDay _timeOfDay;
    [SerializeField] private WindowLight[] _windowLights;

    private Coroutine _slowSwitchWindowsLight;

    private void OnEnable()
    {
        _timeOfDay.OnMorning += OnWindowsLight;
        _timeOfDay.OnDay += OffWindowsLight;
        _timeOfDay.OnEvening += OnWindowsLight;
        _timeOfDay.OnNight += OnWindowsLight;
    }

    private void OnDisable()
    {
        _timeOfDay.OnMorning -= OnWindowsLight;
        _timeOfDay.OnDay -= OffWindowsLight;
        _timeOfDay.OnEvening -= OnWindowsLight;
        _timeOfDay.OnNight -= OnWindowsLight;
    }

    private void OnWindowsLight()
    {
        SwitchWindowsLight(true);
    }

    private void OffWindowsLight()
    {
        SwitchWindowsLight(false);
    }

    private void SwitchWindowsLight(bool onLight)
    {
        if (_slowSwitchWindowsLight != null)
            StopCoroutine(_slowSwitchWindowsLight);

        float waitTimeSecondBetweenWindows = _timeOfDay.SecondsInFullDay / _timeOfDay.CountSkybox / _windowLights.Length / 2;

        _slowSwitchWindowsLight = StartCoroutine(SlowSwitchWindowsLight(onLight, waitTimeSecondBetweenWindows));
    }

    private IEnumerator SlowSwitchWindowsLight(bool onLight, float waitTimeSecondBetweenWindows)
    {
        for (int i = 0; i < _windowLights.Length; i++)
        {
            _windowLights[i].gameObject.SetActive(onLight);
            yield return new WaitForSeconds(waitTimeSecondBetweenWindows);
        }

        yield return null;
    }
}
