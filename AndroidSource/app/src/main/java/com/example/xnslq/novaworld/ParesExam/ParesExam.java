package com.example.xnslq.novaworld.ParesExam;

public class ParesExam  {

   public static void main(String[] args) {

       String numStr = "54";

       // 스트링값을 int형 값으로 바꾸는 방법
          int numInt = Integer.parseInt (numStr);
          System.out.print (numInt);

          // int 형을 스트링으로
       String numStr2 = String.valueOf (numInt);
       System.out.print (numStr);
   }
}
