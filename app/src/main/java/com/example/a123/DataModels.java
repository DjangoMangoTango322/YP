package com.example.a123;

import java.io.Serializable;
import java.util.Date;
import java.util.List;

public class DataModels {

    // ---------- USER ----------
    public static class User implements Serializable {
        public int id;
        public String firstName;
        public String lastName;
        public String login;
        public String phone;
        public String password;

        // Геттеры для совместимости
        public int getId() { return id; }
        public String getFirstName() { return firstName; }
        public String getLastName() { return lastName; }
        public String getLogin() { return login; }
        public String getPhone() { return phone; }
        public String getPassword() { return password; }
    }

    // ---------- RESTAURANT ----------
    public static class Restaurant implements Serializable {
        public int id;
        public String name;
        public String address;
        public String cuisineType;
        public double rating;
        public String phone;
        public String openingHours;
        public String imageUrl;
        public List<Dish> menu;
    }

    // ---------- DISH ----------
    public static class Dish implements Serializable {
        public int id;
        public String name;
        public String description;
        public double price;
        public String category;
        public boolean isVegetarian;
        public boolean isSpicy;
        public String imageUrl;
    }

    // ---------- BOOKING ----------
    public static class Booking implements Serializable {
        public int id;
        public int userId;
        public int restaurantId;
        public String restaurantName;
        public Date bookingDate;
        public int numberOfPeople;
        public String status;
        public String specialRequests;
    }
}
