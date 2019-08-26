package com.example.xnslq.novaworld.Adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.view.ViewGroup;

import com.example.xnslq.novaworld.Adapter.DTO.ChatRoom_MsgDTO;

import java.util.ArrayList;

public class ChatRoomMsgAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {

    ArrayList<ChatRoom_MsgDTO> chatRoom_msgDTOs = new ArrayList <> ();
    Context context;
    public ChatRoomMsgAdapter(Context context, ArrayList <ChatRoom_MsgDTO> chatRoom_msgDTOs) {
        this.chatRoom_msgDTOs = chatRoom_msgDTOs;
        this.context = context;
    }

    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent,int viewType) {


        return null;
    }

    @Override
    public void onBindViewHolder(RecyclerView.ViewHolder holder,int position) {

    }

    @Override
    public int getItemCount() {
        return chatRoom_msgDTOs.size ();
    }

    class ChatRoomCall extends RecyclerView.ViewHolder {



        public ChatRoomCall(View itemView) {
            super (itemView);


        }
    }

}
