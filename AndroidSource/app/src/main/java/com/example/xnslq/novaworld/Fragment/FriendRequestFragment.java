package com.example.xnslq.novaworld.Fragment;

import android.annotation.SuppressLint;
import android.app.Fragment;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.toolbox.Volley;
import com.example.xnslq.novaworld.Adapter.DTO.RequestDTO;
import com.example.xnslq.novaworld.R;
import com.example.xnslq.novaworld.Request.FriendRequest_Dle;
import com.example.xnslq.novaworld.Request.FriendRequest_yes;

import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.params.HttpParams;
import org.json.JSONArray;
import org.json.JSONObject;

import java.util.ArrayList;

import de.hdodenhof.circleimageview.CircleImageView;

public class FriendRequestFragment extends Fragment {

    HttpClient httpClient = new DefaultHttpClient ();
    HttpParams params = httpClient.getParams ();
    HttpPost httpPost;
    RecyclerView fragmentrequestitem_recyclerview;
    RequestFragmentRecyclerViewAdapter requestAdapter;
    ArrayList <RequestDTO> requestDTOs = new ArrayList <> ();

    @Nullable
    @Override
    public View onCreateView(final LayoutInflater inflater,@Nullable ViewGroup container,Bundle savedInstanceState) {

        View view = inflater.inflate (R.layout.fragment_request,container,false);

        fragmentrequestitem_recyclerview = (RecyclerView) view.findViewById (R.id.fragmentrequestitem_recyclerview);
        requestAdapter = new RequestFragmentRecyclerViewAdapter (getContext (),requestDTOs);
        RecyclerView.LayoutManager layoutManager = new LinearLayoutManager (inflater.getContext ());
        fragmentrequestitem_recyclerview.setLayoutManager (layoutManager);
        fragmentrequestitem_recyclerview.setAdapter (requestAdapter);


//    try
//    {
////        UtilShared utilShared = new UtilShared ();
////       String request_id = utilShared.getSharedPreference(getActivity (),"LoginId");

//        Log.d ("쉐어드값",request_Id);
//        httpPost  = new HttpPost ("http://13.125.250.235/FriendRequest.php");   // 데이터를 보낼 URL
//        ArrayList<NameValuePair> post = new ArrayList <NameValuePair> ();
//        post.add (new BasicNameValuePair ("myId",request_Id));
//        post.add (new BasicNameValuePair ("profileImage",request_Id));
//        post.add (new BasicNameValuePair ("youerId",request_Id));
//
//
//    }catch (Exception e)
//    {
//        e.printStackTrace ();
//    }



        SharedPreferences spf = getContext ().getSharedPreferences ("LoginId",getActivity ().MODE_PRIVATE);
        final String request_Id = spf.getString ("LoginId","");

        Response.Listener <String> responseListener = new Response.Listener <String> () {

            @SuppressLint("ResourceType")
            @Override
            public void onResponse(String response) {


                try {

                      JSONObject jsonResponse = new JSONObject (response);
                      JSONArray jsonArray =  jsonResponse.getJSONArray ("result");


//                    boolean success = jsonArray.getBoolean ("success");
//                    Log.d ("bb:",String.valueOf (success));





                        if (response != null) {

                            for (int i = 0; i < jsonArray.length (); i++)
                            {

                                jsonResponse = jsonArray.getJSONObject (i);
                             String id =   jsonResponse.getString ("Request_id");
                             String image =   jsonResponse.getString ("Request_profileimage");
                                Log.d ("id",id +"image"+image);
                                int image2 = Integer.parseInt (image);

                                requestDTOs.add (new RequestDTO (image2,id));
                                requestAdapter.notifyDataSetChanged ();
                            }
                        } else {
                        }
                } catch (Exception e) {
                    e.printStackTrace ();
                }
            }
        };
        FriendRequest_Request friendRequest_request = new FriendRequest_Request (request_Id,responseListener);
        RequestQueue queue = Volley.newRequestQueue (getActivity ());
        queue.add (friendRequest_request);



        return view;
    }


    class RequestFragmentRecyclerViewAdapter extends RecyclerView.Adapter <RecyclerView.ViewHolder>


    {

        ArrayList <RequestDTO> requestDTOs = new ArrayList <> ();
        Context context;

        public RequestFragmentRecyclerViewAdapter(Context context,ArrayList <RequestDTO> requestDTOs) {
            this.context = context;
            this.requestDTOs = requestDTOs;

        }

        @Override
        public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent,int viewType) {

            View view = LayoutInflater.from (parent.getContext ()).inflate (R.layout.fragment_item_request,parent,false);


            return new RequestCall (view);
        }

        @Override
        public void onBindViewHolder(final RecyclerView.ViewHolder holder,final int position) {
            final RequestCall requestCall = ((RequestCall) holder);

            requestCall.friendrequestitem_profile.setImageResource (requestDTOs.get (position).profile_image);
            requestCall.friendrequestitem_name.setText (requestDTOs.get (position).name);
            requestCall.friendrequestitem_accept_btn.setOnClickListener (new View.OnClickListener () {
                @Override
                public void onClick(View v) {
                 // 수락
                    SharedPreferences spf = getContext ().getSharedPreferences ("LoginId",getActivity ().MODE_PRIVATE);
                    final String my_id = spf.getString ("LoginId","");
                          String youer_id = requestCall.friendrequestitem_name.getText ().toString ();

                        Response.Listener <String> responseListener = new Response.Listener <String> () {

                            @SuppressLint("ResourceType")
                            @Override
                            public void onResponse(String response) {
                                try {
                                    if (response != null) {

                                        requestDTOs.remove (position);
                                        requestAdapter.notifyDataSetChanged ();


                                        // 친구 요청 삭제
                                        Response.Listener <String> responseListener = new Response.Listener <String> () {

                                            @SuppressLint("ResourceType")
                                            @Override
                                            public void onResponse(String response) {
                                                try {
                                                    JSONObject jsonResponse = new JSONObject (response);
                                                    boolean success = jsonResponse.getBoolean("success");

                                                    if (success) {

//                                                        requestDTOs.remove (position);
//                                                        requestAdapter.notifyDataSetChanged ();

                                                        Toast.makeText (context,"해당플레이어를 친구로 추가했습니다.",Toast.LENGTH_SHORT).show ();


                                                    } else {

                                                        Toast.makeText (getContext (),"친구추가에 실패했습니다.",Toast.LENGTH_LONG).show ();

                                                    } } catch (Exception e) {
                                                    e.printStackTrace (); } }};
                                        String del_id =  requestCall.friendrequestitem_name.getText ().toString ();
                                        FriendRequest_Dle friendRequest_dle = new FriendRequest_Dle (del_id,responseListener);
                                        RequestQueue queue = Volley.newRequestQueue (context);
                                        queue.add (friendRequest_dle);




                                    } else { }

                                } catch (Exception e) {
                                    e.printStackTrace (); } }};

                        FriendRequest_yes friendRequest_yes = new FriendRequest_yes (my_id,youer_id,responseListener);
                        RequestQueue queue = Volley.newRequestQueue (getActivity ());
                        queue.add (friendRequest_yes); }});

            requestCall.friendrequestitem_refusal_btn.setOnClickListener (new View.OnClickListener () {
                @Override
                public void onClick(View v) {
                  // 거절
                  // 거절할시 친구신성 테이블의 거절 당한 아이디 값 삭제
                    String del_id =  requestCall.friendrequestitem_name.getText ().toString ();
                    Toast.makeText (getContext (),del_id,Toast.LENGTH_SHORT).show ();
                        Response.Listener <String> responseListener = new Response.Listener <String> () {

                            @SuppressLint("ResourceType")
                            @Override
                            public void onResponse(String response) {
                                try {
                                    JSONObject jsonResponse = new JSONObject (response);
                                    boolean success = jsonResponse.getBoolean("success");

                                    if (success) {

                                        requestDTOs.remove (position);
                                        requestAdapter.notifyDataSetChanged ();

                                        Toast.makeText (context,"해당플레이어를 삭제 했습니다.",Toast.LENGTH_LONG).show ();


                                    } else {

                                        Toast.makeText (getContext (),"친구삭제에 실패했습니다.",Toast.LENGTH_LONG).show ();

                                    } } catch (Exception e) {
                                    e.printStackTrace (); } }};

                    FriendRequest_Dle friendRequest_dle = new FriendRequest_Dle (del_id,responseListener);
                        RequestQueue queue = Volley.newRequestQueue (context);
                        queue.add (friendRequest_dle);

                }
            });

        }



        @Override
        public int getItemCount() {
            return requestDTOs.size ();
        }
    }

    class RequestCall extends RecyclerView.ViewHolder {

        CircleImageView friendrequestitem_profile;
        TextView friendrequestitem_name;
        Button friendrequestitem_accept_btn, friendrequestitem_refusal_btn;

        public RequestCall(View view) {
            super (view);

            friendrequestitem_profile = (CircleImageView) view.findViewById (R.id.friendrequestitem_profile);
            friendrequestitem_name = (TextView) view.findViewById (R.id.friendrequestitem_name);
            friendrequestitem_accept_btn = (Button) view.findViewById (R.id.friendrequestitem_accept);
            friendrequestitem_refusal_btn = (Button) view.findViewById (R.id.friendrequestitem_refusal);


        }
    }

}
