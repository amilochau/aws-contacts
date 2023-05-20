import { computed } from 'vue';
import { useRoute } from 'vue-router';
import { authorizedDomains } from '../data/config';

export function useUrl() {

  const route = useRoute()

  const returnUrl = computed(() => {
    if (!route.query.returnUrl) {
      return undefined
    }
    try {
      return new URL(decodeURIComponent(route.query.returnUrl?.toString()))
    } catch (error) {
      return undefined
    }
  })

  const encodedReturnUrl = computed(() => {
    if (!returnUrl.value) {
      return undefined
    }

    return encodeURIComponent(returnUrl.value.toString())
  })

  const hasValidReturnUrl = computed(() => {
    if (!returnUrl.value) {
      return false
    }

    const hostSegments = getDomainFromHost(returnUrl.value.hostname)
    if (!hostSegments) {
      return false
    }

    return authorizedDomains.includes(hostSegments)
  })

  const getDomainFromHost = (host: string) => {
    const hostSegments = host.split('.')
    if (hostSegments.length >= 2) {
      return `${hostSegments.at(-2)}.${hostSegments.at(-1)}`
    } else {
      return hostSegments[0]
    }
  }

  return {
    returnUrl,
    encodedReturnUrl,
    hasValidReturnUrl,
    getDomainFromHost,
  }
}
