using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum EnemyUnitStatus : uint
{
    NONE = 0x00,
    UNKNOWN = 0x01,

    //�Ʒ��� Enum �� �ۼ�
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
        if (ControlPoints.Count == 0) return false; // ��ο� ���� ������ �Լ� ����

        // ���� ��ġ���� ���� ��Ʈ�� ����Ʈ������ �Ÿ� ���
        float   step = moveSpeed * moveSpeedMultiplier * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, ControlPoints[currentPointIndex], step);

        // ȸ�� ó�� - ������ �������� ������Ʈ ȸ��
        Vector3 targetDirection = ControlPoints[currentPointIndex] - transform.position;
        if (targetDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            // '�Ӹ�'�� �⺻������ 12�� ������ ����Ű���� ����, �׷��� ���⼭�� ������ ��ȭ�� ���� �ʽ��ϴ�.
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // -90�� ���� ��������Ʈ�� �̵� ������ ����Ű���� ����
        }


        // ���� ���� �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, ControlPoints[currentPointIndex]) < 0.1f)
        {
            currentPointIndex++; // ���� ������ �ε��� �̵�
            if (currentPointIndex >= ControlPoints.Count)
            {
                currentPointIndex = 0; // ����Ʈ ���� �����ϸ� ó������ ���ư�
                return false;
            }
        }
        return true;
    }

    IEnumerator ResetRotationCoroutine()
    {
        Quaternion startRotation = transform.rotation; // ���� ȸ�� ���¸� ����
        Quaternion endRotation = Quaternion.Euler(0, 0, 0); // ��ǥ ȸ�� ���� (0, 0, 0)
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, time);
            time += Time.deltaTime * rotationResetSpeed; // �� ���� �����Ͽ� ȸ�� �ӵ��� ����
            yield return null; // ���� �����ӱ��� ��ٸ�
        }

        transform.rotation = endRotation; // ��Ȯ�� ��ġ�� Ȯ���� ����
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
        if (ControlPoints.Count > 1) // �� �� �̻��� ����Ʈ�� �ʿ�
        {
            Gizmos.color = Color.red; // Gizmo ���� ����
            for (int i = 0; i < ControlPoints.Count - 1; i++)
            {
                // ���� ������ ���� ������ ������ �׸��ϴ�.
                Gizmos.DrawLine(ControlPoints[i], ControlPoints[i + 1]);
                Gizmos.DrawCube(ControlPoints[i], size);
            }
        }
    }

}
