<template>
  <div class="container">
    <div class="card">
      <h1>🔗 URL Shortener</h1>
      <p class="subtitle">Transform your long URLs into short, shareable links</p>

      <form @submit.prevent="handleSubmit">
        <div class="input-group">
          <input type="url"
                 placeholder="https://www.yourlongurl.com"
                 v-model="longUrl"
                 required />
          <button type="submit" :disabled="isLoading">
            <span v-if="!isLoading">Shorten</span>
            <span v-else class="loader"></span>
          </button>
        </div>
      </form>

      <transition name="fade">
        <div class="result" v-if="shortUrl">
          <p>Your short URL:</p>
          <a :href="shortUrl" target="_blank">{{ shortUrl }}</a>
          <button class="copy-btn" @click="copyToClipboard">📋 Copy</button>
        </div>
      </transition>

      <transition name="fade">
        <p class="error" v-if="error">{{ error }}</p>
      </transition>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { shortenUrl } from './services/urlShortenerService'

  const longUrl = ref('');
  const shortUrl = ref('');
  const error = ref('');
  const isLoading = ref(false);

  onMounted(() => {
    document.documentElement.style.height = '100%';
    document.documentElement.style.overflow = 'hidden';
    document.body.style.height = '100%';
    document.body.style.overflow = 'hidden';
    document.body.style.margin = '0';
    document.body.style.padding = '0';
  });

  async function handleSubmit() {
    error.value = '';
    shortUrl.value = '';
    isLoading.value = true;

    try {
      const response = await shortenUrl(longUrl.value);
      shortUrl.value = response;
    } catch (err: any) {
      error.value = err.message || 'Error while shortening the URL.';
    } finally {
      isLoading.value = false;
    }
  }

  function copyToClipboard() {
    navigator.clipboard.writeText(shortUrl.value);
  }
</script>

<style scoped>
  .container {
    height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    overflow: hidden;
  }

  .card {
    background: #ffffff;
    border-radius: 16px;
    box-shadow: 0 10px 40px rgba(0, 0, 0, 0.2);
    padding: 2.5rem;
    width: 90%;
    max-width: 500px;
    text-align: center;
    animation: slideUp 0.5s ease-out;
  }

  @keyframes slideUp {
    from {
      opacity: 0;
      transform: translateY(30px);
    }

    to {
      opacity: 1;
      transform: translateY(0);
    }
  }

  h1 {
    color: #333;
    font-size: 2rem;
    margin-bottom: 0.5rem;
    font-weight: 700;
  }

  .subtitle {
    color: #666;
    font-size: 0.95rem;
    margin-bottom: 2rem;
    font-weight: 500;
  }

  .input-group {
    display: flex;
    flex-direction: column;
    gap: 0.75rem;
  }

  input {
    padding: 1rem;
    font-size: 1rem;
    border: 2px solid #e0e0e0;
    border-radius: 10px;
    outline: none;
    transition: border-color 0.3s ease, box-shadow 0.3s ease;
    width: 100%;
    box-sizing: border-box;
    font-family: 'Poppins', sans-serif;
    font-weight: 500;
  }

    input::placeholder {
      color: #bbb;
    }

    input:focus {
      border-color: #667eea;
      box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.2);
    }

  button {
    padding: 1rem 1.5rem;
    font-size: 1rem;
    font-weight: 600;
    color: #fff;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    border: none;
    border-radius: 10px;
    cursor: pointer;
    transition: transform 0.2s ease, box-shadow 0.2s ease;
    min-height: 52px;
    font-family: 'Poppins', sans-serif;
  }

    button:hover:not(:disabled) {
      transform: translateY(-2px);
      box-shadow: 0 5px 20px rgba(102, 126, 234, 0.4);
    }

    button:active:not(:disabled) {
      transform: translateY(0);
    }

    button:disabled {
      opacity: 0.7;
      cursor: not-allowed;
    }

  .loader {
    display: inline-block;
    width: 20px;
    height: 20px;
    border: 3px solid rgba(255, 255, 255, 0.3);
    border-top-color: #fff;
    border-radius: 50%;
    animation: spin 0.8s linear infinite;
  }

  @keyframes spin {
    to {
      transform: rotate(360deg);
    }
  }

  .result {
    margin-top: 2rem;
    padding: 1.5rem;
    background: linear-gradient(135deg, #f0f4ff 0%, #f9f0ff 100%);
    border-radius: 12px;
    border: 1px solid #e0e0ff;
  }

    .result p {
      color: #666;
      margin-bottom: 0.75rem;
      font-size: 0.9rem;
      font-weight: 500;
    }

    .result a {
      color: #667eea;
      font-weight: 600;
      font-size: 1.1rem;
      word-break: break-all;
      text-decoration: none;
      transition: color 0.2s ease;
      display: block;
      margin-bottom: 1.5rem;
    }

      .result a:hover {
        color: #764ba2;
        text-decoration: underline;
      }

  .copy-btn {
    margin: 0;
    padding: 0.6rem 1.2rem;
    font-size: 0.85rem;
    background: #fff;
    color: #667eea;
    border: 2px solid #667eea;
    min-height: auto;
  }

    .copy-btn:hover {
      background: #667eea;
      color: #fff;
      transform: translateY(-2px);
      box-shadow: 0 5px 15px rgba(102, 126, 234, 0.3);
    }

  .error {
    margin-top: 1.5rem;
    padding: 1rem;
    color: #d32f2f;
    background-color: #ffebee;
    border-radius: 10px;
    border: 1px solid #ffcdd2;
    font-weight: 500;
  }

  .fade-enter-active,
  .fade-leave-active {
    transition: opacity 0.3s ease, transform 0.3s ease;
  }

  .fade-enter-from,
  .fade-leave-to {
    opacity: 0;
    transform: translateY(-10px);
  }

  @media (min-width: 480px) {
    .input-group {
      flex-direction: row;
    }

    input {
      flex: 1;
    }

    button {
      flex-shrink: 0;
    }
  }

  @media (max-width: 480px) {
    .card {
      padding: 1.5rem;
    }

    h1 {
      font-size: 1.5rem;
    }

    .subtitle {
      font-size: 0.85rem;
    }

    input {
      padding: 0.85rem;
      font-size: 0.95rem;
    }

    button {
      min-height: 48px;
      padding: 0.85rem 1.2rem;
    }
  }
</style>
