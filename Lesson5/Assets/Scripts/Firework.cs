using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        StartCoroutine(Delay_Fire());
    }

    private IEnumerator Delay_Fire()
    {
        var time = 0f;
        int count;
        while (true)
        {
            time = Random.Range(2, 5);
            yield return new WaitForSeconds(time);

            count = Random.Range(3, 7);
            for (int i = 0; i < count; i++)
            {
                particleSystem.Emit(1);
                AudioManager.PlaySFX(AudioManager.AudioClips.FireworkBoom, 2f);
            }
        }
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }
}
