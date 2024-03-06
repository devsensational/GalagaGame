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
    [SerializeField] private int                Score;
    [SerializeField] private List<TextAsset>    PattenFile;
    [SerializeField] private GameObject         Bullet;
    [SerializeField] private List<Sprite>       SpriteList;
    [SerializeField] private int                SpriteChangeTime;
    [SerializeField] private float              rotationResetSpeed = 1f;

    //public
    public int              UnitIndex        { get; set; }
    public int              UnitSeqeunceIdx  { get; set; }
    public int              EnemyUnitType    { get; set; }
    public Vector3          UnitPosition     { get; set; }
    public List<Vector3>    ControlPoints    { get; set; }

    //private
    GameEnemyUnitController enemyUnitController;
    GameObjectPoolManager   objectPoolManager;
    GameManager             gameManager;    
    GameObject              playerUnit;
    EnemyUnitStatus         enemyUnitStatus;

    WaitForSeconds          unitFirstMoveInterval;

    SpriteRenderer          spriteRenderer;
    WaitForSeconds          spriteChangeWaitForSeconds;

    int                     spriteIdx = 0;
    int                     currentPointIndex = 0;


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
        CheckUnitDead();
    }

    public void StartUnitMove(List<Vector3> ControlPoints, float unitAttackIntervalTime, float unitMoveIntervalTime)
    {
        Debug.Log("(GameEnemyUnit) Move Start: " + gameObject.name);
        this.ControlPoints = ControlPoints;
        //this.ControlPoints.Add(UnitPosition);

        transform.position = ControlPoints[0];
        unitFirstMoveInterval = new WaitForSeconds(unitAttackIntervalTime + unitMoveIntervalTime);
        StartCoroutine(UnitMove(unitFirstMoveInterval));
    }

    private IEnumerator UnitMove(WaitForSeconds intervalTime)
    {
        yield return intervalTime;
        while (FollowPath()) { yield return null; }
        Debug.Log("UnitMoveComplete");
        StartCoroutine(ResetRotationCoroutine());
    }

    private bool FollowPath()
    {
        if (ControlPoints.Count == 0) return false; // 경로에 점이 없으면 함수 종료

        // 현재 위치에서 다음 컨트롤 포인트까지의 거리 계산
        float   step = moveSpeed * moveSpeedMultiplier * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, ControlPoints[currentPointIndex], step);

        // 회전 처리 - 목적지 방향으로 오브젝트 회전
        Vector3 targetDirection = ControlPoints[currentPointIndex] - transform.position;
        if (targetDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            // '머리'가 기본적으로 12시 방향을 가리키도록 설정, 그래서 여기서는 각도에 변화를 주지 않습니다.
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // -90을 더해 스프라이트가 이동 방향을 가리키도록 조정
        }


        // 현재 점에 도착했는지 확인
        if (Vector3.Distance(transform.position, ControlPoints[currentPointIndex]) < 0.1f)
        {
            currentPointIndex++; // 다음 점으로 인덱스 이동
            if (currentPointIndex >= ControlPoints.Count)
            {
                currentPointIndex = 0; // 리스트 끝에 도달하면 처음으로 돌아감
                return false;
            }
        }
        return true;
    }

    IEnumerator ResetRotationCoroutine()
    {
        Quaternion startRotation = transform.rotation; // 현재 회전 상태를 저장
        Quaternion endRotation = Quaternion.Euler(0, 0, 0); // 목표 회전 상태 (0, 0, 0)
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, time);
            time += Time.deltaTime * rotationResetSpeed; // 이 값을 조정하여 회전 속도를 변경
            yield return null; // 다음 프레임까지 기다림
        }

        transform.rotation = endRotation; // 정확한 위치에 확실히 고정
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
    }

    void Start()
    {

    }

    void Update() 
    {

    }

    private void OnEnable()
    {
        StartCoroutine(OnAnimationSwitch());
    }

    void OnDrawGizmosSelected()
    {
        Vector3 size = new Vector3(0.1f, 0.1f, 0.1f);
        if (ControlPoints.Count > 1) // 두 개 이상의 포인트가 필요
        {
            Gizmos.color = Color.red; // Gizmo 색상 설정
            for (int i = 0; i < ControlPoints.Count - 1; i++)
            {
                // 현재 점에서 다음 점까지 라인을 그립니다.
                Gizmos.DrawLine(ControlPoints[i], ControlPoints[i + 1]);
                Gizmos.DrawCube(ControlPoints[i], size);
            }
        }
    }

}
