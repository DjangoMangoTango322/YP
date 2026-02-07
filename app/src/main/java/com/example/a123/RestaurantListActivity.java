package com.example.a123;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button; // Добавлено
import android.widget.ImageButton;
import android.widget.ProgressBar;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.a123.ApiClient;
import com.example.a123.DataModels;
import com.example.a123.R;
import com.example.a123.RestaurantDetailActivity;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class RestaurantListActivity extends AppCompatActivity {

    private RecyclerView restaurantRecyclerView;
    private ProgressBar progressBar;
    private Button btnGoToNews; // Кнопка уже была в объявлении, теперь задействуем
    private int userId;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_restaurant_list);

        restaurantRecyclerView = findViewById(R.id.restaurantRecyclerView);
        progressBar = findViewById(R.id.progressBar);
        btnGoToNews = findViewById(R.id.btnGoToNews); // Инициализация кнопки из XML

        restaurantRecyclerView.setLayoutManager(new LinearLayoutManager(this));

        userId = getIntent().getIntExtra("userId", 0);

        // Обработка перехода на новости
        btnGoToNews.setOnClickListener(v -> {
            Intent intent = new Intent(RestaurantListActivity.this, NewsActivity.class);
            startActivity(intent);
        });

        ImageButton backButton = findViewById(R.id.backButton);
        ImageButton profileButton = findViewById(R.id.profileButton);

        backButton.setOnClickListener(v -> {
            if (profileButton.getVisibility() == View.GONE) {
                profileButton.setVisibility(View.VISIBLE);
            } else {
                profileButton.setVisibility(View.GONE);
            }
        });

        profileButton.setOnClickListener(v -> {
            int userIdPref = getSharedPreferences("app_prefs", MODE_PRIVATE).getInt("user_id", 0);
            if (userIdPref == 0) {
                Toast.makeText(this, "Пользователь не авторизован", Toast.LENGTH_SHORT).show();
                return;
            }
            Intent intent = new Intent(this, ProfileActivity.class);
            intent.putExtra("userId", userIdPref);
            startActivity(intent);
        });

        loadRestaurants();
    }

    private void loadRestaurants() {
        progressBar.setVisibility(View.VISIBLE);

        ApiClient.getApiService().getRestaurants().enqueue(new Callback<List<DataModels.Restaurant>>() {
            @Override
            public void onResponse(Call<List<DataModels.Restaurant>> call, Response<List<DataModels.Restaurant>> response) {
                progressBar.setVisibility(View.GONE);
                if (response.isSuccessful() && response.body() != null) {
                    List<DataModels.Restaurant> list = response.body();

                    Adapters.RestaurantAdapter adapter = new Adapters.RestaurantAdapter(list, restaurant -> {
                        Intent intent = new Intent(RestaurantListActivity.this, RestaurantDetailActivity.class);
                        intent.putExtra("restaurantId", restaurant.id);
                        intent.putExtra("userId", userId);
                        startActivity(intent);
                    });

                    restaurantRecyclerView.setAdapter(adapter);

                } else {
                    Toast.makeText(RestaurantListActivity.this, "Ошибка загрузки ресторанов", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<List<DataModels.Restaurant>> call, Throwable t) {
                progressBar.setVisibility(View.GONE);
                Toast.makeText(RestaurantListActivity.this, "Ошибка сети: " + t.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });
    }
}