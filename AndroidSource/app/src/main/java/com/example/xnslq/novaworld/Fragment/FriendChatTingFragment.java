//package com.example.xnslq.novaworld.Fragment;
//
//import android.annotation.SuppressLint;
//import android.app.Activity;
//import android.app.Fragment;
//import android.content.Context;
//import android.content.SharedPreferences;
//import android.os.Bundle;
//import android.os.Handler;
//import android.os.Message;
//import android.support.annotation.Nullable;
//import android.support.v7.widget.LinearLayoutManager;
//import android.support.v7.widget.RecyclerView;
//import android.util.Log;
//import android.view.LayoutInflater;
//import android.view.View;
//import android.view.ViewGroup;
//import android.widget.TextView;
//import android.widget.Toast;
//
//import com.android.volley.RequestQueue;
//import com.android.volley.Response;
//import com.android.volley.toolbox.Volley;
//import com.example.xnslq.novaworld.Adapter.DTO.ChatListDTO;
//import com.example.xnslq.novaworld.Adapter.DTO.MsgDTO;
//import com.example.xnslq.novaworld.R;
//import com.example.xnslq.novaworld.Request.MyChatKey;
//
//import org.json.JSONObject;
//
//import java.text.SimpleDateFormat;
//import java.util.ArrayList;
//import java.util.Date;
//import java.util.TimeZone;
//
//import de.hdodenhof.circleimageview.CircleImageView;
//
//public class FriendChatTingFragment extends Fragment {
//
//    Handler msgHandler;
//    RecyclerView friendList_RecyclerView;
//    ChatTingFragmentRcyclerViewAdapter chatAdapter;
//    ArrayList<ChatListDTO> chatListDTOs = new ArrayList <> ();
//    private SimpleDateFormat simpleDateFormat = new SimpleDateFormat ("yyyy.MM.dd hh:mm");
//
//    @Nullable
//    @Override
//    public View onCreateView(LayoutInflater inflater,@Nullable ViewGroup container,Bundle savedInstanceState) {
//
//        View view = inflater.inflate (R.layout.fragment_friendchat,container,false);
//
//        friendList_RecyclerView = (RecyclerView) view.findViewById (R.id.chattingfragment_recyclerview);
//        RecyclerView.LayoutManager layoutManager = new LinearLayoutManager (inflater.getContext ());
//        chatAdapter = new ChatTingFragmentRcyclerViewAdapter (getContext (),chatListDTOs);
//        friendList_RecyclerView.setLayoutManager (layoutManager);
//        friendList_RecyclerView.setAdapter (chatAdapter);
//        int image = R.drawable.kakao_default_profile_image;
//        int msgNum = 1;
//
//         simpleDateFormat.setTimeZone (TimeZone.getTimeZone ("Asia/Seoul"));
//
////         Date date = new Date (unixTime);
////        chatListDTOs.add (new ChatListDTO (image,"이기섭","칙촉","오후 13시23분","1" ));
////        chatAdapter.notifyDataSetChanged ();
//
//        SharedPreferences spf = getContext ().getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
//        String loginId = spf.getString ("LoginId","");
//
//        Response.Listener<String> responseListener = new Response.Listener<String> () {
//
//            @SuppressLint("ResourceType")
//            @Override
//            public void onResponse(String response) {
//                try {
//
//                    JSONObject jsonResponse = new JSONObject (response);
//                     String chat_key = jsonResponse.getString ("chat_key");
////                    boolean success = jsonResponse.getBoolean("success");
//
//                    // 중복되는 이메일이 없다면 다이얼로그로 메시지 출력
//                    if (response != null) {
//                        SharedPreferences spf = getContext ().getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
//                        SharedPreferences.Editor editor = spf.edit ();
//                        editor.putString ("chat_key",chat_key);
//                        editor.commit ();
//                    }
//                    // 이미 가입한 이메일 이라면 토스트 메시지 출력
//                    else {
//
//                    }
//
//                } catch (Exception e)  {
//
//                    e.printStackTrace ();
//
//                }
//
//            }
//        };
//
//        MyChatKey myChatKey = new MyChatKey (loginId, responseListener);
//        RequestQueue queue = Volley.newRequestQueue (getActivity ());
//        queue.add (myChatKey);
//
//
//        msgHandler = new Handler (){
//            // 백그라운드 스레드에서 받은 메시지를 처리
//            @Override
//            public void handleMessage(Message msg) {
//                super.handleMessage (msg);
//
//                if (msg.what == 1111){
//                    // 채팅서버로 부터 수신한 메시지를 텍스트뷰에 추가
//
//                    SharedPreferences spf = getContext ().getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
//                    String chat_key = spf.getString ("chat_key","");
//
//                    String[] msgtest = msg.obj.toString ().split (":");
//                    Log.d ("msg","username :" + msgtest[0] +"msg"+msgtest[1]);
//
//
//
//                    if (chat_key.equals (msgtest[2]))
//                    {
//                        int image = R.drawable.kakao_default_profile_image;
//                        ChatListDTO chatListDTO = new ChatListDTO (image,msgtest[0],msgtest[1],"오후 13:23","1");
//
//                        chatListDTOs.add (chatListDTO);
//                        chatAdapter.notifyDataSetChanged ();
//                    }
//
////                    MainWorldChat.setText (msg.obj.toString ());
//
//                }
//            }
//        };
//
//
//
//        return view;
//    }
//
//
//    class ChatTingFragmentRcyclerViewAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {
//
//        ArrayList<ChatListDTO> chatListDTOs = new ArrayList <> ();
//        Context context;
//
//        public ChatTingFragmentRcyclerViewAdapter(Context context,ArrayList <ChatListDTO> friendListDTOs) {
//            this.chatListDTOs = friendListDTOs;
//            this.context = context;
//        }
//
//        @Override
//        public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent,int viewType) {
//
//        View view = LayoutInflater.from (parent.getContext ()).inflate (R.layout.fragment_item_chatlist,parent,false);
//
//            return new ChatlistCall (view);
//        }
//
//        @Override
//        public void onBindViewHolder(RecyclerView.ViewHolder holder,int position) {
//        ChatlistCall chatlistCall = ((ChatlistCall)holder);
//
//            chatlistCall.friendListItem_ProfileIv.setImageResource (chatListDTOs.get (position).profileImage);
//            chatlistCall.friendListItem_Name.setText (chatListDTOs.get (position).name);
//            chatlistCall.friendListItem_message.setText (chatListDTOs.get (position).msg);
//            chatlistCall.friendListItem_date.setText (chatListDTOs.get (position).date);
//            chatlistCall.friendListItem_MsgNum.setText (chatListDTOs.get (position).msgNum);
//
//
//        }
//
//        @Override
//        public int getItemCount() {
//            return chatListDTOs.size ();
//        }
//    }
//
//    class ChatlistCall extends RecyclerView.ViewHolder {
//
//      public CircleImageView friendListItem_ProfileIv;
//      public TextView friendListItem_Name,friendListItem_message,friendListItem_date,friendListItem_MsgNum;
//
//        public ChatlistCall(View view) {
//            super (view);
//
//            friendListItem_ProfileIv =  view.findViewById (R.id.friendListItem_ProfileIv);
//            friendListItem_Name = view.findViewById (R.id.friendListItem_Name);
//            friendListItem_message = view.findViewById (R.id.friendListItem_message);
//            friendListItem_date = view.findViewById (R.id.friendListItem_date);
//            friendListItem_MsgNum = view.findViewById (R.id.friendListItem_MsgNum);
//
//
//        }
//    }
//}
//
