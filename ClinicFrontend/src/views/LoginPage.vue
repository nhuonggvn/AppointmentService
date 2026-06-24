<template>
  <div class="login-split-wrapper">

    <!-- BÊN TRÁI: Banner y khoa nghệ thuật -->
    <div class="split-left d-none d-md-flex">
      <div class="left-overlay"></div>
      <div class="left-content-premium">
        
        <!-- Logo -->
        <div class="brand-header-premium" @click="goHome">
          <div class="brand-logo-circle">
            <i class="fa-solid fa-heart-pulse"></i>
          </div>
          <span>ClinicFlow</span>
        </div>

        <!-- Khẩu hiệu lớn -->
        <div class="branding-message-premium">
          <h2>Giải pháp quản lý <br /><span class="glow-emerald-text">y tế liên thông số</span></h2>
          <p>
            Hệ thống quản lý phòng khám thế hệ mới, tối ưu hóa quy trình tiếp nhận,
            khám bệnh lâm sàng và thanh toán viện phí tức thì.
          </p>
        </div>

        <!-- Danh sách tính năng -->
        <div class="branding-bullets">
          <div class="bullet-item">
            <div class="bullet-icon"><i class="fa-solid fa-shield-halved"></i></div>
            <div class="bullet-text">
              <h5>Mã hóa bảo mật cao</h5>
              <p>Xác thực người dùng bằng cơ chế JWT token bảo mật tuyệt đối.</p>
            </div>
          </div>
          <div class="bullet-item">
            <div class="bullet-icon"><i class="fa-solid fa-network-wired"></i></div>
            <div class="bullet-text">
              <h5>Liên thông API thực tế</h5>
              <p>Kết nối chặt chẽ giữa 3 nhóm nghiệp vụ Đặt lịch, Bệnh án và Viện phí.</p>
            </div>
          </div>
        </div>

      </div>
    </div>

    <!-- BÊN PHẢI: Form Đăng nhập / Đăng ký tối giản -->
    <div class="split-right">
      
      <!-- Nút cấu hình API lơ lửng góc trên phải -->
      <div class="config-btn-wrapper">
        <v-btn
          icon="mdi-cog-outline"
          variant="text"
          color="grey-darken-2"
          @click="configDialog = true"
        />
      </div>

      <!-- Khung Form Container -->
      <div class="form-container-premium">
        
        <div class="form-header-premium">
          <div class="d-md-none mb-4" @click="goHome" style="cursor: pointer; display: inline-flex; align-items: center; gap: 8px;">
            <i class="fa-solid fa-heart-pulse" style="font-size: 24px; color: var(--primary-color);"></i>
            <span style="font-weight: 700; font-size: 18px;">ClinicFlow</span>
          </div>
          <h3>{{ isLoginMode ? 'Chào mừng quay trở lại' : 'Đăng ký tài khoản' }}</h3>
          <p>{{ isLoginMode ? 'Vui lòng nhập tài khoản để truy cập hệ thống quản trị' : 'Đăng ký nhanh tài khoản bệnh nhân để đặt lịch khám' }}</p>
        </div>

        <v-form @submit.prevent="handleSubmit">
          
          <!-- Các trường bổ sung khi Đăng ký -->
          <template v-if="!isLoginMode">
            <div class="form-group">
              <label>Họ và tên đầy đủ</label>
              <input
                v-model="authForm.fullName"
                type="text"
                placeholder="Nguyễn Văn An"
                required
              />
            </div>
            <div class="form-group">
              <label>Email liên hệ</label>
              <input
                v-model="authForm.email"
                type="email"
                placeholder="an.nguyen@example.com"
                required
              />
            </div>
          </template>

          <!-- Tên đăng nhập -->
          <div class="form-group">
            <label>Tên đăng nhập</label>
            <input
              v-model="authForm.username"
              type="text"
              placeholder="Tên tài khoản hoặc SĐT..."
              required
            />
          </div>

          <!-- Mật khẩu -->
          <div class="form-group">
            <label>Mật khẩu</label>
            <div style="position: relative; display: flex; align-items: center;">
              <input
                v-model="authForm.password"
                :type="showPassword ? 'text' : 'password'"
                placeholder="Nhập mật khẩu..."
                required
                style="padding-right: 44px;"
              />
              <v-icon
                :icon="showPassword ? 'mdi-eye-off-outline' : 'mdi-eye-outline'"
                size="18"
                color="grey-darken-1"
                style="position: absolute; right: 14px; cursor: pointer;"
                @click="showPassword = !showPassword"
              />
            </div>
          </div>

          <!-- Alert thông báo lỗi -->
          <div v-if="errorMsg" class="error-alert-premium">
            <i class="fa-solid fa-triangle-exclamation mr-2"></i>
            <span>{{ errorMsg }}</span>
          </div>

          <!-- Nút Đăng nhập / Đăng ký -->
          <button type="submit" class="btn-auth-premium" :disabled="loading">
            <span v-if="!loading">{{ isLoginMode ? 'Đăng Nhập Hệ Thống' : 'Hoàn Tất Đăng Ký' }}</span>
            <span v-else><i class="fa-solid fa-circle-notch fa-spin mr-2"></i> Đang xử lý...</span>
          </button>

        </v-form>

        <!-- Chuyển đổi Đăng nhập / Đăng ký -->
        <div class="toggle-mode-premium">
          <span>{{ isLoginMode ? 'Chưa có tài khoản?' : 'Đã có tài khoản?' }}</span>
          <a @click="toggleMode">{{ isLoginMode ? 'Đăng ký tài khoản' : 'Đăng nhập ngay' }}</a>
        </div>

        <!-- Trở lại trang chủ -->
        <div class="back-home-premium">
          <a @click="goHome">
            <i class="fa-solid fa-arrow-left mr-2"></i> Quay lại trang chủ
          </a>
        </div>

      </div>

    </div>

    <!-- Cấu hình Dialog API liên thông -->
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
/* SPLIT-SCREEN LAYOUT */
.login-split-wrapper {
  min-height: 100vh;
  display: flex;
  overflow: hidden;
  background-color: #ffffff;
}

/* LEFT PANEL (60%) */
.split-left {
  flex: 0 0 60%;
  position: relative;
  background: linear-gradient(135deg, rgba(15, 23, 42, 0.95) 0%, rgba(30, 64, 175, 0.8) 100%), 
              url('https://lh3.googleusercontent.com/aida-public/AB6AXuBVwqkJfQ_E8nZmgTRR2nUfcAFyOGmWAvtLoeYAf9LkzXzPzn33tqK80m9NjAxhpStTu9IYbqwP6AediDbii-nUQHLWX0XpAZZnakbkO29kZrlVoG6atv1rO1p2irB3qBJd-t7mKAPccpyVoJ8BypZLcoqR97YUIw5yzuOohXxDhScdo36cfrIpOM0SX36-ErULmHKgHHvISC0HOM5q4QUtlp7mEXY6sfomhG29IXaRP1vjAd5_96a5HkzlGBgNgVnXPudFHhNZSyd0');
  background-size: cover;
  background-position: center;
  padding: 64px 80px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.left-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: radial-gradient(circle at 10% 20%, rgba(16, 185, 129, 0.1) 0%, transparent 60%);
  pointer-events: none;
  z-index: 1;
}

.left-content-premium {
  position: relative;
  z-index: 10;
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.brand-header-premium {
  display: flex;
  align-items: center;
  gap: 12px;
  color: #ffffff;
  font-size: 24px;
  font-weight: 700;
  cursor: pointer;
}

.brand-logo-circle {
  width: 40px;
  height: 40px;
  background: rgba(255, 255, 255, 0.15);
  border: 1px solid rgba(255, 255, 255, 0.25);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 20px;
}

.branding-message-premium {
  margin: 60px 0;
}

.branding-message-premium h2 {
  font-size: 40px;
  font-weight: 800;
  line-height: 1.2;
  color: #ffffff;
  margin-bottom: 20px;
  letter-spacing: -1px;
}

.glow-emerald-text {
  background: linear-gradient(135deg, #a7f3d0 0%, #10b981 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.branding-message-premium p {
  color: #94a3b8;
  font-size: 16px;
  line-height: 1.7;
  max-width: 560px;
}

.branding-bullets {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.bullet-item {
  display: flex;
  align-items: flex-start;
  gap: 16px;
}

.bullet-icon {
  width: 44px;
  height: 44px;
  background: rgba(255, 255, 255, 0.08);
  border: 1px solid rgba(255, 255, 255, 0.15);
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #60a5fa;
  font-size: 16px;
  flex-shrink: 0;
}

.bullet-text h5 {
  font-size: 15px;
  font-weight: 600;
  color: #ffffff;
  margin: 0 0 4px 0;
}

.bullet-text p {
  font-size: 13px;
  color: #94a3b8;
  margin: 0;
  line-height: 1.5;
}

/* RIGHT PANEL (40%) */
.split-right {
  flex: 1;
  background-color: #f8fafc;
  padding: 64px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  position: relative;
}

.config-btn-wrapper {
  position: absolute;
  top: 24px;
  right: 24px;
}

.form-container-premium {
  max-width: 420px;
  width: 100%;
  margin: 0 auto;
}

.form-header-premium {
  margin-bottom: 32px;
}

.form-header-premium h3 {
  font-size: 26px;
  font-weight: 700;
  color: var(--text-primary);
  margin-bottom: 8px;
  letter-spacing: -0.5px;
}

.form-header-premium p {
  font-size: 14px;
  color: var(--text-secondary);
  margin: 0;
  line-height: 1.5;
}

/* FORM SUBMIT & ERROR STYLE */
.btn-auth-premium {
  width: 100%;
  padding: 14px;
  background: linear-gradient(135deg, var(--primary-color) 0%, #3b82f6 100%);
  color: #ffffff;
  border-radius: 8px;
  font-weight: 600;
  font-size: 15px;
  border: none;
  cursor: pointer;
  box-shadow: 0 4px 12px rgba(30, 64, 175, 0.2);
  transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
  margin-top: 8px;
}

.btn-auth-premium:hover {
  transform: translateY(-1px);
  box-shadow: 0 8px 20px rgba(30, 64, 175, 0.3);
}

.error-alert-premium {
  display: flex;
  align-items: center;
  background-color: #fef2f2;
  border: 1px solid #fee2e2;
  color: var(--danger-color);
  padding: 12px 16px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 500;
  margin-bottom: 20px;
}

.toggle-mode-premium {
  margin-top: 24px;
  text-align: center;
  font-size: 14px;
}

.toggle-mode-premium span {
  color: var(--text-secondary);
}

.toggle-mode-premium a {
  color: var(--primary-color);
  font-weight: 600;
  text-decoration: none;
  cursor: pointer;
  margin-left: 6px;
}

.toggle-mode-premium a:hover {
  text-decoration: underline;
}

.back-home-premium {
  margin-top: 32px;
  text-align: center;
  border-top: 1px solid #e2e8f0;
  padding-top: 24px;
}

.back-home-premium a {
  cursor: pointer;
  font-size: 13px;
  font-weight: 600;
  color: var(--text-secondary);
  text-decoration: none;
  transition: color 0.2s;
}

.back-home-premium a:hover {
  color: var(--primary-color);
}

/* RESPONSIVE */
@media (max-width: 960px) {
  .split-right {
    padding: 40px 24px;
  }
}
</style>
