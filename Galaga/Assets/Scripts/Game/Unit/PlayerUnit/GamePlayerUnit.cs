using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum PlayerUnitStatus : uint
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

public class GamePlayerUnit : GameUnit, IGameUnitAttack, IGameUnitHit
{
    //Inspector
    [Header("Player Unit Inspector")]
    [SerializeField] private GameObject Bullet;

    //private
    PlayerUnitStatus        status;
    GameObjectPoolManager   poolManager;
    GameEventManager        gameEventManager;
    GameObject              bulletPtr;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.ToString());
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            UnitHit();
        }
    }

    public void UnitAttack()
    {
        bulletPtr = poolManager.OnGetGameObject(GameUnitObjectType.PLAYERBULLET);
        bulletPtr.transform.position = gameObject.transform.position;
        bulletPtr.SetActive(true);
        bulletPtr.GetComponent<GameBullet>().ShootBullet(Vector3.up, GameUnitObjectType.PLAYERBULLET, "PlayerBullet");
    }

    public void UnitHit()
    {
        Hp -= 1;
        if(Hp <= 0)
        {
            gameEventManager.OnTriggerGameEvent(GameStatus.PLAYERDEAD);
        }
    }

    public void UnitMoveControl(Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * moveSpeedMultiplier * Time.deltaTime);
    }

    private void OnUnitDead()
    {
        Debug.Log("Player unit dead");
        Destroy(gameObject);
    }

    void Awake()
    {
        status      = PlayerUnitStatus.DEFAULT;

    }

    void Start()
    {
        poolManager         = GameObjectPoolManager.Instance;
        gameEventManager    = GameEventManager.Instance;

        poolManager.CreateGameObjectPool(GameUnitObjectType.PLAYERBULLET, Bullet, 3);

        gameEventManager.AddEvent(GameStatus.PLAYERDEAD, OnUnitDead);
        Debug.Log("Unit init complete");
    }

    void Update()
    {

    }
}
