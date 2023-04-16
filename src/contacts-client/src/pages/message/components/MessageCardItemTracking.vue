<template>
  <v-timeline
    align="start"
    density="compact"
    truncate-line="both"
    side="end">
    <message-card-item-tracking-item
      v-for="(tracking, i) in props.trackings"
      :key="i"
      :tracking="tracking" />
  </v-timeline>
</template>

<script setup lang="ts">
import type { MessageTracking } from '@/types/messages';
import MessageCardItemTrackingItem from './MessageCardItemTrackingItem.vue';

const props = defineProps<{
  trackings: MessageTracking[],
}>()
</script>
