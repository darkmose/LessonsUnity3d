using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boxes : MonoBehaviour
{
    private const float ExplosionUpwardModifier = 10f;
    private const float ExplosionRadius = 50f;
    private const float ExplosionForce = 100f;
    private const int ExplosionDelay = 5;
    [SerializeField] private Transform _target;
    [SerializeField] private TextMeshProUGUI _explosionTimerText;
    [SerializeField] private GameObject _explosionTimerWindow;

    private void Awake()
    {
        EventDelegate.OnPlatformButtonPressEvent += OnPlatformButtonPressedHandler;   
    }

    private void OnPlatformButtonPressedHandler()
    {
        StartCoroutine(DelayExplosion());
    }



    private void ExplodeBoxes() 
    {
        var hits = Physics.SphereCastAll(_target.position, ExplosionRadius, Vector3.up);
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(ExplosionForce, _target.position, ExplosionRadius, ExplosionUpwardModifier, ForceMode.Impulse);
            }
        }
    }


    private IEnumerator DelayExplosion()
    {
        _explosionTimerWindow.SetActive(true);
        for (int i = ExplosionDelay; i > 0; i--)
        {
            var text = $"00:0{i}";
            _explosionTimerText.text = text;
            yield return new WaitForSeconds(1);
        }
        _explosionTimerText.text = "00:00";
        ExplodeBoxes();
    }
}
