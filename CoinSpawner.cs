using Unity.VisualScripting;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject CoinObject;
    public int CheckCoin;

    [Header("SpawnLocationBorder")]
    [SerializeField]
    private float MinX = -26f;
    [SerializeField]
    private float MaxX = 20f;
    [SerializeField]
    private float MaxZ = 27f;
    [SerializeField]
    private float MinZ = -7f;


    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckCoin-=1;
        }
    }

    private void Spawn()
    {
        if (CheckCoin <= 12)
        {
            Vector3 SpawnPos = new Vector3(Random.Range(MinX,MaxX),0.5f,Random.Range(MinZ,MaxZ));
            Instantiate(CoinObject,SpawnPos,Quaternion.identity);
            CheckCoin+=1;
        }
    }
    void Update()
    {
        Spawn();
    }
}
