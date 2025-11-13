package com.example.a123;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.*;
import androidx.annotation.NonNull;
import androidx.cardview.widget.CardView;
import androidx.recyclerview.widget.RecyclerView;

import java.util.List;
import java.util.Locale;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class Adapters {

    // RestaurantAdapter (RecyclerView)
    public static class RestaurantAdapter extends RecyclerView.Adapter<RestaurantAdapter.ViewHolder> {

        public interface OnRestaurantClickListener {
            void onRestaurantClick(DataModels.Restaurant restaurant);
        }

        private List<DataModels.Restaurant> restaurantList;
        private OnRestaurantClickListener listener;

        public RestaurantAdapter(List<DataModels.Restaurant> restaurantList, OnRestaurantClickListener listener) {
            this.restaurantList = restaurantList;
            this.listener = listener;
        }

        @NonNull @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_restaurant, parent, false);
            return new ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            DataModels.Restaurant r = restaurantList.get(position);
            holder.nameTextView.setText(r.name);
            holder.addressTextView.setText(r.address);
            holder.ratingTextView.setText(String.format(Locale.getDefault(), "★ %.1f", r.rating));
            holder.cuisineTextView.setText(r.cuisineType != null ? r.cuisineType : "");
            holder.cardView.setOnClickListener(v -> { if (listener != null) listener.onRestaurantClick(r); });
        }

        @Override public int getItemCount() { return restaurantList != null ? restaurantList.size() : 0; }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            CardView cardView;
            TextView nameTextView, addressTextView, ratingTextView, cuisineTextView;
            public ViewHolder(@NonNull View itemView) {
                super(itemView);
                cardView = itemView.findViewById(R.id.cardView);
                nameTextView = itemView.findViewById(R.id.restaurantNameTextView);
                addressTextView = itemView.findViewById(R.id.addressTextView);
                ratingTextView = itemView.findViewById(R.id.ratingTextView);
                cuisineTextView = itemView.findViewById(R.id.cuisineTextView);
            }
        }
    }

    // MenuAdapter
    public static class MenuAdapter extends RecyclerView.Adapter<MenuAdapter.ViewHolder> {
        private List<DataModels.Dish> menuList;
        public MenuAdapter(List<DataModels.Dish> menuList) { this.menuList = menuList; }

        @NonNull @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_menu, parent, false);
            return new ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            DataModels.Dish d = menuList.get(position);
            holder.dishNameTextView.setText(d.name);
            holder.dishDescriptionTextView.setText(d.description);
            holder.dishPriceTextView.setText(String.format(Locale.getDefault(), "%.0f ₽", d.price));
            holder.dishCategoryTextView.setText(d.category != null ? d.category : "");
            holder.vegetarianBadge.setVisibility(d.isVegetarian ? View.VISIBLE : View.GONE);
        }

        @Override public int getItemCount() { return menuList != null ? menuList.size() : 0; }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            TextView dishNameTextView, dishDescriptionTextView, dishPriceTextView, dishCategoryTextView, vegetarianBadge;
            public ViewHolder(@NonNull View itemView) {
                super(itemView);
                dishNameTextView = itemView.findViewById(R.id.dishNameTextView);
                dishDescriptionTextView = itemView.findViewById(R.id.dishDescriptionTextView);
                dishPriceTextView = itemView.findViewById(R.id.dishPriceTextView);
                dishCategoryTextView = itemView.findViewById(R.id.dishCategoryTextView);
                vegetarianBadge = itemView.findViewById(R.id.vegetarianBadge);
            }
        }
    }

    // BookingsAdapter (kept simple — uses item_booking layout)
    // BookingsAdapter (исправлено: безопасное обращение к position)
    public static class BookingsAdapter extends RecyclerView.Adapter<BookingsAdapter.ViewHolder> {
        private List<DataModels.Booking> bookingList;

        public BookingsAdapter(List<DataModels.Booking> bookingList) {
            this.bookingList = bookingList;
        }

        @NonNull
        @Override
        public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_booking, parent, false);
            return new ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
            DataModels.Booking b = bookingList.get(position);
            holder.restaurantNameTextView.setText(b.restaurantName != null ? b.restaurantName : "");
            holder.bookingDateTextView.setText(b.bookingDate != null ? Utils.DateUtils.formatBookingDate(b.bookingDate) : "");
            holder.peopleCountTextView.setText((b.numberOfPeople) + " чел.");
            holder.statusTextView.setText(Utils.BookingHelper.getStatusText(b.status));
            holder.cancelButton.setVisibility(
                    ("completed".equalsIgnoreCase(b.status) || "cancelled".equalsIgnoreCase(b.status))
                            ? View.GONE : View.VISIBLE
            );

            holder.cancelButton.setOnClickListener(v -> {
                int currentPosition = holder.getAdapterPosition();
                if (currentPosition == RecyclerView.NO_POSITION) return;

                DataModels.Booking booking = bookingList.get(currentPosition);

                ApiClient.getApiService().cancelBooking(booking.id).enqueue(new Callback<Void>() {
                    @Override
                    public void onResponse(Call<Void> call, Response<Void> response) {
                        if (response.isSuccessful()) {
                            booking.status = "cancelled";
                            notifyItemChanged(currentPosition);
                        } else {
                            Utils.UI.showToast(v.getContext(), "Ошибка отмены бронирования");
                        }
                    }

                    @Override
                    public void onFailure(Call<Void> call, Throwable t) {
                        Utils.UI.showToast(v.getContext(), "Ошибка соединения");
                    }
                });
            });
        }

        @Override
        public int getItemCount() {
            return bookingList != null ? bookingList.size() : 0;
        }

        public static class ViewHolder extends RecyclerView.ViewHolder {
            TextView restaurantNameTextView, bookingDateTextView, peopleCountTextView, statusTextView;
            Button cancelButton;

            public ViewHolder(@NonNull View itemView) {
                super(itemView);
                restaurantNameTextView = itemView.findViewById(R.id.restaurantNameTextView);
                bookingDateTextView = itemView.findViewById(R.id.bookingDateTextView);
                peopleCountTextView = itemView.findViewById(R.id.peopleCountTextView);
                statusTextView = itemView.findViewById(R.id.statusTextView);
                cancelButton = itemView.findViewById(R.id.cancelButton);
            }
        }
    }
}

