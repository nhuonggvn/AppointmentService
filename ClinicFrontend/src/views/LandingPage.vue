<template>
  <div class="lp-root">

    <!-- NAV -->
    <nav class="lp-nav" :class="{ scrolled: scrolled }">
      <div class="lp-nav-inner">
        <div class="lp-logo" @click="$router.push('/')">
          <div class="lp-logo-icon"><v-icon icon="mdi-heart-pulse" color="white" size="20" /></div>
          <span>ClinicFlow</span>
        </div>
        <div class="lp-nav-links">
          <a href="#services">Dịch vụ</a>
          <a href="#how">Quy trình</a>
          <a href="#stats">Thống kê</a>
          <a href="#doctors">Bác sĩ</a>
          
        </div>
        <div class="lp-nav-actions">
          <button class="lp-btn-ghost" @click="goAuth('login')">Đăng nhập</button>
          <button class="lp-btn-solid" @click="goAuth('login')">Đặt lịch ngay</button>
        </div>
      </div>
    </nav>

    <!-- HERO -->
    <section class="lp-hero">
      <div class="lp-hero-bg"></div>
      <div class="lp-hero-orb orb1"></div>
      <div class="lp-hero-orb orb2"></div>
      <div class="lp-hero-inner">
        <div class="lp-hero-left">
          <div class="lp-badge">
            <span class="lp-badge-dot"></span>
            Hệ thống y tế liên thông — Nhóm 5
          </div>
          <h1 class="lp-hero-title">
            Đặt lịch khám<br/>
            <span class="lp-gradient-text">thông minh & nhanh chóng</span>
          </h1>
          <p class="lp-hero-desc">
            Kết nối liên thông tức thì giữa Đặt lịch khám, Hồ sơ bệnh án điện tử
            và Quản lý dược phẩm. Tối ưu quy trình khám chữa bệnh theo tiêu chuẩn số hóa.
          </p>
          <div class="lp-hero-actions">
            <button class="lp-btn-solid lp-btn-lg" @click="goAuth('login')">
              <v-icon icon="mdi-calendar-check" size="18" class="mr-2"/>
              Đặt lịch khám trực tuyến
            </button>
            <a href="#services" class="lp-btn-outline lp-btn-lg">
              Tìm hiểu thêm
              <v-icon icon="mdi-arrow-right" size="16" class="ml-2"/>
            </a>
          </div>
          <div class="lp-trust-row">
            <div class="lp-trust-item">
              <v-icon icon="mdi-shield-check" color="#00a06b" size="18"/>
              <span>Bảo mật JWT</span>
            </div>
            <div class="lp-trust-item">
              <v-icon icon="mdi-clock-fast" color="#1274e3" size="18"/>
              <span>Xác nhận tức thì</span>
            </div>
            <div class="lp-trust-item">
              <v-icon icon="mdi-hospital-building" color="#f59e0b" size="18"/>
              <span>Đa chuyên khoa</span>
            </div>
          </div>
        </div>
        <div class="lp-hero-right">
          <div class="lp-hero-card-main">
            <div class="lp-hc-header">
              <v-icon icon="mdi-calendar-clock" color="#1274e3" size="22"/>
              <span>Lịch hẹn hôm nay</span>
              <div class="lp-live-dot"></div>
            </div>
            <div v-for="appt in demoAppointments" :key="appt.time" class="lp-appt-row">
              <div class="lp-appt-avatar" :style="`background:${appt.color}`">{{ appt.initials }}</div>
              <div class="lp-appt-info">
                <div class="lp-appt-name">{{ appt.name }}</div>
                <div class="lp-appt-spec">{{ appt.spec }}</div>
              </div>
              <div class="lp-appt-time">{{ appt.time }}</div>
            </div>
          </div>
          <div class="lp-hero-card-mini mini-left">
            <v-icon icon="mdi-account-group" color="#1274e3" size="24"/>
            <div class="lp-mini-num">500+</div>
            <div class="lp-mini-label">Bệnh nhân/ngày</div>
          </div>
          <div class="lp-hero-card-mini mini-right">
            <v-icon icon="mdi-check-circle" color="#00a06b" size="24"/>
            <div class="lp-mini-num">99%</div>
            <div class="lp-mini-label">Hài lòng</div>
          </div>
        </div>
      </div>
    </section>

    <!-- SERVICES -->
    <section id="services" class="lp-section">
      <div class="lp-section-inner">
        <div class="lp-section-header">
          <span class="lp-section-tag">Chức năng cốt lõi</span>
          <h2>Giải pháp liên thông toàn diện</h2>
          <p>Ba hệ thống nghiệp vụ y tế kết nối liền mạch, đồng bộ dữ liệu theo thời gian thực.</p>
        </div>
        <div class="lp-bento">
          <div class="lp-bento-card lp-bento-blue">
            <div class="lp-bento-icon"><v-icon icon="mdi-calendar-multiselect" size="32" color="#1274e3"/></div>
            <h3>Đặt lịch hẹn linh hoạt</h3>
            <p>Chủ động chọn bác sĩ chuyên khoa, đặt giờ khám chính xác. Hệ thống tự động kiểm tra xung đột lịch và chặn trùng giờ trực quan.</p>
            <div class="lp-bento-tags">
              <span>Chọn bác sĩ</span><span>Chọn ca khám</span><span>Xác nhận tức thì</span>
            </div>
          </div>
          <div class="lp-bento-card lp-bento-green">
            <div class="lp-bento-icon"><v-icon icon="mdi-file-medical" size="32" color="#00a06b"/></div>
            <h3>Bệnh án điện tử EMR</h3>
            <p>Lưu trữ lịch sử khám bệnh, chẩn đoán, ghi chú lâm sàng và đơn thuốc điện tử an toàn. Bác sĩ tra cứu tức thì để đưa ra phác đồ điều trị.</p>
            <div class="lp-bento-tags">
              <span>Hồ sơ bệnh nhân</span><span>Lịch sử khám</span><span>Đơn thuốc số</span>
            </div>
          </div>
          <div class="lp-bento-card lp-bento-orange">
            <div class="lp-bento-icon"><v-icon icon="mdi-pill" size="32" color="#f59e0b"/></div>
            <h3>Dược phẩm & Viện phí</h3>
            <p>Liên thông kho thuốc nhà thuốc bệnh viện, tự động trừ tồn kho và xuất hóa đơn thanh toán tiền khám chuẩn xác theo từng bệnh nhân.</p>
            <div class="lp-bento-tags">
              <span>Kho thuốc thực</span><span>Hóa đơn tự động</span><span>Viện phí</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- HOW IT WORKS -->
    <section id="how" class="lp-section lp-section-alt">
      <div class="lp-section-inner">
        <div class="lp-section-header">
          <span class="lp-section-tag">Quy trình</span>
          <h2>Đặt lịch chỉ trong 4 bước</h2>
          <p>Từ đăng ký đến gặp bác sĩ — nhanh chóng, minh bạch và không cần chờ đợi lâu.</p>
        </div>
        <div class="lp-steps">
          <div v-for="(step, i) in steps" :key="i" class="lp-step">
            <div class="lp-step-num">{{ i + 1 }}</div>
            <div class="lp-step-icon"><v-icon :icon="step.icon" size="28" :color="step.color"/></div>
            <div class="lp-step-title">{{ step.title }}</div>
            <div class="lp-step-desc">{{ step.desc }}</div>
          </div>
        </div>
      </div>
    </section>

    <!-- STATS -->
    <section id="stats" class="lp-stats-section">
      <div class="lp-stats-inner">
        <div v-for="stat in stats" :key="stat.label" class="lp-stat-col">
          <div class="lp-stat-num">{{ stat.value }}</div>
          <div class="lp-stat-label">{{ stat.label }}</div>
        </div>
      </div>
    </section>

    <!-- DOCTORS -->
    <section id="doctors" class="lp-section">
      <div class="lp-section-inner">
        <div class="lp-section-header">
          <span class="lp-section-tag">Đội ngũ y tế</span>
          <h2>Chuyên gia y khoa đầu ngành</h2>
          <p>Đội ngũ bác sĩ tận tâm, giàu kinh nghiệm luôn sẵn sàng phục vụ sức khỏe của bạn.</p>
        </div>
        <div class="lp-doctors-grid">
          <div v-for="doc in doctors" :key="doc.name" class="lp-doctor-card">
            <div class="lp-doctor-avatar" :style="`background: linear-gradient(135deg, ${doc.c1}, ${doc.c2})`">
              <v-icon icon="mdi-account-circle" size="64" color="rgba(255,255,255,0.8)"/>
            </div>
            <div class="lp-doctor-info">
              <div class="lp-doctor-name">{{ doc.name }}</div>
              <div class="lp-doctor-spec">{{ doc.spec }}</div>
              <div class="lp-doctor-degree">{{ doc.degree }}</div>
              <div class="lp-doctor-tags">
                <span v-for="t in doc.tags" :key="t">{{ t }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA -->
    <section class="lp-cta-section">
      <div class="lp-cta-inner">
        <v-icon icon="mdi-heart-pulse" size="48" color="rgba(255,255,255,0.3)" class="mb-4"/>
        <h2 class="lp-cta-title">Sẵn sàng đặt lịch khám ngay hôm nay?</h2>
        <p class="lp-cta-desc">Tham gia cùng hàng nghìn bệnh nhân đang sử dụng ClinicFlow để chăm sóc sức khỏe chủ động và hiệu quả hơn mỗi ngày.</p>
        <div class="lp-cta-actions">
          <button class="lp-btn-white" @click="goAuth('login')">
            <v-icon icon="mdi-calendar-plus" size="18" class="mr-2"/>
            Đặt lịch khám ngay
          </button>
          <button class="lp-btn-outline-white" @click="goAuth('login')">
            Đăng nhập hệ thống
          </button>
        </div>
      </div>
    </section>

    <!-- FOOTER -->
    <footer class="lp-footer">
      <div class="lp-footer-inner">
        <div class="lp-footer-col lp-footer-brand-col">
          <div class="lp-footer-logo">
            <div class="lp-logo-icon small"><v-icon icon="mdi-heart-pulse" color="white" size="16"/></div>
            <span>ClinicFlow</span>
          </div>
          <p class="lp-footer-about">Giải pháp số hóa toàn diện quy trình quản lý phòng khám và liên thông dữ liệu y tế thế hệ mới. Xây dựng bởi Nhóm 5 — BTL Fullstack 2026.</p>
        </div>
        <div class="lp-footer-col">
          <div class="lp-footer-col-title">Dịch vụ</div>
          <a href="#services">Đặt lịch hẹn</a>
          <a href="#services">Bệnh án điện tử</a>
          <a href="#services">Quản lý dược phẩm</a>
          <a href="#how">Quy trình khám</a>
        </div>
        <div class="lp-footer-col">
          <div class="lp-footer-col-title">Hệ thống</div>
          <a href="#" @click.prevent="goAuth('login')">Đăng nhập</a>
          <a href="#" @click.prevent="goAuth('register')">Đăng ký</a>
          <a href="#doctors">Danh sách bác sĩ</a>
          <a href="#stats">Thống kê hệ thống</a>
        </div>
        <div class="lp-footer-col">
          <div class="lp-footer-col-title">Kết nối liên thông</div>
          <div class="lp-footer-service">
            <v-icon icon="mdi-circle" size="8" color="#1274e3"/> Nhóm 5 — Appointment (26.88.31.108)
          </div>
          <div class="lp-footer-service">
            <v-icon icon="mdi-circle" size="8" color="#00a06b"/> Nhóm 4 — Medical (26.79.10.201)
          </div>
          <div class="lp-footer-service">
            <v-icon icon="mdi-circle" size="8" color="#f59e0b"/> Nhóm 6 — Pharmacy (26.71.15.204)
          </div>
        </div>
      </div>
      <div class="lp-footer-bottom">
        <span>© 2026 ClinicFlow — BTL Fullstack Nhóm 5. Bản quyền thuộc về nhóm phát triển.</span>
        <span>Hệ thống Đặt lịch & Quản lý Hàng chờ Phòng khám</span>
      </div>
    </footer>

  </div>
</template>

<script>
import { ref, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'

export default {
  name: 'LandingPage',
  setup() {
    const router = useRouter()
    const scrolled = ref(false)

    const handleScroll = () => {
      scrolled.value = window.scrollY > 40
    }

    onMounted(() => window.addEventListener('scroll', handleScroll))
    onUnmounted(() => window.removeEventListener('scroll', handleScroll))

    const goAuth = (mode) => router.push({ name: 'Auth', query: { mode } })

    const demoAppointments = [
      { initials: 'NT', name: 'Nguyễn Thị Lan', spec: 'Nội khoa', time: '08:30', color: '#1274e3' },
      { initials: 'PV', name: 'Phạm Văn Hùng', spec: 'Nhi khoa', time: '09:00', color: '#00a06b' },
      { initials: 'LT', name: 'Lê Thị Mai', spec: 'Da liễu', time: '09:30', color: '#f59e0b' },
    ]

    const steps = [
      { icon: 'mdi-account-plus', color: '#1274e3', title: 'Tạo tài khoản', desc: 'Đăng ký nhanh chóng với số điện thoại và thông tin cá nhân cơ bản.' },
      { icon: 'mdi-doctor', color: '#00a06b', title: 'Chọn bác sĩ', desc: 'Tìm kiếm bác sĩ theo chuyên khoa, xem lịch trực và phí khám minh bạch.' },
      { icon: 'mdi-calendar-check', color: '#f59e0b', title: 'Đặt lịch hẹn', desc: 'Chọn ca khám phù hợp, hệ thống tự động kiểm tra xung đột và xác nhận ngay.' },
      { icon: 'mdi-hospital', color: '#ef4444', title: 'Đến khám & lấy kết quả', desc: 'Đến đúng giờ, nhận số thứ tự, bác sĩ khám và lưu bệnh án điện tử tức thì.' },
    ]

    const stats = [
      { value: '500+', label: 'Lượt khám mỗi ngày' },
      { value: '50+', label: 'Bác sĩ chuyên khoa' },
      { value: '100%', label: 'Dữ liệu liên thông' },
      { value: '99%', label: 'Bệnh nhân hài lòng' },
    ]

    const doctors = [
      { name: 'BS. Nguyễn Văn An', spec: 'Khoa Nội tổng quát', degree: 'Thạc sĩ Bác sĩ', c1: '#1274e3', c2: '#3b82f6', tags: ['Nội khoa', 'Tim mạch', 'Huyết áp'] },
      { name: 'BS. Trần Thị Bình', spec: 'Khoa Nhi khoa', degree: 'Bác sĩ Chuyên khoa 1', c1: '#00a06b', c2: '#34d399', tags: ['Nhi khoa', 'Sơ sinh', 'Dinh dưỡng'] },
      { name: 'BS. Lê Hoàng Nam', spec: 'Khoa Da liễu', degree: 'Bác sĩ Chuyên khoa 2', c1: '#f59e0b', c2: '#fbbf24', tags: ['Da liễu', 'Thẩm mỹ', 'Dị ứng'] },
      { name: 'BS. Phạm Thu Hà', spec: 'Khoa Phụ sản', degree: 'Tiến sĩ Y khoa', c1: '#ec4899', c2: '#f472b6', tags: ['Phụ sản', 'Siêu âm', 'Thai sản'] },
    ]

    return { scrolled, goAuth, demoAppointments, steps, stats, doctors }
  }
}
</script>

<style scoped>
/* ROOT */
.lp-root { font-family: 'Be Vietnam Pro', sans-serif; background: #f8fafc; color: #1e293b; }

/* NAV */
.lp-nav {
  position: fixed; top: 0; left: 0; right: 0; z-index: 200;
  background: rgba(255,255,255,0.7);
  backdrop-filter: blur(20px);
  border-bottom: 1px solid transparent;
  transition: all 0.3s ease;
  padding: 16px 0;
}
.lp-nav.scrolled {
  background: rgba(255,255,255,0.95);
  border-bottom-color: #e2e8f0;
  box-shadow: 0 4px 20px rgba(15,23,42,0.08);
  padding: 12px 0;
}
.lp-nav-inner {
  max-width: 1200px; margin: 0 auto; padding: 0 32px;
  display: flex; align-items: center; justify-content: space-between;
}
.lp-logo { display: flex; align-items: center; gap: 10px; cursor: pointer; text-decoration: none; }
.lp-logo span { font-size: 20px; font-weight: 800; color: #1e293b; letter-spacing: -0.5px; }
.lp-logo-icon {
  width: 36px; height: 36px; border-radius: 10px;
  background: linear-gradient(135deg, #1274e3, #3b82f6);
  display: flex; align-items: center; justify-content: center;
  box-shadow: 0 4px 12px rgba(18,116,227,0.35);
}
.lp-logo-icon.small { width: 28px; height: 28px; border-radius: 7px; }
.lp-nav-links { display: flex; gap: 32px; }
.lp-nav-links a {
  text-decoration: none; color: #64748b; font-size: 14px; font-weight: 500;
  transition: color 0.2s; padding: 4px 0; position: relative;
}
.lp-nav-links a:hover { color: #1274e3; }
.lp-nav-links a::after {
  content: ''; position: absolute; bottom: -2px; left: 0; width: 0; height: 2px;
  background: #1274e3; transition: width 0.25s;
}
.lp-nav-links a:hover::after { width: 100%; }
.lp-nav-actions { display: flex; gap: 12px; align-items: center; }

/* BUTTONS */
.lp-btn-ghost {
  background: transparent; border: 1.5px solid #e2e8f0; color: #475569;
  padding: 9px 20px; border-radius: 8px; font-weight: 600; font-size: 14px;
  cursor: pointer; transition: all 0.2s;
}
.lp-btn-ghost:hover { border-color: #1274e3; color: #1274e3; }
.lp-btn-solid {
  background: linear-gradient(135deg, #1274e3 0%, #3b82f6 100%);
  border: none; color: #fff; padding: 9px 20px; border-radius: 8px;
  font-weight: 600; font-size: 14px; cursor: pointer;
  box-shadow: 0 4px 12px rgba(18,116,227,0.3);
  transition: all 0.2s; display: inline-flex; align-items: center;
}
.lp-btn-solid:hover { transform: translateY(-2px); box-shadow: 0 8px 20px rgba(18,116,227,0.4); }
.lp-btn-outline {
  border: 1.5px solid rgba(255,255,255,0.35); background: transparent;
  color: #fff; padding: 9px 20px; border-radius: 8px;
  font-weight: 600; font-size: 14px; cursor: pointer; text-decoration: none;
  transition: all 0.2s; display: inline-flex; align-items: center;
}
.lp-btn-outline:hover { background: rgba(255,255,255,0.1); border-color: #fff; }
.lp-btn-lg { padding: 14px 28px; font-size: 15px; }
.lp-btn-white {
  background: #fff; border: none; color: #1274e3;
  padding: 14px 32px; border-radius: 10px; font-weight: 700;
  font-size: 15px; cursor: pointer; display: inline-flex; align-items: center;
  box-shadow: 0 4px 20px rgba(0,0,0,0.15); transition: all 0.2s;
}
.lp-btn-white:hover { transform: translateY(-2px); box-shadow: 0 8px 30px rgba(0,0,0,0.2); }
.lp-btn-outline-white {
  background: transparent; border: 2px solid rgba(255,255,255,0.45);
  color: #fff; padding: 13px 32px; border-radius: 10px; font-weight: 600;
  font-size: 15px; cursor: pointer; transition: all 0.2s;
}
.lp-btn-outline-white:hover { background: rgba(255,255,255,0.12); border-color: #fff; }

/* HERO */
.lp-hero {
  min-height: 100vh; padding: 120px 32px 80px;
  background: linear-gradient(135deg, #060d1f 0%, #0f1e40 55%, #1a3a8a 100%);
  color: #fff; position: relative; overflow: hidden; display: flex; align-items: center;
}
.lp-hero-bg {
  position: absolute; inset: 0; pointer-events: none;
  background: radial-gradient(ellipse at 75% 30%, rgba(59,130,246,0.18) 0%, transparent 60%),
              radial-gradient(ellipse at 20% 80%, rgba(0,160,107,0.1) 0%, transparent 50%);
}
.lp-hero-orb {
  position: absolute; border-radius: 50%; filter: blur(80px); pointer-events: none; opacity: 0.5;
}
.orb1 { width: 400px; height: 400px; background: rgba(18,116,227,0.25); top: -100px; right: 5%; }
.orb2 { width: 300px; height: 300px; background: rgba(0,160,107,0.15); bottom: 0; left: 5%; }
.lp-hero-inner {
  max-width: 1200px; margin: 0 auto; display: grid; grid-template-columns: 1fr 1fr;
  gap: 64px; align-items: center; position: relative; z-index: 10; width: 100%;
}
.lp-badge {
  display: inline-flex; align-items: center; gap: 10px;
  background: rgba(255,255,255,0.08); border: 1px solid rgba(255,255,255,0.15);
  border-radius: 9999px; padding: 8px 18px; margin-bottom: 28px;
  font-size: 12px; font-weight: 600; letter-spacing: 0.5px; color: #cbd5e1;
}
.lp-badge-dot {
  width: 8px; height: 8px; border-radius: 50%; background: #00a06b;
  box-shadow: 0 0 10px #00a06b; animation: pulse-dot 2s infinite;
}
@keyframes pulse-dot {
  0%, 100% { box-shadow: 0 0 6px #00a06b; } 50% { box-shadow: 0 0 14px #00a06b; }
}
.lp-hero-title {
  font-size: clamp(32px, 4vw, 52px); font-weight: 900;
  line-height: 1.12; margin-bottom: 24px; letter-spacing: -1.5px;
}
.lp-gradient-text {
  background: linear-gradient(135deg, #60a5fa 0%, #34d399 100%);
  -webkit-background-clip: text; -webkit-text-fill-color: transparent;
}
.lp-hero-desc { font-size: 16px; line-height: 1.75; color: #94a3b8; margin-bottom: 40px; }
.lp-hero-actions { display: flex; gap: 16px; flex-wrap: wrap; margin-bottom: 40px; }
.lp-trust-row { display: flex; gap: 24px; flex-wrap: wrap; }
.lp-trust-item {
  display: flex; align-items: center; gap: 8px;
  font-size: 13px; font-weight: 500; color: #94a3b8;
}

/* HERO RIGHT */
.lp-hero-right { position: relative; display: flex; justify-content: center; align-items: center; height: 420px; }
.lp-hero-card-main {
  background: rgba(255,255,255,0.06); backdrop-filter: blur(16px);
  border: 1px solid rgba(255,255,255,0.12); border-radius: 20px;
  padding: 24px; width: 320px;
}
.lp-hc-header {
  display: flex; align-items: center; gap: 10px; margin-bottom: 20px;
  font-size: 14px; font-weight: 600; color: #e2e8f0;
}
.lp-live-dot {
  width: 8px; height: 8px; border-radius: 50%; background: #00a06b;
  margin-left: auto; animation: pulse-dot 2s infinite;
}
.lp-appt-row {
  display: flex; align-items: center; gap: 12px;
  background: rgba(255,255,255,0.04); border-radius: 12px;
  padding: 12px; margin-bottom: 10px;
}
.lp-appt-avatar {
  width: 38px; height: 38px; border-radius: 10px;
  display: flex; align-items: center; justify-content: center;
  font-weight: 700; font-size: 13px; color: #fff; flex-shrink: 0;
}
.lp-appt-info { flex: 1; }
.lp-appt-name { font-size: 13px; font-weight: 600; color: #f1f5f9; }
.lp-appt-spec { font-size: 11px; color: #64748b; margin-top: 2px; }
.lp-appt-time { font-size: 12px; font-weight: 700; color: #60a5fa; }
.lp-hero-card-mini {
  position: absolute; background: rgba(255,255,255,0.08);
  backdrop-filter: blur(12px); border: 1px solid rgba(255,255,255,0.15);
  border-radius: 16px; padding: 16px 20px; text-align: center;
}
.mini-left { left: -20px; bottom: 40px; }
.mini-right { right: -20px; top: 60px; }
.lp-mini-num { font-size: 24px; font-weight: 900; color: #fff; margin: 6px 0 2px; }
.lp-mini-label { font-size: 11px; color: #94a3b8; font-weight: 500; }

/* SECTION */
.lp-section { padding: 96px 32px; }
.lp-section-alt { background: #fff; }
.lp-section-inner { max-width: 1200px; margin: 0 auto; }
.lp-section-header { text-align: center; margin-bottom: 60px; }
.lp-section-tag {
  display: inline-block; font-size: 11px; font-weight: 700; letter-spacing: 2px;
  text-transform: uppercase; color: #1274e3; margin-bottom: 12px;
}
.lp-section-header h2 {
  font-size: clamp(26px, 3vw, 38px); font-weight: 800;
  color: #1e293b; margin-bottom: 16px; letter-spacing: -0.5px;
}
.lp-section-header p { font-size: 15px; color: #64748b; max-width: 560px; margin: 0 auto; line-height: 1.7; }

/* BENTO */
.lp-bento { display: grid; grid-template-columns: repeat(auto-fit, minmax(300px, 1fr)); gap: 28px; }
.lp-bento-card {
  border-radius: 20px; padding: 36px 32px; position: relative; overflow: hidden;
  border: 1px solid #e2e8f0; background: #fff;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}
.lp-bento-card:hover { transform: translateY(-8px); box-shadow: 0 20px 40px rgba(15,23,42,0.1); }
.lp-bento-blue { border-top: 3px solid #1274e3; }
.lp-bento-green { border-top: 3px solid #00a06b; }
.lp-bento-orange { border-top: 3px solid #f59e0b; }
.lp-bento-icon { margin-bottom: 20px; }
.lp-bento-card h3 { font-size: 20px; font-weight: 700; color: #1e293b; margin-bottom: 12px; letter-spacing: -0.3px; }
.lp-bento-card p { font-size: 14px; color: #64748b; line-height: 1.7; margin-bottom: 20px; }
.lp-bento-tags { display: flex; gap: 8px; flex-wrap: wrap; }
.lp-bento-tags span {
  background: #f1f5f9; color: #475569; border-radius: 9999px;
  padding: 4px 12px; font-size: 12px; font-weight: 500;
}

/* STEPS */
.lp-steps { display: grid; grid-template-columns: repeat(auto-fit, minmax(220px, 1fr)); gap: 32px; position: relative; }
.lp-step { text-align: center; padding: 32px 20px; position: relative; }
.lp-step-num {
  width: 44px; height: 44px; border-radius: 50%;
  background: linear-gradient(135deg, #1274e3, #3b82f6);
  color: #fff; font-size: 18px; font-weight: 800;
  display: flex; align-items: center; justify-content: center;
  margin: 0 auto 16px; box-shadow: 0 6px 16px rgba(18,116,227,0.3);
}
.lp-step-icon { margin-bottom: 16px; }
.lp-step-title { font-size: 17px; font-weight: 700; color: #1e293b; margin-bottom: 10px; }
.lp-step-desc { font-size: 13px; color: #64748b; line-height: 1.65; }

/* STATS */
.lp-stats-section { background: linear-gradient(135deg, #0f172a 0%, #1e293b 100%); padding: 80px 32px; }
.lp-stats-inner {
  max-width: 1000px; margin: 0 auto;
  display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 40px; text-align: center;
}
.lp-stat-num { font-size: 52px; font-weight: 900; color: #34d399; letter-spacing: -2px; margin-bottom: 8px; }
.lp-stat-label { font-size: 12px; color: #94a3b8; font-weight: 500; text-transform: uppercase; letter-spacing: 0.5px; }

/* DOCTORS */
.lp-doctors-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 28px; }
.lp-doctor-card {
  background: #fff; border-radius: 20px; overflow: hidden;
  border: 1px solid #e2e8f0; transition: all 0.3s ease;
}
.lp-doctor-card:hover { transform: translateY(-8px); box-shadow: 0 20px 40px rgba(15,23,42,0.1); }
.lp-doctor-avatar {
  height: 160px; display: flex; align-items: center; justify-content: center;
}
.lp-doctor-info { padding: 24px; }
.lp-doctor-name { font-size: 17px; font-weight: 700; color: #1e293b; margin-bottom: 4px; }
.lp-doctor-spec { font-size: 13px; color: #1274e3; font-weight: 600; margin-bottom: 4px; }
.lp-doctor-degree { font-size: 12px; color: #94a3b8; margin-bottom: 16px; }
.lp-doctor-tags { display: flex; gap: 6px; flex-wrap: wrap; }
.lp-doctor-tags span {
  background: #f1f5f9; color: #475569;
  border-radius: 9999px; padding: 3px 10px;
  font-size: 11px; font-weight: 500;
}

/* CTA */
.lp-cta-section {
  background: linear-gradient(135deg, #1274e3 0%, #0f57c1 100%);
  padding: 100px 32px; text-align: center; color: #fff;
}
.lp-cta-inner { max-width: 700px; margin: 0 auto; }
.lp-cta-title { font-size: clamp(26px, 3vw, 40px); font-weight: 900; margin-bottom: 20px; letter-spacing: -0.5px; }
.lp-cta-desc { font-size: 15px; color: rgba(255,255,255,0.8); line-height: 1.75; margin-bottom: 40px; }
.lp-cta-actions { display: flex; gap: 16px; justify-content: center; flex-wrap: wrap; }

/* FOOTER */
.lp-footer { background: #0b1120; color: #fff; padding: 72px 32px 0; }
.lp-footer-inner {
  max-width: 1200px; margin: 0 auto;
  display: grid; grid-template-columns: 2fr 1fr 1fr 1.5fr; gap: 48px;
  padding-bottom: 56px; border-bottom: 1px solid #1e293b;
}
.lp-footer-logo { display: flex; align-items: center; gap: 10px; margin-bottom: 20px; }
.lp-footer-logo span { font-size: 18px; font-weight: 800; color: #fff; }
.lp-footer-about { font-size: 13px; color: #64748b; line-height: 1.75; }
.lp-footer-col-title { font-size: 12px; font-weight: 700; letter-spacing: 1.5px; text-transform: uppercase; color: #94a3b8; margin-bottom: 20px; }
.lp-footer-col a {
  display: block; font-size: 13px; color: #64748b; text-decoration: none;
  margin-bottom: 12px; transition: color 0.2s; font-weight: 500;
}
.lp-footer-col a:hover { color: #e2e8f0; }
.lp-footer-service { font-size: 12px; color: #64748b; margin-bottom: 12px; display: flex; align-items: center; gap: 8px; }
.lp-footer-bottom {
  max-width: 1200px; margin: 0 auto;
  display: flex; justify-content: space-between; align-items: center;
  padding: 24px 0; font-size: 12px; color: #475569;
}
@media (max-width: 768px) {
  .lp-hero-inner { grid-template-columns: 1fr; }
  .lp-hero-right { display: none; }
  .lp-nav-links { display: none; }
  .lp-footer-inner { grid-template-columns: 1fr 1fr; }
  .lp-footer-bottom { flex-direction: column; gap: 8px; text-align: center; }
}
</style>

