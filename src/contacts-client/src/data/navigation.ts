import { mdiGavel, mdiHome } from '@mdi/js'
import { computed } from 'vue'
import { useI18n } from 'vue-i18n'

export default computed(() => {

  const { t, mergeLocaleMessage } = useI18n()

  mergeLocaleMessage('en', {
    navigation: {
      home: "Home",
      privacy: "Privacy",
    }
  })
  mergeLocaleMessage('fr', {
    navigation: {
      home: "Accueil",
      privacy: "Confidentialité",
    }
  })

  return [
    { title: t('navigation.home'), prependIcon: mdiHome, to: { name: 'Home' }, exact: true },
    { type: 'divider' },
    { title: t('navigation.privacy'), prependIcon: mdiGavel, to: { name: 'Privacy' } },
  ]
})
