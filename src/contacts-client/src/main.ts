import App from './App.vue'
import { CoreVue3Auth } from '@amilochau/core-vue3-auth'
import { coreOptions } from './data/config'

import 'vuetify/styles'

export const createApp = CoreVue3Auth(App, coreOptions);
