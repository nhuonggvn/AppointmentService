<template>
  <div class="landing-page">

    <!-- Thanh điều hướng Glassmorphism -->
    <nav class="public-nav-glass">
      <div class="nav-container">
        <div class="nav-logo" @click="goToAuth('login')">
          <div class="logo-icon-wrapper">
            <i class="fa-solid fa-heart-pulse"></i>
          </div>
          <span class="brand-title">ClinicFlow</span>
        </div>
        <div class="nav-links d-none d-md-flex">
          <a href="#" class="nav-link-item active">Trang chủ</a>
          <a href="#services" class="nav-link-item">Dịch vụ</a>
          <a href="#stats" class="nav-link-item">Thống kê</a>
          <a href="#doctors" class="nav-link-item">Bác sĩ</a>
        </div>
        <div class="nav-auth">
          <button class="btn-premium btn-secondary" @click="goToAuth('login')">Đăng nhập</button>
          <button class="btn-premium btn-primary" @click="goToAuth('register')">Đăng ký ngay</button>
        </div>
      </div>
    </nav>

    <!-- Hero Section Premium Gradient -->
    <section class="hero-section-premium">
      <div class="hero-bg-overlay"></div>
      <div class="hero-container">
        <div class="hero-tag">
          <span class="tag-accent-line"></span>
          <span class="tag-text">Giải pháp y tế liên thông số 1</span>
        </div>
        <h1 class="hero-title">
          Chăm sóc sức khỏe <br />
          <span class="text-glow">thông minh &amp; đột phá</span>
        </h1>
        <p class="hero-desc">
          Kết nối liên thông tức thì giữa Đặt lịch khám, Hồ sơ bệnh án điện tử và Quản lý kho dược phẩm, viện phí.
          Tối ưu hóa quy trình khám chữa bệnh theo tiêu chuẩn công nghệ tương lai.
        </p>
        <div class="hero-actions">
          <button class="btn-premium btn-primary btn-large" @click="goToAuth('login')">
            <i class="fa-solid fa-calendar-check mr-2"></i>Đặt lịch khám trực tuyến
          </button>
          <a href="#services" class="btn-premium btn-outline btn-large">
            Tìm hiểu tính năng <i class="fa-solid fa-arrow-right ml-2"></i>
          </a>
        </div>
      </div>
    </section>

    <!-- Bento Services Section -->
    <main class="public-main-premium" id="services">
      <div class="section-header-premium">
        <span class="section-subtitle">Chức năng cốt lõi</span>
        <h2>Giải pháp liên thông toàn diện</h2>
        <p>Chúng tôi đồng bộ dữ liệu giữa 3 nhóm nghiệp vụ y tế lớn nhằm đem lại trải nghiệm khám chữa bệnh liền mạch.</p>
      </div>

      <div class="bento-grid">
        
        <div class="bento-card card-primary-glow">
          <div class="card-icon-circle bg-blue-glow">
            <i class="fa-solid fa-calendar-days"></i>
          </div>
          <h3>Đặt lịch hẹn linh hoạt</h3>
          <p>
            Chủ động chọn bác sĩ chuyên khoa, đặt giờ khám chính xác. Hệ thống tự động kiểm tra xung đột lịch và chặn trùng giờ trực quan.
          </p>
        </div>

        <div class="bento-card card-accent-glow">
          <div class="card-icon-circle bg-emerald-glow">
            <i class="fa-solid fa-file-medical"></i>
          </div>
          <h3>Bệnh án điện tử EMR</h3>
          <p>
            Lưu trữ lịch sử khám bệnh, chẩn đoán, ghi chú lâm sàng và đơn thuốc điện tử an toàn. Bác sĩ tra cứu tức thì để đưa ra phác đồ.
          </p>
        </div>

        <div class="bento-card card-danger-glow">
          <div class="card-icon-circle bg-red-glow">
            <i class="fa-solid fa-prescription-bottle-medical"></i>
          </div>
          <h3>Dược phẩm &amp; Viện phí</h3>
          <p>
            Liên thông kho thuốc thực tế của nhà thuốc bệnh viện, tự động trừ tồn kho và xuất hóa đơn thanh toán tiền khám chuẩn xác.
          </p>
        </div>

      </div>
    </main>

    <!-- Stats Section -->
    <section id="stats" class="stats-section-premium">
      <div class="stats-container">
        <div class="stats-row">
          <div class="stat-col">
            <div class="stat-number">500+</div>
            <div class="stat-label">Lượt khám mỗi ngày</div>
          </div>
          <div class="stat-col">
            <div class="stat-number">100%</div>
            <div class="stat-label">Dữ liệu liên thông</div>
          </div>
          <div class="stat-col">
            <div class="stat-number">50+</div>
            <div class="stat-label">Bác sĩ chuyên khoa giỏi</div>
          </div>
          <div class="stat-col">
            <div class="stat-number">99%</div>
            <div class="stat-label">Bệnh nhân hài lòng</div>
          </div>
        </div>
      </div>
    </section>

    <!-- Doctors Section -->
    <section class="public-main-premium" id="doctors" style="margin-top: 64px; margin-bottom: 96px;">
      <div class="section-header-premium">
        <span class="section-subtitle">Đội ngũ y tế</span>
        <h2>Chuyên gia y khoa đầu ngành</h2>
        <p>Đội ngũ bác sĩ tận tâm, tận lực và giàu kinh nghiệm luôn sẵn sàng phục vụ sức khỏe của bạn.</p>
      </div>

      <div class="doctors-grid">
        
        <div class="doctor-premium-card">
          <div class="doctor-image-wrapper">
            <v-img cover height="300" src="https://lh3.googleusercontent.com/aida-public/AB6AXuC24E-nLI9uPt1LeRLQOoU5rssjsKB1tE_qJICztXVWvOyRDSb-HdS5-hr4_jKhzfcxrxKA_oXNWnSN6qs3i7a5Utyy_YtFsHXNiz84unIUNhIlg3rXFQhgai26z0kUGiqDw6PkemUX_nQG3dC0A5-gJjG0sCnPaKItPaMEB5lz-2sdtzu7MHoNl0gu11ZzAkoT5mQx5WnYX3WepxIAz4ivlwiImYl6PFEL4PSpCfNBZAgWFt-mD2JbNTlV9NxwyKr93G6vOzZlmn3O" />
            <div class="doctor-overlay-info">
              <h4>BS. Nguyễn Văn An</h4>
              <p>Khoa Nội tổng quát</p>
            </div>
          </div>
        </div>

        <div class="doctor-premium-card">
          <div class="doctor-image-wrapper">
            <v-img cover height="300" src="https://lh3.googleusercontent.com/aida-public/AB6AXuBZ4MgdGYpNjNLyFJHa3Rgov39Rqjdvw1It5-uUtcMk1tQ_MhPB8ere1R2Pr5WnmnCxFaTfHB1-39GEkrCBGHd4eLWJoP-_3M8bJ0VrGw8d4c657XzKP8hDw6o6UluBIMnpPy2T80WdEHk7QOHRhh6_Iyds-thpJSgGSS2fROB1XEobnr8IYQNQ1c7j8Hmx8HgLcm5XEl0OCQ72NSvXsCHSmXEUfdZ_nKbonbBzhaW2s74N2Cg1NgeFA6V2RwCQa7oO-0_-eYcTLWW2" />
            <div class="doctor-overlay-info">
              <h4>BS. Trần Thị Bình</h4>
              <p>Khoa Nhi khoa</p>
            </div>
          </div>
        </div>

        <div class="doctor-premium-card">
          <div class="doctor-image-wrapper">
            <v-img cover height="300" src="https://lh3.googleusercontent.com/aida-public/AB6AXuBHpw2sjC3D4ziEey2UPeQACgfJmkuXyn-Q56pB308kzkpS0uDFO1Vl9_bFMbDpun-GQx9dEIaJQBOtKkzX_Bso0aOMBqm3ccBhDuj5LaHt8SXkAHn_44evZISCMKR5ODN2fe8mH232eclYw-0bIOfw_4gY9njZ832rBPXca6c77Szjbm_6EJhdRvn_ESCH-amsO8U_-r_ejbnO0a_k9Ok3GiGuoUVKB_Ib7hj6x13t4EReNchkyHrOfOkMMXHFeHhyFttcZXW8-7WG" />
            <div class="doctor-overlay-info">
              <h4>BS. Lê Hoàng Nam</h4>
              <p>Khoa Da liễu</p>
            </div>
          </div>
        </div>

      </div>
    </section>

    <!-- Footer Premium -->
    <footer class="public-footer-premium">
      <div class="footer-container">
        <div class="footer-brand">
          <i class="fa-solid fa-heart-pulse"></i>
          <span>ClinicFlow</span>
        </div>
        <p class="footer-desc">Giải pháp số hóa toàn diện quy trình quản lý phòng khám và liên thông dữ liệu y tế thế hệ mới.</p>
        <div class="footer-divider"></div>
        <p class="footer-copy">&copy; 2026 ClinicFlow. Bản quyền thiết kế cao cấp thuộc về nhóm phát triển.</p>
      </div>
    </footer>

  </div>
</template>

<script>
import { useRouter } from 'vue-router'

export default {
  name: 'LandingPage',
  setup() {
    const router = useRouter()

    const goToAuth = (mode) => {
      router.push({ name: 'Auth', query: { mode } })
    }

    return {
      goToAuth
    }
  }
}
</script>

<style scoped>
/* NAVBAR GLASSMORPHISM */
.public-nav-glass {
  background: rgba(255, 255, 255, 0.85);
  backdrop-filter: blur(16px);
  -webkit-backdrop-filter: blur(16px);
  border-bottom: 1px solid rgba(241, 245, 249, 0.6);
  padding: 16px 40px;
  position: sticky;
  top: 0;
  z-index: 100;
  box-shadow: 0 10px 30px -10px rgba(15, 23, 42, 0.05);
}

.nav-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  max-width: 1200px;
  margin: 0 auto;
}

.nav-logo {
  display: flex;
  align-items: center;
  gap: 12px;
  cursor: pointer;
}

.logo-icon-wrapper {
  width: 36px;
  height: 36px;
  background: linear-gradient(135deg, var(--primary-color) 0%, #3b82f6 100%);
  border-radius: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #ffffff;
  font-size: 18px;
  box-shadow: 0 4px 12px rgba(30, 64, 175, 0.2);
}

.brand-title {
  font-size: 20px;
  font-weight: 700;
  color: var(--text-primary);
  letter-spacing: -0.5px;
}

.nav-links {
  display: flex;
  gap: 32px;
}

.nav-link-item {
  text-decoration: none;
  color: var(--text-secondary);
  font-weight: 500;
  font-size: 14px;
  transition: all 0.25s ease;
  padding: 6px 0;
  position: relative;
}

.nav-link-item:hover,
.nav-link-item.active {
  color: var(--primary-color);
}

.nav-link-item::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 0;
  height: 2px;
  background-color: var(--primary-color);
  transition: width 0.25s ease;
}

.nav-link-item:hover::after,
.nav-link-item.active::after {
  width: 100%;
}

.nav-auth {
  display: flex;
  gap: 16px;
}

/* BUTTONS */
.btn-premium {
  padding: 10px 22px;
  border-radius: 8px;
  font-weight: 600;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
  border: none;
  outline: none;
}

.btn-primary {
  background: linear-gradient(135deg, var(--primary-color) 0%, #3b82f6 100%);
  color: #ffffff;
  box-shadow: 0 4px 12px rgba(30, 64, 175, 0.2);
}

.btn-primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(30, 64, 175, 0.3);
}

.btn-secondary {
  background-color: #f1f5f9;
  color: var(--text-primary);
}

.btn-secondary:hover {
  background-color: #e2e8f0;
}

.btn-outline {
  border: 1.5px solid rgba(255, 255, 255, 0.4);
  background-color: transparent;
  color: #ffffff;
}

.btn-outline:hover {
  background-color: rgba(255, 255, 255, 0.1);
  border-color: #ffffff;
  transform: translateX(4px);
}

.btn-large {
  padding: 14px 28px;
  font-size: 15px;
}

/* HERO SECTION PREMIUM */
.hero-section-premium {
  position: relative;
  background: linear-gradient(135deg, #070b19 0%, #111827 50%, #1e3a8a 100%);
  color: #ffffff;
  padding: 140px 40px;
  text-align: center;
  overflow: hidden;
}

.hero-bg-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-image: radial-gradient(circle at 80% 20%, rgba(59, 130, 246, 0.15) 0%, transparent 50%);
  pointer-events: none;
}

.hero-container {
  max-width: 800px;
  margin: 0 auto;
  position: relative;
  z-index: 10;
}

.hero-tag {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: rgba(255, 255, 255, 0.08);
  border: 1px solid rgba(255, 255, 255, 0.12);
  border-radius: 9999px;
  padding: 6px 16px;
  margin-bottom: 24px;
}

.tag-accent-line {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background-color: var(--accent-color);
  box-shadow: 0 0 10px var(--accent-color);
}

.tag-text {
  font-size: 12px;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 1px;
  color: #e2e8f0;
}

.hero-title {
  font-size: clamp(36px, 5vw, 56px);
  font-weight: 800;
  line-height: 1.15;
  margin-bottom: 24px;
  letter-spacing: -1.5px;
}

.text-glow {
  background: linear-gradient(135deg, #60a5fa 0%, var(--accent-color) 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.hero-desc {
  font-size: 17px;
  line-height: 1.7;
  color: #94a3b8;
  margin-bottom: 40px;
}

.hero-actions {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 20px;
  flex-wrap: wrap;
}

/* BENTO SERVICES */
.public-main-premium {
  max-width: 1200px;
  margin: 96px auto;
  padding: 0 40px;
}

.section-header-premium {
  text-align: center;
  margin-bottom: 56px;
}

.section-subtitle {
  font-size: 12px;
  font-weight: 600;
  text-transform: uppercase;
  color: var(--primary-color);
  letter-spacing: 2px;
  display: block;
  margin-bottom: 8px;
}

.section-header-premium h2 {
  font-size: 32px;
  font-weight: 700;
  color: var(--text-primary);
  margin-bottom: 16px;
  letter-spacing: -0.5px;
}

.section-header-premium p {
  color: var(--text-secondary);
  font-size: 15px;
  max-width: 600px;
  margin: 0 auto;
  line-height: 1.6;
}

.bento-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
  gap: 32px;
}

.bento-card {
  background: #ffffff;
  border: 1px solid #f1f5f9;
  border-radius: 20px;
  padding: 40px 32px;
  box-shadow: var(--shadow-md);
  transition: all 0.35s cubic-bezier(0.4, 0, 0.2, 1);
  position: relative;
  overflow: hidden;
}

.bento-card:hover {
  transform: translateY(-8px);
  box-shadow: var(--shadow-xl);
}

.card-icon-circle {
  width: 64px;
  height: 64px;
  border-radius: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
  margin-bottom: 24px;
}

.bg-blue-glow {
  background: rgba(30, 64, 175, 0.08);
  color: var(--primary-color);
}
.bg-emerald-glow {
  background: rgba(0, 160, 107, 0.08);
  color: var(--accent-color);
}
.bg-red-glow {
  background: rgba(239, 68, 68, 0.08);
  color: var(--danger-color);
}

.bento-card h3 {
  font-size: 20px;
  font-weight: 700;
  color: var(--text-primary);
  margin-bottom: 12px;
  letter-spacing: -0.3px;
}

.bento-card p {
  color: var(--text-secondary);
  font-size: 14px;
  line-height: 1.6;
  margin-bottom: 0;
}

/* STATS SECTION PREMIUM */
.stats-section-premium {
  background: linear-gradient(135deg, #0f172a 0%, #1e293b 100%);
  color: #ffffff;
  padding: 80px 40px;
}

.stats-container {
  max-width: 1200px;
  margin: 0 auto;
}

.stats-row {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 40px;
  text-align: center;
}

.stat-number {
  font-size: 44px;
  font-weight: 800;
  color: var(--accent-color);
  margin-bottom: 8px;
  letter-spacing: -1px;
}

.stat-label {
  font-size: 13px;
  color: #94a3b8;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

/* DOCTORS GRID */
.doctors-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 32px;
}

.doctor-premium-card {
  border-radius: 20px;
  overflow: hidden;
  box-shadow: var(--shadow-md);
  transition: all 0.35s cubic-bezier(0.4, 0, 0.2, 1);
  border: 1px solid #f1f5f9;
}

.doctor-premium-card:hover {
  transform: translateY(-8px);
  box-shadow: var(--shadow-xl);
}

.doctor-image-wrapper {
  position: relative;
  overflow: hidden;
}

.doctor-overlay-info {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  background: linear-gradient(to top, rgba(15, 23, 42, 0.95) 0%, rgba(15, 23, 42, 0.4) 70%, transparent 100%);
  padding: 24px;
  color: #ffffff;
}

.doctor-overlay-info h4 {
  font-size: 18px;
  font-weight: 700;
  margin: 0 0 4px 0;
}

.doctor-overlay-info p {
  font-size: 13px;
  color: #94a3b8;
  margin: 0;
}

/* FOOTER PREMIUM */
.public-footer-premium {
  background-color: #0b0f19;
  color: #ffffff;
  padding: 64px 40px 48px 40px;
  text-align: center;
  border-top: 1px solid #1e293b;
}

.footer-container {
  max-width: 1200px;
  margin: 0 auto;
}

.footer-brand {
  display: inline-flex;
  align-items: center;
  gap: 12px;
  font-size: 24px;
  font-weight: 700;
  margin-bottom: 16px;
}

.footer-brand i {
  color: var(--accent-color);
}

.footer-desc {
  color: #64748b;
  font-size: 14px;
  max-width: 500px;
  margin: 0 auto 32px auto;
  line-height: 1.6;
}

.footer-divider {
  height: 1px;
  background-color: #1e293b;
  margin-bottom: 24px;
}

.footer-copy {
  color: #475569;
  font-size: 12px;
  margin-bottom: 0;
}
</style>
