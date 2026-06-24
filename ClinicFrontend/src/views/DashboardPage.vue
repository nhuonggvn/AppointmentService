<template>
  <v-layout>
    <!-- =====================================================
         SIDEBAR NAVIGATION DRAWER (Co rút khi TV mode)
         ===================================================== -->
    <v-navigation-drawer
      v-model="sidebarOpen"
      :rail="sidebarRail"
      permanent
      color="surface"
      border
      width="260"
    >
      <!-- Logo & Branding -->
      <div class="sidebar-logo d-flex align-center pa-4" style="height: 64px; border-bottom: 1px solid var(--border-color);">
        <i class="fa-solid fa-heart-pulse text-primary flex-shrink-0" style="font-size: 24px;"></i>
        <Transition name="fade">
          <div v-if="!sidebarRail" class="ml-3">
            <div class="text-subtitle-1 font-weight-black text-primary" style="line-height: 1.1;">ClinicFlow</div>
            <div class="text-caption text-medium-emphasis" style="font-size: 10px;">Quản lý phòng khám</div>
          </div>
        </Transition>
      </div>

      <!-- Navigation Menu -->
      <v-list nav class="px-2 py-3" density="compact">

        <!-- Patient: Dat lich kham -->
        <v-list-item
          v-if="currentUser.token && hasRole('Patient, Admin')"
          :active="activeTab === 'booking'"
          value="booking"
          rounded="lg"
          class="mb-1 sidebar-item"
          :class="{ 'sidebar-item-active': activeTab === 'booking' }"
          @click="activeTab = 'booking'"
        >
          <template v-slot:prepend>
            <v-icon :icon="activeTab === 'booking' ? 'mdi-calendar-plus' : 'mdi-calendar-plus-outline'" :color="activeTab === 'booking' ? 'primary' : 'medium-emphasis'" />
          </template>
          <v-list-item-title class="font-weight-medium">Đặt Lịch Khám</v-list-item-title>
          <template v-slot:subtitle>
            <span class="text-caption">Bệnh nhân</span>
          </template>
        </v-list-item>

        <!-- Receptionist: Tiep tan -->
        <v-list-item
          v-if="currentUser.token && hasRole('Receptionist, Nurse, Admin')"
          :active="activeTab === 'reception'"
          value="reception"
          rounded="lg"
          class="mb-1 sidebar-item"
          :class="{ 'sidebar-item-active': activeTab === 'reception' }"
          @click="activeTab = 'reception'"
        >
          <template v-slot:prepend>
            <v-icon :icon="activeTab === 'reception' ? 'mdi-clipboard-text-play' : 'mdi-clipboard-text-play-outline'" :color="activeTab === 'reception' ? 'primary' : 'medium-emphasis'" />
          </template>
          <v-list-item-title class="font-weight-medium">Quầy Tiếp Tân</v-list-item-title>
          <template v-slot:subtitle>
            <span class="text-caption">Tiếp tân / Y tá</span>
          </template>
        </v-list-item>

        <!-- Doctor: Phong kham -->
        <v-list-item
          v-if="currentUser.token && hasRole('Doctor, Admin')"
          :active="activeTab === 'doctor'"
          value="doctor"
          rounded="lg"
          class="mb-1 sidebar-item"
          :class="{ 'sidebar-item-active': activeTab === 'doctor' }"
          @click="activeTab = 'doctor'"
        >
          <template v-slot:prepend>
            <v-icon :icon="activeTab === 'doctor' ? 'mdi-doctor' : 'mdi-doctor'" :color="activeTab === 'doctor' ? 'primary' : 'medium-emphasis'" />
          </template>
          <v-list-item-title class="font-weight-medium">Phòng Khám Bác Sĩ</v-list-item-title>
          <template v-slot:subtitle>
            <span class="text-caption">Bác sĩ</span>
          </template>
        </v-list-item>

        <!-- Pharmacy: Kho thuoc -->
        <v-list-item
          v-if="currentUser.token && hasRole('Receptionist, Nurse, Admin')"
          :active="activeTab === 'pharmacy'"
          value="pharmacy"
          rounded="lg"
          class="mb-1 sidebar-item"
          :class="{ 'sidebar-item-active': activeTab === 'pharmacy' }"
          @click="activeTab = 'pharmacy'"
        >
          <template v-slot:prepend>
            <v-icon icon="mdi-pill" :color="activeTab === 'pharmacy' ? 'primary' : 'medium-emphasis'" />
          </template>
          <v-list-item-title class="font-weight-medium">Kho Thuốc</v-list-item-title>
          <template v-slot:subtitle>
            <span class="text-caption">Dược sĩ</span>
          </template>
        </v-list-item>

        <!-- Billing: Vien phi -->
        <v-list-item
          v-if="currentUser.token && hasRole('Receptionist, Nurse, Admin')"
          :active="activeTab === 'billing'"
          value="billing"
          rounded="lg"
          class="mb-1 sidebar-item"
          :class="{ 'sidebar-item-active': activeTab === 'billing' }"
          @click="activeTab = 'billing'"
        >
          <template v-slot:prepend>
            <v-icon :icon="activeTab === 'billing' ? 'mdi-cash-register' : 'mdi-cash-register'" :color="activeTab === 'billing' ? 'primary' : 'medium-emphasis'" />
          </template>
          <v-list-item-title class="font-weight-medium">Viện Phí</v-list-item-title>
          <template v-slot:subtitle>
            <span class="text-caption">Thu ngân</span>
          </template>
        </v-list-item>

        <!-- Admin: Quan tri -->
        <v-list-item
          v-if="currentUser.token && hasRole('Admin')"
          :active="activeTab === 'admin'"
          value="admin"
          rounded="lg"
          class="mb-1 sidebar-item"
          :class="{ 'sidebar-item-active': activeTab === 'admin' }"
          @click="activeTab = 'admin'"
        >
          <template v-slot:prepend>
            <v-icon :icon="activeTab === 'admin' ? 'mdi-shield-crown' : 'mdi-shield-crown-outline'" :color="activeTab === 'admin' ? 'primary' : 'medium-emphasis'" />
          </template>
          <v-list-item-title class="font-weight-medium">Quản Trị Hệ Thống</v-list-item-title>
          <template v-slot:subtitle>
            <span class="text-caption">Admin</span>
          </template>
        </v-list-item>

        <!-- Tivi hang cho (Moi nguoi xem) -->
        <v-list-item
          :active="activeTab === 'tv'"
          value="tv"
          rounded="lg"
          class="mb-1 sidebar-item"
          :class="{ 'sidebar-item-active': activeTab === 'tv' }"
          @click="activeTab = 'tv'"
        >
          <template v-slot:prepend>
            <v-icon icon="mdi-television-classic" :color="activeTab === 'tv' ? 'warning' : 'medium-emphasis'" />
          </template>
          <v-list-item-title class="font-weight-medium">Màn Hình Tivi</v-list-item-title>
          <template v-slot:subtitle>
            <span class="text-caption">Hàng chờ công cộng</span>
          </template>
        </v-list-item>

      </v-list>

      <!-- Spacer -->
      <v-spacer />

      <!-- Footer: User Info + Actions -->
      <template v-slot:append>
        <v-divider />

        <!-- Rail toggle button -->
        <div class="pa-2 text-center">
          <v-btn
            :icon="sidebarRail ? 'mdi-chevron-right' : 'mdi-chevron-left'"
            variant="text"
            size="small"
            @click="sidebarRail = !sidebarRail"
          />
        </div>

        <!-- User profile -->
        <div v-if="currentUser.token && !sidebarRail" class="pa-3" style="border-top: 1px solid rgba(0,0,0,0.06);">
          <div class="d-flex align-center gap-3 mb-2">
            <v-avatar color="primary" size="36" class="text-caption font-weight-bold text-white flex-shrink-0">
              {{ (currentUser.username || 'TV').substring(0, 2).toUpperCase() }}
            </v-avatar>
            <div class="flex-grow-1" style="min-width: 0;">
              <div class="text-caption font-weight-bold text-truncate">{{ currentUser.username }}</div>
              <v-chip size="x-small" color="primary" variant="flat" class="mt-1">{{ currentUser.role }}</v-chip>
            </div>
          </div>
          <v-btn
            block
            variant="tonal"
            color="error"
            size="small"
            prepend-icon="mdi-logout"
            class="font-weight-medium"
            @click="handleLogout"
          >
            Đăng xuất
          </v-btn>
        </div>

        <!-- Rail mode: compact user avatar + logout -->
        <div v-else-if="currentUser.token && sidebarRail" class="pa-2 d-flex flex-column align-center gap-2" style="border-top: 1px solid rgba(0,0,0,0.06);">
          <v-avatar color="primary" size="32" class="text-caption font-weight-bold text-white">
            {{ (currentUser.username || 'TV').substring(0, 2).toUpperCase() }}
          </v-avatar>
          <v-btn icon="mdi-logout" variant="text" color="error" size="x-small" @click="handleLogout" />
        </div>
      </template>
    </v-navigation-drawer>

    <!-- =====================================================
         HEADER BAR (Luon hien thi khi da dang nhap)
         ===================================================== -->
    <v-app-bar flat border="b" color="surface" height="64">
      <!-- Hamburger toggle -->
      <v-btn
        :icon="sidebarOpen ? 'mdi-menu-open' : 'mdi-menu'"
        variant="text"
        class="ml-1"
        @click="sidebarOpen = !sidebarOpen"
      />

      <!-- Logo (chi hien khi sidebar dong) -->
      <v-app-bar-title v-if="!sidebarOpen" class="font-weight-black text-primary">
        <v-icon icon="mdi-heart-pulse" color="primary" class="mr-1" />
        ClinicFlow
      </v-app-bar-title>

      <v-spacer />

      <!-- Search Box -->
      <v-text-field
        v-model="headerSearch"
        placeholder="Tìm bệnh nhân, lịch khám..."
        prepend-inner-icon="mdi-magnify"
        variant="outlined"
        density="compact"
        hide-details
        rounded="lg"
        bg-color="background"
        style="max-width: 320px;"
        class="mr-3"
      />

      <!-- Notification Bell -->
      <v-btn icon="mdi-bell-outline" variant="text" color="medium-emphasis" class="mr-1">
        <v-icon />
        <v-badge color="error" content="3" floating />
      </v-btn>

      <!-- API Config Button -->
      <v-menu v-model="menuOpen" :close-on-content-click="false" location="bottom end">
        <template v-slot:activator="{ props }">
          <v-btn v-bind="props" icon="mdi-cog-outline" variant="text" color="medium-emphasis" class="mr-2" />
        </template>
        <v-card min-width="340" class="pa-4 bg-surface" elevation="8" rounded="lg">
          <div class="text-subtitle-1 font-weight-bold mb-3">Cài đặt kết nối API</div>
          <v-text-field
            v-model="apiUrl"
            label="API URL Base"
            density="compact"
            variant="outlined"
            placeholder="http://localhost:5000/api"
            hide-details
            class="mb-3"
            @keyup.enter="applyConfiguration"
          />
          <v-text-field
            v-model="authApiUrl"
            label="API Đăng Nhập (Nhóm 6)"
            density="compact"
            variant="outlined"
            placeholder="http://26.71.15.204:5000/api"
            hide-details
            class="mb-3"
            @keyup.enter="applyConfiguration"
          />
          <v-text-field
            v-model="medicalApiUrl"
            label="API Bệnh Án (Nhóm 4)"
            density="compact"
            variant="outlined"
            placeholder="http://26.15.45.202:5000/api"
            hide-details
            class="mb-3"
            @keyup.enter="applyConfiguration"
          />
          <v-text-field
            v-model="jwtToken"
            label="JWT Token"
            density="compact"
            variant="outlined"
            hide-details
            class="mb-3"
            @keyup.enter="applyConfiguration"
          />
          <v-btn block color="primary" size="small" class="mb-2 font-weight-bold" @click="applyConfiguration">
            Áp dụng cấu hình
          </v-btn>
          <div v-if="activeRole" class="text-caption text-success mt-1">
            Vai trò hiện tại: <strong>{{ activeRole }}</strong>
          </div>
        </v-card>
      </v-menu>

      <!-- User Avatar Chip (chi hien khi chua dang nhap hoac dang o che do TV) -->
      <div v-if="!currentUser.token" class="mr-2">
        <v-btn color="primary" size="small" variant="flat" prepend-icon="mdi-login" @click="router.push({ name: 'Auth' })">
          Đăng nhập
        </v-btn>
      </div>
    </v-app-bar>

    <!-- =====================================================
         MAIN CONTENT AREA
         ===================================================== -->
    <v-main class="bg-background">
      <v-container fluid class="pa-6">

        <!-- MAN HINH DANG NHAP (Hien thi neu chua dang nhap va khong phai TV) -->
        <div v-if="!currentUser.token && activeTab !== 'tv'" class="d-flex align-center justify-center" style="min-height: 75vh;">
          <v-card width="480" class="pa-8 bg-surface rounded-xl" elevation="4">
            <div class="text-center mb-6">
              <div class="sidebar-logo-icon mx-auto mb-4">
                <v-icon icon="mdi-heart-pulse" color="primary" size="48" />
              </div>
              <h2 class="text-h5 font-weight-black text-primary mb-1">Chào mừng đến ClinicFlow</h2>
              <div class="text-body-2 text-medium-emphasis">Hệ thống đặt lịch & quản lý phòng khám liên thông</div>
            </div>

            <!-- Login Form -->
            <v-form @submit.prevent="handleLogin">
              <v-text-field
                v-model="loginForm.username"
                label="Tên đăng nhập"
                prepend-inner-icon="mdi-account-outline"
                variant="outlined"
                density="comfortable"
                rounded="lg"
                class="mb-3"
                required
              />
              <v-text-field
                v-model="loginForm.password"
                label="Mật khẩu"
                prepend-inner-icon="mdi-lock-outline"
                type="password"
                variant="outlined"
                density="comfortable"
                rounded="lg"
                class="mb-5"
                required
              />
              <v-btn type="submit" color="primary" block size="large" rounded="lg" class="font-weight-bold mb-4" :loading="loading">
                Đăng Nhập vào Hệ Thống
              </v-btn>
            </v-form>

            <v-divider class="mb-4" />

            <div class="text-center">
              <v-btn variant="tonal" color="warning" size="small" prepend-icon="mdi-television-classic" @click="activeTab = 'tv'">
                Xem Tivi Hàng Chờ (Công cộng)
              </v-btn>
            </div>
          </v-card>
        </div>

        <!-- GIAO DIEN CHINH SAU KHI DANG NHAP HOAC XEM TV -->
        <div v-else>

        <!-- Dynamic Alert Notification -->
        <v-alert
          v-if="alert.show"
          :type="alert.type"
          closable
          class="mb-6"
          variant="tonal"
          rounded="lg"
          @click:close="alert.show = false"
        >
          {{ alert.message }}
        </v-alert>

        <!-- Tabs Content -->
        <v-window v-model="activeTab">
          <!-- 1. PATIENT BOOKING TAB -->
          <v-window-item value="booking">
            <v-row>
              <!-- Doctor & Schedule Selection -->
              <v-col cols="12" md="7">
                <v-card border flat class="bg-surface pa-6 mb-6">
                  <div class="d-flex align-center mb-4">
                    <v-icon icon="mdi-doctor" color="primary" class="mr-2" />
                    <h2 class="text-h6 font-weight-bold">Bước 1: Chọn bác sĩ & ca khám</h2>
                  </div>

                  <!-- Specialty filter -->
                  <v-select
                    v-model="selectedSpecialty"
                    :items="specialties"
                    label="Lọc theo Chuyên Khoa"
                    variant="outlined"
                    density="comfortable"
                    clearable
                    class="mb-4"
                    @update:model-value="onFilterChange"
                  />

                  <!-- Doctors List -->
                  <div class="text-subtitle-2 font-weight-bold mb-2">Danh sách Bác sĩ</div>
                  <v-row>
                    <v-col v-for="doc in filteredDoctors" :key="doc.id" cols="12" sm="6">
                      <v-card
                        border
                        flat
                        :color="bookingForm.doctorId === doc.id ? 'primary-darken-1' : 'surface'"
                        class="pa-4 cursor-pointer hover-card"
                        :style="bookingForm.doctorId === doc.id ? 'border: 2px solid #003D9B !important' : ''"
                        @click="selectDoctor(doc)"
                      >
                        <div class="d-flex justify-between align-center mb-1">
                          <span class="font-weight-bold text-subtitle-1">{{ doc.fullName }}</span>
                          <v-chip size="x-small" color="primary" variant="flat">{{ doc.specialty }}</v-chip>
                        </div>
                        <div class="text-caption mb-2" :class="bookingForm.doctorId === doc.id ? 'text-grey-lighten-2' : 'text-grey-darken-1'">{{ doc.qualifications }}</div>
                        <div class="d-flex justify-space-between align-center">
                          <span class="text-caption text-grey">Phí khám:</span>
                          <span class="font-weight-bold text-success">{{ formatMoney(doc.consultationFee) }}đ</span>
                        </div>
                      </v-card>
                    </v-col>
                  </v-row>

                  <v-divider class="my-6" />

                  <!-- Available Schedules -->
                  <div class="d-flex align-center mb-3">
                    <v-icon icon="mdi-clock-outline" color="secondary" class="mr-2" />
                    <div class="text-subtitle-1 font-weight-bold">Ca khám trống khả dụng</div>
                  </div>

                  <v-alert v-if="!bookingForm.doctorId" type="info" variant="text" density="comfortable">
                    Vui lòng chọn một bác sĩ ở danh sách trên để xem ca khám trống.
                  </v-alert>
                  <v-alert v-else-if="availableSchedules.length === 0" type="warning" variant="text" density="comfortable">
                    Bác sĩ này hiện tại không có ca làm việc nào còn trống trong 7 ngày tới.
                  </v-alert>

                  <v-row v-else>
                    <v-col v-for="sch in availableSchedules" :key="sch.id" cols="12" sm="6">
                      <v-card
                        border
                        flat
                        :color="bookingForm.scheduleId === sch.id ? 'secondary-darken-1' : 'surface'"
                        class="pa-3 cursor-pointer hover-card"
                        :style="bookingForm.scheduleId === sch.id ? 'border: 2px solid #818CF8 !important' : ''"
                        @click="selectSchedule(sch)"
                      >
                        <div class="d-flex justify-space-between align-center mb-1">
                          <span class="font-weight-bold">{{ formatDate(sch.date) }}</span>
                          <v-chip size="x-small" :color="sch.shift === 'Sang' ? 'warning' : 'info'">Ca {{ sch.shift }}</v-chip>
                        </div>
                        <div class="text-caption mb-1">
                          Thời gian: <strong>{{ formatTimeSpan(sch.startTime) }} - {{ formatTimeSpan(sch.endTime) }}</strong>
                        </div>
                        <div class="d-flex justify-space-between align-center">
                          <span class="text-caption">Đã đặt:</span>
                          <v-chip size="x-small" variant="outlined" color="success">
                            {{ sch.currentBookings }} / {{ sch.maxPatients }}
                          </v-chip>
                        </div>
                      </v-card>
                    </v-col>
                  </v-row>
                </v-card>
              </v-col>

              <!-- Patient Information & Submit -->
              <v-col cols="12" md="5">
                <v-card border flat class="bg-surface pa-6">
                  <div class="d-flex align-center mb-4">
                    <v-icon icon="mdi-account-details" color="success" class="mr-2" />
                    <h2 class="text-h6 font-weight-bold">Bước 2: Thông tin bệnh nhân</h2>
                  </div>

                  <v-form @submit.prevent="bookAppointment">
                    <v-text-field
                      v-model="bookingForm.patientName"
                      label="Họ và tên Bệnh nhân"
                      variant="outlined"
                      class="mb-3"
                      required
                    />
                    <v-text-field
                      v-model="bookingForm.patientPhone"
                      label="Số điện thoại liên hệ"
                      variant="outlined"
                      class="mb-3"
                      required
                    />
                    <v-text-field
                      v-model="bookingForm.patientEmail"
                      label="Email liên hệ"
                      variant="outlined"
                      class="mb-3"
                    />
                    
                    <!-- Choose specific time slot inside shift hours -->
                    <div class="mb-3">
                      <div class="text-subtitle-2 mb-2 font-weight-bold text-grey-darken-3">Chọn khung giờ khám cụ thể:</div>
                      
                      <div v-if="bookingForm.scheduleId">
                        <v-chip-group
                          v-model="bookingForm.timeSlot"
                          column
                          selected-class="bg-primary text-white"
                        >
                          <v-chip
                            v-for="slot in activeTimeSlotsList"
                            :key="slot.time"
                            :value="slot.time"
                            :disabled="slot.isBooked"
                            :color="slot.isBooked ? 'error' : (bookingForm.timeSlot === slot.time ? 'primary' : 'default')"
                            variant="elevated"
                            class="ma-1 font-weight-bold"
                          >
                            {{ slot.displayTime }}
                            <span v-if="slot.isBooked" class="text-caption ml-1 font-weight-regular">(Đã đặt)</span>
                          </v-chip>
                        </v-chip-group>
                        
                        <div v-if="!bookingForm.timeSlot" class="text-caption text-red-darken-2 mt-1 font-weight-bold">
                          * Vui lòng chọn một khung giờ khám còn trống ở trên.
                        </div>
                      </div>
                      
                      <div v-else class="text-caption text-grey-darken-1 italic">
                        * Vui lòng chọn một ca khám trống khả dụng ở bên trái để hiển thị danh sách giờ khám.
                      </div>
                    </div>

                    <v-textarea
                      v-model="bookingForm.notes"
                      label="Triệu chứng lâm sàng / Ghi chú"
                      variant="outlined"
                      rows="3"
                      class="mb-4"
                    />

                    <v-btn
                      type="submit"
                      color="primary"
                      block
                      size="large"
                      class="font-weight-bold"
                      :disabled="!bookingForm.doctorId || !bookingForm.scheduleId || !bookingForm.timeSlot || loading"
                      :loading="loading"
                    >
                      Đăng Ký Đặt Lịch Khám
                    </v-btn>
                  </v-form>
                </v-card>

                <!-- Ticket details showing after booking success -->
                <v-card v-if="recentTicket" border flat class="bg-surface pa-6 mt-6 border-success">
                  <div class="d-flex align-center justify-space-between mb-3">
                    <span class="text-success font-weight-bold d-flex align-center">
                      <v-icon icon="mdi-check-circle" class="mr-1" />
                      Đặt lịch thành công!
                    </span>
                    <v-chip color="success" size="small">{{ recentTicket.status }}</v-chip>
                  </div>
                  <div class="text-center bg-slate-900 pa-4 rounded-lg border border-dashed mb-4">
                    <div class="text-caption text-grey">MÃ LỊCH HẸN</div>
                    <div class="text-h4 font-weight-black text-primary">{{ recentTicket.appointmentCode }}</div>
                  </div>
                  <div class="text-subtitle-2 mb-1">Bác sĩ: <strong>{{ recentTicket.doctorName }}</strong></div>
                  <div class="text-subtitle-2 mb-1">Bệnh nhân: <strong>{{ recentTicket.patientName }}</strong></div>
                  <div class="text-subtitle-2 mb-1">Ngày hẹn: <strong>{{ formatDate(recentTicket.appointmentDate) }}</strong></div>
                  <div class="text-subtitle-2 mb-1">Giờ khám: <strong>{{ recentTicket.timeSlot }}</strong></div>
                  <div class="text-caption text-warning mt-2">
                    Lưu ý: Bệnh nhân mang mã lịch hẹn này đến quầy tiếp tân vào ngày khám để xác nhận và lấy số thứ tự.
                  </div>
                </v-card>
              </v-col>
            </v-row>

            <!-- BỆNH NHÂN: LỊCH SỬ KHÁM BỆNH & ĐƠN THUỐC CŨ -->
            <v-row class="mt-6">
              <v-col cols="12">
                <v-card border flat class="bg-surface pa-6">
                  <div class="d-flex align-center mb-4">
                    <v-icon icon="mdi-history" color="info" class="mr-2" />
                    <h2 class="text-h6 font-weight-bold">Tra cứu Lịch sử khám bệnh & Đơn thuốc cũ</h2>
                  </div>
                  <v-row density="comfortable" class="align-center">
                    <v-col cols="12" sm="8" md="6">
                      <v-text-field
                        v-model="patientSearchPhone"
                        label="Nhập số điện thoại bệnh nhân"
                        variant="outlined"
                        density="comfortable"
                        prepend-inner-icon="mdi-phone"
                        placeholder="Ví dụ: 0987654321"
                        hide-details
                        @keyup.enter="fetchPatientHistory(patientSearchPhone)"
                      />
                    </v-col>
                    <v-col cols="12" sm="4" md="2">
                      <v-btn
                        block
                        color="info"
                        size="large"
                        class="font-weight-bold"
                        prepend-icon="mdi-magnify"
                        :loading="patientHistoryLoading"
                        @click="fetchPatientHistory(patientSearchPhone)"
                      >
                        Tra Cứu
                      </v-btn>
                    </v-col>
                  </v-row>

                  <v-divider class="my-6" v-if="patientHistories.length > 0 || patientHistoryLoading" />

                  <v-progress-linear v-if="patientHistoryLoading" indeterminate color="info" class="mb-4" />

                  <div v-if="patientHistories.length > 0">
                    <div class="text-subtitle-1 font-weight-bold mb-4 text-info">Lịch sử các lần khám trước:</div>
                    <v-expansion-panels variant="popout">
                      <v-expansion-panel
                        v-for="hist in patientHistories"
                        :key="hist.id"
                        class="mb-3 border rounded"
                        bg-color="slate-900"
                      >
                        <v-expansion-panel-title class="font-weight-bold text-subtitle-2">
                          <div class="d-flex align-center justify-space-between w-100 pr-4">
                            <span>Ngày khám: {{ formatDate(hist.appointmentDate) }} - Bác sĩ: {{ hist.doctorName }}</span>
                            <v-chip color="success" size="small">{{ hist.diagnosis }}</v-chip>
                          </div>
                        </v-expansion-panel-title>
                        <v-expansion-panel-text class="pt-3">
                          <div class="mb-2"><strong>Triệu chứng:</strong> {{ hist.symptoms }}</div>
                          <div class="mb-2"><strong>Chẩn đoán bệnh:</strong> {{ hist.diagnosis }}</div>
                          <div class="mb-3"><strong>Lời dặn của Bác sĩ:</strong> {{ hist.notes || 'Uống thuốc đúng liều lượng' }}</div>
                          
                          <div class="text-subtitle-2 font-weight-bold mb-2 text-success">Đơn thuốc được kê:</div>
                          <v-table density="comfortable" class="bg-surface rounded border mb-4">
                            <thead>
                              <tr>
                                <th class="text-left font-weight-bold">Tên thuốc</th>
                                <th class="text-left font-weight-bold">Số lượng</th>
                                <th class="text-left font-weight-bold">Liều dùng</th>
                              </tr>
                            </thead>
                            <tbody>
                              <tr v-for="item in hist.prescription" :key="item.drugId || item.name">
                                <td class="font-weight-bold text-primary">{{ item.drugName || item.name }}</td>
                                <td>{{ item.quantity }}</td>
                                <td>{{ item.dosage }}</td>
                              </tr>
                              <tr v-if="!hist.prescription || hist.prescription.length === 0">
                                <td colspan="3" class="text-center text-grey">Không kê đơn thuốc</td>
                              </tr>
                            </tbody>
                          </v-table>
                          
                          <div class="text-right">
                            <v-btn color="primary" class="font-weight-bold" prepend-icon="mdi-printer" @click="printPrescription(hist)">
                              In Đơn Thuốc
                            </v-btn>
                          </div>
                        </v-expansion-panel-text>
                      </v-expansion-panel>
                    </v-expansion-panels>
                  </div>
                </v-card>
              </v-col>
            </v-row>
          </v-window-item>

          <!-- 2. RECEPTIONIST BOARD TAB -->
          <v-window-item value="reception">
            <v-row>
              <!-- Search & Pending Bookings list -->
              <v-col cols="12" md="8">
                <v-card border flat class="bg-surface pa-6 mb-6">
                  <div class="d-flex align-center justify-space-between mb-4">
                    <div class="d-flex align-center">
                      <v-icon icon="mdi-calendar-clock" color="primary" class="mr-2" />
                      <h2 class="text-h6 font-weight-bold">Danh sách Lịch hẹn Chờ Xác Nhận</h2>
                    </div>
                    <v-btn size="small" variant="outlined" prepend-icon="mdi-refresh" @click="fetchPendingAppointments" :loading="loading">
                      Làm mới
                    </v-btn>
                  </div>

                  <!-- Quick search by SĐT or Mã -->
                  <v-row density="comfortable" class="mb-4">
                    <v-col cols="12" sm="5">
                      <v-text-field
                        v-model="searchQuery.code"
                        label="Tra cứu Mã lịch hẹn"
                        density="compact"
                        variant="outlined"
                        hide-details
                      />
                    </v-col>
                    <v-col cols="12" sm="5">
                      <v-text-field
                        v-model="searchQuery.phone"
                        label="Tra cứu Số điện thoại"
                        density="compact"
                        variant="outlined"
                        hide-details
                      />
                    </v-col>
                    <v-col cols="12" sm="2">
                      <v-btn block color="primary" height="40" @click="searchAppointments">
                        Tìm kiếm
                      </v-btn>
                    </v-col>
                  </v-row>

                  <!-- Search results if searching -->
                  <div v-if="searchResults !== null" class="mb-4">
                    <div class="d-flex align-center justify-space-between mb-2">
                      <span class="text-subtitle-2 text-primary">Kết quả tra cứu ({{ searchResults.length }})</span>
                      <v-btn size="x-small" variant="text" @click="clearSearch">Quay lại danh sách chờ</v-btn>
                    </div>
                    
                    <v-alert v-if="searchResults.length === 0" type="info" variant="tonal">
                      Không tìm thấy cuộc hẹn nào phù hợp.
                    </v-alert>

                    <v-row v-else density="comfortable">
                      <v-col v-for="app in searchResults" :key="app.id" cols="12">
                        <v-card border flat class="pa-4 bg-slate-900 d-flex justify-space-between align-center">
                          <div>
                            <div class="d-flex align-center gap-2 mb-1">
                              <span class="font-weight-bold text-subtitle-1">{{ app.patientName }}</span>
                              <v-chip size="x-small" color="primary">{{ app.appointmentCode }}</v-chip>
                              <v-chip size="x-small" :color="getStatusColor(app.status)">{{ app.status }}</v-chip>
                            </div>
                            <div class="text-caption text-grey">
                              Bác sĩ: <strong>{{ app.doctorName }}</strong> | Khung giờ: {{ app.timeSlot }} | Ngày: {{ formatDate(app.appointmentDate) }}
                            </div>
                          </div>
                          <div class="d-flex gap-2">
                            <v-btn
                              v-if="app.status === 'ChoXacNhan'"
                              color="success"
                              size="small"
                              @click="confirmAppointment(app.id)"
                            >
                              Xác Nhận & Xếp Lớp
                            </v-btn>
                            <v-btn
                              v-if="app.status !== 'DaHuy' && app.status !== 'DaKham'"
                              color="error"
                              variant="outlined"
                              size="small"
                              @click="cancelAppointment(app.id)"
                            >
                              Hủy Lịch
                            </v-btn>
                          </div>
                        </v-card>
                      </v-col>
                    </v-row>
                  </div>

                  <!-- Default list of pending appointments -->
                  <div v-else>
                    <v-alert v-if="pendingAppointments.length === 0" type="info" variant="text">
                      Hiện tại không có lịch hẹn nào đang ở trạng thái chờ xác nhận.
                    </v-alert>

                    <v-row v-else density="comfortable">
                      <v-col v-for="app in pendingAppointments" :key="app.id" cols="12">
                        <v-card border flat class="pa-4 d-flex justify-space-between align-center">
                          <div>
                            <div class="d-flex align-center gap-2 mb-1">
                              <span class="font-weight-bold text-subtitle-1">{{ app.patientName }}</span>
                              <v-chip size="x-small" color="primary">{{ app.appointmentCode }}</v-chip>
                            </div>
                            <div class="text-caption text-grey">
                              SĐT: {{ app.patientPhone }} | Bác sĩ: <strong>{{ app.doctorName }}</strong> | Khung giờ: <strong>{{ app.timeSlot }}</strong> | Triệu chứng: {{ app.notes || 'Không ghi chú' }}
                            </div>
                          </div>
                          <div class="d-flex gap-2">
                            <v-btn
                              color="success"
                              size="small"
                              @click="confirmAppointment(app.id)"
                            >
                              Xác Nhận
                            </v-btn>
                            <v-btn
                              color="error"
                              variant="outlined"
                              size="small"
                              @click="cancelAppointment(app.id)"
                            >
                              Hủy
                            </v-btn>
                          </div>
                        </v-card>
                      </v-col>
                    </v-row>
                  </div>
                </v-card>
              </v-col>

              <!-- Reception Info Card -->
              <v-col cols="12" md="4">
                <v-card border flat class="bg-surface pa-6">
                  <h3 class="text-subtitle-1 font-weight-bold mb-3 d-flex align-center">
                    <v-icon icon="mdi-printer" class="mr-2" color="primary" />
                    Phiếu Tiếp Nhận Gần Nhất
                  </h3>
                  <v-alert v-if="!lastConfirmedApp" type="info" variant="tonal" density="compact">
                    Chưa có phiếu khám nào được tiếp nhận trong phiên làm việc này.
                  </v-alert>
                  <v-card v-else border class="pa-4 bg-slate-900 border-success">
                    <div class="text-center font-weight-black text-h3 text-success mb-2">
                      SỐ: {{ lastConfirmedApp.queueNumber }}
                    </div>
                    <div class="text-caption text-center text-grey mb-4">HÀNG CHỜ PHÒNG KHÁM</div>
                    <v-divider class="mb-4" />
                    <div class="text-subtitle-2 mb-1">Bệnh nhân: <strong>{{ lastConfirmedApp.patientName }}</strong></div>
                    <div class="text-subtitle-2 mb-1">Bác sĩ: <strong>{{ lastConfirmedApp.doctorName }}</strong></div>
                    <div class="text-subtitle-2 mb-1">Khung giờ hẹn: <strong>{{ lastConfirmedApp.timeSlot }}</strong></div>
                    <div class="text-subtitle-2 mb-1">Trạng thái: <span class="text-success">Đã xác nhận & xếp hàng</span></div>
                    <v-btn block color="success" variant="outlined" size="small" class="mt-4" prepend-icon="mdi-printer">
                      In Phiếu Khám Số Thứ Tự
                    </v-btn>
                  </v-card>
                </v-card>
              </v-col>
            </v-row>
          </v-window-item>



          <!-- 3. DOCTOR PORTAL TAB (Khám bệnh & Kê đơn thuốc) -->
          <v-window-item value="doctor">
            <v-row>
              <!-- Danh sách bệnh nhân chờ khám -->
              <v-col cols="12" md="4">
                <v-card border flat class="bg-surface pa-6 mb-6">
                  <div class="text-subtitle-1 font-weight-bold mb-4">Bác sĩ trực ca</div>
                  <v-select
                    v-model="doctorPortal.doctorId"
                    :items="doctors"
                    item-title="fullName"
                    item-value="id"
                    label="Chọn Bác sĩ"
                    variant="outlined"
                    density="comfortable"
                    @update:model-value="fetchDoctorActiveQueue"
                  />

                  <v-divider class="my-4" />

                  <div class="d-flex justify-space-between align-center mb-3">
                    <span class="font-weight-bold">Bệnh nhân đợi khám</span>
                    <v-chip v-if="doctorPortal.doctorId" color="primary" size="small">
                      Đợi: {{ doctorQueue.filter(q => q.status === 'ChoKham').length }}
                    </v-chip>
                  </div>

                  <v-alert v-if="!doctorPortal.doctorId" type="info" variant="tonal" density="comfortable">
                    Chọn bác sĩ trực ca để xem danh sách chờ.
                  </v-alert>
                  <v-alert v-else-if="doctorQueue.length === 0" type="warning" variant="tonal" density="comfortable">
                    Hàng chờ trống.
                  </v-alert>
                  
                  <v-list v-else bg-color="transparent" class="pa-0">
                    <v-card
                      v-for="qItem in sortedDoctorQueue"
                      :key="qItem.id"
                      border
                      flat
                      class="mb-2 pa-3"
                      :color="qItem.status === 'DangKham' ? 'slate-900' : 'surface'"
                      :style="qItem.status === 'DangKham' ? 'border: 1px solid #38BDF8 !important' : ''"
                    >
                      <div class="d-flex justify-space-between align-center">
                        <div>
                          <div class="font-weight-bold">Số {{ qItem.queueNumber }}. {{ qItem.patientName }}</div>
                          <div class="text-caption text-grey">Trạng thái: {{ qItem.status }}</div>
                        </div>
                        <div class="d-flex gap-1">
                          <v-btn
                            v-if="qItem.status === 'ChoKham'"
                            color="primary"
                            size="x-small"
                            @click="updateQueueStatus(qItem.id, 2)"
                          >
                            Gọi khám
                          </v-btn>
                          <v-btn
                            v-if="qItem.status === 'DangKham'"
                            color="success"
                            size="x-small"
                            @click="selectConsultingPatient(qItem)"
                          >
                            Ghi bệnh án
                          </v-btn>
                          <v-btn
                            v-if="qItem.status === 'ChoKham' || qItem.status === 'DangKham'"
                            color="error"
                            variant="text"
                            size="x-small"
                            @click="updateQueueStatus(qItem.id, 3)"
                          >
                            Bỏ qua
                          </v-btn>
                        </div>
                      </div>
                    </v-card>
                  </v-list>
                </v-card>
              </v-col>

              <!-- Khu vực Bệnh án & Kê đơn thuốc -->
              <v-col cols="12" md="8">
                <v-card border flat class="bg-surface pa-6 h-100">
                  <div class="text-h6 font-weight-bold mb-4 text-primary">Khu vực khám bệnh & Kê đơn thuốc</div>
                  
                  <v-alert v-if="!activeConsultation.queueId" type="info" variant="tonal">
                    Vui lòng nhấn Gọi khám và chọn Ghi bệnh án của bệnh nhân đang khám để thực hiện chẩn đoán.
                  </v-alert>

                  <v-form v-else @submit.prevent="submitConsultation">
                    <v-row>
                      <v-col cols="12" sm="6">
                        <div class="text-subtitle-2 font-weight-bold text-primary">Thông tin bệnh nhân</div>
                        <div class="text-body-1 py-1">Họ tên: <strong>{{ activeConsultation.patientName }}</strong></div>
                        <div class="text-body-1 py-1">Số thứ tự khám: <strong>{{ activeConsultation.queueNumber }}</strong></div>
                        <div class="text-body-1 py-1">Số điện thoại: <strong>{{ activeConsultation.patientPhone }}</strong></div>
                      </v-col>
                      <v-col cols="12" sm="6">
                        <div class="text-subtitle-2 font-weight-bold text-primary mb-1">Hồ sơ sức khỏe & tiền sử</div>
                        <div class="mb-1">
                          <span class="text-caption text-grey">Dị ứng:</span>
                          <v-chip size="small" :color="patientProfile.allergies !== 'Không' && patientProfile.allergies !== 'Không dị ứng' ? 'error' : 'grey'" class="ml-2 font-weight-bold">
                            {{ patientProfile.allergies }}
                          </v-chip>
                        </div>
                        <div class="mb-2">
                          <span class="text-caption text-grey">Tiền sử:</span>
                          <v-chip size="small" color="blue-grey" class="ml-2">
                            {{ patientProfile.medicalHistory }}
                          </v-chip>
                        </div>
                        <v-btn size="small" variant="outlined" color="info" prepend-icon="mdi-history" @click="viewPatientHistory(activeConsultation.patientPhone)">
                          Xem lịch sử khám
                        </v-btn>
                      </v-col>
                    </v-row>

                    <v-divider class="my-4" />

                    <!-- Bệnh án -->
                    <div class="text-subtitle-1 font-weight-bold mb-3 text-secondary">Ghi nhận Bệnh án</div>
                    <v-text-field
                      v-model="activeConsultation.symptoms"
                      label="Triệu chứng lâm sàng"
                      variant="outlined"
                      density="comfortable"
                      class="mb-3"
                      required
                    />
                    <v-text-field
                      v-model="activeConsultation.diagnosis"
                      label="Chẩn đoán xác định"
                      variant="outlined"
                      density="comfortable"
                      class="mb-3"
                      required
                    />
                    <v-text-field
                      v-model="activeConsultation.notes"
                      label="Ghi chú dặn dò bác sĩ"
                      variant="outlined"
                      density="comfortable"
                      class="mb-4"
                    />

                    <v-divider class="my-4" />

                    <!-- Kê đơn thuốc -->
                    <div class="text-subtitle-1 font-weight-bold mb-3 text-success">Kê đơn thuốc điều trị</div>
                    <v-row density="comfortable" class="align-center">
                      <v-col cols="12" sm="4">
                        <v-autocomplete
                          v-model="activeConsultation.currentDrug.drugId"
                          :items="drugs"
                          item-title="name"
                          item-value="id"
                          label="Chọn thuốc"
                          variant="outlined"
                          density="comfortable"
                          hide-details
                          @update:model-value="onDrugSelected"
                        />
                      </v-col>
                      <v-col cols="12" sm="2">
                        <v-text-field
                          v-model.number="activeConsultation.currentDrug.quantity"
                          label="Số lượng"
                          type="number"
                          variant="outlined"
                          density="comfortable"
                          hide-details
                        />
                      </v-col>
                      <v-col cols="12" sm="4">
                        <v-text-field
                          v-model="activeConsultation.currentDrug.dosage"
                          label="Liều dùng cách dùng"
                          placeholder="Uống ngày 2 lần, mỗi lần 1 viên"
                          variant="outlined"
                          density="comfortable"
                          hide-details
                        />
                      </v-col>
                      <v-col cols="12" sm="2">
                        <v-btn block color="success" class="font-weight-bold" @click="addDrugToPrescription">
                          Thêm
                        </v-btn>
                      </v-col>
                    </v-row>

                    <!-- Danh sách thuốc đã kê -->
                    <div v-if="activeConsultation.prescription.length > 0" class="mt-4">
                      <div class="text-subtitle-2 font-weight-bold mb-2">Đơn thuốc đã kê:</div>
                      <v-table density="compact" class="bg-surface rounded border">
                        <thead>
                          <tr>
                            <th class="text-left font-weight-bold">Tên thuốc</th>
                            <th class="text-left font-weight-bold">Số lượng</th>
                            <th class="text-left font-weight-bold">Đơn giá</th>
                            <th class="text-left font-weight-bold">Liều dùng</th>
                            <th class="text-left font-weight-bold">Thành tiền</th>
                            <th class="text-left font-weight-bold">Thao tác</th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr v-for="(item, idx) in activeConsultation.prescription" :key="idx">
                            <td>{{ item.drugName }}</td>
                            <td>{{ item.quantity }}</td>
                            <td>{{ formatMoney(item.price) }}đ</td>
                            <td>{{ item.dosage }}</td>
                            <td>{{ formatMoney(item.price * item.quantity) }}đ</td>
                            <td>
                              <v-btn color="error" variant="text" icon="mdi-trash-can-outline" size="small" @click="removeDrugFromPrescription(idx)" />
                            </td>
                          </tr>
                        </tbody>
                      </v-table>
                    </div>

                    <v-btn type="submit" color="primary" block size="large" class="font-weight-bold mt-6" :loading="loading">
                      Lưu bệnh án, Kê đơn & Hoàn thành khám
                    </v-btn>
                  </v-form>
                </v-card>
              </v-col>
            </v-row>
          </v-window-item>

          <!-- 4. PHARMACY & BILLING TAB (Dược sĩ & Thu ngân) -->
          <v-window-item value="pharmacy">
            <!-- Header -->
            <div class="d-flex justify-space-between align-center mb-6">
              <div>
                <h2 class="text-h5 font-weight-black text-primary mb-1">Quản lý Kho Thuốc</h2>
                <p class="text-body-2 text-grey-darken-1 mb-0">Nhập kho, xuất thuốc theo đơn, kiểm soát tồn kho dược phẩm</p>
              </div>
              <div class="d-flex gap-2">
                <v-chip color="primary" variant="tonal" prepend-icon="mdi-pill">
                  {{ drugs.length }} loại thuốc
                </v-chip>
                <v-btn color="primary" prepend-icon="mdi-plus" class="font-weight-bold" @click="addDrugDialog = true">
                  Nhập thuốc mới
                </v-btn>
              </div>
            </div>

            <v-row>
              <!-- Canh bao ton kho thap -->
              <v-col cols="12" md="4">
                <v-card border flat class="bg-surface pa-5 rounded-xl" style="border-color: rgba(133,24,0,0.2) !important; background-color: rgba(133,24,0,0.02) !important;">
                  <div class="text-subtitle-2 font-weight-bold mb-3 text-warning d-flex align-center">
                    <v-icon icon="mdi-alert-circle" class="mr-2" size="18" />
                    Cảnh báo tồn kho thấp
                  </div>
                  <div v-if="drugs.filter(d => d.stock <= 100).length === 0" class="text-caption text-grey-darken-1">
                    Tất cả thuốc đều đủ tồn kho
                  </div>
                  <div v-for="d in drugs.filter(d => d.stock <= 100)" :key="'low-'+d.id" class="d-flex justify-space-between align-center mb-2">
                    <div>
                      <div class="text-body-2 font-weight-bold">{{ d.name }}</div>
                      <div class="text-caption text-grey-darken-1">{{ d.activeIngredient }}</div>
                    </div>
                    <v-chip size="x-small" color="error" variant="flat" class="font-weight-bold">
                      Còn {{ d.stock }} {{ d.unit }}
                    </v-chip>
                  </div>
                </v-card>
              </v-col>

              <!-- Danh sach thuoc ton kho -->
              <v-col cols="12" md="8">
                <v-card border flat class="bg-surface rounded-xl overflow-hidden">
                  <div class="pa-5 border-b d-flex justify-space-between align-center">
                    <div>
                      <h3 class="text-subtitle-1 font-weight-bold">Danh sách kho dược phẩm</h3>
                      <p class="text-caption text-grey-darken-1 mb-0">Cập nhật theo thời gian thực từ PharmacyDB</p>
                    </div>
                    <v-btn variant="tonal" color="primary" size="small" prepend-icon="mdi-refresh" @click="fetchDrugs">
                      Làm mới
                    </v-btn>
                  </div>
                  <v-table density="comfortable" class="bg-surface">
                    <thead>
                      <tr>
                        <th class="font-weight-bold text-caption text-grey-darken-1">TÊN THUỐC / HOẠT CHẤT</th>
                        <th class="font-weight-bold text-caption text-grey-darken-1">ĐƠN VỊ</th>
                        <th class="font-weight-bold text-caption text-grey-darken-1">ĐƠN GIÁ</th>
                        <th class="font-weight-bold text-caption text-grey-darken-1">TỒN KHO</th>
                        <th class="font-weight-bold text-caption text-grey-darken-1">TRẠNG THÁI</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="d in drugs" :key="d.id" class="hover-row">
                        <td>
                          <div class="font-weight-bold text-body-2">{{ d.name }}</div>
                          <div class="text-caption text-grey-darken-1">{{ d.activeIngredient }}</div>
                        </td>
                        <td class="text-body-2">{{ d.unit }}</td>
                        <td class="font-weight-bold text-body-2">{{ formatMoney(d.price) }}đ</td>
                        <td class="font-weight-bold">{{ d.stock }}</td>
                        <td>
                          <v-chip
                            size="x-small"
                            :color="d.stock > 500 ? 'success' : d.stock > 100 ? 'warning' : 'error'"
                            variant="flat"
                            class="font-weight-bold"
                          >
                            {{ d.stock > 500 ? 'An toàn' : d.stock > 100 ? 'Sắp hết' : 'Khẩn cấp' }}
                          </v-chip>
                        </td>
                      </tr>
                      <tr v-if="drugs.length === 0">
                        <td colspan="5" class="text-center text-grey-darken-1 py-8">
                          <v-icon icon="mdi-package-variant" size="36" class="mb-2 d-block mx-auto" />
                          Chưa có dữ liệu kho thuốc
                        </td>
                      </tr>
                    </tbody>
                  </v-table>
                </v-card>
              </v-col>
            </v-row>
          </v-window-item>

          <!-- 4b. BILLING TAB (Vien phi) -->
          <v-window-item value="billing">
            <!-- Header -->
            <div class="d-flex justify-space-between align-center mb-6">
              <div>
                <h2 class="text-h5 font-weight-black text-primary mb-1">Thu Viện Phí</h2>
                <p class="text-body-2 text-grey-darken-1 mb-0">Xác nhận thanh toán hóa đơn phí khám + tiền thuốc theo đơn</p>
              </div>
              <v-chip color="warning" variant="tonal" prepend-icon="mdi-clock-outline">
                {{ bills.filter(b => b.status === 'ChuaThanhToan').length }} chờ thanh toán
              </v-chip>
            </div>

            <v-row>
              <!-- Danh sach hoa don cho thanh toan -->
              <v-col cols="12" lg="8">
                <v-card border flat class="bg-surface rounded-xl overflow-hidden">
                  <div class="pa-5 border-b d-flex justify-space-between align-center">
                    <div>
                      <h3 class="text-subtitle-1 font-weight-bold">Danh sách hóa đơn viện phí</h3>
                      <p class="text-caption text-grey-darken-1 mb-0">Phí khám + thuốc theo đơn từ Medical Record Service</p>
                    </div>
                    <v-btn variant="tonal" color="primary" size="small" prepend-icon="mdi-refresh" @click="fetchBills">
                      Làm mới
                    </v-btn>
                  </div>

                  <div class="pa-4">
                    <v-alert v-if="bills.length === 0" type="info" variant="tonal" rounded="lg">
                      Không có hóa đơn nào cần xử lý hôm nay.
                    </v-alert>

                    <v-card
                      v-for="b in bills"
                      :key="b.id"
                      border
                      flat
                      class="mb-4 rounded-xl overflow-hidden"
                      :style="b.status === 'DaThanhToan' ? 'opacity: 0.7;' : ''"
                    >
                      <!-- Bill header -->
                      <div class="pa-4 d-flex justify-space-between align-center" :class="b.status === 'DaThanhToan' ? 'bg-success-lighten-5' : 'bg-warning-lighten-5'">
                        <div class="d-flex align-center gap-3">
                          <v-avatar :color="b.status === 'DaThanhToan' ? 'success' : 'warning'" variant="tonal" size="40">
                            <v-icon :icon="b.status === 'DaThanhToan' ? 'mdi-check-circle' : 'mdi-clock-outline'" />
                          </v-avatar>
                          <div>
                            <div class="font-weight-bold text-body-1">{{ b.patientName }}</div>
                            <div class="text-caption text-grey-darken-1">SĐT: {{ b.patientPhone }}</div>
                          </div>
                        </div>
                        <v-chip
                          :color="b.status === 'DaThanhToan' ? 'success' : 'warning'"
                          variant="flat"
                          size="small"
                          class="font-weight-bold"
                        >
                          {{ b.status === 'DaThanhToan' ? 'Đã Thanh Toán' : 'Chờ Thanh Toán' }}
                        </v-chip>
                      </div>

                      <!-- Bill detail -->
                      <div class="pa-4">
                        <div class="text-caption text-grey-darken-1 mb-3">Ngày khám: {{ formatDate(b.date) }}</div>
                        <v-divider class="mb-3" />
                        <div class="d-flex justify-space-between text-body-2 mb-2">
                          <span class="text-grey-darken-1">Phí khám bệnh:</span>
                          <span class="font-weight-bold">{{ formatMoney(b.consultationFee) }}đ</span>
                        </div>
                        <div class="d-flex justify-space-between text-body-2 mb-2">
                          <span class="text-grey-darken-1">Tiền thuốc:</span>
                          <span class="font-weight-bold">{{ formatMoney(b.medicationFee) }}đ</span>
                        </div>
                        <v-divider class="my-2" />
                        <div class="d-flex justify-space-between text-subtitle-1 font-weight-black">
                          <span>Tổng cộng:</span>
                          <span class="text-success">{{ formatMoney(b.totalAmount) }}đ</span>
                        </div>
                        <v-btn
                          v-if="b.status === 'ChuaThanhToan'"
                          color="success"
                          block
                          class="mt-4 font-weight-bold"
                          prepend-icon="mdi-cash-check"
                          :loading="loading"
                          @click="payBill(b.id)"
                        >
                          Xác nhận thu viện phí
                        </v-btn>
                      </div>
                    </v-card>
                  </div>
                </v-card>
              </v-col>

              <!-- Panel thong ke nhanh -->
              <v-col cols="12" lg="4">
                <v-card border flat class="bg-surface pa-5 rounded-xl mb-4">
                  <h3 class="text-subtitle-1 font-weight-bold mb-4">Tổng kết hôm nay</h3>
                  <div class="d-flex justify-space-between align-center mb-4">
                    <div class="text-caption text-grey-darken-1">Tổng hóa đơn</div>
                    <div class="text-h6 font-weight-black">{{ bills.length }}</div>
                  </div>
                  <div class="d-flex justify-space-between align-center mb-4">
                    <div class="text-caption text-grey-darken-1">Đã thanh toán</div>
                    <div class="text-h6 font-weight-black text-success">{{ bills.filter(b => b.status === 'DaThanhToan').length }}</div>
                  </div>
                  <div class="d-flex justify-space-between align-center mb-4">
                    <div class="text-caption text-grey-darken-1">Chờ thu</div>
                    <div class="text-h6 font-weight-black text-warning">{{ bills.filter(b => b.status === 'ChuaThanhToan').length }}</div>
                  </div>
                  <v-divider class="my-3" />
                  <div class="d-flex justify-space-between align-center">
                    <div class="text-caption text-grey-darken-1 font-weight-bold">Doanh thu đã thu</div>
                    <div class="text-subtitle-1 font-weight-black text-success">
                      {{ formatMoney(bills.filter(b => b.status === 'DaThanhToan').reduce((s, b) => s + (b.totalAmount || 0), 0)) }}đ
                    </div>
                  </div>
                </v-card>

                <v-card border flat class="bg-surface pa-5 rounded-xl">
                  <h3 class="text-subtitle-1 font-weight-bold mb-3">Hướng dẫn quy trình</h3>
                  <div class="d-flex align-start gap-3 mb-3">
                    <v-avatar color="primary" variant="tonal" size="28" class="mt-1">
                      <span class="text-caption font-weight-bold">1</span>
                    </v-avatar>
                    <p class="text-body-2 text-grey-darken-1 mb-0">Bệnh nhân khám xong, bác sĩ kê đơn thuốc (prescription.created)</p>
                  </div>
                  <div class="d-flex align-start gap-3 mb-3">
                    <v-avatar color="primary" variant="tonal" size="28" class="mt-1">
                      <span class="text-caption font-weight-bold">2</span>
                    </v-avatar>
                    <p class="text-body-2 text-grey-darken-1 mb-0">Hệ thống tự động tính phí khám + tiền thuốc theo đơn</p>
                  </div>
                  <div class="d-flex align-start gap-3">
                    <v-avatar color="success" variant="tonal" size="28" class="mt-1">
                      <span class="text-caption font-weight-bold">3</span>
                    </v-avatar>
                    <p class="text-body-2 text-grey-darken-1 mb-0">Thu ngân bấm "Xác nhận thu viện phí" để hoàn tất thanh toán</p>
                  </div>
                </v-card>
              </v-col>
            </v-row>
          </v-window-item>

          <!-- 5. ADMIN PORTAL TAB (Quản trị Bác sĩ & Lịch hẹn của Nhóm 1) -->
          <v-window-item value="admin">
            <!-- Tabs phụ của Admin -->
            <v-tabs v-model="adminSubTab" color="primary" rounded="lg" class="mb-6" border>
              <v-tab value="doctors-management" class="font-weight-bold">
                <v-icon icon="mdi-doctor" class="mr-2" />
                Quản lý Bác sĩ
              </v-tab>
              <v-tab value="schedules-management" class="font-weight-bold" @click="fetchAdminSchedules">
                <v-icon icon="mdi-calendar-clock" class="mr-2" />
                Quản lý Lịch trực
              </v-tab>
              <v-tab value="financial-reports" class="font-weight-bold" @click="fetchAdminFinancials">
                <v-icon icon="mdi-finance" class="mr-2" />
                Báo cáo Tài chính
              </v-tab>
            </v-tabs>

            <v-window v-model="adminSubTab">
              <!-- Subtab 1: Quản lý Bác sĩ (CRUD) -->
              <v-window-item value="doctors-management">
                <v-row>
                  <!-- Danh sách Bác sĩ hiện tại -->
                  <v-col cols="12">
                    <v-card border flat class="bg-surface pa-6 mb-6">
                      <div class="d-flex justify-space-between align-center mb-4">
                        <div class="text-h6 font-weight-bold text-primary mb-0">Danh sách bác sĩ phòng khám</div>
                        <v-btn color="success" prepend-icon="mdi-plus" class="font-weight-bold" @click="addDoctorDialog = true">
                          Thêm bác sĩ
                        </v-btn>
                      </div>
                      
                      <v-table density="comfortable" class="bg-surface rounded border">
                        <thead>
                          <tr>
                            <th class="text-left font-weight-bold">Bác sĩ</th>
                            <th class="text-left font-weight-bold">Chuyên khoa</th>
                            <th class="text-left font-weight-bold">Bằng cấp</th>
                            <th class="text-left font-weight-bold">Phí khám</th>
                            <th class="text-left font-weight-bold">Trạng thái</th>
                            <th class="text-center font-weight-bold">Thao tác</th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr v-for="doc in doctors" :key="doc.id">
                            <td class="font-weight-bold">{{ doc.fullName }}</td>
                            <td>
                              <v-chip color="primary" size="small">{{ doc.specialty }}</v-chip>
                            </td>
                            <td>{{ doc.qualifications }}</td>
                            <td class="text-success font-weight-bold">{{ formatMoney(doc.consultationFee) }}đ</td>
                            <td>
                              <v-chip size="small" :color="doc.isActive ? 'success' : 'error'">
                                {{ doc.isActive ? 'Đang trực' : 'Nghỉ trực' }}
                              </v-chip>
                            </td>
                            <td class="text-center">
                              <v-btn icon="mdi-pencil" size="x-small" variant="text" color="info" class="mr-1" @click="editDoctor(doc)" />
                              <v-btn icon="mdi-power" size="x-small" variant="text" :color="doc.isActive ? 'error' : 'success'" @click="deleteDoctorInfo(doc.id)" />
                            </td>
                          </tr>
                        </tbody>
                      </v-table>
                    </v-card>
                  </v-col>
                </v-row>
              </v-window-item>

              <!-- Subtab 2: Quản lý Lịch trực -->
              <v-window-item value="schedules-management">
                <v-row>
                  <!-- Form Thêm lịch trực -->
                  <v-col cols="12" md="4">
                    <v-card border flat class="bg-surface pa-6 mb-6">
                      <div class="text-h6 font-weight-bold mb-4 text-primary">Tạo Lịch trực Bác sĩ</div>
                      <v-form @submit.prevent="createAdminSchedule">
                        <v-select
                          v-model="adminScheduleForm.doctorId"
                          :items="doctors"
                          item-title="fullName"
                          item-value="id"
                          label="Chọn Bác sĩ"
                          variant="outlined"
                          density="comfortable"
                          class="mb-3"
                          required
                        />
                        <v-text-field
                          v-model="adminScheduleForm.date"
                          label="Ngày trực"
                          type="date"
                          variant="outlined"
                          density="comfortable"
                          class="mb-3"
                          required
                        />
                        <v-select
                          v-model="adminScheduleForm.shiftType"
                          :items="['Sang', 'Chieu', 'Toi']"
                          label="Ca khám"
                          variant="outlined"
                          density="comfortable"
                          class="mb-3"
                          required
                          @update:model-value="(val) => {
                            if (val === 'Sang') {
                              adminScheduleForm.startTime = '08:00:00';
                              adminScheduleForm.endTime = '12:00:00';
                            } else if (val === 'Chieu') {
                              adminScheduleForm.startTime = '13:30:00';
                              adminScheduleForm.endTime = '17:30:00';
                            } else {
                              adminScheduleForm.startTime = '18:00:00';
                              adminScheduleForm.endTime = '21:00:00';
                            }
                          }"
                        />
                        <v-row dense>
                          <v-col cols="6">
                            <v-text-field
                              v-model="adminScheduleForm.startTime"
                              label="Giờ bắt đầu"
                              placeholder="08:00:00"
                              variant="outlined"
                              density="comfortable"
                              class="mb-3"
                              required
                            />
                          </v-col>
                          <v-col cols="6">
                            <v-text-field
                              v-model="adminScheduleForm.endTime"
                              label="Giờ kết thúc"
                              placeholder="12:00:00"
                              variant="outlined"
                              density="comfortable"
                              class="mb-3"
                              required
                            />
                          </v-col>
                        </v-row>
                        <v-text-field
                          v-model.number="adminScheduleForm.maxPatients"
                          label="Số bệnh nhân tối đa"
                          type="number"
                          variant="outlined"
                          density="comfortable"
                          class="mb-4"
                          required
                        />
                        <v-btn type="submit" color="primary" block size="large" class="font-weight-bold" :loading="loading">
                          Lưu Lịch Trực
                        </v-btn>
                      </v-form>
                    </v-card>
                  </v-col>

                  <!-- Bảng Lịch trực hiện tại -->
                  <v-col cols="12" md="8">
                    <v-card border flat class="bg-surface pa-6 mb-6">
                      <div class="text-h6 font-weight-bold mb-4 text-primary">Lịch phân ca trực bác sĩ</div>
                      
                      <v-table density="comfortable" class="bg-surface rounded border">
                        <thead>
                          <tr>
                            <th class="text-left font-weight-bold">Bác sĩ</th>
                            <th class="text-left font-weight-bold">Ngày trực</th>
                            <th class="text-left font-weight-bold">Ca</th>
                            <th class="text-left font-weight-bold">Khung giờ</th>
                            <th class="text-left font-weight-bold">Bệnh nhân đã đặt</th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr v-for="sch in adminSchedules" :key="sch.id">
                            <td class="font-weight-bold">{{ sch.doctorName }}</td>
                            <td>{{ formatDate(sch.date) }}</td>
                            <td>
                              <v-chip :color="sch.shiftType === 'Sang' ? 'success' : (sch.shiftType === 'Chieu' ? 'info' : 'warning')" size="small">
                                {{ sch.shiftType }}
                              </v-chip>
                            </td>
                            <td>{{ sch.startTime }} - {{ sch.endTime }}</td>
                            <td>
                              <strong>{{ sch.currentBookings }}</strong> / {{ sch.maxPatients }}
                            </td>
                          </tr>
                          <tr v-if="adminSchedules.length === 0">
                            <td colspan="5" class="text-center text-grey py-4">Chưa có lịch phân ca nào được tải hoặc thiết lập</td>
                          </tr>
                        </tbody>
                      </v-table>
                    </v-card>
                  </v-col>
                </v-row>
              </v-window-item>

              <!-- Subtab 3: Báo cáo Tài chính -->
              <v-window-item value="financial-reports">
                <v-card border flat class="bg-surface pa-6 mb-6">
                  <div class="text-h6 font-weight-bold mb-4 text-primary">Báo cáo tài chính phòng khám</div>
                  
                  <v-row class="mb-6">
                    <v-col cols="12" sm="4">
                      <v-card color="primary-darken-1" border class="pa-4">
                        <div class="text-caption text-grey-lighten-2">TỔNG DOANH THU ĐÃ THU</div>
                        <div class="text-h4 font-weight-black mt-2 text-white">{{ formatMoney(adminFinancials.totalRevenue) }}đ</div>
                      </v-card>
                    </v-col>
                    <v-col cols="12" sm="4">
                      <v-card color="success-darken-1" border class="pa-4">
                        <div class="text-caption text-grey-lighten-2">HÓA ĐƠN ĐÃ THANH TOÁN</div>
                        <div class="text-h4 font-weight-black mt-2 text-white">{{ adminFinancials.paidCount }} hóa đơn</div>
                        <div class="text-body-2 text-success mt-1">Đã thu: {{ formatMoney(adminFinancials.paidAmount) }}đ</div>
                      </v-card>
                    </v-col>
                    <v-col cols="12" sm="4">
                      <v-card color="warning-darken-1" border class="pa-4">
                        <div class="text-caption text-grey-lighten-2">HÓA ĐƠN CHƯA THANH TOÁN</div>
                        <div class="text-h4 font-weight-black mt-2 text-white">{{ adminFinancials.pendingCount }} hóa đơn</div>
                        <div class="text-body-2 text-warning mt-1">Chờ thu: {{ formatMoney(adminFinancials.pendingAmount) }}đ</div>
                      </v-card>
                    </v-col>
                  </v-row>

                  <div class="text-subtitle-1 font-weight-bold mb-3">Danh sách hóa đơn chi tiết:</div>
                  <v-table density="comfortable" class="bg-surface rounded border">
                    <thead>
                      <tr>
                        <th class="text-left font-weight-bold">Bệnh nhân</th>
                        <th class="text-left font-weight-bold">Số điện thoại</th>
                        <th class="text-left font-weight-bold">Phí khám</th>
                        <th class="text-left font-weight-bold">Phí thuốc</th>
                        <th class="text-left font-weight-bold">Tổng hóa đơn</th>
                        <th class="text-left font-weight-bold">Trạng thái</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="bill in bills" :key="bill.id">
                        <td class="font-weight-bold">{{ bill.patientName }}</td>
                        <td>{{ bill.patientPhone }}</td>
                        <td>{{ formatMoney(bill.consultationFee) }}đ</td>
                        <td>{{ formatMoney(bill.medicationFee) }}đ</td>
                        <td class="text-success font-weight-bold">{{ formatMoney(bill.totalAmount) }}đ</td>
                        <td>
                          <v-chip size="small" :color="bill.status === 'DaThanhToan' || bill.status === 'Paid' ? 'success' : 'warning'">
                            {{ bill.status === 'DaThanhToan' || bill.status === 'Paid' ? 'Đã thu tiền' : 'Chưa thanh toán' }}
                          </v-chip>
                        </td>
                      </tr>
                    </tbody>
                  </v-table>
                </v-card>
              </v-window-item>
            </v-window>
          </v-window-item>

          <!-- Dialog Chỉnh Sửa Thông Tin Bác Sĩ -->
          <v-dialog v-model="editDoctorDialog" max-width="500">
            <v-card border class="bg-surface pa-4">
              <v-card-title class="text-h6 font-weight-bold text-primary px-2">
                Chỉnh sửa thông tin Bác sĩ
              </v-card-title>
              <v-card-text class="px-2 pt-4 pb-0">
                <v-text-field
                  v-model="editingDoctor.fullName"
                  label="Họ và tên"
                  variant="outlined"
                  density="comfortable"
                  class="mb-3"
                />
                <v-text-field
                  v-model="editingDoctor.specialty"
                  label="Chuyên khoa"
                  variant="outlined"
                  density="comfortable"
                  class="mb-3"
                />
                <v-text-field
                  v-model="editingDoctor.qualifications"
                  label="Học vị/Bằng cấp"
                  variant="outlined"
                  density="comfortable"
                  class="mb-3"
                />
                <v-text-field
                  v-model.number="editingDoctor.consultationFee"
                  label="Phí khám bệnh"
                  type="number"
                  variant="outlined"
                  density="comfortable"
                  class="mb-3"
                />
                <v-checkbox
                  v-model="editingDoctor.isActive"
                  label="Bác sĩ đang hoạt động (Đang trực)"
                  color="success"
                  hide-details
                  class="mb-2"
                />
              </v-card-text>
              <v-card-actions class="px-2 pt-2">
                <v-spacer />
                <v-btn variant="text" color="grey" @click="editDoctorDialog = false">Hủy</v-btn>
                <v-btn color="primary" class="font-weight-bold" @click="updateDoctorInfo" :loading="loading">Lưu thay đổi</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>

          <!-- Dialog Lịch Sử Khám Bệnh Bệnh Nhân (Xem từ phía Bác sĩ) -->
          <v-dialog v-model="patientHistoryDialog" max-width="800">
            <v-card border class="bg-surface pa-6">
              <v-card-title class="text-h6 font-weight-bold text-primary d-flex align-center justify-space-between px-0">
                <span>Lịch sử khám bệnh cũ</span>
                <v-btn icon="mdi-close" variant="text" size="small" @click="patientHistoryDialog = false" />
              </v-card-title>
              
              <v-card-text class="px-0 pt-4">
                <v-progress-linear v-if="patientHistoryLoading" color="primary" indeterminate class="mb-4" />
                
                <div v-if="patientHistories.length === 0" class="text-center py-8 text-grey">
                  Chưa ghi nhận lịch sử khám bệnh cũ cho số điện thoại này.
                </div>
                
                <v-expansion-panels v-else variant="popout" class="bg-surface">
                  <v-expansion-panel
                    v-for="hist in patientHistories"
                    :key="hist.id"
                    class="mb-3 border rounded"
                    bg-color="slate-900"
                  >
                    <v-expansion-panel-title class="font-weight-bold text-subtitle-2 text-info">
                      <div class="d-flex align-center justify-space-between w-100 pr-4">
                        <span>{{ formatDate(hist.appointmentDate) }} - {{ hist.doctorName }}</span>
                        <v-chip color="success" size="x-small">{{ hist.diagnosis }}</v-chip>
                      </div>
                    </v-expansion-panel-title>
                    <v-expansion-panel-text class="pt-3">
                      <div class="mb-2"><strong>Triệu chứng lâm sàng:</strong> {{ hist.symptoms }}</div>
                      <div class="mb-2"><strong>Chẩn đoán:</strong> {{ hist.diagnosis }}</div>
                      <div class="mb-3"><strong>Lời dặn:</strong> {{ hist.notes || 'Uống thuốc theo đơn' }}</div>
                      
                      <div class="text-subtitle-2 font-weight-bold mb-1 text-success">Đơn thuốc:</div>
                      <v-table density="compact" class="bg-surface rounded border mb-3">
                        <thead>
                          <tr>
                            <th>Tên Thuốc</th>
                            <th>Số Lượng</th>
                            <th>Liều dùng</th>
                          </tr>
                        </thead>
                        <tbody>
                          <tr v-for="item in hist.prescription" :key="item.drugId || item.name">
                            <td class="font-weight-bold">{{ item.drugName || item.name }}</td>
                            <td>{{ item.quantity }}</td>
                            <td>{{ item.dosage }}</td>
                          </tr>
                          <tr v-if="!hist.prescription || hist.prescription.length === 0">
                            <td colspan="3" class="text-center text-grey">Không kê đơn thuốc</td>
                          </tr>
                        </tbody>
                      </v-table>
                      
                      <div class="text-right">
                        <v-btn size="small" variant="outlined" color="primary" prepend-icon="mdi-printer" @click="printPrescription(hist)">
                          In đơn thuốc này
                        </v-btn>
                      </div>
                    </v-expansion-panel-text>
                  </v-expansion-panel>
                </v-expansion-panels>
              </v-card-text>
            </v-card>
          </v-dialog>

          <!-- 6. TV QUEUE SCREEN TAB -->
          <v-window-item value="tv">
            <v-card border flat class="bg-surface pa-8">
              <div class="d-flex align-center justify-space-between mb-6">
                <div class="d-flex align-center">
                  <span class="pulse-dot mr-3"></span>
                  <h1 class="text-h4 font-weight-black text-primary">MÀN HÌNH THEO DÕI HÀNG CHỜ PHÒNG KHÁM</h1>
                </div>
                <div class="text-subtitle-1 text-grey-darken-1">
                  Cập nhật thời gian thực (5s/lần)
                </div>
              </div>

              <v-divider class="mb-6" />

              <v-alert v-if="tvQueue.length === 0" type="info" variant="tonal" class="text-center pa-8">
                Hiện tại hàng chờ hôm nay trống. Hãy thực hiện đặt lịch khám và xác nhận để xếp số thứ tự.
              </v-alert>

              <!-- Queue Board Split by Doctors -->
              <v-row v-else>
                <v-col v-for="group in tvQueueGroupedByDoctor" :key="group.doctorName" cols="12" md="6" lg="4">
                  <v-card border flat class="pa-5 bg-slate-900 h-100">
                    <div class="text-h6 font-weight-bold text-primary mb-3 d-flex align-center">
                      <v-icon icon="mdi-doctor" class="mr-2" />
                      {{ group.doctorName }}
                    </div>
                    
                    <!-- Currently consultation patient (DangKham) -->
                    <div class="bg-surface pa-4 rounded-lg border border-primary mb-4 text-center">
                      <div class="text-caption text-primary font-weight-bold">ĐANG KHÁM</div>
                      <div class="text-h2 font-weight-black text-success py-2">
                        {{ group.currentlyCalling ? group.currentlyCalling.queueNumber : '---' }}
                      </div>
                      <div class="text-subtitle-1 text-truncate font-weight-medium">
                        {{ group.currentlyCalling ? group.currentlyCalling.patientName : '(Chờ gọi khám)' }}
                      </div>
                    </div>

                    <!-- Patients waiting (ChoKham) -->
                    <div class="text-caption text-grey font-weight-bold mb-2">DANH SÁCH CHỜ ĐỢI KHÁM:</div>
                    <div v-if="group.waitingList.length === 0" class="text-body-2 text-grey-darken-1 text-center py-2">
                      Không có bệnh nhân chờ.
                    </div>
                    <v-list v-else bg-color="transparent" density="compact" class="pa-0">
                      <v-list-item v-for="wait in group.waitingList" :key="wait.id" class="px-0 py-1">
                        <template v-slot:prepend>
                          <v-chip color="primary" size="small" class="font-weight-black mr-2">{{ wait.queueNumber }}</v-chip>
                        </template>
                        <v-list-item-title class="font-weight-medium text-body-1">{{ wait.patientName }}</v-list-item-title>
                      </v-list-item>
                    </v-list>
                  </v-card>
                </v-col>
              </v-row>
            </v-card>
          </v-window-item>
        </v-window>
      </div> <!-- Đóng thẻ div v-else -->
      </v-container>
    </v-main>

    <!-- Dialog Nhap thuoc moi -->
    <v-dialog v-model="addDrugDialog" max-width="500">
      <v-card class="rounded-xl pa-6 bg-surface">
        <div class="text-subtitle-1 font-weight-bold mb-4 text-primary d-flex align-center">
          <v-icon icon="mdi-plus-circle" class="mr-2" />
          Nhập thuốc mới vào kho
        </div>
        <v-form @submit.prevent="addNewDrug">
          <v-text-field
            v-model="drugForm.name"
            label="Tên thuốc"
            variant="outlined"
            density="comfortable"
            rounded="lg"
            class="mb-3"
            prepend-inner-icon="mdi-pill"
            required
          />
          <v-text-field
            v-model="drugForm.activeIngredient"
            label="Hoạt chất chính"
            variant="outlined"
            density="comfortable"
            rounded="lg"
            class="mb-3"
            prepend-inner-icon="mdi-molecule"
          />
          <v-row dense>
            <v-col cols="4">
              <v-text-field
                v-model="drugForm.unit"
                label="Đơn vị"
                variant="outlined"
                density="comfortable"
                rounded="lg"
                required
              />
            </v-col>
            <v-col cols="4">
              <v-text-field
                v-model.number="drugForm.price"
                label="Giá bán"
                type="number"
                variant="outlined"
                density="comfortable"
                rounded="lg"
                required
              />
            </v-col>
            <v-col cols="4">
              <v-text-field
                v-model.number="drugForm.stock"
                label="Tồn kho"
                type="number"
                variant="outlined"
                density="comfortable"
                rounded="lg"
                required
              />
            </v-col>
          </v-row>
          <div class="d-flex gap-2 mt-6">
            <v-btn variant="outlined" color="grey" class="font-weight-bold flex-grow-1" @click="addDrugDialog = false">
              Hủy
            </v-btn>
            <v-btn type="submit" color="primary" class="font-weight-bold flex-grow-1" prepend-icon="mdi-package-variant-plus" :loading="loading">
              Nhập kho thuốc
            </v-btn>
          </div>
        </v-form>
      </v-card>
    </v-dialog>

    <!-- Dialog Thêm Bác sĩ mới -->
    <v-dialog v-model="addDoctorDialog" max-width="500">
      <v-card class="rounded-xl pa-6 bg-surface">
        <div class="text-h6 font-weight-bold mb-4 text-primary">Thêm Bác sĩ mới</div>
        <v-form @submit.prevent="addNewDoctor">
          <v-text-field
            v-model="doctorForm.fullName"
            label="Họ và tên Bác sĩ"
            variant="outlined"
            density="comfortable"
            class="mb-3"
            required
          />
          <v-text-field
            v-model="doctorForm.specialty"
            label="Chuyên khoa"
            placeholder="Nội khoa, Da liễu, Nhi khoa, Răng Hàm Mặt"
            variant="outlined"
            density="comfortable"
            class="mb-3"
            required
          />
          <v-text-field
            v-model="doctorForm.qualifications"
            label="Học vị bằng cấp"
            placeholder="Thạc sĩ Bác sĩ, Tiến sĩ Y khoa"
            variant="outlined"
            density="comfortable"
            class="mb-3"
            required
          />
          <v-text-field
            v-model.number="doctorForm.consultationFee"
            label="Phí khám bệnh (đ)"
            type="number"
            variant="outlined"
            density="comfortable"
            class="mb-4"
            required
          />
          <div class="d-flex gap-2 mt-4">
            <v-btn variant="outlined" color="grey" class="font-weight-bold flex-grow-1" @click="addDoctorDialog = false">
              Hủy
            </v-btn>
            <v-btn type="submit" color="primary" class="font-weight-bold flex-grow-1" :loading="loading">
              Lưu thông tin bác sĩ
            </v-btn>
          </div>
        </v-form>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script>
import { ref, onMounted, onUnmounted, computed, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'

export default {
  name: 'DashboardPage',
  setup() {
    const router = useRouter()
    const route = useRoute()

    // Sidebar & Layout State
    const sidebarOpen = ref(true)
    const sidebarRail = ref(false)
    const headerSearch = ref('')

    // API Configurations
    const menuOpen = ref(false)
    const apiUrl = ref(localStorage.getItem('clinic_api_url') || 'http://localhost:5000/api')
    const authApiUrl = ref(localStorage.getItem('clinic_auth_api_url') || 'http://26.71.15.204:5000/api')
    const medicalApiUrl = ref(localStorage.getItem('clinic_medical_api_url') || 'http://26.15.45.202:5000/api')
    
    // Mock Mode & Auth Session
    const isMockMode = ref(false)
    const currentUser = ref({
      username: localStorage.getItem('clinic_user_name') || '',
      role: localStorage.getItem('clinic_user_role') || '',
      token: localStorage.getItem('clinic_jwt_token') || ''
    })

    const jwtToken = computed({
      get: () => currentUser.value.token,
      set: (val) => {
        currentUser.value.token = val
        if (val) {
          localStorage.setItem('clinic_jwt_token', val)
        } else {
          localStorage.removeItem('clinic_jwt_token')
        }
      }
    })

    const activeRole = computed({
      get: () => currentUser.value.role,
      set: (val) => {
        currentUser.value.role = val
        if (val) {
          localStorage.setItem('clinic_user_role', val)
        } else {
          localStorage.removeItem('clinic_user_role')
        }
      }
    })

    const loginForm = ref({
      username: '',
      password: ''
    })

    // App state
    const activeTab = ref('booking')

    // Helper giải mã JWT Token
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

    const syncUserFromToken = (token) => {
      if (!token) {
        currentUser.value.role = ''
        currentUser.value.username = ''
        localStorage.removeItem('clinic_user_role')
        localStorage.removeItem('clinic_user_name')
        return false
      }

      const decoded = decodeJwt(token)
      if (decoded) {
        const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || decoded['role']
        const username = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || decoded['unique_name'] || decoded['sub'] || decoded['name']
        
        if (role) {
          currentUser.value.role = role
          currentUser.value.username = username || 'Người dùng JWT'
          localStorage.setItem('clinic_user_role', role)
          localStorage.setItem('clinic_user_name', currentUser.value.username)
          
          // Tự động chuyển tab mặc định tương ứng với vai trò của người dùng
          if (activeTab.value === 'tv' || activeTab.value === 'booking') {
            if (role === 'Admin') activeTab.value = 'admin'
            else if (role === 'Doctor') activeTab.value = 'doctor'
            else if (role === 'Receptionist' || role === 'Nurse') activeTab.value = 'reception'
            else if (role === 'Patient') activeTab.value = 'booking'
          }
          return true
        }
      }
      return false
    }

    const hasRole = (rolesString) => {
      if (!currentUser.value || !currentUser.value.role) return false
      const currentRole = currentUser.value.role.trim().toLowerCase()
      const allowedRoles = rolesString.split(',').map(r => r.trim().toLowerCase())
      return allowedRoles.includes(currentRole)
    }
    const loading = ref(false)
    const alert = ref({
      show: false,
      type: 'info',
      message: ''
    })

    // Data lists
    const doctors = ref([
      {
        id: 'd1d11111-1111-1111-1111-111111111111',
        fullName: 'Nguyễn Văn An',
        specialty: 'Nội khoa',
        qualifications: 'Thạc sĩ Bác sĩ',
        consultationFee: 150000,
        isActive: true
      },
      {
        id: 'd2d22222-2222-2222-2222-222222222222',
        fullName: 'Trần Thị Bình',
        specialty: 'Nhi khoa',
        qualifications: 'Bác sĩ Chuyên khoa 1',
        consultationFee: 200000,
        isActive: true
      },
      {
        id: 'd3d33333-3333-3333-3333-333333333333',
        fullName: 'Lê Hoàng Nam',
        specialty: 'Da liễu',
        qualifications: 'Bác sĩ Chuyên khoa 2',
        consultationFee: 250000,
        isActive: true
      },
      {
        id: 'd4d44444-4444-4444-4444-444444444444',
        fullName: 'Phạm Minh Đức',
        specialty: 'Răng Hàm Mặt',
        qualifications: 'Tiến sĩ Y khoa',
        consultationFee: 300000,
        isActive: true
      }
    ])
    const specialties = ref([])
    const selectedSpecialty = ref(null)
    const availableSchedules = ref([])
    const bookedSlots = ref([])
    const pendingAppointments = ref([])
    const recentTicket = ref(null)

    // Nhóm 3: Kho thuốc & Viện phí Mock
    const drugs = ref([
      { id: 'dr1', name: 'Paracetamol 500mg', activeIngredient: 'Paracetamol', unit: 'Viên', price: 2000, stock: 1500 },
      { id: 'dr2', name: 'Amoxicillin 500mg', activeIngredient: 'Amoxicillin', unit: 'Viên', price: 5000, stock: 800 },
      { id: 'dr3', name: 'Ibuprofen 400mg', activeIngredient: 'Ibuprofen', unit: 'Viên', price: 3500, stock: 600 },
      { id: 'dr4', name: 'Decolgen Forte', activeIngredient: 'Paracetamol + Phenylephrine', unit: 'Vỉ', price: 15000, stock: 120 },
      { id: 'dr5', name: 'Cetirizine 10mg', activeIngredient: 'Cetirizine Hydrochloride', unit: 'Viên', price: 4000, stock: 450 }
    ])

    const bills = ref([
      {
        id: 'b1',
        patientName: 'Lê Văn Luyện',
        patientPhone: '0987654321',
        appointmentId: 'ap1',
        consultationFee: 150000,
        medicationFee: 30000,
        totalAmount: 180000,
        status: 'ChuaThanhToan',
        date: new Date().toISOString().split('T')[0]
      }
    ])

    const medicalRecords = ref([
      {
        id: 'mr1',
        patientName: 'Lê Văn Luyện',
        patientPhone: '0987654321',
        date: '2026-06-15',
        symptoms: 'Đau đầu, sốt nhẹ',
        diagnosis: 'Cảm cúm',
        notes: 'Uống thuốc đều đặn và nghỉ ngơi',
        prescription: [
          { drugId: 'dr1', drugName: 'Paracetamol 500mg', quantity: 10, price: 2000, dosage: 'Uống ngày 2 lần, mỗi lần 1 viên sau ăn' }
        ]
      }
    ])

    // Forms
    const drugForm = ref({
      name: '',
      activeIngredient: '',
      unit: 'Viên',
      price: 0,
      stock: 0
    })

    const doctorForm = ref({
      fullName: '',
      specialty: '',
      qualifications: '',
      consultationFee: 100000
    })

    // Consulting Patient (Doctor portal)
    const activeConsultation = ref({
      queueId: '',
      patientName: '',
      patientPhone: '',
      queueNumber: 0,
      symptoms: '',
      diagnosis: '',
      notes: '',
      prescription: [],
      currentDrug: {
        drugId: '',
        quantity: 1,
        dosage: ''
      }
    })
    
    // Search Receptionist
    const searchQuery = ref({
      code: '',
      phone: ''
    })
    const searchResults = ref(null)
    const lastConfirmedApp = ref(null)

    // Doctor portal
    const doctorPortal = ref({
      doctorId: null
    })
    const doctorQueue = ref([])

    // TV Queue portal
    const tvQueue = ref([])
    let tvInterval = null

    // Helper functions
    const showAlert = (message, type = 'info') => {
      alert.value.message = message
      alert.value.type = type
      alert.value.show = true
      setTimeout(() => {
        alert.value.show = false
      }, 5000)
    }

    const formatMoney = (val) => {
      if (!val) return '0'
      return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
    }

    const formatDate = (dateString) => {
      if (!dateString) return ''
      const date = new Date(dateString)
      return date.toLocaleDateString('vi-VN', { day: '2-digit', month: '2-digit', year: 'numeric' })
    }

    const formatTime = (timeString) => {
      if (!timeString) return ''
      const date = new Date(timeString)
      return date.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' })
    }

    const formatTimeSpan = (timeSpanString) => {
      if (!timeSpanString) return ''
      // TimeSpan return usually "hh:mm:ss"
      return timeSpanString.substring(0, 5)
    }

    const getStatusColor = (status) => {
      switch (status) {
        case 'ChoXacNhan': return 'warning'
        case 'DaXacNhan': return 'success'
        case 'DangKham': return 'primary'
        case 'DaKham': return 'grey'
        case 'DaHuy': return 'error'
        default: return 'info'
      }
    }

    // Fetch initial data
    const fetchDoctors = async () => {
      if (isMockMode.value) {
        specialties.value = [...new Set(doctors.value.map(d => d.specialty))]
        return
      }
      try {
        const url = `${apiUrl.value}/doctors?isActive=true`
        const res = await fetch(url)
        if (!res.ok) throw new Error('Không thể tải danh sách bác sĩ từ máy chủ')
        const data = await res.json()
        doctors.value = data
        specialties.value = [...new Set(data.map(d => d.specialty))]
      } catch (err) {
        showAlert(err.message, 'error')
      }
    }

    const filteredDoctors = computed(() => {
      if (!selectedSpecialty.value) return doctors.value
      return doctors.value.filter(d => d.specialty === selectedSpecialty.value)
    })

    const onFilterChange = () => {
      bookingForm.value.doctorId = ''
      bookingForm.value.scheduleId = ''
      availableSchedules.value = []
    }

    // Selecting Doctor & Schedule
    const bookingForm = ref({
      patientName: '',
      patientPhone: '',
      patientEmail: '',
      appointmentDate: '',
      timeSlot: '',
      doctorId: '',
      scheduleId: '',
      notes: ''
    })

    const selectDoctor = async (doc) => {
      bookingForm.value.doctorId = doc.id
      bookingForm.value.scheduleId = ''
      bookingForm.value.timeSlot = ''
      availableSchedules.value = []
      bookedSlots.value = []
      
      if (isMockMode.value) {
        // Tự sinh lịch làm việc 3 ngày tới cho bác sĩ được chọn ở chế độ giả lập
        const today = new Date()
        const mockShifts = []
        for (let i = 1; i <= 3; i++) {
          const targetDay = new Date(today)
          targetDay.setDate(today.getDate() + i)
          const dateStr = targetDay.toISOString().split('T')[0]
          
          mockShifts.push({
            id: `sch_mock_${doc.id}_${i}_1`,
            doctorId: doc.id,
            date: dateStr,
            shift: 'Sang',
            startTime: '08:00:00',
            endTime: '11:30:00',
            maxPatients: 12,
            currentBookings: 2
          })
          mockShifts.push({
            id: `sch_mock_${doc.id}_${i}_2`,
            doctorId: doc.id,
            date: dateStr,
            shift: 'Chieu',
            startTime: '13:30:00',
            endTime: '17:00:00',
            maxPatients: 10,
            currentBookings: 4
          })
        }
        availableSchedules.value = mockShifts
        return
      }

      try {
        const url = `${apiUrl.value}/schedules/available?doctorId=${doc.id}`
        const res = await fetch(url)
        if (!res.ok) throw new Error('Không thể tải lịch của bác sĩ')
        const data = await res.json()
        availableSchedules.value = data
      } catch (err) {
        showAlert(err.message, 'error')
      }
    }

    const generateTimeSlots = (startTime, endTime) => {
      if (!startTime || !endTime) return []
      const slots = []
      const parseTimeToMinutes = (timeStr) => {
        const parts = timeStr.split(':')
        const hh = parseInt(parts[0], 10) || 0
        const mm = parseInt(parts[1], 10) || 0
        return hh * 60 + mm
      }
      const formatMinutesToTime = (totalMinutes) => {
        const hh = Math.floor(totalMinutes / 60)
        const mm = totalMinutes % 60
        const pad = (n) => String(n).padStart(2, '0')
        return `${pad(hh)}:${pad(mm)}:00`
      }
      let current = parseTimeToMinutes(startTime)
      const end = parseTimeToMinutes(endTime)
      while (current <= end - 30) {
        slots.push(formatMinutesToTime(current))
        current += 30
      }
      if (slots.length === 0) {
        slots.push(startTime)
      }
      return slots
    }

    const fetchBookedSlots = async (doctorId, date) => {
      if (isMockMode.value) {
        bookedSlots.value = ['09:00:00', '14:30:00']
        return
      }
      if (!doctorId || !date) {
        bookedSlots.value = []
        return
      }
      try {
        const dateStr = date.split('T')[0]
        const url = `${apiUrl.value}/appointments/booked-slots?doctorId=${doctorId}&date=${dateStr}`
        const res = await fetch(url)
        if (!res.ok) throw new Error('Không thể tải các giờ đã đặt')
        const data = await res.json()
        bookedSlots.value = data
      } catch (err) {
        console.error('Lỗi khi tải các slot đã đặt:', err)
        bookedSlots.value = []
      }
    }

    const activeTimeSlotsList = computed(() => {
      const sch = availableSchedules.value.find(s => s.id === bookingForm.value.scheduleId)
      if (!sch) return []
      const slots = generateTimeSlots(sch.startTime, sch.endTime)
      return slots.map(slot => {
        const isBooked = bookedSlots.value.includes(slot)
        return {
          time: slot,
          isBooked: isBooked,
          displayTime: slot.substring(0, 5)
        }
      })
    })

    const selectSchedule = async (sch) => {
      bookingForm.value.scheduleId = sch.id
      bookingForm.value.appointmentDate = sch.date
      bookingForm.value.timeSlot = ''
      await fetchBookedSlots(bookingForm.value.doctorId, sch.date)
    }

    // Book Appointment
    const bookAppointment = async () => {
      try {
        loading.value = true
        const matchedDoc = doctors.value.find(d => d.id === bookingForm.value.doctorId)
        
        if (isMockMode.value) {
          const mockTicket = {
            id: 'app_mock_' + Date.now(),
            appointmentCode: 'CLINIC' + Math.floor(100000 + Math.random() * 900000),
            patientName: bookingForm.value.patientName,
            patientPhone: bookingForm.value.patientPhone,
            patientEmail: bookingForm.value.patientEmail,
            appointmentDate: bookingForm.value.appointmentDate,
            timeSlot: bookingForm.value.timeSlot,
            doctorId: bookingForm.value.doctorId,
            doctorName: matchedDoc ? matchedDoc.fullName : 'Bác sĩ phòng khám',
            specialty: matchedDoc ? matchedDoc.specialty : 'Nội khoa',
            consultationFee: matchedDoc ? matchedDoc.consultationFee : 150000,
            status: 'ChoXacNhan',
            notes: bookingForm.value.notes
          }

          // Đẩy vào danh sách chờ tiếp nhận giả lập
          pendingAppointments.value.unshift(mockTicket)
          recentTicket.value = mockTicket
          showAlert('Đăng ký đặt lịch khám thành công (Chế độ giả lập)!', 'success')
          
          bookingForm.value = {
            patientName: '', patientPhone: '', patientEmail: '',
            appointmentDate: '', timeSlot: '', doctorId: '',
            scheduleId: '', notes: ''
          }
          return
        }

        const url = `${apiUrl.value}/appointments/book`
        const res = await fetch(url, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(bookingForm.value)
        })

        const text = await res.text()
        let data
        try {
          data = JSON.parse(text)
        } catch (e) {
          data = text
        }

        if (!res.ok) {
          throw new Error(data || 'Đăng ký đặt lịch thất bại')
        }

        recentTicket.value = data
        showAlert('Đăng ký đặt lịch thành công!', 'success')
        
        bookingForm.value = {
          patientName: '', patientPhone: '', patientEmail: '',
          appointmentDate: '', timeSlot: '', doctorId: '',
          scheduleId: '', notes: ''
        }
        
        await fetchDoctors()
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    // Receptionist Functions
    const fetchPendingAppointments = async () => {
      if (isMockMode.value) {
        // Nếu danh sách rỗng, khởi tạo một số cuộc hẹn chờ tiếp nhận mẫu
        if (pendingAppointments.value.length === 0) {
          pendingAppointments.value = [
            {
              id: 'ap_mock_1',
              appointmentCode: 'CLINIC741258',
              patientName: 'Phạm Thị Thảo',
              patientPhone: '0901234567',
              patientEmail: 'thao@gmail.com',
              appointmentDate: new Date().toISOString().split('T')[0],
              timeSlot: '08:30:00',
              doctorId: 'd1d11111-1111-1111-1111-111111111111',
              doctorName: 'Nguyễn Văn An',
              specialty: 'Nội khoa',
              status: 'ChoXacNhan',
              notes: 'Bị ho và sốt kéo dài'
            },
            {
              id: 'ap_mock_2',
              appointmentCode: 'CLINIC963852',
              patientName: 'Trần Minh Hoàng',
              patientPhone: '0987654321',
              patientEmail: 'hoang@gmail.com',
              appointmentDate: new Date().toISOString().split('T')[0],
              timeSlot: '14:00:00',
              doctorId: 'd2d22222-2222-2222-2222-222222222222',
              doctorName: 'Trần Thị Bình',
              specialty: 'Nhi khoa',
              status: 'ChoXacNhan',
              notes: 'Khám dinh dưỡng định kỳ'
            }
          ]
        }
        return
      }
      try {
        loading.value = true
        const url = `${apiUrl.value}/appointments/pending`
        const headers = {}
        if (jwtToken.value) {
          headers['Authorization'] = `Bearer ${jwtToken.value}`
        }

        const res = await fetch(url, { headers })
        if (res.status === 401) throw new Error('Yêu cầu Token để đăng nhập (Xem phần Cấu hình kết nối)')
        if (res.status === 403) throw new Error('Bạn không có quyền hạn Tiếp Tân')
        if (!res.ok) throw new Error('Không thể tải cuộc hẹn chờ xác nhận')
        
        const data = await res.json()
        pendingAppointments.value = data
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    const confirmAppointment = async (id) => {
      try {
        loading.value = true
        
        if (isMockMode.value) {
          const idx = pendingAppointments.value.findIndex(p => p.id === id)
          if (idx !== -1) {
            const app = pendingAppointments.value[idx]
            pendingAppointments.value.splice(idx, 1)

            // Tính số thứ tự hàng chờ
            const nextQueueNum = doctorQueue.value.length + 1
            const newQueueItem = {
              id: 'q_' + Date.now(),
              appointmentId: app.id,
              doctorId: app.doctorId,
              doctorName: app.doctorName,
              patientName: app.patientName,
              patientPhone: app.patientPhone,
              queueNumber: nextQueueNum,
              checkInTime: new Date().toISOString(),
              status: 'ChoKham',
              notes: app.notes
            }
            
            doctorQueue.value.push(newQueueItem)
            lastConfirmedApp.value = { ...app, queueNumber: nextQueueNum }
            showAlert(`Đã tiếp nhận bệnh nhân: ${app.patientName}. Số thứ tự khám: ${nextQueueNum}`, 'success')
            
            if (searchResults.value) {
              searchResults.value = searchResults.value.filter(s => s.id !== id)
            }
          }
          return
        }

        const url = `${apiUrl.value}/queue/confirm-appointment/${id}`
        const headers = {
          'Content-Type': 'application/json'
        }
        if (jwtToken.value) {
          headers['Authorization'] = `Bearer ${jwtToken.value}`
        }

        const res = await fetch(url, {
          method: 'POST',
          headers
        })

        const data = await res.json()
        if (!res.ok) throw new Error(data || 'Xác nhận lịch hẹn thất bại')

        lastConfirmedApp.value = data
        showAlert(`Đã xác nhận thành công bệnh nhân: ${data.patientName}. Cấp số: ${data.queueNumber}`, 'success')
        
        if (searchResults.value) {
          searchAppointments()
        } else {
          fetchPendingAppointments()
        }
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    const cancelAppointment = async (id) => {
      if (!confirm('Bạn có chắc chắn muốn hủy cuộc hẹn này không?')) return

      try {
        loading.value = true
        if (isMockMode.value) {
          pendingAppointments.value = pendingAppointments.value.filter(p => p.id !== id)
          showAlert('Đã hủy lịch hẹn thành công (Giả lập)', 'success')
          if (searchResults.value) {
            searchResults.value = searchResults.value.filter(s => s.id !== id)
          }
          return
        }

        const url = `${apiUrl.value}/appointments/${id}/cancel`
        const res = await fetch(url, {
          method: 'PUT'
        })

        if (!res.ok) throw new Error('Hủy lịch hẹn thất bại')

        showAlert('Đã hủy lịch hẹn thành công', 'success')
        
        if (searchResults.value) {
          searchAppointments()
        } else {
          fetchPendingAppointments()
        }
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    const searchAppointments = async () => {
      try {
        const { code, phone } = searchQuery.value
        if (!code && !phone) {
          showAlert('Vui lòng nhập Mã hoặc SĐT tra cứu', 'warning')
          return
        }

        loading.value = true
        if (isMockMode.value) {
          const list = [...pendingAppointments.value]
          searchResults.value = list.filter(p => 
            (code && p.appointmentCode.toLowerCase().includes(code.toLowerCase())) ||
            (phone && p.patientPhone.includes(phone))
          )
          return
        }

        let url = `${apiUrl.value}/appointments/search?`
        if (code) url += `code=${code}&`
        if (phone) url += `phone=${phone}`

        const res = await fetch(url)
        if (!res.ok) throw new Error('Lỗi tìm kiếm cuộc hẹn')
        
        const data = await res.json()
        searchResults.value = data
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    const clearSearch = () => {
      searchResults.value = null
      searchQuery.value = { code: '', phone: '' }
      fetchPendingAppointments()
    }

    // Doctor Board Functions
    const fetchDoctorActiveQueue = async () => {
      if (!doctorPortal.value.doctorId) return
      
      if (isMockMode.value) {
        // Tải hàng chờ giả lập lọc theo bác sĩ
        return
      }

      try {
        const url = `${apiUrl.value}/queue/active?doctorId=${doctorPortal.value.doctorId}`
        const res = await fetch(url)
        if (!res.ok) throw new Error('Không thể tải hàng chờ của bác sĩ')
        const data = await res.json()
        doctorQueue.value = data
      } catch (err) {
        showAlert(err.message, 'error')
      }
    }

    const sortedDoctorQueue = computed(() => {
      const list = isMockMode.value 
        ? doctorQueue.value.filter(q => q.doctorId === doctorPortal.value.doctorId)
        : doctorQueue.value
      
      return [...list].sort((a, b) => {
        if (a.status === 'DangKham' && b.status !== 'DangKham') return -1
        if (a.status !== 'DangKham' && b.status === 'DangKham') return 1
        return a.queueNumber - b.queueNumber
      })
    })

    const updateQueueStatus = async (queueId, statusCode) => {
      try {
        loading.value = true
        
        if (isMockMode.value) {
          const item = doctorQueue.value.find(q => q.id === queueId)
          if (item) {
            const statusMap = { 2: 'DangKham', 3: 'DaHuy', 4: 'DaKham' }
            item.status = statusMap[statusCode] || item.status
            showAlert('Cập nhật trạng thái khám bệnh nhân (Giả lập) thành công', 'success')
            
            // Nếu là gọi khám, tự động cập nhật activeConsultation
            if (statusCode === 2) {
              selectConsultingPatient(item)
            }
          }
          return
        }

        const url = `${apiUrl.value}/queue/${queueId}/status`
        const headers = {
          'Content-Type': 'application/json'
        }
        if (jwtToken.value) {
          headers['Authorization'] = `Bearer ${jwtToken.value}`
        }

        const res = await fetch(url, {
          method: 'PUT',
          headers,
          body: JSON.stringify({ status: statusCode })
        })

        if (res.status === 401) throw new Error('Token không hợp lệ hoặc đã hết hạn')
        if (res.status === 403) throw new Error('Không có quyền thay đổi trạng thái khám')
        if (!res.ok) throw new Error('Không thể cập nhật trạng thái hàng chờ')

        showAlert('Cập nhật trạng thái khám thành công', 'success')
        
        // Nếu là gọi khám, tự động chọn bệnh nhân
        if (statusCode === 2) {
          const item = doctorQueue.value.find(q => q.id === queueId)
          if (item) selectConsultingPatient(item)
        }
        fetchDoctorActiveQueue()
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    // TV Queue Functions
    const fetchTvQueue = async () => {
      if (isMockMode.value) {
        // Sử dụng toàn bộ hàng chờ ảo doctorQueue ở chế độ giả lập
        tvQueue.value = doctorQueue.value
        return
      }
      try {
        const url = `${apiUrl.value}/queue/active`
        const res = await fetch(url)
        if (!res.ok) throw new Error('Không thể tải hàng chờ Tivi')
        const data = await res.json()
        tvQueue.value = data
      } catch (err) {
        // fail silently for auto-refresh
        console.error(err.message)
      }
    }

    const tvQueueGroupedByDoctor = computed(() => {
      const groups = {}
      
      // Group by DoctorName
      tvQueue.value.forEach(item => {
        const name = item.doctorName || 'Bác sĩ trực ban'
        if (!groups[name]) {
          groups[name] = {
            doctorName: name,
            currentlyCalling: null,
            waitingList: []
          }
        }
        
        if (item.status === 'DangKham') {
          groups[name].currentlyCalling = item
        } else if (item.status === 'ChoKham') {
          groups[name].waitingList.push(item)
        }
      })
      
      // Sort waiting list by queueNumber
      Object.keys(groups).forEach(key => {
        groups[key].waitingList.sort((a, b) => a.queueNumber - b.queueNumber)
      })

      return Object.values(groups)
    })

    // TestAuth Token generator
    const fetchTestToken = async (role) => {
      try {
        const url = `${apiUrl.value}/testauth/token?role=${role}`
        const res = await fetch(url)
        if (!res.ok) throw new Error('Không thể tạo token thử nghiệm')
        const data = await res.json()
        
        jwtToken.value = data.token
        activeRole.value = role
        showAlert(`Đăng nhập thành công dưới quyền ${role} (Token đã được áp dụng!)`, 'success')
        
        // Trigger reload tabs if in specific sections
        if (activeTab.value === 'reception') {
          fetchPendingAppointments()
        } else if (activeTab.value === 'doctor') {
          fetchDoctorActiveQueue()
        }
      } catch (err) {
        showAlert(err.message, 'error')
      }
    }

    const applyConfiguration = () => {
      localStorage.setItem('clinic_api_url', apiUrl.value)
      localStorage.setItem('clinic_auth_api_url', authApiUrl.value)
      localStorage.setItem('clinic_medical_api_url', medicalApiUrl.value)
      localStorage.setItem('clinic_jwt_token', jwtToken.value)
      
      const syncSuccess = syncUserFromToken(jwtToken.value)
      if (syncSuccess) {
        showAlert(`Đã áp dụng cấu hình, giải mã vai trò thành công: ${currentUser.value.role}!`, 'success')
      } else if (jwtToken.value) {
        showAlert('Đã áp dụng cấu hình, nhưng không thể giải mã vai trò từ JWT token!', 'warning')
      } else {
        showAlert('Đã áp dụng cấu hình API URL!', 'success')
      }
      
      menuOpen.value = false

      // Tự động reload lại dữ liệu tương ứng của tab đang hoạt động để người dùng thấy ngay kết quả
      if (activeTab.value === 'booking') {
        fetchDoctors()
      } else if (activeTab.value === 'reception' && jwtToken.value) {
        fetchPendingAppointments()
      } else if (activeTab.value === 'doctor' && doctorPortal.value.doctorId) {
        fetchDoctorActiveQueue()
      } else if (activeTab.value === 'tv') {
        fetchTvQueue()
      }
    }

    const handleLogin = async () => {
      try {
        loading.value = true
        if (isMockMode.value) {
          // Giả lập login nhanh dựa theo username
          const name = loginForm.value.username.toLowerCase()
          const role = name.includes('admin') ? 'Admin' :
                       name.includes('doc') ? 'Doctor' :
                       name.includes('recep') ? 'Receptionist' : 'Patient'
          
          currentUser.value = {
            username: loginForm.value.username,
            role: role,
            token: 'mock_jwt_token_for_' + role.toLowerCase()
          }
          localStorage.setItem('clinic_user_name', currentUser.value.username)
          localStorage.setItem('clinic_user_role', currentUser.value.role)
          localStorage.setItem('clinic_jwt_token', currentUser.value.token)
          showAlert('Đăng nhập giả lập thành công!', 'success')
        } else {
          // Gọi API xác thực thực tế trực tiếp tới Nhóm 6
          const url = `${authApiUrl.value}/Auth/login`
          const res = await fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(loginForm.value)
          })
          if (!res.ok) throw new Error('Đăng nhập thất bại. Tài khoản hoặc mật khẩu không chính xác.')
          const data = await res.json()
          
          currentUser.value.token = data.token
          localStorage.setItem('clinic_jwt_token', data.token)
          
          // Giải mã token thực tế để lấy vai trò và tên người dùng chuẩn xác
          const syncSuccess = syncUserFromToken(data.token)
          if (!syncSuccess) {
            // Fallback nếu giải mã JWT không thành công
            currentUser.value.role = data.role || 'Patient'
            currentUser.value.username = data.username || loginForm.value.username
            localStorage.setItem('clinic_user_role', currentUser.value.role)
            localStorage.setItem('clinic_user_name', currentUser.value.username)
            
            if (activeTab.value === 'tv' || activeTab.value === 'booking') {
              const r = currentUser.value.role
              if (r === 'Admin') activeTab.value = 'admin'
              else if (r === 'Doctor') activeTab.value = 'doctor'
              else if (r === 'Receptionist' || r === 'Nurse') activeTab.value = 'reception'
              else activeTab.value = 'booking'
            }
          }
          showAlert('Đăng nhập liên thông qua API Gateway thành công!', 'success')
        }
        loginForm.value = { username: '', password: '' }
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    const handleLogout = () => {
      currentUser.value = { username: '', role: '', token: '' }
      localStorage.removeItem('clinic_user_name')
      localStorage.removeItem('clinic_user_role')
      localStorage.removeItem('clinic_jwt_token')
      router.push({ name: 'Landing' })
    }

    const quickLogin = async (role) => {
      if (isMockMode.value) {
        currentUser.value = {
          username: 'Demo_' + role,
          role: role,
          token: 'mock_jwt_token_for_' + role.toLowerCase()
        }
        localStorage.setItem('clinic_user_name', currentUser.value.username)
        localStorage.setItem('clinic_user_role', currentUser.value.role)
        localStorage.setItem('clinic_jwt_token', currentUser.value.token)
        showAlert(`Đăng nhập thành công với vai trò ${role} (Giả lập)`, 'success')
      } else {
        try {
          loading.value = true
          // Gọi API TestAuth của nhóm 5 để sinh token nhanh
          const url = `${apiUrl.value}/testauth/token?role=${role}`
          const res = await fetch(url)
          if (!res.ok) throw new Error('Không thể lấy Token thử nghiệm từ máy chủ')
          const data = await res.json()
          currentUser.value = {
            username: 'Test_' + role,
            role: role,
            token: data.token
          }
          localStorage.setItem('clinic_user_name', currentUser.value.username)
          localStorage.setItem('clinic_user_role', currentUser.value.role)
          localStorage.setItem('clinic_jwt_token', currentUser.value.token)
          showAlert(`Đăng nhập thành công với vai trò ${role} (Token từ API!)`, 'success')
        } catch (err) {
          showAlert(err.message, 'error')
        } finally {
          loading.value = false
        }
      }
    }

    const selectConsultingPatient = (qItem) => {
      activeConsultation.value = {
        queueId: qItem.id,
        patientName: qItem.patientName,
        patientPhone: qItem.patientPhone || '0900000000',
        queueNumber: qItem.queueNumber,
        symptoms: qItem.notes || '',
        diagnosis: '',
        notes: '',
        prescription: [],
        currentDrug: {
          drugId: '',
          quantity: 1,
          dosage: ''
        }
      }
      fetchPatientProfile(activeConsultation.value.patientPhone)
      fetchDrugs() // Đồng bộ tồn kho thuốc thực tế từ Nhóm 6 để lấy số lượng chính xác khi kê đơn
      showAlert(`Bắt đầu ghi nhận bệnh án cho bệnh nhân: ${qItem.patientName}`, 'info')
    }

    const addDrugToPrescription = () => {
      const cur = activeConsultation.value.currentDrug
      if (!cur.drugId) {
        showAlert('Vui lòng chọn thuốc!', 'warning')
        return
      }
      const drug = drugs.value.find(d => d.id === cur.drugId)
      if (!drug) return
      
      if (drug.stock < cur.quantity) {
        showAlert(`Không đủ thuốc trong kho. Tồn kho hiện tại: ${drug.stock}`, 'error')
        return
      }

      activeConsultation.value.prescription.push({
        drugId: drug.id,
        drugName: drug.name,
        quantity: cur.quantity,
        price: drug.price,
        dosage: cur.dosage || 'Uống theo chỉ định của bác sĩ'
      })

      activeConsultation.value.currentDrug = {
        drugId: '',
        quantity: 1,
        dosage: ''
      }
    }

    const removeDrugFromPrescription = (idx) => {
      activeConsultation.value.prescription.splice(idx, 1)
    }

    const onDrugSelected = (drugId) => {
      const drug = drugs.value.find(d => d.id === drugId)
      if (drug) {
        activeConsultation.value.currentDrug.dosage = `Uống ngày 2 lần, mỗi lần 1 ${drug.unit} sau ăn`
      }
    }

    const submitConsultation = async () => {
      try {
        loading.value = true
        const prescriptionData = activeConsultation.value.prescription
        const medFee = prescriptionData.reduce((total, item) => total + (item.price * item.quantity), 0)
        const doctor = doctors.value.find(d => d.id === doctorPortal.value.doctorId)
        const consultFee = doctor ? doctor.consultationFee : 150000

        if (isMockMode.value) {
          // Lưu bệnh án giả lập
          const newRecord = {
            id: 'mr_' + Date.now(),
            patientName: activeConsultation.value.patientName,
            patientPhone: activeConsultation.value.patientPhone,
            date: new Date().toISOString().split('T')[0],
            symptoms: activeConsultation.value.symptoms,
            diagnosis: activeConsultation.value.diagnosis,
            notes: activeConsultation.value.notes,
            prescription: [...prescriptionData]
          }
          medicalRecords.value.unshift(newRecord)

          // Sinh hóa đơn viện phí
          const newBill = {
            id: 'b_' + Date.now(),
            patientName: activeConsultation.value.patientName,
            patientPhone: activeConsultation.value.patientPhone,
            appointmentId: activeConsultation.value.queueId,
            consultationFee: consultFee,
            medicationFee: medFee,
            totalAmount: consultFee + medFee,
            status: 'ChuaThanhToan',
            date: new Date().toISOString().split('T')[0]
          }
          bills.value.unshift(newBill)

          // Trừ kho thuốc
          prescriptionData.forEach(pItem => {
            const drug = drugs.value.find(d => d.id === pItem.drugId)
            if (drug) drug.stock = Math.max(0, drug.stock - pItem.quantity)
          })

          // Cập nhật trạng thái khám bệnh nhân trong queue
          const queueItem = doctorQueue.value.find(q => q.id === activeConsultation.value.queueId)
          if (queueItem) queueItem.status = 'DaKham'

          showAlert('Khám bệnh hoàn tất! Đơn thuốc và Hóa đơn viện phí đã chuyển sang bộ phận Thu ngân.', 'success')
        } else {
          const phone = activeConsultation.value.patientPhone
          const urlCheckPatient = `${medicalApiUrl.value}/Patients/search?phone=${phone}`
          let patientProfileId = null

          // 1. Kiểm tra xem bệnh nhân đã tồn tại ở Nhóm 4 chưa
          try {
            const resCheck = await fetch(urlCheckPatient, {
              headers: { 'Authorization': `Bearer ${currentUser.value.token}` }
            })
            if (resCheck.ok) {
              const patientData = await resCheck.json()
              // Nhóm 4 /Patients/search có thể trả về mảng hoặc đối tượng đơn
              if (Array.isArray(patientData) && patientData.length > 0) {
                patientProfileId = patientData[0].id || patientData[0].patientProfileId
              } else if (patientData && (patientData.id || patientData.patientProfileId)) {
                patientProfileId = patientData.id || patientData.patientProfileId
              }
            } else {
              // Thử endpoint dự phòng /Patients?phone=
              const resCheckBackup = await fetch(`${medicalApiUrl.value}/Patients?phone=${phone}`, {
                headers: { 'Authorization': `Bearer ${currentUser.value.token}` }
              })
              if (resCheckBackup.ok) {
                const patientData = await resCheckBackup.json()
                if (patientData && (patientData.id || patientData.patientProfileId)) {
                  patientProfileId = patientData.id || patientData.patientProfileId
                }
              }
            }
          } catch (e) {
            console.warn('Không thể kiểm tra sự tồn tại của bệnh nhân ở Nhóm 4, sẽ thử tự động tạo mới.', e)
          }

          // 2. Nếu bệnh nhân chưa có hồ sơ, tự động tạo hồ sơ bệnh nhân mới lên Nhóm 4
          if (!patientProfileId) {
            const urlCreatePatient = `${medicalApiUrl.value}/Patients`
            try {
              const resCreate = await fetch(urlCreatePatient, {
                method: 'POST',
                headers: {
                  'Content-Type': 'application/json',
                  'Authorization': `Bearer ${currentUser.value.token}`
                },
                body: JSON.stringify({
                  fullName: activeConsultation.value.patientName,
                  dateOfBirth: new Date(1990, 0, 1).toISOString(), // Mặc định 01/01/1990
                  gender: 0, // Mặc định 0 (Male)
                  phoneNumber: phone,
                  address: 'Chưa cập nhật',
                  medicalHistory: 'Không có tiền sử bệnh lý đặc biệt',
                  allergies: 'Không'
                })
              })
              if (!resCreate.ok) {
                const errText = await resCreate.text()
                throw new Error(`Tự động tạo hồ sơ bệnh nhân ở Nhóm 4 thất bại: ${errText || resCreate.statusText}`)
              }
              const newPatient = await resCreate.json()
              patientProfileId = newPatient.id || newPatient.patientProfileId
            } catch (createErr) {
              throw new Error(`Lỗi khi tạo hồ sơ bệnh nhân mới tại Nhóm 4: ${createErr.message}`)
            }
          }

          // 3. Tiến hành gọi API lưu bệnh án của Nhóm 4
          const urlRecord = `${medicalApiUrl.value}/MedicalRecords`
          let resRecord
          let medicalRecordId = null
          try {
            resRecord = await fetch(urlRecord, {
              method: 'POST',
              headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${currentUser.value.token}`
              },
              body: JSON.stringify({
                patientProfileId: patientProfileId,
                doctorId: doctorPortal.value.doctorId,
                appointmentId: activeConsultation.value.queueId,
                examinationDate: new Date().toISOString(),
                symptoms: activeConsultation.value.symptoms,
                diagnosis: activeConsultation.value.diagnosis,
                doctorNotes: activeConsultation.value.notes || 'Không có ghi chú'
              })
            })
          } catch (fetchErr) {
            throw new Error(`Không thể kết nối tới máy chủ Nhóm 4 tại địa chỉ ${urlRecord}. Chi tiết: ${fetchErr.message}`)
          }

          if (!resRecord.ok) {
            let errorDetail = ''
            try {
              const errData = await resRecord.json()
              errorDetail = errData.message || errData.error || JSON.stringify(errData)
            } catch (_) {
              try {
                errorDetail = await resRecord.text()
              } catch (_) {
                errorDetail = `Mã lỗi HTTP: ${resRecord.status} ${resRecord.statusText}`
              }
            }
            throw new Error(`Máy chủ Nhóm 4 từ chối lưu bệnh án. Chi tiết lỗi: ${errorDetail}`)
          }

          const createdRecord = await resRecord.json()
          medicalRecordId = createdRecord.id || createdRecord.medicalRecordId

          // Lọc danh sách thuốc thực tế (không chứa id giả lập dạng "dr1", "dr2") để tránh lỗi phía API các nhóm
          const realPrescriptionData = prescriptionData.filter(item => {
            const idStr = String(item.drugId);
            return !idStr.startsWith('dr') && !isNaN(parseInt(idStr));
          });

          // 4. Nếu có kê đơn thuốc, gọi API Nhóm 4 để lưu đơn thuốc liên kết
          if (realPrescriptionData.length > 0 && medicalRecordId) {
            const urlPrescription = `${medicalApiUrl.value}/Prescriptions`
            try {
              const resPresc = await fetch(urlPrescription, {
                method: 'POST',
                headers: {
                  'Content-Type': 'application/json',
                  'Authorization': `Bearer ${currentUser.value.token}`
                },
                body: JSON.stringify({
                  medicalRecordId: medicalRecordId,
                  notes: 'Đơn thuốc điều trị phòng khám',
                  details: realPrescriptionData.map(item => ({
                    medicineId: String(item.drugId),
                    quantity: parseInt(item.quantity),
                    dosageInstruction: item.dosage
                  }))
                })
              })
              if (!resPresc.ok) {
                const errPrescText = await resPresc.text()
                console.warn('Không thể lưu đơn thuốc lên Nhóm 4:', errPrescText)
              }
            } catch (prescErr) {
              console.warn('Lỗi kết nối khi lưu đơn thuốc lên Nhóm 4:', prescErr.message)
            }
          }

          // 5. Gọi API fallback HTTP của Nhóm 6 để tạo hóa đơn và tự động trừ kho thuốc (giải quyết khi RabbitMQ chưa thông)
          const urlConsumePrescription = `${authApiUrl.value}/Invoice/consume-prescription`
          try {
            const resConsume = await fetch(urlConsumePrescription, {
              method: 'POST',
              headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${currentUser.value.token}`
              },
              body: JSON.stringify({
                patientName: activeConsultation.value.patientName,
                consultationFee: consultFee,
                items: realPrescriptionData.map(item => ({
                  medicineId: parseInt(item.drugId),
                  quantity: parseInt(item.quantity)
                }))
              })
            })
            if (!resConsume.ok) {
              const errConsumeText = await resConsume.text()
              console.warn('Tạo hóa đơn và trừ kho trực tiếp trên Nhóm 6 thất bại:', errConsumeText)
            } else {
              console.log('Tạo hóa đơn và trừ kho trực tiếp trên Nhóm 6 thành công.')
            }
          } catch (consumeErr) {
            console.warn('Lỗi kết nối khi gọi API tạo hóa đơn Nhóm 6:', consumeErr.message)
          }

          // Gọi API Nhóm 1 cập nhật hoàn thành khám
          await updateQueueStatus(activeConsultation.value.queueId, 4)
          showAlert('Khám bệnh, ghi bệnh án và kê đơn thuốc liên thông thành công!', 'success')
        }

        // Reset form
        activeConsultation.value = {
          queueId: '',
          patientName: '',
          patientPhone: '',
          queueNumber: 0,
          symptoms: '',
          diagnosis: '',
          notes: '',
          prescription: [],
          currentDrug: { drugId: '', quantity: 1, dosage: '' }
        }
        
        await fetchDoctorActiveQueue()
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    const payBill = async (billId) => {
      try {
        loading.value = true
        if (isMockMode.value) {
          const bill = bills.value.find(b => b.id === billId)
          if (bill) bill.status = 'DaThanhToan'
          showAlert('Thu viện phí thành công (Giả lập)!', 'success')
        } else {
          // Gọi API Nhóm 6 trực tiếp để cập nhật trạng thái hóa đơn sang đã thanh toán (Paid)
          const url = `${authApiUrl.value}/Invoice/${billId}/status`
          const res = await fetch(url, {
            method: 'PUT',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${currentUser.value.token}`
            },
            body: JSON.stringify({ status: 'Paid' })
          })
          if (!res.ok) throw new Error('Thanh toán hóa đơn viện phí thất bại ở máy chủ Nhóm 6')
          
          const bill = bills.value.find(b => b.id === billId)
          if (bill) bill.status = 'DaThanhToan'
          showAlert('Đã thu viện phí liên thông thành công!', 'success')
        }
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    const addNewDrug = async () => {
      try {
        loading.value = true
        if (isMockMode.value) {
          const id = 'dr_' + Date.now()
          drugs.value.push({
            id,
            name: drugForm.value.name,
            activeIngredient: drugForm.value.activeIngredient,
            unit: drugForm.value.unit,
            price: drugForm.value.price,
            stock: drugForm.value.stock
          })
          showAlert(`Đã thêm thuốc mới ${drugForm.value.name} vào kho thành công (Giả lập)!`, 'success')
          drugForm.value = { name: '', activeIngredient: '', unit: 'Viên', price: 0, stock: 0 }
          addDrugDialog.value = false
        } else {
          const url = `${authApiUrl.value}/Medicine`
          const res = await fetch(url, {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${currentUser.value.token}`
            },
            body: JSON.stringify({
              name: drugForm.value.name,
              activeIngredient: drugForm.value.activeIngredient,
              unit: drugForm.value.unit,
              price: parseFloat(drugForm.value.price),
              stockQuantity: parseInt(drugForm.value.stock)
            })
          })
          if (!res.ok) {
            const errText = await res.text()
            throw new Error(`Không thể thêm thuốc mới vào kho Nhóm 6. Chi tiết: ${errText || res.statusText}`)
          }
          showAlert(`Đã thêm thuốc mới ${drugForm.value.name} vào kho Nhóm 6 thành công!`, 'success')
          drugForm.value = { name: '', activeIngredient: '', unit: 'Viên', price: 0, stock: 0 }
          addDrugDialog.value = false
          await fetchDrugs() // Tải lại danh sách thuốc thực tế từ Nhóm 6
        }
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    const addNewDoctor = async () => {
      try {
        loading.value = true
        const newDoc = {
          id: 'doc_' + Math.random().toString(36).substr(2, 9),
          fullName: doctorForm.value.fullName,
          specialty: doctorForm.value.specialty,
          qualifications: doctorForm.value.qualifications,
          consultationFee: doctorForm.value.consultationFee,
          isActive: true
        }

        if (isMockMode.value) {
          doctors.value.push(newDoc)
          specialties.value = [...new Set(doctors.value.map(d => d.specialty))]
          showAlert(`Đã thêm bác sĩ ${doctorForm.value.fullName} (Giả lập)`, 'success')
          addDoctorDialog.value = false
        } else {
          // Gọi API tạo bác sĩ thật Nhóm 1
          const url = `${apiUrl.value}/doctors`
          const res = await fetch(url, {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${currentUser.value.token}`
            },
            body: JSON.stringify(newDoc)
          })
          if (!res.ok) throw new Error('Không thể thêm bác sĩ vào máy chủ')
          await fetchDoctors()
          showAlert(`Đã thêm bác sĩ ${doctorForm.value.fullName} thành công!`, 'success')
          addDoctorDialog.value = false
        }
        doctorForm.value = { fullName: '', specialty: '', qualifications: '', consultationFee: 100000 }
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    // Bổ sung các biến và hàm cho đề tài 05
    const adminSubTab = ref('doctors-management')
    const patientSearchPhone = ref('')
    const patientHistories = ref([])
    const patientHistoryLoading = ref(false)
    const adminSchedules = ref([])
    const adminScheduleForm = ref({
      doctorId: '',
      date: new Date().toISOString().split('T')[0],
      startTime: '08:00:00',
      endTime: '12:00:00',
      shiftType: 'Sang',
      maxPatients: 10
    })
    const editDoctorDialog = ref(false)
    const addDrugDialog = ref(false)
    const addDoctorDialog = ref(false)
    const editingDoctor = ref({
      id: '',
      fullName: '',
      specialty: '',
      qualifications: '',
      consultationFee: 100000,
      isActive: true
    })
    const patientProfile = ref({
      phone: '',
      allergies: 'Không',
      medicalHistory: 'Không'
    })
    const patientHistoryDialog = ref(false)
    const adminFinancials = ref({
      totalRevenue: 0,
      paidCount: 0,
      paidAmount: 0,
      pendingCount: 0,
      pendingAmount: 0
    })

    const fetchPatientProfile = async (phone) => {
      if (!phone) return
      try {
        if (isMockMode.value) {
          patientProfile.value = {
            phone: phone,
            allergies: phone.endsWith('1') ? 'Dị ứng với Penicillin' : 'Không',
            medicalHistory: phone.endsWith('2') ? 'Tiền sử tăng huyết áp' : 'Không có tiền sử bệnh lý đặc biệt'
          }
        } else {
          const url = `${medicalApiUrl.value}/Patients?phone=${phone}`
          const res = await fetch(url, {
            headers: { 'Authorization': `Bearer ${currentUser.value.token}` }
          })
          if (res.ok) {
            const data = await res.json()
            patientProfile.value = {
              phone: phone,
              allergies: data.allergies || 'Không',
              medicalHistory: data.medicalHistory || 'Không'
            }
          } else {
            patientProfile.value = {
              phone: phone,
              allergies: 'Không tìm thấy dữ liệu',
              medicalHistory: 'Không tìm thấy dữ liệu'
            }
          }
        }
      } catch (e) {
        console.error('Không thể tải hồ sơ bệnh nhân', e)
        patientProfile.value = {
          phone: phone,
          allergies: 'Lỗi kết nối API hồ sơ bệnh nhân',
          medicalHistory: 'Lỗi kết nối API hồ sơ bệnh nhân'
        }
      }
    }

    const fetchPatientHistory = async (phone) => {
      if (!phone) return
      patientHistoryLoading.value = true
      try {
        if (isMockMode.value) {
          const records = medicalRecords.value.filter(r => r.patientPhone === phone)
          patientHistories.value = records.map(r => ({
            id: r.id,
            appointmentCode: 'APP_' + Math.floor(Math.random() * 100000),
            patientName: r.patientName,
            patientPhone: r.patientPhone,
            appointmentDate: r.date,
            timeSlot: '09:00:00',
            doctorName: 'Bác sĩ mẫu',
            status: 'DaKham',
            notes: r.notes,
            diagnosis: r.diagnosis,
            symptoms: r.symptoms,
            prescription: r.prescription || []
          }))
        } else {
          const appUrl = `${apiUrl.value}/appointments/by-phone/${phone}`
          const appRes = await fetch(appUrl)
          let appointmentsData = []
          if (appRes.ok) {
            appointmentsData = await appRes.json()
          }

          const mrUrl = `${medicalApiUrl.value}/MedicalRecords?phone=${phone}`
          let medicalRecordsData = []
          try {
            const mrRes = await fetch(mrUrl, {
              headers: { 'Authorization': `Bearer ${currentUser.value.token}` }
            })
            if (mrRes.ok) {
              medicalRecordsData = await mrRes.json()
            }
          } catch (e) {
            console.error('Không thể kết nối Medical Record Service', e)
          }

          patientHistories.value = appointmentsData.map(app => {
            const record = medicalRecordsData.find(mr => 
              mr.appointmentId === app.id || 
              (mr.patientPhone === app.patientPhone && mr.date.startsWith(app.appointmentDate.split('T')[0]))
            )
            return {
              id: app.id,
              appointmentCode: app.appointmentCode,
              patientName: app.patientName,
              patientPhone: app.patientPhone,
              appointmentDate: app.appointmentDate,
              timeSlot: app.timeSlot,
              doctorName: app.doctorName,
              status: app.status,
              notes: app.notes,
              diagnosis: record ? record.diagnosis : 'Chưa ghi nhận',
              symptoms: record ? record.symptoms : 'Chưa ghi nhận',
              prescription: record ? record.prescription : []
            }
          })
        }
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        patientHistoryLoading.value = false
      }
    }

    const viewPatientHistory = async (phone) => {
      await fetchPatientHistory(phone)
      patientHistoryDialog.value = true
    }

    const editDoctor = (doc) => {
      editingDoctor.value = { ...doc }
      editDoctorDialog.value = true
    }

    const updateDoctorInfo = async () => {
      try {
        loading.value = true
        if (isMockMode.value) {
          const idx = doctors.value.findIndex(d => d.id === editingDoctor.value.id)
          if (idx !== -1) {
            doctors.value[idx] = { ...editingDoctor.value }
            specialties.value = [...new Set(doctors.value.map(d => d.specialty))]
          }
          showAlert(`Đã cập nhật thông tin bác sĩ ${editingDoctor.value.fullName} (Giả lập)`, 'success')
        } else {
          const url = `${apiUrl.value}/doctors/${editingDoctor.value.id}`
          const res = await fetch(url, {
            method: 'PUT',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${currentUser.value.token}`
            },
            body: JSON.stringify(editingDoctor.value)
          })
          if (!res.ok) throw new Error('Không thể cập nhật thông tin bác sĩ')
          await fetchDoctors()
          showAlert(`Đã cập nhật thông tin bác sĩ ${editingDoctor.value.fullName} thành công!`, 'success')
        }
        editDoctorDialog.value = false
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    const deleteDoctorInfo = async (id) => {
      const docName = doctors.value.find(d => d.id === id)?.fullName || ''
      if (!confirm(`Bạn có chắc chắn muốn ngừng kích hoạt bác sĩ ${docName}?`)) return
      try {
        loading.value = true
        if (isMockMode.value) {
          const doc = doctors.value.find(d => d.id === id)
          if (doc) {
            doc.isActive = !doc.isActive
          }
          showAlert('Đã thay đổi trạng thái hoạt động bác sĩ (Giả lập)', 'success')
        } else {
          const url = `${apiUrl.value}/doctors/${id}`
          const res = await fetch(url, {
            method: 'DELETE',
            headers: {
              'Authorization': `Bearer ${currentUser.value.token}`
            }
          })
          if (!res.ok) throw new Error('Không thể thay đổi trạng thái hoạt động bác sĩ')
          await fetchDoctors()
          showAlert('Đã thay đổi trạng thái hoạt động bác sĩ thành công!', 'success')
        }
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    const fetchAdminSchedules = async () => {
      if (isMockMode.value) {
        adminSchedules.value = [
          { id: 'sch1', doctorId: 'd1d11111-1111-1111-1111-111111111111', doctorName: 'Nguyễn Văn An', date: '2026-06-18', startTime: '08:00:00', endTime: '12:00:00', shiftType: 'Sang', currentBookings: 2, maxPatients: 10 },
          { id: 'sch2', doctorId: 'd2d22222-2222-2222-2222-222222222222', doctorName: 'Trần Thị Bình', date: '2026-06-18', startTime: '13:30:00', endTime: '17:30:00', shiftType: 'Chieu', currentBookings: 1, maxPatients: 15 }
        ]
        return
      }
      try {
        const url = `${apiUrl.value}/schedules`
        const res = await fetch(url, {
          headers: {
            'Authorization': `Bearer ${currentUser.value.token}`
          }
        })
        if (!res.ok) throw new Error('Không thể tải lịch trực của bác sĩ')
        const data = await res.json()
        adminSchedules.value = data.map(s => {
          const doc = doctors.value.find(d => d.id === s.doctorId)
          return {
            ...s,
            doctorName: doc ? doc.fullName : 'Không xác định',
            shiftType: s.shiftType === 0 ? 'Sang' : (s.shiftType === 1 ? 'Chieu' : 'Toi')
          }
        })
      } catch (err) {
        showAlert(err.message, 'error')
      }
    }

    const createAdminSchedule = async () => {
      try {
        loading.value = true
        const shiftVal = adminScheduleForm.value.shiftType === 'Sang' ? 0 : (adminScheduleForm.value.shiftType === 'Chieu' ? 1 : 2)
        const newSchedule = {
          doctorId: adminScheduleForm.value.doctorId,
          date: adminScheduleForm.value.date,
          startTime: adminScheduleForm.value.startTime,
          endTime: adminScheduleForm.value.endTime,
          shiftType: shiftVal,
          maxPatients: adminScheduleForm.value.maxPatients
        }

        if (isMockMode.value) {
          const doc = doctors.value.find(d => d.id === newSchedule.doctorId)
          adminSchedules.value.push({
            id: 'sch_' + Math.random().toString(36).substr(2, 9),
            doctorId: newSchedule.doctorId,
            doctorName: doc ? doc.fullName : 'Bác sĩ giả lập',
            date: newSchedule.date,
            startTime: newSchedule.startTime,
            endTime: newSchedule.endTime,
            shiftType: adminScheduleForm.value.shiftType,
            currentBookings: 0,
            maxPatients: newSchedule.maxPatients
          })
          showAlert('Đã thêm lịch trực mới (Giả lập)', 'success')
        } else {
          const url = `${apiUrl.value}/schedules`
          const res = await fetch(url, {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${currentUser.value.token}`
            },
            body: JSON.stringify(newSchedule)
          })
          if (!res.ok) {
            const errMsg = await res.text()
            throw new Error(errMsg || 'Không thể tạo lịch trực mới')
          }
          await fetchAdminSchedules()
          showAlert('Đã tạo lịch trực mới thành công!', 'success')
        }
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    const fetchAdminFinancials = async () => {
      if (isMockMode.value) {
        const paid = bills.value.filter(b => b.status === 'DaThanhToan' || b.status === 'Paid')
        const pending = bills.value.filter(b => b.status === 'ChuaThanhToan' || b.status === 'Unpaid')
        const totalPaid = paid.reduce((sum, b) => sum + (b.totalAmount || 0), 0)
        const totalPending = pending.reduce((sum, b) => sum + (b.totalAmount || 0), 0)
        adminFinancials.value = {
          totalRevenue: totalPaid,
          paidCount: paid.length,
          paidAmount: totalPaid,
          pendingCount: pending.length,
          pendingAmount: totalPending
        }
        return
      }
      try {
        const url = `${authApiUrl.value}/Invoice`
        const res = await fetch(url, {
          headers: { 'Authorization': `Bearer ${currentUser.value.token}` }
        })
        if (!res.ok) throw new Error('Không thể tải dữ liệu báo cáo tài chính')
        const data = await res.json()
        const paid = data.filter(b => b.status === 'DaThanhToan' || b.status === 'Paid')
        const pending = data.filter(b => b.status === 'ChuaThanhToan' || b.status === 'Unpaid')
        const totalPaid = paid.reduce((sum, b) => sum + (b.totalAmount || b.amount || 0), 0)
        const totalPending = pending.reduce((sum, b) => sum + (b.totalAmount || b.amount || 0), 0)
        adminFinancials.value = {
          totalRevenue: totalPaid,
          paidCount: paid.length,
          paidAmount: totalPaid,
          pendingCount: pending.length,
          pendingAmount: totalPending
        }
      } catch (err) {
        showAlert(err.message, 'error')
      }
    }

    const fetchBills = async () => {
      if (isMockMode.value) return
      try {
        const url = `${authApiUrl.value}/Invoice`
        const res = await fetch(url, {
          headers: { 'Authorization': `Bearer ${currentUser.value.token}` }
        })
        if (!res.ok) throw new Error('Không thể tải danh sách hóa đơn từ Nhóm 6')
        const data = await res.json()
        
        // Chuẩn hóa dữ liệu hóa đơn từ Nhóm 6
        bills.value = data.map(b => ({
          id: b.id || b.billId,
          patientName: b.patientName || 'Bệnh nhân',
          patientPhone: b.patientPhone || 'Không có',
          consultationFee: b.consultationFee || 0,
          medicationFee: b.medicationFee || 0,
          totalAmount: b.totalAmount || b.amount || 0,
          status: (b.status === 'Paid' || b.status === 'DaThanhToan') ? 'DaThanhToan' : 'ChuaThanhToan',
          date: b.date || b.createdDate || new Date().toISOString()
        }))
      } catch (err) {
        console.error('Lỗi tải danh sách hóa đơn:', err.message)
      }
    }

    const fetchDrugs = async () => {
      if (isMockMode.value) return
      try {
        const url = `${authApiUrl.value}/Medicine`
        const res = await fetch(url, {
          headers: { 'Authorization': `Bearer ${currentUser.value.token}` }
        })
        if (!res.ok) throw new Error('Không thể tải danh sách thuốc từ Nhóm 6')
        const data = await res.json()
        drugs.value = data.map(d => ({
          id: d.id,
          name: d.name,
          activeIngredient: d.activeIngredient || '',
          unit: d.unit || 'Viên',
          price: d.price || 0,
          stock: d.stockQuantity || d.stock || 0
        }))
      } catch (err) {
        console.error('Lỗi tải danh sách thuốc:', err.message)
      }
    }

    const printPrescription = (historyItem) => {
      const printWindow = window.open('', '_blank');
      const htmlContent = `
        <html>
          <head>
            <title>Đơn Thuốc - Phòng Khám Clinic</title>
            <style>
              body { font-family: 'Arial', sans-serif; padding: 40px; color: #000; }
              h1, h3 { text-align: center; margin-bottom: 5px; }
              .info { margin-bottom: 20px; line-height: 1.6; }
              .label { font-weight: bold; }
              table { width: 100%; border-collapse: collapse; margin-top: 20px; }
              th, td { border: 1px solid #000; padding: 10px; text-align: left; }
              th { background-color: #f2f2f2; }
              .footer { margin-top: 40px; text-align: right; font-style: italic; }
            </style>
          </head>
          <body>
            <h1>ĐƠN THUỐC ĐIỆN TỬ</h1>
            <h3>Phòng Khám Đa Khoa Spro</h3>
            <hr />
            <div class="info">
              <p><span class="label">Bệnh nhân:</span> ${historyItem.patientName}</p>
              <p><span class="label">Số điện thoại:</span> ${historyItem.patientPhone}</p>
              <p><span class="label">Ngày khám:</span> ${formatDate(historyItem.appointmentDate)}</p>
              <p><span class="label">Bác sĩ kê đơn:</span> ${historyItem.doctorName}</p>
              <p><span class="label">Triệu chứng:</span> ${historyItem.symptoms || 'Chưa ghi nhận'}</p>
              <p><span class="label">Chẩn đoán:</span> ${historyItem.diagnosis || 'Chưa ghi nhận'}</p>
              <p><span class="label">Lời dặn:</span> ${historyItem.notes || 'Uống thuốc theo đơn'}</p>
            </div>
            <table>
              <thead>
                <tr>
                  <th>Tên Thuốc</th>
                  <th>Số Lượng</th>
                  <th>Liều Dùng</th>
                </tr>
              </thead>
              <tbody>
                ${(historyItem.prescription || []).map(p => `
                  <tr>
                    <td>${p.drugName || p.name}</td>
                    <td>${p.quantity}</td>
                    <td>${p.dosage}</td>
                  </tr>
                `).join('')}
              </tbody>
            </table>
            <div class="footer">
              <p>Ngày ${new Date().toLocaleDateString('vi-VN')}</p>
              <p>Bác sĩ ký tên</p>
              <br/><br/><br/>
              <p>${historyItem.doctorName}</p>
            </div>
            <script>
              window.onload = function() { window.print(); window.close(); }
            </scr${'ipt'}>
          </body>
        </html>
      `;
      printWindow.document.write(htmlContent);
      printWindow.document.close();
    }

    const printQueueTicket = (ticket) => {
      const printWindow = window.open('', '_blank');
      const htmlContent = `
        <html>
          <head>
            <title>Phiếu Khám Bệnh - ${ticket.patientName}</title>
            <style>
              body { font-family: 'Arial', sans-serif; padding: 30px; text-align: center; color: #000; }
              .ticket-container { border: 2px solid #000; padding: 20px; border-radius: 10px; max-width: 400px; margin: 0 auto; }
              .clinic-name { font-size: 18px; font-weight: bold; margin-bottom: 5px; }
              .number { font-size: 72px; font-weight: bold; margin: 20px 0; }
              .details { text-align: left; line-height: 1.8; margin-top: 15px; border-top: 1px dashed #000; padding-top: 10px; }
              .label { font-weight: bold; }
              .footer { margin-top: 20px; font-size: 12px; font-style: italic; }
            </style>
          </head>
          <body>
            <div class="ticket-container">
              <div class="clinic-name">PHÒNG KHÁM ĐA KHOA SPRO</div>
              <div>PHIẾU KHÁM BỆNH</div>
              <div class="number">${ticket.queueNumber || 'N/A'}</div>
              <div class="details">
                <div><span class="label">Bệnh nhân:</span> ${ticket.patientName}</div>
                <div><span class="label">Số điện thoại:</span> ${ticket.patientPhone}</div>
                <div><span class="label">Bác sĩ:</span> ${ticket.doctorName}</div>
                <div><span class="label">Khung giờ:</span> ${ticket.timeSlot}</div>
                <div><span class="label">Ngày khám:</span> ${formatDate(ticket.appointmentDate)}</div>
              </div>
              <div class="footer">Vui lòng chờ gọi số thứ tự tại cửa phòng khám.</div>
            </div>
            <script>
              window.onload = function() { window.print(); window.close(); }
            </scr${'ipt'}>
          </body>
        </html>
      `;
      printWindow.document.write(htmlContent);
      printWindow.document.close();
    }

    // Watch tab changes to auto-load data for the selected tab
    watch(activeTab, (newTab) => {
      if (newTab === 'reception' && jwtToken.value) {
        fetchPendingAppointments()
      } else if (newTab === 'doctor' && doctorPortal.value.doctorId) {
        fetchDoctorActiveQueue()
      } else if (newTab === 'tv') {
        fetchTvQueue()
      } else if (newTab === 'pharmacy' && jwtToken.value) {
        fetchBills()
      }
    })

    // Auto update TV panel
    onMounted(() => {
      // Phục hồi activeTab nếu có từ query url
      if (route.query.tab) {
        activeTab.value = route.query.tab
      }

      // Giải mã và phục hồi trạng thái từ token có sẵn trong localStorage
      if (currentUser.value.token) {
        syncUserFromToken(currentUser.value.token)
        fetchBills()
        fetchDrugs()
      }

      fetchDoctors()
      fetchTvQueue()

      // Set interval for TV board every 5s
      tvInterval = setInterval(() => {
        fetchTvQueue()
        if (activeTab.value === 'doctor') {
          fetchDoctorActiveQueue()
        }
        if (activeTab.value === 'reception' && jwtToken.value) {
          fetchPendingAppointments()
        }
        if (activeTab.value === 'pharmacy' && jwtToken.value) {
          fetchDrugs()
        }
        if (activeTab.value === 'billing' && jwtToken.value) {
          fetchBills()
        }
      }, 5000)
    })

    onUnmounted(() => {
      if (tvInterval) clearInterval(tvInterval)
    })

    return {
      // Sidebar & Layout
      sidebarOpen,
      sidebarRail,
      headerSearch,
      menuOpen,
      apiUrl,
      authApiUrl,
      medicalApiUrl,
      jwtToken,
      activeRole,
      activeTab,
      loading,
      alert,
      
      // Mock & Auth
      isMockMode,
      currentUser,
      loginForm,
      drugs,
      bills,
      medicalRecords,
      drugForm,
      doctorForm,
      activeConsultation,
      
      // Data refs
      doctors,
      specialties,
      selectedSpecialty,
      availableSchedules,
      bookedSlots,
      activeTimeSlotsList,
      pendingAppointments,
      recentTicket,
      bookingForm,
      
      // Search
      searchQuery,
      searchResults,
      lastConfirmedApp,
      
      // Doctor board
      doctorPortal,
      doctorQueue,
      sortedDoctorQueue,
      
      // TV Queue
      tvQueue,
      tvQueueGroupedByDoctor,

      // Bổ sung đề tài 05
      adminSubTab,
      patientSearchPhone,
      patientHistories,
      patientHistoryLoading,
      adminSchedules,
      adminScheduleForm,
      editDoctorDialog,
      editingDoctor,
      patientProfile,
      patientHistoryDialog,
      adminFinancials,

      // Functions
      selectDoctor,
      selectSchedule,
      fetchBookedSlots,
      bookAppointment,
      confirmAppointment,
      cancelAppointment,
      searchAppointments,
      clearSearch,
      fetchPendingAppointments,
      fetchDoctorActiveQueue,
      fetchBills,
      fetchDrugs,
      updateQueueStatus,
      fetchTestToken,
      onFilterChange,
      applyConfiguration,
      handleLogin,
      handleLogout,
      quickLogin,
      selectConsultingPatient,
      addDrugToPrescription,
      removeDrugFromPrescription,
      onDrugSelected,
      viewPatientHistory,
      submitConsultation,
      payBill,
      addNewDrug,
      addDrugDialog,
      addNewDoctor,
      addDoctorDialog,
      editDoctor,
      updateDoctorInfo,
      deleteDoctorInfo,
      fetchAdminSchedules,
      createAdminSchedule,
      fetchAdminFinancials,
      fetchPatientProfile,
      fetchPatientHistory,
      printPrescription,
      printQueueTicket,
      
      // Helpers
      hasRole,
      formatMoney,
      formatDate,
      formatTime,
      formatTimeSpan,
      getStatusColor,
      filteredDoctors
    }
  }
}
</script>

<style>
.v-navigation-drawer {
  position: fixed !important;
  height: 100vh !important;
  top: 0 !important;
}

.hover-card {
  transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
  border: 1px solid #DFE1E6 !important;
}

.hover-card:hover {
  transform: translateY(-4px);
  box-shadow: 0px 8px 24px rgba(0, 61, 155, 0.08) !important;
  border-color: #003D9B !important;
}

/* Custom styles for background container */
.v-application {
  background: #F4F5F7 !important;
}

.bg-background {
  background-color: #F4F5F7 !important;
}

.bg-surface {
  background-color: #FFFFFF !important;
}

.bg-slate-900 {
  background-color: #F8F9FB !important;
}

/* Premium card shadow for login and modal */
.elevation-12 {
  box-shadow: 0px 8px 30px rgba(0, 61, 155, 0.08) !important;
  border: 1px solid #DFE1E6 !important;
}

/* TV screen pulsing indicator */
.pulse-dot {
  width: 12px;
  height: 12px;
  background-color: #F87171;
  border-radius: 50%;
  display: inline-block;
  box-shadow: 0 0 0 0 rgba(248, 113, 113, 0.7);
  animation: pulse 1.6s infinite;
}

@keyframes pulse {
  0% {
    transform: scale(0.95);
    box-shadow: 0 0 0 0 rgba(248, 113, 113, 0.7);
  }
  70% {
    transform: scale(1);
    box-shadow: 0 0 0 10px rgba(248, 113, 113, 0);
  }
  100% {
    transform: scale(0.95);
    box-shadow: 0 0 0 0 rgba(248, 113, 113, 0);
  }
}

/* Dashboard interactions */
.hover-bar {
  transition: all 0.2s ease-in-out;
}
.hover-bar:hover {
  opacity: 0.8 !important;
  transform: scaleY(1.02);
}

.hover-row {
  transition: background-color 0.2s ease;
}
.hover-row:hover {
  background-color: rgba(0, 61, 155, 0.03) !important;
}

.hover-card-border {
  transition: border-color 0.2s ease;
}
.hover-card-border:hover {
  border-color: #003D9B !important;
}


/* ============================================
   SIDEBAR NAVIGATION STYLES (Đồng bộ PHP qlsinhvien)
   ============================================ */

/* Sidebar menu item base */
.sidebar-item {
  transition: all 0.2s ease !important;
  border-radius: 8px !important;
  color: var(--text-secondary) !important;
}

.sidebar-item:hover {
  background-color: var(--primary-color) !important;
  color: #ffffff !important;
}

.sidebar-item:hover .v-icon,
.sidebar-item:hover .v-list-item-title,
.sidebar-item:hover .v-list-item-subtitle {
  color: #ffffff !important;
}

/* Active sidebar item */
.sidebar-item-active {
  background-color: var(--primary-color) !important;
  color: #ffffff !important;
}

.sidebar-item-active .v-icon,
.sidebar-item-active .v-list-item-title,
.sidebar-item-active .v-list-item-subtitle {
  color: #ffffff !important;
  font-weight: 600 !important;
}

/* Fade transition for sidebar label */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

/* Header search field refinement */
.v-app-bar .v-text-field {
  border-radius: 8px;
}

/* Ensure content area has proper left padding when sidebar is open */
.v-main {
  background-color: var(--secondary-color) !important;
}
</style>
