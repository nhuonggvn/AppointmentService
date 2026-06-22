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
    background: '#F4F5F7',
    surface: '#FFFFFF',
    primary: '#003D9B',
    'primary-darken-1': '#002C70',
    secondary: '#006C47',
    'secondary-darken-1': '#004F34',
    success: '#006C47',
    'success-darken-1': '#004F34',
    warning: '#851800',
    'warning-darken-1': '#611100',
    error: '#BA1A1A',
    'error-darken-1': '#8A1313',
    info: '#0052CC',
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
  },
  // Đặt font mặc định cho tất cả Vuetify typography
  defaults: {
    VBtn: { style: "font-family: 'Inter', sans-serif;" },
    VTextField: { style: "font-family: 'Inter', sans-serif;" },
    VSelect: { style: "font-family: 'Inter', sans-serif;" },
    VListItem: { style: "font-family: 'Inter', sans-serif;" },
    VCard: { style: "font-family: 'Inter', sans-serif;" },
  }
})

const app = createApp(App)
app.use(createPinia())
app.use(router)
app.use(vuetify)
app.mount('#app')
