package com.example.xnslq.novaworld.Adapter.DTO;

import android.widget.Button;

public class RequestDTO {

   public int profile_image;
   public String name;

    public RequestDTO(int profile_image,String name) {
        this.profile_image = profile_image;
        this.name = name;
    }
}
