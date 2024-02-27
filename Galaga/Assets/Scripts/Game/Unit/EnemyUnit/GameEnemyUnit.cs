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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            UnitHit();
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

    public void StartUnitMove(List<Vector3> ControlPoints)
    {
        Debug.Log("(GameEnemyUnit) Move Start: " + gameObject.name);
        this.ControlPoints = ControlPoints;
        this.ControlPoints.Add(UnitPosition);

        transform.position = ControlPoints[0];
        StartCoroutine(CalulatePath());
    }

    private IEnumerator CalulatePath()
    {
        float t = 0; // ������ ��� ���� �̵��ϴ� ���� ��ġ�� �Ű�����

        while (t < 1.0f)
        {
            t += 0.001f * moveSpeed * moveSpeedMultiplier * Time.deltaTime; // �ð��� ���� t�� �������� ������ ��� ���� �̵�

            Vector3 currentPosition = CalculateBezierPoint(t, ControlPoints);
            transform.position = currentPosition; // ������Ʈ�� ��ġ ������Ʈ

            if (t < 1.0f)
            {
                Vector3 nextPosition = CalculateBezierPoint(Mathf.Min(t + 0.01f, 1.0f), ControlPoints);
                Vector3 direction = (nextPosition - currentPosition).normalized; // �̵� ���� ���

                /*if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5); // �ε巴�� ȸ��
                }*/
            }

            yield return null; // ���� �����ӱ��� ���
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

    public Vector3 CalculateBezierPoint(float t, List<Vector3> controlPoints)
    {
        int n = controlPoints.Count - 1; // �������� ������ ���� ���� n
        Vector3 point = Vector3.zero; // �ʱ�ȭ
        for (int i = 0; i <= n; i++)
        {
            // ���װ�� * (1-t)^(n-i) * t^i
            float binomialCoefficient = BinomialCoefficient(n, i);
            float term = binomialCoefficient * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i);
            point += term * controlPoints[i];
        }
        return point;
    }

    private float BinomialCoefficient(int n, int k)
    {
        return Factorial(n) / (Factorial(k) * Factorial(n - k));
    }

    private float Factorial(int n)
    {
        float result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

    void Awake() 
    {
        playerUnit          = GameObject.Find("SpaceShip");
        enemyUnitController = GameObject.Find("GameEnemyController").GetComponent<GameEnemyUnitController>();
        objectPoolManager   = GameObjectPoolManager.Instance;
        gameManager         = GameManager.Instance;
    }

    void Update() 
    {

    }

}
