using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Werewolf.SpellIndicators.Demo
{
    public class Skill1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        Character character;
        public SplatManager Splats;

        // Use this for initialization
        void Start()
        {
            Splats = GameObject.Find("SplatManager").GetComponent<SplatManager>();
            character = GameObject.Find("Character").GetComponent<Character>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Splats.Direction.Select();
            Splats.Direction.Scale = 5f;
            Debug.Log("스킬 범위 표시!");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("스킬 발동!");
        }

      
    }
}
