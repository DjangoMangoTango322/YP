package com.example.a123;

import android.content.Context;
import android.content.SharedPreferences;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.widget.Toast;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

public class Utils {// Network Utilities
    // Network Utilities
    public static class Network {
        public static boolean isNetworkAvailable(Context context) {
            ConnectivityManager cm = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
            if (cm != null) {
                NetworkInfo activeNetwork = cm.getActiveNetworkInfo();
                return activeNetwork != null && activeNetwork.isConnected();
            }
            return false;
        }
    }

    // SharedPreferences Helper
    public static class Prefs {
        private static final String PREFS_NAME = "RestaurantAppPrefs";
        private static final String KEY_USER_ID = "user_id";
        private static final String KEY_USER_NAME = "user_name";
        private static final String KEY_USER_EMAIL = "user_email";
        private static final String KEY_IS_LOGGED_IN = "is_logged_in";
        private static final String KEY_AUTH_TOKEN = "auth_token";

        private SharedPreferences prefs;

        public Prefs(Context context) {
            prefs = context.getSharedPreferences(PREFS_NAME, Context.MODE_PRIVATE);
        }

        public void saveUserData(String userId, String name, String email, String token) {
            SharedPreferences.Editor editor = prefs.edit();
            editor.putString(KEY_USER_ID, userId);
            editor.putString(KEY_USER_NAME, name);
            editor.putString(KEY_USER_EMAIL, email);
            editor.putString(KEY_AUTH_TOKEN, token);
            editor.putBoolean(KEY_IS_LOGGED_IN, true);
            editor.apply();
        }

        public void clearUserData() {
            SharedPreferences.Editor editor = prefs.edit();
            editor.remove(KEY_USER_ID);
            editor.remove(KEY_USER_NAME);
            editor.remove(KEY_USER_EMAIL);
            editor.remove(KEY_AUTH_TOKEN);
            editor.putBoolean(KEY_IS_LOGGED_IN, false);
            editor.apply();
        }

        public boolean isLoggedIn() {
            return prefs.getBoolean(KEY_IS_LOGGED_IN, false);
        }

        public String getUserId() {
            return prefs.getString(KEY_USER_ID, null);
        }

        public String getUserName() {
            return prefs.getString(KEY_USER_NAME, null);
        }

        public String getUserEmail() {
            return prefs.getString(KEY_USER_EMAIL, null);
        }

        public String getAuthToken() {
            return prefs.getString(KEY_AUTH_TOKEN, null);
        }
    }

    // UI Utilities
    public static class UI {
        public static void showToast(Context context, String message) {
            Toast.makeText(context, message, Toast.LENGTH_SHORT).show();
        }

        public static void showLongToast(Context context, String message) {
            Toast.makeText(context, message, Toast.LENGTH_LONG).show();
        }
    }

    // Date Utilities
    public static class DateUtils {
        public static String formatBookingDate(Date date) {
            return new SimpleDateFormat("dd MMMM yyyy 'в' HH:mm", new Locale("ru")).format(date);
        }

        public static String formatDateTime(Date date) {
            return new SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault()).format(date);
        }

        public static String getCurrentDate() {
            return new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault()).format(new Date());
        }

        public static String getCurrentTime() {
            return new SimpleDateFormat("HH:mm", Locale.getDefault()).format(new Date());
        }
    }

    // Booking Utilities
    public static class BookingHelper {
        public static String getStatusText(String status) {
            if (status == null) return "";

            switch (status.toLowerCase()) {
                case "confirmed":
                    return "Подтверждено";
                case "pending":
                    return "Ожидание подтверждения";
                case "completed":
                    return "Завершено";
                case "cancelled":
                    return "Отменено";
                default:
                    return status;
            }
        }

        public static boolean isValidPeopleCount(int count) {
            return count > 0 && count <= 20;
        }
    }
}