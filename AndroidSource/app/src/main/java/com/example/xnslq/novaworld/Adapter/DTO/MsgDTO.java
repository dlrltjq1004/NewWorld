package com.example.xnslq.novaworld.Adapter.DTO;

import android.os.Message;

public class MsgDTO {
   public int msgImage;
   public String name;
   public String msg;

    public MsgDTO(int msgImage,String name,String msg) {
        this.msgImage = msgImage;
        this.name = name;
        this.msg = msg;
    }
}
