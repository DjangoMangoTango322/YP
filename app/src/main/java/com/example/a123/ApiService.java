package com.example.a123;

import com.example.a123.DataModels.Booking;
import com.example.a123.DataModels.Dish;
import com.example.a123.DataModels.Restaurant;
import com.example.a123.DataModels.User;
import com.example.a123.DataModels.News;
import java.util.List;
import retrofit2.Call;
import retrofit2.http.GET;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface ApiService {

    // Регистрация
    @POST("api/UserController/AddUser")
    Call<Void> register(@Body UserRequest user);
    // Безопасный логин
    @GET("api/UserController/GetAllUsers")
    Call<List<UserIdResponse>> getAllUsers();

    @GET("api/UserController/GetUserById/{id}")
    Call<UserRequest> getUserById(@Path("id") int id);

    // Обновление пользователя — НОВЫЙ МЕТОД
    @POST("api/UserController/UpdateUser")
    Call<Void> updateUser(@Body UserRequest user);

    // Остальные методы (рестораны, меню, брони и т.д.)
    @GET("api/RestaurantController/GetAllRestaurants")
    Call<List<Restaurant>> getRestaurants();

    @GET("api/RestaurantController/GetRestaurantById/{id}")
    Call<DataModels.Restaurant> getRestaurantById(@Path("id") int id);
    @GET("api/RestaurantDishController/GetDishesByRestaurantId/{restaurantId}")
    Call<List<DataModels.Dish>> getRestaurantMenu(@Path("restaurantId") int id);

    @POST("api/BookingController/CreateBooking")
    Call<Void> createBooking(@Body BookingRequest booking);
    @GET("api/BookingController/GetAllBookings")
    Call<List<Booking>> getAllBookings();

    @DELETE("api/BookingController/DeleteBooking/{userId}/{restaurantId}")
    Call<Void> cancelBooking(
            @Path("userId") int userId,
            @Path("restaurantId") int restaurantId
    );
    @GET("api/News/GetAllNews")
    Call<List<News>> getAllNews();
}