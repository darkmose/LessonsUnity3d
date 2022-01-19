using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private const int ExplosionForce = 30;
    private const int ExplosionRadius = 10;
    private const int UpwardsModifier = 3;
    private const int SecondsToBlowUp = 3;
    private const string EnemyTagName = "Enemy";
    private const int SecondsToDestroy = 1;
    [SerializeField] private ParticleSystem _explosionParticles;
    [SerializeField] private AudioSource _explosionAudio;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private AudioClip _explClip;
    [SerializeField] private SphereCollider _sphereCollider;
    private int _damage;

    public void Throw(Vector3 direction, int damage) 
    {
        this._damage = damage;
        _rigidbody.AddForce(direction, ForceMode.Impulse);
        StartCoroutine(DelayBlowUp());
    }

    private void BlowUp() 
    {
        _sphereCollider.enabled = true;
        if (_explClip !=null)
        {
            _explosionAudio.PlayOneShot(_explClip);
        }
        _explosionParticles.Play();
        _rigidbody.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, UpwardsModifier, ForceMode.Impulse);
    }

    private IEnumerator DelayBlowUp() 
    {
        yield return new WaitForSeconds(SecondsToBlowUp);
        BlowUp();
        yield return new WaitForSeconds(SecondsToDestroy);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(EnemyTagName))
        {
            if (other.TryGetComponent(out NPCController nPCController))
            {
                nPCController.TakeDamage(_damage, "Killer", Weapon.WeaponType.Grenade);
            }
        }
    }
}
