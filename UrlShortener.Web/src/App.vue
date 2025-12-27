<template>
  <div class="container">

    <h1>URL Shortener!</h1>

    <form @submit.prevent="handleSubmit">
      <input type="url"
             placeholder="http://www.yourlongurl.com"
             v-model="longUrl"
             required
      />
      <button type="submit">Shorten</button>
    </form>

    <div class="result" v-if="shortUrl">
      <p>You short URL:</p>
      <a :href="shortUrl" target="_blank">{{ shortUrl }}</a>
    </div>

    <p class="error" v-if="error">{{ error }}</p>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue'
  import { shortenUrl } from './services/urlShortenerService'

  const longUrl = ref('');
  const shortUrl = ref('');
  const error = ref('');

  async function handleSubmit() {
    error.value = '';
    shortUrl.value = '';

    try {
      const response = await shortenUrl(longUrl.value);
      shortUrl.value = response;
    } catch (err: any) {
      error.value = err.message || 'Error while shortening the URL.';
    }
  }
</script>

<style scoped>
  .container {
    max-width: 600px;
    margin: 2rem auto;
    padding: 1rem;
    text-align: center;
  }

  form {
    margin-bottom: 1rem;
  }

  input {
    padding: 0.5rem;
    width: 70%;
    margin-right: 0.5rem;    
  }

  button {
    padding: 0.5rem 1rem;
  }

    button:hover {
      background-color: #1565c0;
    }

  .result {
    margin-top: 1rem;
    font-weight: bold;
  }

  .error {
    margin-top: 1rem;
    color: red;
    font-weight: bold;
  }
</style>
