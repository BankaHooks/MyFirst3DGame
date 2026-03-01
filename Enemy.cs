using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private Transform target;
    [SerializeField]
    private float _speed;
    private float PlayerXpos;
    private float PlayerZpos;
    private float _memoryTimer = 0f;
    [SerializeField] private float _memoryDuration = 3f;

    [Header("Search Algo")] 
    [SerializeField]
    private GameObject SearchArea;
    private bool _isPlayerDetected = false;

    [Header("Fighting")]
    [SerializeField]
    private int _HealtPoints = 75;

    private Rigidbody _rb;
    private void EnemySearch()
    {
        if(!_isPlayerDetected && _memoryTimer <=0) return;

        if(_memoryTimer >0)
        {
            _memoryTimer-=Time.deltaTime;
        }

        Vector3 move = new Vector3(PlayerXpos-transform.position.x,0,PlayerZpos-transform.position.z);
        Vector3 direction = move.normalized;

        Vector3 newpos = _rb.position + direction * Time.deltaTime * _speed;
        _rb.MovePosition(newpos);
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerDetected = true;
            Debug.Log("PLAYER DETECTED");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //_isPlayerDetected = false;
            if(_memoryTimer<=0)
                _memoryTimer = _memoryDuration;
            Debug.Log("PLAYER LOST");
        }
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerXpos = target.position.x;
        PlayerZpos = target.position.z;
    }

    void FixedUpdate()
    {
        EnemySearch();
    }
}
