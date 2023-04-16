<template>
  <v-container>
    <v-row justify="center">
      <v-col
        cols="12"
        sm="6"
        class="text-center">
        <h1 class="mt-4 text-h4 text-primary">
          {{ t("title") }}
        </h1>
        <p class="mb-4">
          {{ t('description') }}
        </p>
      </v-col>
    </v-row>
    <v-row justify="center">
      <v-col
        cols="12"
        sm="6">
        <v-form
          ref="form"
          :readonly="loading">
          <v-card
            elevation="0">
            <v-alert
              v-if="!isAuthenticated"
              type="info"
              density="compact"
              variant="tonal"
              border="start"
              class="mx-4">
              {{ t('loginAlert') }}
            </v-alert>
            <v-card-text>
              <v-text-field
                v-model="request.content.senderEmail"
                :disabled="isAuthenticated"
                :label="t('senderEmail')"
                :rules="[ required(), maxLength(100), emailAddress() ]"
                variant="underlined"
                density="comfortable"
                hide-details="auto"
                type="email"
                inputmode="email"
                class="mb-3"
                required />
              <v-text-field
                v-model="request.content.senderName"
                :disabled="isAuthenticated"
                :label="t('senderName')"
                :rules="[ required(), maxLength(100) ]"
                variant="underlined"
                density="comfortable"
                hide-details="auto"
                type="text"
                class="mb-3"
                required />
              <v-textarea
                v-model="request.content.message"
                :label="t('message')"
                :rules="[ required(), maxLength(2000) ]"
                variant="underlined"
                density="comfortable"
                hide-details="auto"
                type="text"
                class="mb-3"
                rows="3"
                auto-grow
                counter="2000" />
            </v-card-text>
            <v-card-text class="text-center">
              <v-btn
                :disabled="loading || !online"
                :loading="loading"
                :prepend-icon="mdiSend"
                color="primary"
                variant="text"
                @click="send">
                {{ t('send') }}
              </v-btn>
            </v-card-text>
          </v-card>
        </v-form>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { mdiSend } from '@mdi/js'
import { useMessagesAnonymousApi, useMessagesApi } from '@/composition/api';
import { MessagesCreateRequest } from '@/types/messages';
import { useAppStore, useHandle, useIdentityStore, usePage, useValidationRules } from '@amilochau/core-vue3';
import { useOnline } from '@vueuse/core';
import { storeToRefs } from 'pinia';
import { watch, type Ref } from 'vue';
import { ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRoute } from 'vue-router';
import { useRouter } from 'vue-router';

usePage()
const route = useRoute()
const router = useRouter()
const identityStore = useIdentityStore()
const { attributes, isAuthenticated } = storeToRefs(identityStore);
const { handleFormValidation, handleLoadAndError } = useHandle()
const appStore = useAppStore()
const messagesApi = useMessagesApi()
const messagesAnonymousApi = useMessagesAnonymousApi()
const online = useOnline()
const { loading } = storeToRefs(appStore)
const { required, maxLength, emailAddress } = useValidationRules()
const { t } = useI18n()

const form: Ref<any> = ref(null)
const request: Ref<MessagesCreateRequest> = ref(new MessagesCreateRequest())

const initRequest = () => {
  request.value = new MessagesCreateRequest()
  if (isAuthenticated.value) {
    request.value.content.senderEmail = attributes.value.email
    request.value.content.senderName = attributes.value.name
  }
}

initRequest();
watch(isAuthenticated, initRequest)

async function send() {
  if (!await handleFormValidation(form)) {
    return
  }

  request.value.content.culture = route.params.lang.toString()

  let messageId = '';
  return handleLoadAndError(async () => {
    if (isAuthenticated.value) {
      const response = await messagesApi.create(request.value)
      messageId = response.id;
    } else {
      const response = await messagesAnonymousApi.create(request.value)
      messageId = response.id;
    }
    appStore.displaySuccessMessage(t('successfullySended'), undefined, 'snackbar')
    initRequest()
    router.push({ name: 'Message', params: { id: messageId } })
  }, 'snackbar')
}
</script>

<i18n lang="json">
  {
    "en": {
      "title": "Welcome!",
      "description": "Here you can send a new message.",
      "loginAlert": "You can login to keep track on your message.",
      "senderEmail": "Your email address",
      "senderName": "Your name",
      "message": "Your message",
      "send": "Send the message",
      "successfullySended": "Your message has been sent!"
    },
    "fr": {
      "title": "Bienvenue !",
      "description": "Vous pouvez ici envoyer un nouveau message.",
      "loginAlert": "Vous pouvez vous connecter pour garder une trace de votre message.",
      "senderEmail": "Votre adresse email",
      "senderName": "Votre nom",
      "message": "Votre message",
      "send": "Envoyer le message",
      "successfullySended": "Votre message a bien été envoyé !"
    }
  }
</i18n>

<i18n lang="json">
  {
    "en": {
      "pageTitle": "",
      "pageDescription": "New message page"
    },
    "fr": {
      "pageTitle": "",
      "pageDescription": "Page de nouveau message"
    }
  }
</i18n>
