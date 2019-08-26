package com.example.xnslq.novaworld.Adapter;

import android.app.Activity;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.example.xnslq.novaworld.Adapter.DTO.MemberDTO;
import com.example.xnslq.novaworld.Adapter.DTO.MsgDTO;
import com.example.xnslq.novaworld.Main_Activity;
import com.example.xnslq.novaworld.R;

import java.util.ArrayList;

import de.hdodenhof.circleimageview.CircleImageView;

public class MsgAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder>{

    Context context;
    ArrayList<MsgDTO> msgDTOs = new ArrayList <> ();



    public MsgAdapter(Context context ,ArrayList<MsgDTO> msgDTOs) {

    this.context = context;
    this.msgDTOs = msgDTOs;


    }


    // XML 객체로 만드는 부분
    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent,int viewType) {

        View view = LayoutInflater.from (parent.getContext ()).inflate (R.layout.chatmsg,parent,false);

        return new MsgCell(view);
    }


    @Override
    public void onBindViewHolder(RecyclerView.ViewHolder holder,int position) {
         // 바인드
        MsgCell msgCell = ((MsgCell)holder);

        Main_Activity main_activity = new Main_Activity ();

       SharedPreferences spf = context.getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);

        int image = R.drawable.kakao_default_profile_image;

        String loginId = spf.getString ("LoginId","");
        String outputid = spf.getString ("Receive","");

      //  String[] msgtest = msg.obj.toString ().split ("/");

     //   String yourid = msgtest[0];

     //   MyReceiver myReceiver =  new MyReceiver ();

//      String outputId = myReceiver.outputId;


        Log.d ("outputId: ",outputid +"/"+"loginId: "+loginId);


        // 메시지를 전송할때 보여지는 UI
        if (msgDTOs.get (position).name.equals (loginId))
        {
            msgCell.ImageView_porifle_right.setImageResource (msgDTOs.get (position).msgImage);
            msgCell.textView_name_right.setText (msgDTOs.get (position).name);
            msgCell.textView_name_right.setTextSize (18);
            msgCell.textVeiw_message_right.setTextSize (15);
            msgCell.textVeiw_message_right.setText (msgDTOs.get (position).msg);
            msgCell.textVeiw_message_right.setBackgroundResource (R.drawable.righttbubble);
            msgCell.linearLayout_destination_left.setVisibility (View.GONE);
            msgCell.linearLayout_destination_right.setVisibility (View.VISIBLE);
            msgCell.messageItem_linearlayout_main.setGravity (Gravity.RIGHT);
        } else  // 메시지를 받았을때 보여지는 UI

        {
            msgCell.ImageView_porifle_left.setImageResource (msgDTOs.get (position).msgImage);
            msgCell.textView_name_left.setText (msgDTOs.get (position).name);
            msgCell.textView_name_left.setTextSize (18);
            msgCell.textVeiw_message_left.setTextSize (15);
            msgCell.textVeiw_message_left.setText (msgDTOs.get (position).msg);
            msgCell.textVeiw_message_left.setBackgroundResource (R.drawable.leftbubble);
            msgCell.linearLayout_destination_right.setVisibility (View.GONE);
            msgCell.linearLayout_destination_left.setVisibility (View.VISIBLE);
            msgCell.messageItem_linearlayout_main.setGravity (Gravity.LEFT);



            // msgCell.linearLayout_destination.setGravity ();
        }

    }

    @Override
    public void onDetachedFromRecyclerView(RecyclerView recyclerView) {
        super.onDetachedFromRecyclerView (recyclerView);

        recyclerView.scrollToPosition (msgDTOs.size () -1);
    }

    @Override
    public int getItemCount() {
        // 카운터

        return msgDTOs.size ();
    }

    private static class MsgCell extends RecyclerView.ViewHolder {

        public CircleImageView ImageView_porifle_left,ImageView_porifle_right;
        public TextView textView_name_left,textVeiw_message_left,textView_name_right,textVeiw_message_right;
        public LinearLayout linearLayout_destination_left,linearLayout_destination_right,messageItem_linearlayout_main;

        public MsgCell(View view) {
            super (view);
            messageItem_linearlayout_main = (LinearLayout) view.findViewById (R.id.messageItem_linearlayout_main);

            // 왼쪽 상대방이 보낸 메시지
            ImageView_porifle_left = (CircleImageView) view.findViewById (R.id.messageItem_imageview_profile_left);
            textView_name_left = (TextView) view.findViewById (R.id.messageItem_textview_name_left);
            textVeiw_message_left = (TextView)view.findViewById (R.id.messageItem_textVeiw_message_left);
            linearLayout_destination_left = (LinearLayout)view.findViewById (R.id.messageItem_linearlayout_destination_left);

            // 오른쪽 내가 보낸 메시지
            ImageView_porifle_right = (CircleImageView) view.findViewById (R.id.messageItem_imageview_profile_right);
            textView_name_right = (TextView) view.findViewById (R.id.messageItem_textview_name_right);
            textVeiw_message_right = (TextView)view.findViewById (R.id.messageItem_textVeiw_message_right);
            linearLayout_destination_right = (LinearLayout)view.findViewById (R.id.messageItem_linearlayout_destination_right);
        }
    }

 public static class MyReceiver extends BroadcastReceiver{

       public String outputId;

    public void onReceive(Context context,Intent intent){
        Bundle bundle = intent.getExtras ();
         this.outputId = bundle.getString ("outputId");

        SharedPreferences spf = context.getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
        SharedPreferences.Editor editor = spf.edit ();
        editor.putString ("Receive",outputId);
        editor.commit ();

       Log.d ("test",outputId);


    }

}


}
