package com.example.a123;

import com.example.a123.DataModels;
import java.util.List;
import retrofit2.Call;
import retrofit2.http.*;

public interface ApiService {

    // ---------- USER ----------
    @FormUrlEncoded
    @POST("api/UserController/AddUser")
    Call<Void> register(
            @Field("FirstName") String firstName,
            @Field("LastName") String lastName,
            @Field("Login") String login,
            @Field("Phone") String phone,
            @Field("Password") String password
    );

    @FormUrlEncoded
    @POST("api/UserController/LoginUser")
    Call<ApiClient.LoginResult> login(
            @Field("login") String login,
            @Field("password") String password
    );

    @GET("api/UserController/GetUserById/{id}")
    Call<DataModels.User> getUserById(@Path("id") int id);

    @FormUrlEncoded
    @PUT("api/UserController/UpdateUser")
    Call<Void> updateUser(
            @Field("Id") int id,
            @Field("FirstName") String firstName,
            @Field("LastName") String lastName,
            @Field("Login") String login,
            @Field("Phone") String phone,
            @Field("Password") String password
    );

    // ---------- RESTAURANTS ----------
    @GET("api/RestaurantController/GetAllRestaurants")
    Call<List<DataModels.Restaurant>> getRestaurants();

    @GET("api/RestaurantController/GetRestaurantById/{id}")
    Call<DataModels.Restaurant> getRestaurantById(@Path("id") int id);

    // ---------- BOOKINGS ----------
    @FormUrlEncoded
    @POST("api/BookingController/CreateBooking")
    Call<Void> createBooking(
            @Field("UserId") int userId,
            @Field("RestaurantId") int restaurantId,
            @Field("BookingDate") String bookingDate,
            @Field("NumberOfPeople") int numberOfPeople,
            @Field("Status") String status,
            @Field("SpecialRequests") String specialRequests
    );

    @GET("api/BookingController/GetBookingsByUserId/{userId}")
    Call<List<DataModels.Booking>> getBookingsByUserId(@Path("userId") int userId);

    @DELETE("api/BookingController/DeleteBooking/{userId}")
    Call<Void> cancelBooking(@Path("userId") int userId);
}
