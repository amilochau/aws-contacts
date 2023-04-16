<template>
  <v-timeline-item
    :icon="trackingType.icon"
    :dot-color="trackingType.color"
    line-inset="2">
    <v-row>
      <v-col cols="5">
        <strong>{{ d(tracking.creation) }}</strong>
        <div class="text-caption">
          {{ d(tracking.creation, 'hour') }}
        </div>
      </v-col>
      <v-col cols="7">
        <strong>{{ trackingType.title }}</strong>
      </v-col>
    </v-row>
  </v-timeline-item>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useI18n } from 'vue-i18n';
import { useFormat } from '@/composition/format';
import type { MessageTracking } from '@/types/messages';

const { formatMessageTrackingType } = useFormat()
const { d, mergeDateTimeFormat } = useI18n()

const props = defineProps<{
  tracking: MessageTracking,
}>()

const trackingType = computed(() => formatMessageTrackingType(props.tracking.type))

mergeDateTimeFormat('en', {
  datetime: {
    year: 'numeric', month: 'numeric', day: 'numeric', hour: 'numeric', minute: 'numeric'
  },
  hour: {
    hour: 'numeric', minute: 'numeric'
  }
})
mergeDateTimeFormat('fr', {
  datetime: {
    year: 'numeric', month: 'numeric', day: 'numeric', hour: 'numeric', minute: 'numeric'
  },
  hour: {
    hour: 'numeric', minute: 'numeric'
  }
})
</script>
