package com.example.xnslq.novaworld.Friend;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

//import com.example.xnslq.novaworld.Fragment.FriendChatTingFragment;
import com.example.xnslq.novaworld.Fragment.FriendPeopleFragment;
import com.example.xnslq.novaworld.Fragment.FriendRequestFragment;
import com.example.xnslq.novaworld.Fragment.FriendaddFragment;
import com.example.xnslq.novaworld.R;

public class FriendListActivity extends AppCompatActivity {


  TextView friendListItem_friendList_btn,friendListItem_Chatting_btn,
          friendListItem_friendadd_btn,friendListItem_friendRequest_btn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate (savedInstanceState);
        setContentView (R.layout.activity_friend_list);

       TextView friendListItem_friendList_btn = (TextView) findViewById (R.id.friendListItem_friendList_btn);
//       TextView friendListItem_Chatting_btn = (TextView) findViewById (R.id.friendListItem_Chatting_btn);
       TextView friendListItem_friendadd_btn = (TextView) findViewById (R.id.friendListItem_friendadd_btn);
       TextView friendListItem_friendRequest_btn = (TextView) findViewById (R.id.friendListItem_friendRequest_btn);



        getFragmentManager ().beginTransaction ().replace (R.id.friendListItem_framelayout,new FriendPeopleFragment ()).commit ();

        FragmentClickEvent(); // 프래그먼트를 클릭시 해당 프래그먼트로 이동 시키는 메소드


    }

   // 프래그먼트를 클릭시 해당 프래그먼트로 이동 시키는 메소드
   private void FragmentClickEvent() {

        final View.OnClickListener handler = new View.OnClickListener () {

            @Override
            public void onClick(View v) {

                switch (v.getId ())
                {
                    case R.id.friendListItem_friendList_btn:

                        getFragmentManager ().beginTransaction ().replace (R.id.friendListItem_framelayout,new FriendPeopleFragment ()).commit ();

                        break;
//                    case R.id.friendListItem_Chatting_btn:
//                        getFragmentManager ().beginTransaction ().replace (R.id.friendListItem_framelayout,new FriendChatTingFragment ()).commit ();
//
//                        break;
                    case R.id.friendListItem_friendadd_btn:
                        getFragmentManager ().beginTransaction ().replace (R.id.friendListItem_framelayout,new FriendaddFragment ()).commit ();

                        break;
                    case R.id.friendListItem_friendRequest_btn:
                        getFragmentManager ().beginTransaction ().replace (R.id.friendListItem_framelayout,new FriendRequestFragment ()).commit ();

                        break;
                }

            }


        };

      findViewById (R.id.friendListItem_friendList_btn).setOnClickListener (handler);
//      findViewById (R.id.friendListItem_Chatting_btn).setOnClickListener (handler);
      findViewById (R.id.friendListItem_friendadd_btn).setOnClickListener (handler);
      findViewById (R.id.friendListItem_friendRequest_btn).setOnClickListener (handler);
    }
}
