using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Character_Mantis : MonoBehaviour
{

    private GameObject   taiLungSpell0, taiLungSpell1, taiLungSpell2, taiLungSpell3;
    private Text characterName;
    private Image taiLungSpell_Image0, taiLungSpell_Image1, taiLungSpell_Image2, taiLungSpell_Image3;
    public GameObject characterChange, characterChange2;
    // Use this for initialization
    void Start()
    {

     
        characterName = GameObject.FindWithTag("CharacterName").GetComponent<Text>();  // 캐릭터 선택창의 텍스트 뷰 바인딩

        // 캐릭터 선택창 스킬아이콘 이미지 바인딩
        //taiLungSpell0 = GameObject.FindWithTag("spell_0");
        //taiLungSpell_Image0 = taiLungSpell0.GetComponent<Image>();

        taiLungSpell1 = GameObject.FindWithTag("spell_1");
        taiLungSpell_Image1 = taiLungSpell1.GetComponent<Image>();

        taiLungSpell2 = GameObject.FindWithTag("spell_2");
        taiLungSpell_Image2 = taiLungSpell2.GetComponent<Image>();

        taiLungSpell3 = GameObject.FindWithTag("spell_3");
        taiLungSpell_Image3 = taiLungSpell3.GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void OnClickExit()
    {


        characterName.text = "맨티스";
        // 선택한 캐릭터 생성 또는 없애기 이벤트
        // 현재 캐릭터가 2개 이기 때문에 문제가 없지만
        // 캐릭터가 많아질 경우 내가 선택하지 않은 캐릭터들을 모두 등록하여 꺼줘야하는 번거로움이 있기 때문에
        // 캐릭터가 많아 지면 코드를 수정해야함
        characterChange.SetActive(true);
        characterChange2.SetActive(false);

        taiLungSpell_Image0.sprite = Resources.Load<Sprite>("Mantis_spell_0") as Sprite;
        taiLungSpell_Image1.sprite = Resources.Load<Sprite>("Mantis_spell_1") as Sprite;
        taiLungSpell_Image2.sprite = Resources.Load<Sprite>("Mantis_spell_2") as Sprite;
        taiLungSpell_Image3.sprite = Resources.Load<Sprite>("Mantis_spell_3") as Sprite;


        Debug.Log("Mantis");
    }
}
