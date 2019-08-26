using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public LayerMask whatIsProp;
    public AudioSource explosionAudio;

    public float maxDamage = 150f;
    public float explosionForce = 1000f;
    public float explosionRadius = 10f;

	

    private void OnTriggerEnter(Collider other)
    {
      Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius,whatIsProp);
      
        for(int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            Prop targetProp = colliders[i].GetComponent<Prop>();

            float damage = CalculateDamage(colliders[i].transform.position);

         //   targetProp.TakeDamage(damage);
        }

    
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        // 나의 위치에서 상대방 에게 가는 방향과 거리를 구한다.
        Vector3 AttackToTarget = targetPosition - transform.position;
        // 나의 위치에서 상대방 까지의 x y z 로 받은 거리를  magnitude(피타고라스) 사용하여 
        // 변환시킴
        // distance 가 0이면 나와 상대방 거리가 0이라는 뜻이 된다.
        float distance = AttackToTarget.magnitude;

         
        //   float edgeToCenterDistance = explosionRadius - distance;

        // %로 표시
        //   float percentage = edgeToCenterDistance / explosionRadius;

        float damage = maxDamage * distance;

        damage = Mathf.Max(0,damage);

        return damage;
    }
}
