package com.example.a123;

import android.os.Bundle;
import android.widget.Toast;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import java.util.List;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class AchievementsActivity extends AppCompatActivity {
    private RecyclerView recyclerView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_news); // Можно переиспользовать лейаут с RecyclerView, только id проверить
        // Или лучше создайте activity_achievements.xml с RecyclerView внутри

        recyclerView = findViewById(R.id.recyclerViewNews); // Если переиспользуем layout
        recyclerView.setLayoutManager(new LinearLayoutManager(this));

        int userId = getSharedPreferences("app_prefs", MODE_PRIVATE).getInt("user_id", 0);
        loadAchievements(userId);
    }

    private void loadAchievements(int userId) {
        ApiClient.getApiService().getUserAchievements(userId).enqueue(new Callback<List<DataModels.AchievementDto>>() {
            @Override
            public void onResponse(Call<List<DataModels.AchievementDto>> call, Response<List<DataModels.AchievementDto>> response) {
                if(response.isSuccessful() && response.body() != null) {
                    recyclerView.setAdapter(new AchievementsAdapter(response.body()));
                }
            }
            @Override
            public void onFailure(Call<List<DataModels.AchievementDto>> call, Throwable t) {
                Toast.makeText(AchievementsActivity.this, "Ошибка", Toast.LENGTH_SHORT).show();
            }
        });
    }
}