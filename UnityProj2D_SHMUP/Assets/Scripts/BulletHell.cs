using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHell : MonoBehaviour
{
    [System.Serializable]
    public struct BulletSet
    {
        [Header("---------[BULLETHELL PARAMS]-------------------------------------")]
        [Space]
        public string projectileTag;
        [Range(0f, 100f)]
        public int replayCount;
        [SerializeField] private bool prewarm;
        [Range(0f, 1000f)]
        public int bulletCount;
        [Range(-360f, 360f)]
        public float startAngle;
        [Range(-360f, 360f)]
        public float endAngle;
        [Range(0f, 50f)]
        public float bulletSpeed;
        [Range(0f, 25f)]
        public float bulletSetDelay;
        [Range(1f, 100f)]
        public float fireRate;
        [Range(1f, 100f)]
        public float radius;
        public GameObject root;
        private Vector2 pointPos;
        private Vector2 moveDirection;
        [SerializeField] private bool isRadian;

        private Vector2[] directions;
        private GameObject[] objects;

        public IEnumerator Fire()
        {
            for (int j = 0; j < replayCount; j++)
            {
                yield return new WaitForSeconds(bulletSetDelay);
                
                directions = new Vector2[bulletCount+1];
                objects = new GameObject[bulletCount+1];             

                float angleStep = (endAngle - startAngle) / bulletCount;
                float angle = startAngle;
                Vector2 rootPos = root.transform.position;
                for (int i = 0; i <= bulletCount; i++)
                {
                    objects[i] = null;
                    pointPos.x = rootPos.x + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
                    pointPos.y = rootPos.y + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
                    moveDirection = (pointPos - rootPos).normalized;
                    objects[i] = ObjectPooler.GetPooledGameObject(projectileTag);
                    objects[i].tag = "EnemyAmmo";
                    var rigidbody = objects[i].GetComponent<Rigidbody2D>();
                    rigidbody.position = pointPos;
                    objects[i].transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
                   
                    if (prewarm)
                    {
                        directions[i] = moveDirection * bulletSpeed;
                    }
                    else
                    {
                        rigidbody.velocity = moveDirection * bulletSpeed;
                    }
                    if (!isRadian)
                    {
                        yield return new WaitForSeconds(1 / fireRate);
                    }
                    angle += angleStep;
                }
                if (prewarm)
                {
                    for (int i = 0; i <= bulletCount; i++)
                    {
                        objects[i].GetComponent<Rigidbody2D>().velocity = directions[i];
                    }
                    
                }
                yield return new WaitForSeconds(1/fireRate);
            }
            yield return null;
        }
    }
    [SerializeField] private bool cyclicSpawn;
    [SerializeField] private List<BulletSet> bulletSets;
    [SerializeField] private bool playOnStart;


    public BulletHell()
    { 
    }

    

    private void Start()
    {
        if (playOnStart)
        {
            StartCoroutine(SpawnBulletHell());
        }
    }


    public IEnumerator SpawnBulletHell() 
    {
        while (true)
        {
            for (int i = 0; i < bulletSets.Count; i++)
            {
               yield return StartCoroutine(bulletSets[i].Fire());
            }
            if (!cyclicSpawn)
            {
                break;
            }
            yield return null;
        }    
    }

    private void OnDestroy()
    {
        StopBulletHell();
    }

    [ContextMenu("Start BulletHell")]
    public void StartBulletHell() 
    {
        StartCoroutine(SpawnBulletHell());
    }

    public void StopBulletHell() 
    {
        cyclicSpawn = false;
        this.StopAllCoroutines();
    }



}
