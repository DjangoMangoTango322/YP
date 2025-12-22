package com.example.a123;

import com.example.a123.DataModels.Booking;
import com.example.a123.DataModels.Dish;
import com.example.a123.DataModels.Restaurant;
import com.example.a123.DataModels.User;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.DELETE;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface ApiService {

    // Регистрация
    @FormUrlEncoded
    @POST("api/UserController/ReginU")
    Call<Void> register(
            @Field("First_Name") String firstName,
            @Field("Last_Name") String lastName,
            @Field("Login") String login,
            @Field("Phone") String phone,
            @Field("Password") String password
    );

    // Безопасный логин
    @GET("api/UserController/Login")
    Call<UserIdResponse> loginUser(
            @Query("login") String login,
            @Query("password") String password
    );

    // Получение пользователя по ID — НОВЫЙ МЕТОД
    @GET("api/UserController/GetUserById")
    Call<User> getUserById(@Query("id") int id);

    // Обновление пользователя — НОВЫЙ МЕТОД
    @FormUrlEncoded
    @POST("api/UserController/UpdateUser")
    Call<Void> updateUser(
            @Field("Id") int id,
            @Field("First_Name") String firstName,
            @Field("Last_Name") String lastName,
            @Field("Login") String login,
            @Field("Phone") String phone,
            @Field("Password") String password
    );

    // Остальные методы (рестораны, меню, брони и т.д.)
    @GET("api/RestaurantController/GetAllRestaurants")
    Call<List<Restaurant>> getRestaurants();

    @GET("api/RestaurantController/GetRestaurantById")
    Call<Restaurant> getRestaurantById(@Query("id") int id);

    @GET("api/RestaurantDishController/GetDishesByRestaurantId")
    Call<List<Dish>> getRestaurantMenu(@Query("restaurantId") int restaurantId);

    @FormUrlEncoded
    @POST("api/BookingController/CreateBooking")
    Call<Void> createBooking(
            @Field("User_Id") int userId,
            @Field("Restaurant_Id") int restaurantId,
            @Field("Booking_Date") String date,
            @Field("Number_Of_Guests") int guests,
            @Field("Status") String status,
            @Field("Created_At") String createdAt
    );

    @GET("api/BookingController/GetAllBookings")
    Call<List<Booking>> getAllBookings();

    @DELETE("api/BookingController/DeleteBooking/{userId}/{restaurantId}")
    Call<Void> cancelBooking(
            @Path("userId") int userId,
            @Path("restaurantId") int restaurantId
    );
}