<template>
  <v-app :theme="theme">
    <router-view />
  </v-app>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useThemeStore } from '@amilochau/core-vue3';

const themeStore = useThemeStore()

const theme = computed(() => themeStore.darkMode ? 'dark' : 'light')
</script>
