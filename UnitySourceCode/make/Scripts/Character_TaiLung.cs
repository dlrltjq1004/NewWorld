using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_TaiLung : MonoBehaviour
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
       

        characterName.text = "타이렁";
        characterChange.SetActive(true);
        characterChange2.SetActive(false);

        taiLungSpell_Image0.sprite = Resources.Load<Sprite>("TaiLung_spell_0") as Sprite;
        taiLungSpell_Image1.sprite = Resources.Load<Sprite>("TaiLung_spell_1") as Sprite;
        taiLungSpell_Image2.sprite = Resources.Load<Sprite>("TaiLung_spell_2") as Sprite;
        taiLungSpell_Image3.sprite = Resources.Load<Sprite>("TaiLung_spell_3") as Sprite;


        Debug.Log("TaiLung");
    }
}
