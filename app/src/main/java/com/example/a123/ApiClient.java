package com.example.a123;

import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import retrofit2.Call;
import retrofit2.http.*;
import java.util.List;
import java.util.Date;
public class ApiClient {
    @POST("api/auth/login") Call<ApiResponse<User>> login(@Body LoginRequest loginRequest);
    @POST("api/auth/register") Call<ApiResponse<User>> register(@Body RegisterRequest registerRequest);
    @GET("api/restaurants") Call<ApiResponse<List<Restaurant>>> getRestaurants();
    @GET("api/restaurants/{id}") Call<ApiResponse<Restaurant>> getRestaurantById(@Path("id") String id);
    @GET("api/restaurants/{id}/menu") Call<ApiResponse<List<Dish>>> getRestaurantMenu(@Path("id") String restaurantId);
    @POST("api/bookings") Call<ApiResponse<Booking>> createBooking(@Body BookingRequest bookingRequest);
    @GET("api/users/{userId}/bookings") Call<ApiResponse<List<Booking>>> getUserBookings(@Path("userId") String userId);
    @DELETE("api/bookings/{id}") Call<ApiResponse<Void>> cancelBooking(@Path("id") String bookingId);
}

// API Models
class ApiResponse<T> {
    boolean success; String message; T data; String error;
    public boolean isSuccess() { return success; }
    public String getMessage() { return message; }
    public T getData() { return data; }
}

class LoginRequest {
    String email, password;
    public LoginRequest(String email, String password) { this.email = email; this.password = password; }
}

class RegisterRequest {
    String name, email, password, phone;
    public RegisterRequest(String name, String email, String password, String phone) {
        this.name = name; this.email = email; this.password = password; this.phone = phone;
    }
}

class BookingRequest {
    String userId, restaurantId, specialRequests;
    Date bookingDate; int numberOfPeople;
    public BookingRequest(String userId, String restaurantId, Date bookingDate, int numberOfPeople, String specialRequests) {
        this.userId = userId; this.restaurantId = restaurantId; this.bookingDate = bookingDate;
        this.numberOfPeople = numberOfPeople; this.specialRequests = specialRequests;
    }
}

// Main API Client
public class ApiClient {
    private static final String BASE_URL = "https://your-api-url.com/";
    private static Retrofit retrofit = null;
    private static ApiService apiService = null;

    public static Retrofit getClient() {
        if (retrofit == null) {
            retrofit = new Retrofit.Builder().baseUrl(BASE_URL)
                    .addConverterFactory(GsonConverterFactory.create()).build();
        }
        return retrofit;
    }

    public static ApiService getApiService() {
        if (apiService == null) {
            apiService = getClient().create(ApiService.class);
        }
        return apiService;
    }
}