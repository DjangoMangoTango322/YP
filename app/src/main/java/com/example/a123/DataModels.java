package com.example.a123;

import java.util.List;
import java.util.Date;

public class DataModels {   public static class User {
    public String id, name, email, phone, role, createdAt;
    public User(String id, String name, String email, String phone, String role) {
        this.id = id; this.name = name; this.email = email; this.phone = phone; this.role = role;
    }
}

    // Restaurant Model
    public static class Restaurant {
        public String id, name, address, cuisineType, phone, email, openingHours, imageUrl;
        public double rating;
        public List<Dish> menu;

        public Restaurant(String id, String name, String address, String cuisineType, double rating,
                          String phone, String openingHours, String imageUrl) {
            this.id = id; this.name = name; this.address = address; this.cuisineType = cuisineType;
            this.rating = rating; this.phone = phone; this.openingHours = openingHours; this.imageUrl = imageUrl;
        }
    }

    // Dish Model
    public static class Dish {
        public String id, name, description, category, imageUrl, restaurantId;
        public double price;
        public boolean isVegetarian, isSpicy;

        public Dish(String id, String name, String description, double price, String category,
                    boolean isVegetarian, boolean isSpicy, String restaurantId) {
            this.id = id; this.name = name; this.description = description; this.price = price;
            this.category = category; this.isVegetarian = isVegetarian; this.isSpicy = isSpicy;
            this.restaurantId = restaurantId;
        }
    }

    // Booking Model
    public static class Booking {
        public String id, userId, restaurantId, restaurantName, status, specialRequests;
        public Date bookingDate, createdAt;
        public int numberOfPeople;

        public Booking(String id, String userId, String restaurantId, String restaurantName,
                       Date bookingDate, int numberOfPeople, String status, String specialRequests) {
            this.id = id; this.userId = userId; this.restaurantId = restaurantId; this.restaurantName = restaurantName;
            this.bookingDate = bookingDate; this.numberOfPeople = numberOfPeople;
            this.status = status; this.specialRequests = specialRequests;
        }
    }
}