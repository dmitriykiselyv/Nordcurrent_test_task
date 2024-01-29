using System.Collections;
using UnityEngine;

public class TankShoot : MonoBehaviour
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private Projectile _projectile;
    [SerializeField] Animator _animator;

    private int _numberOfProjectiles = 5;
    private float _fireTimer;
    private float _fireInterval;
    private GenericObjectPool<Projectile> _projectilePool;


    private void Awake()
    {
        _projectilePool = new GenericObjectPool<Projectile>(CreateProjectiles, _numberOfProjectiles);
        _fireInterval = 1 / _fireRate;
    }

    private void Update()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _fireInterval && Input.GetMouseButton(0))
        {
            _fireTimer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        Projectile projectile = _projectilePool.Get();
        projectile.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);
        projectile.OnConstruct(_projectilePool);

        PlaySmokeAnimation();
    }

    private Projectile CreateProjectiles()
    {
        return Instantiate(_projectile, Vector3.zero, Quaternion.identity);
    }

    private void PlaySmokeAnimation()
    {
        _animator.SetBool(GameTags.ANIMATION_SMOKE_TRIGGER, true);

        float duration = GetAnimationClipLength(GameTags.ANIMATION_CLIP_SMOKE);
        StartCoroutine(ResetSmokeAnimation(duration));
    }

    private IEnumerator ResetSmokeAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
        _animator.SetBool(GameTags.ANIMATION_SMOKE_TRIGGER, false);
    }

    private float GetAnimationClipLength(string clipName)
    {
        RuntimeAnimatorController ac = _animator.runtimeAnimatorController;

        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }

        Debug.LogError("Animation clip not found!");
        return 0f;
    }
}
