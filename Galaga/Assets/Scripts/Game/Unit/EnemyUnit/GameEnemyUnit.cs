using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //public
    public int UnitIndex        { get; set; }
    public int EnemyUnitType    { get; set; }

    //private
    GameEnemyUnitController enemyUnitController;
    GameObjectPoolManager   objectPoolManager;
    GameManager             gameManager;    
    GameObject              playerUnit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy Unit hit from " + collision.gameObject);
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
        
    }

    public void UnitHit()
    {
        Hp -= 1;
        CheckUnitDead();
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
        int n = controlPoints.Count - 1; // 제어점의 개수에 따른 차수 n
        Vector3 point = Vector3.zero; // 초기화
        for (int i = 0; i <= n; i++)
        {
            // 이항계수 * (1-t)^(n-i) * t^i
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
    }

    void Update() 
    {
        //AimPlayerUnit();
    }

}
