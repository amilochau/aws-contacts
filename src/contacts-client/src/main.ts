import { createApp } from 'vue'
import App from './App.vue'
import milochauCore from './plugins/milochau-core'

import 'vuetify/styles'

const app = createApp(App);
app.use(milochauCore);
