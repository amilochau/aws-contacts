import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vuetify from 'vite-plugin-vuetify'
import VueI18n from '@intlify/unplugin-vue-i18n/vite'
import Unfonts from 'unplugin-fonts/vite'
import { setDefaultResultOrder } from 'dns'

setDefaultResultOrder('verbatim')

export default defineConfig({
  plugins: [
    vue(),
    vuetify(),
    VueI18n({
      fullInstall: false,
      compositionOnly: true,
      runtimeOnly: false,
    }),
    Unfonts({
      google: {
        families: [{
          name: 'Roboto',
          styles: 'wght@100;300;400;500;700;900',
        }],
      },
    }),
  ],
  resolve: {
    alias: [
      { find: '@', replacement: fileURLToPath(new URL('./src', import.meta.url)) },
      { find: /^pinia$/, replacement: 'pinia/dist/pinia.mjs' },
    ]
  },
  optimizeDeps: {
    include: [
      "@aws-amplify/auth"
    ]
  }
})
