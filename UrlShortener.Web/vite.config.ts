import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';

export default defineConfig({
  plugins: [vue()],
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:7294', // URL da sua API backend
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
