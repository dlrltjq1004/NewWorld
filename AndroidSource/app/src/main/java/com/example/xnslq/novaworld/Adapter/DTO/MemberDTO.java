package com.example.xnslq.novaworld.Adapter.DTO;

public class MemberDTO {


    public int profileImage;
    public String name;
    public String chat_key;

    public MemberDTO(int profileImage,String name, String chat_key) {
        this.profileImage = profileImage;
        this.name = name;
        this.chat_key = chat_key;
    }
}
