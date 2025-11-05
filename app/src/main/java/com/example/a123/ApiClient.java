package com.example.a123;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;
import retrofit2.Call;
import retrofit2.http.*;
import java.util.List;
import java.util.Date;
public class ApiClient {
    private static final String BASE_URL = "https://your-api-url.com/";
    private static Retrofit retrofit = null;
    private static ApiService apiService = null;

    // API Service Interface
    public interface ApiService {
        @POST("api/auth/login")
        Call<ApiResponse<DataModels.User>> login(@Body LoginRequest loginRequest);

        @POST("api/auth/register")
        Call<ApiResponse<DataModels.User>> register(@Body RegisterRequest registerRequest);

        @GET("api/restaurants")
        Call<ApiResponse<List<DataModels.Restaurant>>> getRestaurants();

        @GET("api/restaurants/{id}")
        Call<ApiResponse<DataModels.Restaurant>> getRestaurantById(@Path("id") String id);

        @GET("api/restaurants/{id}/menu")
        Call<ApiResponse<List<DataModels.Dish>>> getRestaurantMenu(@Path("id") String restaurantId);

        @POST("api/bookings")
        Call<ApiResponse<DataModels.Booking>> createBooking(@Body BookingRequest bookingRequest);

        @GET("api/users/{userId}/bookings")
        Call<ApiResponse<List<DataModels.Booking>>> getUserBookings(@Path("userId") String userId);

        @DELETE("api/bookings/{id}")
        Call<ApiResponse<Void>> cancelBooking(@Path("id") String bookingId);
    }

    // API Response Model
    public static class ApiResponse<T> {
        public boolean success;
        public String message;
        public T data;
        public String error;

        public boolean isSuccess() { return success; }
        public String getMessage() { return message; }
        public T getData() { return data; }
        public String getError() { return error; }
    }

    // Request Models
    public static class LoginRequest {
        public String email, password;
        public LoginRequest(String email, String password) {
            this.email = email; this.password = password;
        }
    }

    public static class RegisterRequest {
        public String name, email, password, phone;
        public RegisterRequest(String name, String email, String password, String phone) {
            this.name = name; this.email = email; this.password = password; this.phone = phone;
        }
    }

    public static class BookingRequest {
        public String userId, restaurantId, specialRequests;
        public Date bookingDate;
        public int numberOfPeople;
        public BookingRequest(String userId, String restaurantId, Date bookingDate,
                              int numberOfPeople, String specialRequests) {
            this.userId = userId; this.restaurantId = restaurantId; this.bookingDate = bookingDate;
            this.numberOfPeople = numberOfPeople; this.specialRequests = specialRequests;
        }
    }

    // Main API Client methods
    public static Retrofit getClient() {
        if (retrofit == null) {
            retrofit = new Retrofit.Builder()
                    .baseUrl(BASE_URL)
                    .addConverterFactory(GsonConverterFactory.create())
                    .build();
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