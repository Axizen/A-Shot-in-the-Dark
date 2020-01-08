using UnityEngine;
using System.Collections;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity {

	public enum State {Idle, Chasing, Attacking, Jumping};
	State currentState;
    
	public ParticleSystem deathEffect;
	public static event System.Action OnDeathStatic;

	NavMeshAgent pathfinder;
	Transform target;
	LivingEntity targetEntity;
	Material skinMaterial;
    Material skinMaterialChild;
    public Animator anim; //For enemies with animations
	Color originalColour;

	float attackDistanceThreshold = .5f;
	float timeBetweenAttacks = 1;
	float damage = 1;

	float nextAttackTime;
	float myCollisionRadius;
	float targetCollisionRadius;

    public float jumpForce;
    public float jumpSpeed;
    public float gravity = 10.0f;

    public bool canJump;
    bool b_hasAnimation;

	bool hasTarget;

	void Awake() {
		pathfinder = GetComponent<NavMeshAgent> ();
        if (anim != null)
        {
            b_hasAnimation = true;
        }
        if (GameObject.FindGameObjectWithTag ("Player") != null) {
			hasTarget = true;
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			targetEntity = target.GetComponent<LivingEntity> ();
			
			myCollisionRadius = GetComponent<CapsuleCollider> ().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius;
		}
	}
	
	protected override void Start () {
		base.Start ();

		if (hasTarget) {
			currentState = State.Chasing;
			targetEntity.OnDeath += OnTargetDeath;

			StartCoroutine (UpdatePath ());
		}
	}

	public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColour) {
		pathfinder.speed = moveSpeed;
        if (b_hasAnimation)
        {
            anim.SetFloat("MoveSpeed", moveSpeed);
        }       
        if (hasTarget) {
			damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
		}
		startingHealth = enemyHealth;

		//deathEffect.startColor = new Color (skinColour.r, skinColour.g, skinColour.b, 1);
		skinMaterial = GetComponent<Renderer> ().material;
		skinMaterial.color = skinColour;
		originalColour = skinMaterial.color;
	}

	public override void TakeHit (float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		AudioManager.instance.PlaySound ("Impact", transform.position);
		if (damage >= health && !dead) {
			if (OnDeathStatic != null) {
				OnDeathStatic ();
                //Play die animation 
                if (b_hasAnimation)
                {
                    anim.SetBool("Dead", true);
                    anim.SetBool("Idle", false);
                    anim.SetBool("Chasing", false);
                    anim.SetBool("Attacking", false);
                }
            }
            AudioManager.instance.PlaySound ("Enemy Death", transform.position);
			Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.startLifetime);
		}
		base.TakeHit (damage, hitPoint, hitDirection);
	}

	void OnTargetDeath() {
		hasTarget = false;
		currentState = State.Idle;
        //Play idle animation 
        if (b_hasAnimation)
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Chasing", false);
            anim.SetBool("Attacking", false);
        }
       
    }

	void Update () {

		if (hasTarget) {
			if (Time.time > nextAttackTime) {
				float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
				if (sqrDstToTarget < Mathf.Pow (attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)) {
					nextAttackTime = Time.time + timeBetweenAttacks;
					AudioManager.instance.PlaySound ("Enemy Attack", transform.position);
					StartCoroutine (Attack ());
				}

			}
		}

	}

	IEnumerator Attack() {

		currentState = State.Attacking;
		pathfinder.enabled = false;
        if (b_hasAnimation)
        {
            anim.SetBool("Chasing", false);
        }

        Vector3 originalPosition = transform.position;
		Vector3 dirToTarget = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

		float attackSpeed = 3;
		float percent = 0;

		skinMaterial.color = Color.red;
		bool hasAppliedDamage = false;

		while (percent <= 1) {

			if (percent >= .5f && !hasAppliedDamage) {
				hasAppliedDamage = true;
                // play attack animation 
                if (b_hasAnimation)
                {
                    anim.SetBool("Attacking", true);

                }
                targetEntity.TakeDamage(damage);
			}

			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;
			transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            
			yield return null;
		}

		skinMaterial.color = originalColour;
		currentState = State.Chasing;
        if (b_hasAnimation)
        {
            anim.SetBool("Attacking", false);

        }
        pathfinder.enabled = true;
	}

    IEnumerator Jump()
    {
        currentState = State.Jumping;
        yield return null;
    }

	IEnumerator UpdatePath() {
		float refreshRate = .25f;

		while (hasTarget) {
			if (currentState == State.Chasing) {
                //Play walking/running animation
                if (b_hasAnimation)
                {
                    anim.SetBool("Chasing", true);
                } 
                Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
				if (!dead) {
					pathfinder.SetDestination (targetPosition);
				}
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
}