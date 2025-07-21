<template>
  <div class="container">

    <h1>URL Shortener!</h1>

    <form>
      <input type="url" placeholder="http://www.yoursite.com" required />
      <button type="submit">Shorten</button>
    </form>

    <div class="resultado">
      <p>You short URL:</p>
      <a target="_blank"></a>
    </div>

    <p class="erro">Error</p>
  </div>
</template>

<script setup lang="ts">
  
</script>

<style scoped>
  .container {
    max-width: 600px;
    margin: 2rem auto;
    padding: 1rem;
    text-align: center;
  }

  form {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    margin-top: 1.5rem;
  }

  input {
    padding: 0.75rem;
    border: 1px solid #ccc;
    border-radius: 8px;
    font-size: 1rem;
  }

  button {
    padding: 0.75rem;
    font-size: 1rem;
    background-color: #1e88e5;
    color: white;
    border: none;
    border-radius: 8px;
    cursor: pointer;
  }

    button:hover {
      background-color: #1565c0;
    }

  .resultado {
    margin-top: 2rem;
    font-size: 1.1rem;
  }

  .erro {
    margin-top: 1rem;
    color: red;
    font-weight: bold;
  }

</style>
