/**
 * main.ts
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Components
import App from './App.vue'
import router from './router'

// Composables
import { createApp } from 'vue'

// Plugins
import { registerPlugins } from '@/plugins'
import { createPinia } from "pinia";

const app = createApp(App)

registerPlugins(app)

app.use(createPinia())
app.use(router)

app.mount('#app')
