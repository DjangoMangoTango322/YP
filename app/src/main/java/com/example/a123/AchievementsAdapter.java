package com.example.a123;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import java.util.List;

public class AchievementsAdapter extends RecyclerView.Adapter<AchievementsAdapter.ViewHolder> {

    private final List<DataModels.AchievementDto> achievementList;

    public AchievementsAdapter(List<DataModels.AchievementDto> achievementList) {
        this.achievementList = achievementList;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        // Используем созданный ранее макет item_achievement
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.item_achievement, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        DataModels.AchievementDto achievement = achievementList.get(position);

        holder.textName.setText(achievement.name);
        holder.textDescription.setText(achievement.description);

        // Форматируем дату, если она пришла из API (убираем лишние символы времени T)
        if (achievement.unlockedAt != null) {
            String cleanDate = achievement.unlockedAt.split("T")[0];
            holder.textDate.setText("Получено: " + cleanDate);
        }
    }

    @Override
    public int getItemCount() {
        return achievementList != null ? achievementList.size() : 0;
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView textName;
        TextView textDescription;
        TextView textDate;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            // Эти ID должны совпадать с теми, что в res/layout/item_achievement.xml
            textName = itemView.findViewById(R.id.achName);
            textDescription = itemView.findViewById(R.id.achDesc);
            textDate = itemView.findViewById(R.id.achDate);
        }
    }
}