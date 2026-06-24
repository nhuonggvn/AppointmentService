import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

// Global styles (phải import trước Vuetify để có thể override)
import './style.css'

// Vuetify
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import '@mdi/font/css/materialdesignicons.css'

// Pinia
import { createPinia } from 'pinia'

const customLightTheme = {
  dark: false,
  colors: {
    background: '#f8fafc',
    surface: '#FFFFFF',
    primary: '#1e40af',
    'primary-darken-1': '#1e3a8a',
    secondary: '#0f172a',
    'secondary-darken-1': '#1e293b',
    success: '#10b981',
    'success-darken-1': '#059669',
    warning: '#f59e0b',
    'warning-darken-1': '#d97706',
    error: '#ef4444',
    'error-darken-1': '#dc2626',
    info: '#3b82f6',
  }
}

const vuetify = createVuetify({
  components,
  directives,
  theme: {
    defaultTheme: 'customLightTheme',
    themes: {
      customLightTheme
    }
  }
})

const app = createApp(App)
app.use(createPinia())
app.use(router)
app.use(vuetify)
app.mount('#app')
