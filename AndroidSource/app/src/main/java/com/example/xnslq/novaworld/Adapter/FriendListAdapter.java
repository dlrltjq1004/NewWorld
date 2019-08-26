//package com.example.xnslq.novaworld.Adapter;
//
//import android.annotation.SuppressLint;
//import android.content.Context;
//import android.content.Intent;
//import android.content.SharedPreferences;
//import android.support.v7.widget.RecyclerView;
//import android.util.Log;
//import android.view.LayoutInflater;
//import android.view.View;
//import android.view.ViewGroup;
//import android.widget.ImageView;
//import android.widget.TextView;
//import android.widget.Toast;
//
//import com.android.volley.RequestQueue;
//import com.android.volley.Response;
//import com.android.volley.toolbox.Volley;
//import com.example.xnslq.novaworld.Adapter.DTO.MemberDTO;
//import com.example.xnslq.novaworld.Adapter.DTO.RequestDTO;
//import com.example.xnslq.novaworld.Friend.FriendListActivity;
//import com.example.xnslq.novaworld.R;
//import com.example.xnslq.novaworld.Request.FriendRequest_List;
//import com.example.xnslq.novaworld.Request.FriendRequest_yes;
//
//import org.json.JSONArray;
//import org.json.JSONObject;
//
//import java.util.ArrayList;
//
//import de.hdodenhof.circleimageview.CircleImageView;
//
//public class FriendListAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {
//
//    private ArrayList<MemberDTO> memberDTOs = new ArrayList <> ();
//Context context ;
//
//    public FriendListAdapter() {
////        memberDTOs.add (new MemberDTO (R.drawable.kakao_default_profile_image,"이기섭"
////                ,R.drawable.ic_sms_black_24dp,R.drawable.present,R.drawable.gold_coin));
////        memberDTOs.add (new MemberDTO (R.drawable.kakao_default_profile_image,"전종환"
////                ,R.drawable.ic_sms_black_24dp,R.drawable.present,R.drawable.gold_coin));
////        memberDTOs.add (new MemberDTO (R.drawable.kakao_default_profile_image,"나재원"
////                ,R.drawable.ic_sms_black_24dp,R.drawable.present,R.drawable.gold_coin));
//
//
//    }
//
//    @Override
//    public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent,int viewType) {
//        // XML 가져오는 부분
//        View view = LayoutInflater.from (parent.getContext ()).inflate (R.layout.worldchat,parent,false);
//
//        return new RowCell (view);
//    }
//
//    @Override
//    public void onBindViewHolder(final RecyclerView.ViewHolder holder,int position) {
//      //데이터를(바인딩) 넣어주는 부분
//
//        ((RowCell)holder).circleImageView.setImageResource (memberDTOs.get (position).profileImage);
//        ((RowCell)holder).name.setText (memberDTOs.get (position).name);
//        ((RowCell) holder).chatstart.setOnClickListener (new View.OnClickListener () {
//            @Override
//            public void onClick(View view) {
//                Intent intent = new Intent (view.getContext (),FriendListActivity.class);
//                view.getContext ().startActivity (intent);
//
//
//            }
//        });
//
//
//    }
//
//    @Override
//    public int getItemCount() {
//
//        // 카운터
//        return memberDTOs.size ();
//    }
//
//    // 소스 코드 절약해주는 부분
//    private static class RowCell extends RecyclerView.ViewHolder {
//
//        CircleImageView circleImageView;
//        TextView name;
//        ImageView chatstart,present,sendcoin;
//
//        public RowCell(View view) {
//            super (view);
//            circleImageView = (CircleImageView) view.findViewById (R.id.profile_image);
//            name = (TextView) view.findViewById (R.id.name);
//            chatstart = (ImageView) view.findViewById (R.id.ChatStart);
//            present = (ImageView) view.findViewById (R.id.SendPresent);
//            sendcoin = (ImageView) view.findViewById (R.id.SendCoin);
//
//        }
//    }
//}
