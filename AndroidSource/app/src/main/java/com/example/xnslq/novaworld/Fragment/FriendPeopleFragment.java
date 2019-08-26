package com.example.xnslq.novaworld.Fragment;

import android.annotation.SuppressLint;
import android.app.Fragment;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.toolbox.Volley;
import com.example.xnslq.novaworld.Adapter.DTO.MemberDTO;

import com.example.xnslq.novaworld.Chat.FriendList_ChatRoomActivity;
import com.example.xnslq.novaworld.Friend.FriendListActivity;
import com.example.xnslq.novaworld.R;
import com.example.xnslq.novaworld.Request.FriendRequest_List;

import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;

import de.hdodenhof.circleimageview.CircleImageView;

public class FriendPeopleFragment extends Fragment {

    ArrayList<MemberDTO> memberDTOs = new ArrayList<>();
    FriendListAdapter friendListAdapter;
    Context context;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater,@Nullable ViewGroup container,Bundle savedInstanceState) {

        View view = inflater.inflate (R.layout.fragment_people,container,false);

        RecyclerView frindListItem_RecyclerView = (RecyclerView) view.findViewById (R.id.peoplefragment_recyclerview);
        RecyclerView.LayoutManager layoutManager = new LinearLayoutManager (inflater.getContext ());
        friendListAdapter = new FriendListAdapter(getActivity (),memberDTOs);
        frindListItem_RecyclerView.setLayoutManager (layoutManager);
        frindListItem_RecyclerView.setAdapter (friendListAdapter);

        Response.Listener <String> responseListener = new Response.Listener <String> () {

            @SuppressLint("ResourceType")
            @Override
            public void onResponse(String response) {
                try {
                    JSONObject jsonResponse = new JSONObject (response);
                    Log.d ("p_boject:",String.valueOf (jsonResponse));
                    JSONArray jsonArray =  jsonResponse.getJSONArray ("People");
                    Log.d ("p_b:",String.valueOf (jsonArray));

                    Log.d ("p_count",String.valueOf (jsonArray.length ()));



                    if (response != null) {

                        for (int i = 0; i < jsonArray.length (); i++) {

                            jsonResponse = jsonArray.getJSONObject (i);
                            String friend_id = jsonResponse.getString ("Friend_id");
                            String chat_key = jsonResponse.getString ("chat_key");
                            int image = R.drawable.kakao_default_profile_image;
                            Log.d ("p_People_id",friend_id + "p_chat_key : " + chat_key);
                            memberDTOs.add (new MemberDTO (image,friend_id,chat_key));
                            friendListAdapter.notifyDataSetChanged ();
                        }
                    } else {

                    }

                } catch (Exception e) {
                    e.printStackTrace (); } }};
        SharedPreferences spf = getContext ().getSharedPreferences ("LoginId",getActivity ().MODE_PRIVATE);
        String my_id = spf.getString ("LoginId","");
        Log.d ("People_id",my_id);
        FriendRequest_List friendRequest_list = new FriendRequest_List (my_id,responseListener);
        RequestQueue queue = Volley.newRequestQueue (getActivity ());
        queue.add (friendRequest_list);

        return view;
    }

    class FriendListAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {

        private ArrayList<MemberDTO> memberDTOs = new ArrayList <> ();
        Context context ;

        public FriendListAdapter(Context context , ArrayList<MemberDTO> memberDTOs) {
//        memberDTOs.add (new MemberDTO (R.drawable.kakao_default_profile_image,"이기섭"
//                ,R.drawable.ic_sms_black_24dp,R.drawable.present,R.drawable.gold_coin));
//        memberDTOs.add (new MemberDTO (R.drawable.kakao_default_profile_image,"전종환"
//                ,R.drawable.ic_sms_black_24dp,R.drawable.present,R.drawable.gold_coin));
//        memberDTOs.add (new MemberDTO (R.drawable.kakao_default_profile_image,"나재원"
//                ,R.drawable.ic_sms_black_24dp,R.drawable.present,R.drawable.gold_coin));

             this.context = context;
             this.memberDTOs = memberDTOs;

        }

        @Override
        public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent,int viewType) {
            // XML 가져오는 부분
            View view = LayoutInflater.from (parent.getContext ()).inflate (R.layout.worldchat,parent,false);

            return new RowCell (view);
        }

        @Override
        public void onBindViewHolder(final RecyclerView.ViewHolder holder,final int position) {
            //데이터를(바인딩) 넣어주는 부분
            final RowCell rowCell = ((RowCell)holder);

            rowCell.circleImageView.setImageResource (memberDTOs.get (position).profileImage);
            rowCell.name.setText (memberDTOs.get (position).name);
            rowCell.friendListItem_chat_key.setText (memberDTOs.get (position).chat_key);
            rowCell.chatstart.setOnClickListener (new View.OnClickListener () {
                @Override
                public void onClick(View view) {

            String friendId = rowCell.name.getText ().toString ();
            String chat_key = rowCell.friendListItem_chat_key.getText ().toString ();

//            Toast.makeText (getContext (),friendId +"key: "+chat_key,Toast.LENGTH_SHORT).show ();

                    Intent intent = new Intent (view.getContext (),FriendList_ChatRoomActivity.class);
                    intent.putExtra ("friendId",friendId);
                    intent.putExtra ("chat_key",chat_key);
                    view.getContext ().startActivity (intent);

                }
            });
        }


        @Override
        public int getItemCount() {

            // 카운터
            return memberDTOs.size ();
        }

        // 소스 코드 절약해주는 부분
    class RowCell extends RecyclerView.ViewHolder {

            CircleImageView circleImageView;
            TextView name,friendListItem_chat_key;
            ImageView chatstart,present,sendcoin;

            public RowCell(View view) {
                super (view);

                circleImageView = (CircleImageView) view.findViewById (R.id.profile_image);
                name = (TextView) view.findViewById (R.id.name);
                friendListItem_chat_key = view.findViewById (R.id.friendListItem_chat_key);
                chatstart = (ImageView) view.findViewById (R.id.ChatStart);
                present = (ImageView) view.findViewById (R.id.SendPresent);
                sendcoin = (ImageView) view.findViewById (R.id.SendCoin);

            }
        }
    }


    }


