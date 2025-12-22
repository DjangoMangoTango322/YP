package com.example.a123;

import java.io.Serializable;
import com.google.gson.annotations.SerializedName;

public class DataModels {



    public class User {

        public int Id;

        @SerializedName("First_Name")
        public String First_Name;

        @SerializedName("Last_Name")
        public String Last_Name;

        @SerializedName("Phone")
        public String Phone;

        @SerializedName("Password")
        public String Password;

        @SerializedName("Login")
        public String Login;

        // Обязательно пустой конструктор для Gson
        public User() {
        }
    }

    public static class Restaurant implements Serializable {
        public int Id;
        public String Name;
        public String Address;
        public int Capacity;
        public String Open_Time;
        public String Close_Time;
        public String Tematic;

        // Для UI
        public double rating = 4.5;
    }

    public static class Dish implements Serializable {
        public int Id;
        public String Name;
        public String Description;
        public double Price;
        public String Category;
        public boolean isVegetarian = false;
    }

    public static class Booking implements Serializable {
        public int Id;
        public int User_Id;
        public int Restaurant_Id;
        public String Booking_Date;
        public int Number_Of_Guests;
        public String Status;
        public String Created_At;

        public String restaurantName; // заполняется вручную
    }
}