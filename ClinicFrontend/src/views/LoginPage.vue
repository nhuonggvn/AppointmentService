<template>
  <div class="login-page-wrapper">

    <!-- Background decorative blobs -->
    <div class="bg-blob blob-1"></div>
    <div class="bg-blob blob-2"></div>

    <!-- Main Card Container -->
    <div class="login-container">

      <!-- LEFT PANEL: Branding -->
      <div class="left-panel d-none d-md-flex">

        <!-- Logo -->
        <div class="left-logo cursor-pointer" @click="goHome">
          <v-icon icon="mdi-heart-pulse" size="36" color="white" class="mr-2" />
          <h1 class="text-h5 font-weight-black text-white mb-0">ClinicFlow</h1>
        </div>

        <!-- Heading -->
        <div class="left-content">
          <h2 class="text-h4 font-weight-black text-white mb-4" style="line-height: 1.25;">
            Giải pháp quản lý y tế thông minh &amp; tin cậy
          </h2>
          <p class="text-body-1 text-white mb-0" style="opacity: 0.82; line-height: 1.7;">
            Tối ưu hóa quy trình khám chữa bệnh, quản lý hồ sơ bệnh nhân và lịch hẹn
            chuyên nghiệp trong một nền tảng duy nhất.
          </p>
        </div>

        <!-- Security badge -->
        <div class="left-badge">
          <v-avatar color="white" variant="tonal" size="44" class="mr-3">
            <v-icon icon="mdi-shield-check" color="white" />
          </v-avatar>
          <div>
            <p class="font-weight-bold text-body-2 text-white mb-0">Hệ thống bảo mật cao</p>
            <p class="text-caption text-white mb-0" style="opacity: 0.7;">Mã hóa JWT và phân quyền chi tiết</p>
          </div>
        </div>

        <!-- Decorative circles -->
        <div class="left-circle circle-top"></div>
        <div class="left-circle circle-bottom"></div>
      </div>

      <!-- RIGHT PANEL: Auth Form -->
      <div class="right-panel">

        <!-- Floating Config Button -->
        <v-btn
          icon="mdi-cog"
          variant="text"
          color="grey-darken-1"
          class="config-btn"
          @click="configDialog = true"
        />

        <!-- Form Header -->
        <div class="form-header">
          <h3 class="text-h5 font-weight-black text-primary mb-2">
            {{ isLoginMode ? 'Chào mừng quay trở lại' : 'Tạo tài khoản mới' }}
          </h3>
          <p class="text-body-2 text-grey-darken-1 mb-0">
            {{ isLoginMode
              ? 'Đăng nhập để truy cập trang quản trị y tế liên thông'
              : 'Đăng ký thông tin bệnh nhân để đặt lịch khám' }}
          </p>
        </div>

        <!-- Auth Form -->
        <v-form @submit.prevent="handleSubmit" class="auth-form">
          <!-- Register extra fields -->
          <template v-if="!isLoginMode">
            <v-text-field
              v-model="authForm.fullName"
              label="Họ và tên đầy đủ"
              prepend-inner-icon="mdi-badge-account-outline"
              variant="outlined"
              density="comfortable"
              rounded="lg"
              class="mb-3"
              required
            />
            <v-text-field
              v-model="authForm.email"
              label="Email"
              prepend-inner-icon="mdi-email-outline"
              type="email"
              variant="outlined"
              density="comfortable"
              rounded="lg"
              class="mb-3"
              required
            />
          </template>

          <v-text-field
            v-model="authForm.username"
            label="Tên đăng nhập"
            prepend-inner-icon="mdi-account-outline"
            variant="outlined"
            density="comfortable"
            rounded="lg"
            class="mb-3"
            required
          />

          <v-text-field
            v-model="authForm.password"
            label="Mật khẩu"
            prepend-inner-icon="mdi-lock-outline"
            :append-inner-icon="showPassword ? 'mdi-eye-off-outline' : 'mdi-eye-outline'"
            :type="showPassword ? 'text' : 'password'"
            variant="outlined"
            density="comfortable"
            rounded="lg"
            class="mb-5"
            required
            @click:append-inner="showPassword = !showPassword"
          />

          <!-- Error alert -->
          <v-alert
            v-if="errorMsg"
            type="error"
            variant="tonal"
            rounded="lg"
            class="mb-4"
            closable
            @click:close="errorMsg = ''"
          >
            {{ errorMsg }}
          </v-alert>

          <v-btn
            type="submit"
            color="primary"
            block
            size="large"
            rounded="lg"
            class="font-weight-bold mb-4"
            :loading="loading"
          >
            {{ isLoginMode ? 'ĐĂNG NHẬP HỆ THỐNG' : 'ĐĂNG KÝ TÀI KHOẢN' }}
          </v-btn>
        </v-form>

        <!-- Toggle mode -->
        <div class="text-center mt-2">
          <span class="text-body-2 text-grey-darken-1">
            {{ isLoginMode ? 'Chưa có tài khoản?' : 'Đã có tài khoản?' }}
          </span>
          <a
            class="text-primary font-weight-bold ml-1 text-body-2 cursor-pointer"
            style="text-decoration: underline;"
            @click="toggleMode"
          >
            {{ isLoginMode ? 'Đăng ký ngay' : 'Đăng nhập ngay' }}
          </a>
        </div>

        <!-- Back to home -->
        <div class="text-center mt-5">
          <v-btn
            variant="text"
            color="grey-darken-1"
            size="small"
            prepend-icon="mdi-arrow-left"
            @click="goHome"
          >
            Quay lại trang chủ
          </v-btn>
        </div>
      </div>
    </div>

    <!-- Connection Config Dialog -->
    <v-dialog v-model="configDialog" max-width="500">
      <v-card class="pa-6 rounded-xl">
        <v-card-title class="text-h6 font-weight-bold text-primary px-0 pb-3 d-flex align-center">
          <v-icon icon="mdi-api" color="primary" class="mr-2" />
          Cấu hình địa chỉ API liên thông
        </v-card-title>
        <v-card-text class="px-0 py-3">
          <p class="text-body-2 text-grey-darken-1 mb-4">
            Để liên thông giữa các nhóm, vui lòng điền chính xác địa chỉ API Gateway hoặc API Auth của Nhóm 6.
          </p>
          <v-text-field
            v-model="authApiUrl"
            label="Địa chỉ API Auth (Nhóm 6)"
            variant="outlined"
            density="comfortable"
            class="mb-4"
            hint="Mặc định: http://26.71.15.204:5000/api"
            persistent-hint
          />
          <v-text-field
            v-model="gatewayApiUrl"
            label="Địa chỉ API Gateway (Nhóm mình)"
            variant="outlined"
            density="comfortable"
            class="mb-4"
            hint="Mặc định: http://localhost:5000/api"
            persistent-hint
          />
          <v-text-field
            v-model="medicalApiUrl"
            label="Địa chỉ API Bệnh Án (Nhóm 4)"
            variant="outlined"
            density="comfortable"
            hint="Mặc định: http://26.15.45.202:5000/api"
            persistent-hint
          />
        </v-card-text>
        <v-card-actions class="px-0 pt-3">
          <v-spacer />
          <v-btn variant="text" class="font-weight-bold" color="grey-darken-1" @click="configDialog = false">Hủy bỏ</v-btn>
          <v-btn color="primary" class="font-weight-bold" @click="saveConfig">Lưu cấu hình</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

  </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'

export default {
  name: 'LoginPage',
  setup() {
    const router = useRouter()
    const route = useRoute()

    const isLoginMode = ref(true)
    const showPassword = ref(false)
    const loading = ref(false)
    const configDialog = ref(false)
    const errorMsg = ref('')

    const authApiUrl = ref(localStorage.getItem('clinic_auth_api_url') || 'http://26.71.15.204:5000/api')
    const gatewayApiUrl = ref(localStorage.getItem('clinic_api_url') || 'http://localhost:5000/api')
    const medicalApiUrl = ref(localStorage.getItem('clinic_medical_api_url') || 'http://26.15.45.202:5000/api')

    const authForm = ref({
      username: '',
      password: '',
      fullName: '',
      email: ''
    })

    onMounted(() => {
      if (route.query.mode === 'register') {
        isLoginMode.value = false
      }
    })

    const goHome = () => {
      router.push({ name: 'Landing' })
    }

    const toggleMode = () => {
      isLoginMode.value = !isLoginMode.value
      errorMsg.value = ''
    }

    const saveConfig = () => {
      localStorage.setItem('clinic_auth_api_url', authApiUrl.value)
      localStorage.setItem('clinic_api_url', gatewayApiUrl.value)
      localStorage.setItem('clinic_medical_api_url', medicalApiUrl.value)
      configDialog.value = false
    }

    const decodeJwt = (token) => {
      try {
        if (!token) return null
        const parts = token.split('.')
        if (parts.length !== 3) return null
        const payloadBase64 = parts[1]
        const base64 = payloadBase64.replace(/-/g, '+').replace(/_/g, '/')
        const jsonPayload = decodeURIComponent(
          window.atob(base64)
            .split('')
            .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
            .join('')
        )
        return JSON.parse(jsonPayload)
      } catch (e) {
        console.error('Lỗi giải mã JWT token:', e)
        return null
      }
    }

    const handleSubmit = async () => {
      if (isLoginMode.value) {
        await handleLogin()
      } else {
        await handleRegister()
      }
    }

    const handleLogin = async () => {
      try {
        errorMsg.value = ''
        loading.value = true

        const url = `${authApiUrl.value}/Auth/login`
        const res = await fetch(url, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            username: authForm.value.username,
            password: authForm.value.password
          })
        })

        if (!res.ok) throw new Error('Đăng nhập thất bại. Tài khoản hoặc mật khẩu không chính xác.')
        const data = await res.json()

        localStorage.setItem('clinic_jwt_token', data.token)

        const decoded = decodeJwt(data.token)
        let role = 'Patient'
        let username = authForm.value.username

        if (decoded) {
          role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
                || decoded['role']
                || 'Patient'
          username = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
                  || decoded['unique_name']
                  || decoded['sub']
                  || decoded['name']
                  || username
        } else {
          role = data.role || 'Patient'
          username = data.username || username
        }

        localStorage.setItem('clinic_user_role', role)
        localStorage.setItem('clinic_user_name', username)

        router.push({ name: 'Dashboard' })
      } catch (err) {
        errorMsg.value = err.message
      } finally {
        loading.value = false
      }
    }

    const handleRegister = async () => {
      try {
        errorMsg.value = ''
        loading.value = true
        await new Promise(resolve => setTimeout(resolve, 800))
        errorMsg.value = ''
        isLoginMode.value = true
        authForm.value.username = ''
        authForm.value.password = ''
      } catch (err) {
        errorMsg.value = err.message
      } finally {
        loading.value = false
      }
    }

    return {
      isLoginMode,
      showPassword,
      loading,
      configDialog,
      errorMsg,
      authApiUrl,
      gatewayApiUrl,
      medicalApiUrl,
      authForm,
      goHome,
      toggleMode,
      saveConfig,
      handleSubmit
    }
  }
}
</script>

<style scoped>
/* ======= PAGE WRAPPER ======= */
.login-page-wrapper {
  min-height: 100vh;
  background-color: #F0F2F5;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 24px;
  position: relative;
  overflow: hidden;
}

/* ======= DECORATIVE BACKGROUND BLOBS ======= */
.bg-blob {
  position: absolute;
  border-radius: 50%;
  filter: blur(80px);
  z-index: 0;
  pointer-events: none;
}
.blob-1 {
  width: 400px;
  height: 400px;
  background: rgba(0, 61, 155, 0.10);
  top: -100px;
  left: -100px;
}
.blob-2 {
  width: 350px;
  height: 350px;
  background: rgba(0, 108, 71, 0.08);
  bottom: -80px;
  right: -80px;
}

/* ======= MAIN CARD CONTAINER ======= */
.login-container {
  position: relative;
  z-index: 10;
  width: 100%;
  max-width: 960px;
  display: flex;
  border-radius: 20px;
  overflow: hidden;
  box-shadow: 0 24px 64px rgba(0, 61, 155, 0.16);
  background-color: #ffffff;
}

/* ======= LEFT PANEL ======= */
.left-panel {
  flex: 0 0 400px;
  background: linear-gradient(145deg, #003D9B 0%, #002C70 100%);
  padding: 48px 40px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  position: relative;
  overflow: hidden;
}

.left-logo {
  display: flex;
  align-items: center;
}

.left-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 40px 0;
}

.left-badge {
  display: flex;
  align-items: center;
  background: rgba(255, 255, 255, 0.12);
  border: 1px solid rgba(255, 255, 255, 0.20);
  border-radius: 12px;
  padding: 16px;
  backdrop-filter: blur(8px);
}

/* Decorative circles on left panel */
.left-circle {
  position: absolute;
  border-radius: 50%;
  border: 1px solid rgba(255, 255, 255, 0.08);
  pointer-events: none;
}
.circle-top {
  width: 260px;
  height: 260px;
  top: -80px;
  right: -80px;
}
.circle-bottom {
  width: 180px;
  height: 180px;
  bottom: -60px;
  left: -60px;
}

/* ======= RIGHT PANEL ======= */
.right-panel {
  flex: 1;
  padding: 48px 48px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  position: relative;
  background: #ffffff;
}

.config-btn {
  position: absolute !important;
  top: 16px;
  right: 16px;
}

.form-header {
  margin-bottom: 32px;
}

.auth-form {
  width: 100%;
}

/* ======= RESPONSIVE ======= */
@media (max-width: 960px) {
  .login-container {
    max-width: 480px;
    flex-direction: column;
  }
  .right-panel {
    padding: 40px 32px;
  }
}
</style>
