package com.example.a123;

import java.io.Serializable;
import com.google.gson.annotations.SerializedName;
import java.util.Date;

public class DataModels {



    public class User {
        @SerializedName("id")
        public int Id;

        @SerializedName("first_Name")
        public String First_Name;

        @SerializedName("last_Name")
        public String Last_Name;

        @SerializedName("login")
        public String Login;

        @SerializedName("phone")
        public String Phone;

        @SerializedName("password")
        public String Password;
    }

    public static class Restaurant implements Serializable {
        public int id;
        public String name;
        public String address;
        public int capacity;
        public String open_Time;
        public String close_Time;
        public String tematic;

        // Для UI
        public double rating = 4.5;
    }
    public class News {
        private int id;
        private String title;
        private String description;
        private String imageUrl;
        private Date publishDate;

        // Геттеры
        public int getId() { return id; }
        public String getTitle() { return title; }
        public String getDescription() { return description; }
        public String getImageUrl() { return imageUrl; }
        public Date getPublishDate() { return publishDate; }
    }
    public static class Dish implements Serializable {
        public int id;
        public String name;
        public String description;
        public double price;
        public String category;
        public boolean isVegetarian;
    }


    public class Booking {
        public int id;
        public int user_Id;
        public int restaurant_Id;
        public String booking_Date;
        public int number_Of_Guests;
        public String status;
        public String created_At;

        public String restaurantName;
        public String restaurantAddress;
        public String restaurantCuisine;
    }
}