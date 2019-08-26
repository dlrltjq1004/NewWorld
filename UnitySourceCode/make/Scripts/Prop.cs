using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prop : MonoBehaviour {

    public ParticleSystem explosionParticle;
    public float hp = 100f;

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            // Instantiate 는 원본 게임 오브젝트를 복사해주는 함수 
            ParticleSystem instance = Instantiate(explosionParticle,transform.position,transform.rotation);

            AudioSource explosionAudio = instance.GetComponent<AudioSource>();

            Destroy(instance.gameObject,instance.duration);
            gameObject.SetActive(false);
            Debug.Log("포탑이 파괴되었습니다.");

        }
    }
	
}
