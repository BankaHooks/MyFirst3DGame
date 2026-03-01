using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private GameObject CoinObject;
    private float PointCount;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private Rigidbody _rb;
    private float _horizontal;
    private float _vertical;

    [Header("Camera")]
    [SerializeField]
    private Transform cameraTarget;

    private bool OnGround = true;
    [SerializeField]
    private CoinSpawner CoinSPAWN;

    [Header("Ground Check")]
    [SerializeField]
    private float groundCheckDistance = 1.1f;
    [SerializeField]
    private LayerMask groundLayer;

    [Header("Fighting")]  // - Don`t work correctly by the moment
    [SerializeField]
    private int _MaxHealtPointsP = 100;
    private int _CurrentHealthPointsP;
    [SerializeField]
    private float InvisibleTime = 2f;
    private float TimerOfInvisible = 0f;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (cameraTarget == null)
        {
            cameraTarget = Camera.main.transform;
        }
        _CurrentHealthPointsP = _MaxHealtPointsP;
    }

    private void PlayerControl()
    {
        Vector3 cameraForward = cameraTarget.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = cameraTarget.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 move = (cameraForward * _vertical + cameraRight * _horizontal);
        move = Vector3.ClampMagnitude(move,1f);

        Vector3 newpos = _rb.position + move * Time.deltaTime * speed;
        _rb.MovePosition(newpos);

        if(move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.deltaTime * 10f);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(TimerOfInvisible<=0)
            {
                _CurrentHealthPointsP -=25;

                TimerOfInvisible = InvisibleTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            PointCount += 15f;
            Destroy(other.gameObject);
            CoinSPAWN.CheckCoin-=1;

            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + PointCount.ToString();
        }
    }
    private void CheckGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, groundCheckDistance, groundLayer))
        {
            OnGround = true;
        }
        else
        {
            OnGround = false;
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {

        if(TimerOfInvisible>0)
        {
            TimerOfInvisible -= Time.deltaTime;
        }

        if (_CurrentHealthPointsP <= 0)
        {
            ReloadScene();
        }

        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        float CurY = transform.position.y;

        CheckGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && OnGround == true)
        {
            _rb.AddForce(Vector3.up * 10,ForceMode.Impulse);
        }

        if(CurY <= -15f)
        {
            ReloadScene();
        }
    }

    void FixedUpdate()
    {
        PlayerControl();
        Debug.Log(PointCount);
    }
}
