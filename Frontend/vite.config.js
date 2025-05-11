import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";

export default defineConfig({
  plugins: [tailwindcss(), react()],
  server: {
    proxy: {
      // Proxy cho các API requests
      "/api": {
        target: "http://localhost:5078", // Địa chỉ backend
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, ""),
        secure: false, // Nếu dùng HTTPS local
      },
      // Thêm các proxy khác nếu cần
    },
  },
  resolve: {
    alias: {
      // Cấu hình alias để import dễ dàng hơn
      "@": "/src",
    },
  },
});
