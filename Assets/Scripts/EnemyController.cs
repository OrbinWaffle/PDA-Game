using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject deathParticle;
    Transform player;
    public float enemyMoveSpeed = 10f;
    public float enemySpinSpeed = 1f;
    public float targetOffset = 5f;
    bool sawlast = false;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.headTransform;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        // If there's not anything between the enemy and the player, move towards the player
        RaycastHit hit;
        bool obstructed = Physics.Linecast(transform.position, player.position, out hit);

        if (!obstructed)
        {
            Vector3 moveDir = ((player.position + new Vector3(0, targetOffset, 0)) - transform.position).normalized;
            //rb.MovePosition(player.transform.position + moveVector * Time.deltaTime);
            rb.AddForce(moveDir * enemyMoveSpeed, ForceMode.Acceleration);
            rb.AddTorque(Vector3.up * enemySpinSpeed, ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Key"))
        {
            Instantiate(deathParticle, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
