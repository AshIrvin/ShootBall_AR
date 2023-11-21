using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileManager : NetworkBehaviour
{
    public enum ProjectileType
    {
        Bullet,
        Rocket,
        Grenade
    }

    [SerializeField] private ProjectileType projectileType;
    [SerializeField] private GameObject launchTube;
    [SerializeField] private Projectile bulletObject;
    [SerializeField] private Projectile rocketObject;
    [SerializeField] private Projectile grenadeObject;

    private ObjectPool<Projectile> pool;

    private void Awake()
    {
        UiManager.OnFireButtonPressed += SetupProjectileFire;
    }

    private void Start()
    {
        pool = new ObjectPool<Projectile>(() =>
        {
            return Instantiate(GetProjectileType());
        }, go =>
        {
            go.gameObject.SetActive(true);
        }, go =>
        {
            go.gameObject.SetActive(false);
        }, go =>
        {
            Destroy(go.gameObject);
            print("Destroyed projectile!?");
        }, false, 10, 20);
    }

    private void SetupProjectileFire()
    {
        if (!IsOwner) return;

        SendServerRpc();
        GetProjectileFromPool();
    }

    [ServerRpc]
    private void SendServerRpc()
    {
        SendClientRpc();
    }

    [ClientRpc]
    private void SendClientRpc()
    {
        if (IsOwner) return;

        print("Fire projectile from server");
        GetProjectileFromPool();
    }

    private void GetProjectileFromPool()
    {
        print("Fire projectile locally");
        var projectile = pool.Get();
        projectile.Init(KillObject);
        SetStartPosition(projectile.gameObject);
        projectile.SetForce(launchTube.transform.parent);
    }

    private void KillObject(Projectile obj)
    {
        pool.Release(obj);
    }

    private void SetStartPosition(GameObject obj)
    {
        obj.transform.position = launchTube.transform.position;
    }

    private Projectile GetProjectileType()
    {
        switch (projectileType)
        {
            case ProjectileType.Bullet:
                return bulletObject;                
            case ProjectileType.Rocket:
                return rocketObject;
            case ProjectileType.Grenade:
                return grenadeObject;
        }

        return null;
    }
}
