<template>
  <h1 class="mt-4 text-h6 text-center">
    {{ t("title") }}
  </h1>
  <v-container>
    <v-row justify="center">
      <v-col
        cols="12"
        lg="8">
        <v-card class="mb-4">
          <v-list :items="messageItems">
            <template #append="{ item }">
              <v-icon
                v-if="item.props?.append?.icon"
                :icon="item.props.append.icon"
                :color="item.props.append.color" />
            </template>
          </v-list>
        </v-card>
        <v-card class="mb-4">
          <v-card-item>
            <v-card-subtitle>
              {{ t('yourMessage') }}
            </v-card-subtitle>
          </v-card-item>
          <v-card-text class="multi-line">
            {{ messageDetails.content.message }}
          </v-card-text>
        </v-card>
        <v-card>
          <v-card-text v-if="messageDetails.trackings && messageDetails.trackings.length">
            <message-card-item-tracking
              :trackings="messageDetails.trackings" />
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
          :href="returnUrl?.toString()"
          target="_blank"
          rel="noopener"
          class="my-3"
          color="primary"
          variant="text">
          {{ t("backToReturnUrl") }}
        </v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { mdiArrowULeftBottom, mdiCalendar, mdiStateMachine } from '@mdi/js'
import { useAppStore, useHandle, useIdentityStore, usePage } from '@amilochau/core-vue3';
import { useOnline } from '@vueuse/core';
import { storeToRefs } from 'pinia';
import { useI18n } from 'vue-i18n';
import { useUrl } from '@/composition/url';
import { computed, ref } from 'vue';
import { MessagesDetailsResponse } from '@/types/messages';
import { useMessagesAnonymousApi, useMessagesApi } from '@/composition/api';
import { useRoute } from 'vue-router';
import { useFormat } from '@/composition/format';
import MessageCardItemTracking from './MessageCardItemTracking.vue'

usePage()
const { t, d, mergeDateTimeFormat } = useI18n()
const online = useOnline()
const route = useRoute()
const identityStore = useIdentityStore()
const { isAuthenticated } = storeToRefs(identityStore);
const appStore = useAppStore()
const messagesApi = useMessagesApi()
const messagesAnonymousApi = useMessagesAnonymousApi()
const { loading } = storeToRefs(appStore)
const { hasReturnUrl, goToReturnUrl } = useUrl()
const { handleLoadAndError } = useHandle()
const { formatMessageStatus } = useFormat()

const messageDetails = ref(new MessagesDetailsResponse())
const messageStatus = formatMessageStatus(messageDetails.value.status)
const messageItems = computed(() => ([{
  title: d(messageDetails.value.creation, 'datetime'),
  props: {
    prependIcon: mdiCalendar,
  },
}, {
  title: messageStatus.title,
  props: {
    prependIcon: mdiStateMachine,
    append: {
      icon: messageStatus.icon,
      color: messageStatus.color
    }
  },
}]))

mergeDateTimeFormat('en', {
  datetime: {
    year: 'numeric', month: 'numeric', day: 'numeric', hour: 'numeric', minute: 'numeric'
  }
})
mergeDateTimeFormat('fr', {
  datetime: {
    year: 'numeric', month: 'numeric', day: 'numeric', hour: 'numeric', minute: 'numeric'
  }
})

// LOAD MESSAGE
await loadMessage()

function loadMessage() {
  return handleLoadAndError(async () => {
    if (isAuthenticated.value) {
      messageDetails.value = await messagesApi.getById(route.params.id.toString())
    } else {
      messageDetails.value = await messagesAnonymousApi.getById(route.params.id.toString())
    }
  }, 'snackbar')
}
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
