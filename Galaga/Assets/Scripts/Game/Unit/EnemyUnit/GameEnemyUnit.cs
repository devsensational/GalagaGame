using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum EnemyUnitStatus : uint
{
    NONE = 0x00,
    UNKNOWN = 0x01,

    //아래에 Enum 값 작성
    DEFAULT = 0x02,
    HIT = 0x04,
    CAPUTRED = 0x08,
    DAMAGED = 0x10,
    MOVE = 0x20,
    DEAD = 0x30,
    //Enum End

    End = 0xFF
}

public class GameEnemyUnit : GameUnit, IGameUnitAttack, IGameUnitHit
{
    //Inspector Field
    [Header("Enemy Unit Inspector")]
    [SerializeField] public  List<TextAsset>     PattenFile;
    [SerializeField] private GameObject         Bullet;
    [SerializeField] private int                Score;

    [Header("Sprite")]
    [SerializeField] private List<Sprite>       SpriteList;
    [SerializeField] private int                SpriteChangeTime;
    [SerializeField] private float              RotationResetValue;
    [SerializeField] private float              RotationResetSpeed;

    //public
    public int              UnitIndex        { get; set; }
    public int              UnitSeqeunceIdx  { get; set; }
    public int              EnemyUnitType    { get; set; }
    public Vector3          UnitPosition     { get; set; }
    public List<Vector3>    ControlPoints    { get; set; }

    //private
    private GameEnemyUnitController enemyUnitController;
    private GameObjectPoolManager   objectPoolManager;
    private GameManager             gameManager;    
    private GameObject              playerUnit;
    private GameObject              resetPosition;
    private EnemyUnitStatus         enemyUnitStatus;

    private WaitForSeconds          unitFirstMoveInterval;
    private WaitForSeconds          rotationResetSpeedInterval;

    private SpriteRenderer          spriteRenderer;
    private WaitForSeconds          spriteChangeWaitForSeconds;

    private Coroutine               moveCoroutine;

    private int                     spriteIdx = 0;
    private int                     currentPointIndex = 0;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            if(!collision.gameObject.GetComponent<GameBullet>().IsHit)
            {
                collision.gameObject.GetComponent<GameBullet>().IsHit = true;
                StopAllCoroutines();
                UnitHit();
            }
        }
        if (collision.gameObject.CompareTag("PlayerUnit"))
        {
            UnitHit();
        }
        if (collision.gameObject.CompareTag("Outside"))
        {
            StopCoroutine(moveCoroutine);
            UnitResetPosition();
        }

    }


    public void UnitAttack()
    {
        if(enemyUnitStatus == EnemyUnitStatus.MOVE)
        {

        }
    }

    public void UnitHit()
    {
        Hp -= 1;
        if(enemyUnitStatus == EnemyUnitStatus.MOVE) enemyUnitController.EnemyUnitArrived();
        CheckUnitDead();
    }

    public void StartUnitMove(List<Vector3> ControlPoints, float unitMoveIntervalTime)
    {
        Debug.Log("(GameEnemyUnit) Move Start: " + gameObject.name);
        this.ControlPoints = ControlPoints;
        enemyUnitStatus = EnemyUnitStatus.MOVE;
        //this.ControlPoints.Add(UnitPosition);

        transform.position = ControlPoints[0];
        unitFirstMoveInterval = new WaitForSeconds(unitMoveIntervalTime);
        moveCoroutine = StartCoroutine(UnitMove(unitFirstMoveInterval));
    }

    public void StartUnitAttack()
    {

    }

    private IEnumerator UnitMove(WaitForSeconds intervalTime)
    {
        yield return intervalTime;
        while (FollowPath()) { yield return null; }
        Debug.Log("UnitMoveComplete");
        enemyUnitStatus = EnemyUnitStatus.DEFAULT;
        enemyUnitController.EnemyUnitArrived();
        StartCoroutine(ResetRotationCoroutine());
    }

    private bool FollowPath()
    {
        if (ControlPoints.Count == 0) return false;

        float step = moveSpeed * moveSpeedMultiplier * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, ControlPoints[currentPointIndex], step);

        Vector3 targetDirection = ControlPoints[currentPointIndex] - transform.position;
        if (targetDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }

        if (Vector3.Distance(transform.position, ControlPoints[currentPointIndex]) < 0.001f)
        {
            currentPointIndex++;
            if (currentPointIndex >= ControlPoints.Count)
            {
                currentPointIndex = 0;
                return false;
            }
        }
        return true;
    }

    private void UnitResetPosition()
    {
        transform.position = resetPosition.transform.position;
        List<Vector3> points = new List<Vector3>();
        points.Add(transform.position);
        points.Add(UnitPosition);
        currentPointIndex = 0;
        StartUnitMove(points, 0f);
        enemyUnitController.AddEnemyUnitList(gameObject);
    }

    IEnumerator ResetRotationCoroutine()
    {
        while (true)
        {
            float currentRotation = transform.eulerAngles.z;

            if (currentRotation > 180)
            {
                currentRotation -= 360;
            }

            if (Mathf.Abs(currentRotation) <= RotationResetValue)
            {
                transform.eulerAngles = Vector3.zero;
                yield break; 
            }

            float rotationStep = currentRotation > 0 ? -RotationResetValue : RotationResetValue;
            transform.Rotate(0, 0, rotationStep);

            yield return rotationResetSpeedInterval;
        }
    }

    private Vector3 AimPlayerUnit()
    {
        if (playerUnit != null) { return Vector3.zero; }
        Vector3 direction = gameObject.transform.position - playerUnit.transform.position;
        return direction.normalized;
    }

    private void CheckUnitDead()
    {
        if (Hp <= 0)
        {
            gameManager.OnAddScore(Score);
            enemyUnitController.RemoveUnit(UnitIndex, gameObject);
        }
    }

    private void CheckUnitArrive()
    {
        
    }

    private void OnGameInProgress()
    {
        resetPosition = GameObject.Find("EnemyTeleportLocation");
    }

    virtual protected IEnumerator OnAnimationSwitch()
    {
        if(SpriteList == null)
        {
            yield break;
        }
        spriteRenderer.sprite = SpriteList[spriteIdx++ % SpriteList.Count];
        yield return spriteChangeWaitForSeconds;
        StartCoroutine(OnAnimationSwitch());
    }


    void Awake() 
    {
        playerUnit                  = GameObject.Find("SpaceShip");
        enemyUnitController         = GameObject.Find("GameEnemyController").GetComponent<GameEnemyUnitController>();
        objectPoolManager           = GameObjectPoolManager.Instance;
        gameManager                 = GameManager.Instance;
        spriteRenderer              = gameObject.GetComponent<SpriteRenderer>();
        spriteChangeWaitForSeconds  = new WaitForSeconds(SpriteChangeTime);
        rotationResetSpeedInterval  = new WaitForSeconds(RotationResetSpeed);

        GameEventManager.Instance.AddEvent(GameStatus.GAMEINPROGRESS, OnGameInProgress);
    }

    void Start()
    {

    }

    void Update() 
    {

    }

    virtual protected void ChildAwake() { }

    private void OnEnable()
    {
        StartCoroutine(OnAnimationSwitch());
    }

    void OnDrawGizmosSelected()
    {
        Vector3 size = new Vector3(0.1f, 0.1f, 0.1f);
        if (ControlPoints.Count > 1) 
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < ControlPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(ControlPoints[i], ControlPoints[i + 1]);
                Gizmos.DrawCube(ControlPoints[i], size);
            }
        }
    }
}
