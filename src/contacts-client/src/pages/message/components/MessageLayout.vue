<template>
  <h1 class="mb-2 text-h4 text-center">
    {{ t("title") }}
  </h1>
  <v-container>
    <v-row justify="center">
      <v-col
        cols="12"
        lg="8">
        <v-card>
          <v-card-item>
            <v-card-subtitle>
              {{ t('yourMessage') }}
            </v-card-subtitle>
          </v-card-item>
          <v-card-text class="multi-line">
            YOUR MESSAGE HERE
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <v-row v-if="hasReturnUrl">
      <v-col
        cols="12"
        lg="8"
        align="center">
        <v-btn
          :disabled="loading || !online"
          :prepend-icon="mdiArrowULeftBottom"
          class="my-3"
          color="primary"
          variant="text"
          @click="goToReturnUrl">
          {{ t("backToReturnUrl") }}
        </v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { mdiArrowULeftBottom } from '@mdi/js'
import { useAppStore, usePage } from '@amilochau/core-vue3';
import { useOnline } from '@vueuse/core';
import { storeToRefs } from 'pinia';
import { useI18n } from 'vue-i18n';
import { useUrl } from '@/composition/url';

usePage()
const { t } = useI18n()
const online = useOnline()
const appStore = useAppStore()
const { loading } = storeToRefs(appStore)
const { hasReturnUrl, goToReturnUrl } = useUrl()

</script>

<i18n lang="json">
  {
    "en": {
      "title": "Message",
      "yourMessage": "Your message",
      "backToReturnUrl": "Go back to site"
    },
    "fr": {
      "title": "Message",
      "yourMessage": "Votre message",
      "backToReturnUrl": "Retourner au site"
    }
  }
</i18n>

<i18n lang="json">
  {
    "en": {
      "pageTitle": "Message",
      "pageDescription": "Message page"
    },
    "fr": {
      "pageTitle": "Message",
      "pageDescription": "Page de message"
    }
  }
</i18n>
