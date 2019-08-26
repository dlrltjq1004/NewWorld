package com.example.xnslq.novaworld.Fragment;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Fragment;
import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.app.AlertDialog;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.toolbox.Volley;
import com.example.xnslq.novaworld.Adapter.DTO.AddFriendDTO;
import com.example.xnslq.novaworld.R;

import org.json.JSONObject;

import java.util.ArrayList;

import de.hdodenhof.circleimageview.CircleImageView;

public class FriendaddFragment extends Fragment {

    RecyclerView friendAddRecyclerView;
 //   AddFriendFragmentRcyclerViewAdapter addAdapter;
    ArrayList<AddFriendDTO> addFriendDTOS = new ArrayList <> ();
    EditText  addFriendItem_SearchEdit;
    Button addFriendItem_Btn;
    AlertDialog dialog;
    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater,@Nullable ViewGroup container,Bundle savedInstanceState) {

        final View view = inflater.inflate (R.layout.fragment_addfriend,container,false);

        friendAddRecyclerView = (RecyclerView) view.findViewById (R.id.Search_RecyclerView);
        RecyclerView.LayoutManager layoutManager = new LinearLayoutManager (inflater.getContext ());
        friendAddRecyclerView.setLayoutManager (layoutManager);
      final AddFriendFragmentRcyclerViewAdapter  addAdapter = new AddFriendFragmentRcyclerViewAdapter (getContext (),addFriendDTOS);
        friendAddRecyclerView.setAdapter (addAdapter);

         addFriendItem_SearchEdit = (EditText) view.findViewById (R.id.addFriendItem_SearchEdit);
         addFriendItem_Btn  = (Button) view.findViewById (R.id.addFriendItem_Btn);



        addFriendItem_Btn.setOnClickListener (new View.OnClickListener () {
            @Override
            public void onClick(View v) {

              final String SearchId = addFriendItem_SearchEdit.getText ().toString ();




                // 아이디 중복체크시 공백 일시 확인 메시지
                if (SearchId.equals (""))
                {
                    final AlertDialog.Builder builder = new AlertDialog.Builder(getActivity ());
                    dialog = builder.setMessage ("아이디를 입력 해주세요.")
                            .setPositiveButton ("확인",null)
                            .create ();

                    // 다이얼로그 확인 버튼 컬러 바꾸기 이벤트
                    dialog.setOnShowListener (new DialogInterface.OnShowListener () {
                        @Override
                        public void onShow(DialogInterface arg0) {
                            dialog.getButton (AlertDialog.BUTTON_POSITIVE).setTextColor(Color.BLACK);
                        }
                    });

                    dialog.show ();
                    return;
                }
                // 아이디 입력란이 공백이 아니라면 DB에 해당 아이디가 있는지 체크
                Response.Listener<String> responseListener = new Response.Listener<String> () {

                    @SuppressLint("ResourceType")
                    @Override
                    public void onResponse(String response) {
                        try {

                            JSONObject jsonResponse = new JSONObject (response);
                            boolean success = jsonResponse.getBoolean("success");

                            // 중복되는 이메일이 없다면 다이얼로그로 메시지 출력
                            if (success) {

                                int profile_image = R.drawable.kakao_default_profile_image;
                                //    int addFriendimage = R.drawable.ic_person_add_black_24dp;
                                addFriendDTOS.add (new AddFriendDTO (profile_image,SearchId));
                                addAdapter.notifyDataSetChanged ();

                            }
                            // 이미 가입한 이메일 이라면 토스트 메시지 출력
                            else {

                                Toast.makeText (getContext (),"해당 플레이어를 찾지 못 했습니다.",Toast.LENGTH_LONG).show ();
                            }

                        } catch (Exception e)  {

                            e.printStackTrace ();

                        }

                    }
                };
                AddFriendSearch addFriendSearch = new AddFriendSearch (SearchId, responseListener);
                RequestQueue queue = Volley.newRequestQueue (getActivity ());
                queue.add (addFriendSearch);
            }
        });


        return view;


    }

   public class AddFriendFragmentRcyclerViewAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {

        ArrayList<AddFriendDTO> addFriendDTOs = new ArrayList <> ();
        Context context;


        public AddFriendFragmentRcyclerViewAdapter(Context context, ArrayList<AddFriendDTO> addFriendDTOs) {
         //  addFriendDTOs.add (new AddFriendDTO (R.drawable.kakao_default_profile_image,"이기섭"));

            this.context = context;
            this.addFriendDTOs = addFriendDTOs;

        }



        @Override
        public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent,int viewType) {

            View view = LayoutInflater.from (parent.getContext ()).inflate (R.layout.fragment_item_search,parent,false);



            return new AddCell(view);
        }

        @Override
        public void onBindViewHolder(RecyclerView.ViewHolder holder,final int position) {

            final AddCell addCell = ((AddCell)holder);

            addCell.search_profile_image.setImageResource (addFriendDTOs.get (position).profileImage);
            addCell.search_name.setText (addFriendDTOs.get (position).name);
            //  addCell.search_addFriend.setImageResource (addFriendDTOs.get (position).addFriendImage);

            addCell.search_addFriend.setOnClickListener (new View.OnClickListener () {
                @Override
                public void onClick(View v) {

                    SharedPreferences spf = getContext ().getSharedPreferences ("LoginId",Activity.MODE_PRIVATE);
                    String Request_id = spf.getString ("LoginId","");
                    Log.d ("로그인아이디",Request_id);
                 String answer_id = addCell.search_name.getText ().toString ();
                 int Request_profileimage = R.drawable.kakao_default_profile_image;

                    Response.Listener<String> responseListener = new Response.Listener<String> () {

                        @SuppressLint("ResourceType")
                        @Override
                        public void onResponse(String response) {
                            try {

                                JSONObject jsonResponse = new JSONObject (response);
                                boolean success = jsonResponse.getBoolean("success");

                                // 중복되는 이메일이 없다면 다이얼로그로 메시지 출력
                                if (success) {

                                 addFriendDTOs.remove (position);
                                 AddFriendFragmentRcyclerViewAdapter addAdapter = new AddFriendFragmentRcyclerViewAdapter (getContext (),addFriendDTOs);
                                 addAdapter.notifyDataSetChanged ();

                                    Toast.makeText (getContext (),"해당플레이어 에게 친구신청 요청을 보넀습니다.",Toast.LENGTH_SHORT).show ();
                                }
                                // 이미 가입한 이메일 이라면 토스트 메시지 출력
                                else {

                                    Toast.makeText (getContext (),"해당플레이어를 친구추가에 실패했습니다.",Toast.LENGTH_SHORT).show ();



                                }

                            } catch (Exception e)  {

                                e.printStackTrace ();

                            }

                        }
                    };

                    AddFriendRequest addFriendRequest = new AddFriendRequest (Request_id,Request_profileimage,answer_id, responseListener);
                    RequestQueue queue = Volley.newRequestQueue (getActivity ());
                    queue.add (addFriendRequest);
                }
            });
        }



        @Override
        public int getItemCount() {

            return addFriendDTOs.size ();
        }
    }

    private static class AddCell extends RecyclerView.ViewHolder {

       public CircleImageView search_profile_image;
       public TextView search_name;
       public ImageView search_addFriend;


        public AddCell(View view) {
            super (view);

            search_profile_image   = (CircleImageView) view.findViewById (R.id.search_profile_image);
            search_name   = (TextView) view.findViewById (R.id.search_name);
            search_addFriend   = (ImageView) view.findViewById (R.id.search_addFriend);


        }
    }
}
