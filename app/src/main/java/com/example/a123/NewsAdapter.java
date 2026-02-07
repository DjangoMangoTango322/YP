package com.example.a123;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import com.bumptech.glide.Glide;
import com.example.a123.DataModels.News;
import java.util.List;

public class NewsAdapter extends RecyclerView.Adapter<NewsAdapter.ViewHolder> {
    private List<News> newsList;

    public NewsAdapter(List<News> newsList) {
        this.newsList = newsList;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_news, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        News news = newsList.get(position);
        holder.title.setText(news.getTitle());
        holder.desc.setText(news.getDescription());

        // Загрузка изображения по URL
        Glide.with(holder.itemView.getContext())
                .load(news.getImageUrl())
                .placeholder(R.drawable.ic_launcher_background) // заглушка пока грузится
                .into(holder.image);
    }

    @Override
    public int getItemCount() {
        return newsList.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView title, desc;
        ImageView image;

        public ViewHolder(View itemView) {
            super(itemView);
            title = itemView.findViewById(R.id.newsTitle);
            desc = itemView.findViewById(R.id.newsDesc);
            image = itemView.findViewById(R.id.newsImage);
        }
    }
}