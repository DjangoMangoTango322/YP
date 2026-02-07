package com.example.a123;

import android.content.Intent;
import android.content.SharedPreferences; // Добавлено для выхода
import android.os.Bundle;
import android.view.View;
import android.widget.ImageButton; // Добавлено для кнопки назад
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.AppCompatButton;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ProfileActivity extends AppCompatActivity {

    private TextView userNameTextView, userEmailTextView;
    private ProgressBar progressBar;
    private AppCompatButton editButton, logoutButton, btnMyAchievements; // Добавлены кнопки
    private ImageButton backButton; // Добавлена кнопка назад
    private RecyclerView bookingsRecyclerView;
    private int userId;

    private List<DataModels.Restaurant> allRestaurants = new ArrayList<>();
    private List<DataModels.Booking> filteredBookings = new ArrayList<>();
    private Adapters.BookingsAdapter bookingsAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profile);

        // Инициализация существующих элементов
        userNameTextView = findViewById(R.id.userNameTextView);
        userEmailTextView = findViewById(R.id.userEmailTextView);
        progressBar = findViewById(R.id.progressBar);
        editButton = findViewById(R.id.editButton);
        bookingsRecyclerView = findViewById(R.id.bookingsRecyclerView);

        // Инициализация НОВЫХ элементов из вашего XML
        backButton = findViewById(R.id.backButton);
        logoutButton = findViewById(R.id.logoutButton);
        btnMyAchievements = findViewById(R.id.btnMyAchievements);

        bookingsRecyclerView.setLayoutManager(new LinearLayoutManager(this));
        bookingsAdapter = new Adapters.BookingsAdapter(filteredBookings);
        bookingsRecyclerView.setAdapter(bookingsAdapter);

        // Получаем ID пользователя
        userId = getIntent().getIntExtra("userId", 0);
        if (userId == 0) {
            Toast.makeText(this, "Пользователь не авторизован", Toast.LENGTH_SHORT).show();
            finish();
            return;
        }

        loadUserData();

        // Логика кнопки "Изменить данные" (уже была)
        editButton.setOnClickListener(v -> {
            Intent intent = new Intent(ProfileActivity.this, EditProfileActivity.class);
            intent.putExtra("userId", userId);
            startActivityForResult(intent, 101);
        });

        // --- НОВАЯ ЛОГИКА ---

        // Кнопка достижений (переход на экран ачивок)
        btnMyAchievements.setOnClickListener(v -> {
            Intent intent = new Intent(ProfileActivity.this, AchievementsActivity.class);
            intent.putExtra("userId", userId);
            startActivity(intent);
        });

        // Кнопка назад (закрытие текущей активити)
        backButton.setOnClickListener(v -> finish());

        // Кнопка выхода (очистка сессии и переход на Login)
        logoutButton.setOnClickListener(v -> {
            SharedPreferences prefs = getSharedPreferences("app_prefs", MODE_PRIVATE);
            prefs.edit().clear().apply(); // Стираем сохраненного пользователя

            Intent intent = new Intent(ProfileActivity.this, LoginActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
            startActivity(intent);
            finish();
        });
    }

    // Метод загрузки данных пользователя (без изменений)
    private void loadUserData() {
        progressBar.setVisibility(View.VISIBLE);

        ApiClient.getApiService().getUserById(userId).enqueue(new Callback<UserRequest>() {
            @Override
            public void onResponse(Call<UserRequest> call, Response<UserRequest> response) {
                if (response.isSuccessful() && response.body() != null) {
                    UserRequest user = response.body();
                    userNameTextView.setText(user.first_Name + " " + user.last_Name);
                    userEmailTextView.setText(user.login);

                    loadRestaurants();
                } else {
                    progressBar.setVisibility(View.GONE);
                    Toast.makeText(ProfileActivity.this, "Ошибка загрузки данных пользователя", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<UserRequest> call, Throwable t) {
                progressBar.setVisibility(View.GONE);
                Toast.makeText(ProfileActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
            }
        });
    }

    // Метод загрузки ресторанов (без изменений)
    private void loadRestaurants() {
        ApiClient.getApiService().getRestaurants().enqueue(new Callback<List<DataModels.Restaurant>>() {
            @Override
            public void onResponse(Call<List<DataModels.Restaurant>> call, Response<List<DataModels.Restaurant>> response) {
                if (response.isSuccessful() && response.body() != null) {
                    allRestaurants.clear();
                    allRestaurants.addAll(response.body());

                    loadBookings();
                } else {
                    progressBar.setVisibility(View.GONE);
                    Toast.makeText(ProfileActivity.this, "Ошибка загрузки ресторанов", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<List<DataModels.Restaurant>> call, Throwable t) {
                progressBar.setVisibility(View.GONE);
                Toast.makeText(ProfileActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
            }
        });
    }

    // Метод загрузки бронирований (без изменений)
    private void loadBookings() {
        ApiClient.getApiService().getAllBookings().enqueue(new Callback<List<DataModels.Booking>>() {
            @Override
            public void onResponse(Call<List<DataModels.Booking>> call, Response<List<DataModels.Booking>> response) {
                progressBar.setVisibility(View.GONE);
                if (response.isSuccessful() && response.body() != null) {
                    filteredBookings.clear();

                    for (DataModels.Booking b : response.body()) {
                        if (b.user_Id == userId) {
                            for (DataModels.Restaurant r : allRestaurants) {
                                if (r.id == b.restaurant_Id) {
                                    b.restaurantName = r.name;
                                    b.restaurantAddress = r.address;
                                    b.restaurantCuisine = r.tematic;
                                    break;
                                }
                            }
                            filteredBookings.add(b);
                        }
                    }

                    bookingsAdapter.notifyDataSetChanged();

                    if (filteredBookings.isEmpty()) {
                        Toast.makeText(ProfileActivity.this, "У вас нет бронирований", Toast.LENGTH_SHORT).show();
                    }

                } else {
                    Toast.makeText(ProfileActivity.this, "Ошибка загрузки бронирований", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<List<DataModels.Booking>> call, Throwable t) {
                progressBar.setVisibility(View.GONE);
                Toast.makeText(ProfileActivity.this, "Ошибка сети", Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 101 && resultCode == RESULT_OK) {
            loadUserData();
        }
    }
}