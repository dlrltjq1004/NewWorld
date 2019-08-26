package com.example.xnslq.novaworld.Adapter.DTO;

public class ChatListDTO {

    public int profileImage;
    public String name;
    public String msg;
    public String date;
    public String msgNum;

    public ChatListDTO(int profileImage,String name,String msg,String date,String msgNum) {
        this.profileImage = profileImage;
        this.name = name;
        this.msg = msg;
        this.date = date;
        this.msgNum = msgNum;
    }
}
