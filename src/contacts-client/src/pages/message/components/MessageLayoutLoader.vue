<template>
  <div class="h-100 d-flex align-center">
    <v-container>
      <v-row>
        <v-col align="center">
          <h1 class="mt-4 text-h6 text-primary">
            {{ t('title') }}
          </h1>
          <p class="mb-4">
            {{ t('description') }}
          </p>
        </v-col>
      </v-row>
      <v-row justify="center">
        <v-col
          cols="8"
          sm="6"
          md="4"
          lg="3">
          <v-progress-linear
            color="primary"
            stream
            height="6" />
        </v-col>
      </v-row>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n';

const { t } = useI18n()
</script>

<i18n lang="json">
  {
    "en": {
      "title": "Loading...",
      "description": "Your message is almost ready!"
    },
    "fr": {
      "title": "Chargement...",
      "description": "Votre message sera bientôt prêt !"
    }
  }
</i18n>
