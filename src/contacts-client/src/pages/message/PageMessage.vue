<template>
  <v-row
    class="h-100"
    no-gutters>
    <v-col>
      <suspense>
        <message-layout />
        <template #fallback>
          <message-layout-loader />
        </template>
      </suspense>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import MessageLayout from './components/MessageLayout.vue'
import MessageLayoutLoader from './components/MessageLayoutLoader.vue';
</script>
