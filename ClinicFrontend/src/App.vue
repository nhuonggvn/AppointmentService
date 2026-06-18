<template>
  <v-app>
    <!-- App Bar with Premium Theme -->
    <v-app-bar flat border color="surface">
      <v-app-bar-title class="font-weight-bold text-primary">
        <v-icon icon="mdi-heart-pulse" color="primary" class="mr-2" />
        CLINIC BOOKING SYSTEM
      </v-app-bar-title>

      <v-spacer />

      <!-- API Configuration Panel -->
      <v-menu v-model="menuOpen" :close-on-content-click="false" location="bottom end">
        <template v-slot:activator="{ props }">
          <v-btn v-bind="props" variant="text" prepend-icon="mdi-cog">
            Cấu hình kết nối
          </v-btn>
        </template>
        <v-card min-width="320" class="pa-4 bg-surface" border>
          <div class="text-subtitle-1 font-weight-bold mb-2">Cài đặt API</div>
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
            v-model="jwtToken"
            label="JWT Token"
            density="compact"
            variant="outlined"
            hide-details
            class="mb-3"
            @keyup.enter="applyConfiguration"
          />
          <v-btn block color="primary" size="small" class="mb-3 font-weight-bold" @click="applyConfiguration">
            Áp dụng cấu hình
          </v-btn>
          
          <v-divider class="my-2" />
          
          <div class="text-subtitle-2 font-weight-bold mb-1">Đăng nhập nhanh (TestAuth)</div>
          <v-row density="comfortable">
            <v-col cols="4">
              <v-btn block size="small" color="primary" @click="fetchTestToken('Admin')">Admin</v-btn>
            </v-col>
            <v-col cols="4">
              <v-btn block size="small" color="secondary" @click="fetchTestToken('Receptionist')">Tiếp Tân</v-btn>
            </v-col>
            <v-col cols="4">
              <v-btn block size="small" color="success" @click="fetchTestToken('Doctor')">Bác Sĩ</v-btn>
            </v-col>
          </v-row>
          <div v-if="activeRole" class="text-caption text-success mt-2">
            Đang chạy với vai trò: <strong>{{ activeRole }}</strong>
          </div>
        </v-card>
      </v-menu>
    </v-app-bar>

    <v-main class="bg-background">
      <v-container fluid class="pa-6">
        
        <!-- MÀN HÌNH ĐĂNG NHẬP (Hiển thị nếu chưa đăng nhập và không phải đang xem màn hình TV) -->
        <div v-if="!currentUser.token && activeTab !== 'tv'" class="d-flex align-center justify-center" style="min-height: 75vh;">
          <v-card width="450" class="pa-8 bg-surface rounded-lg elevation-12" border>
            <div class="text-center mb-6">
              <v-icon icon="mdi-heart-pulse" color="primary" size="48" class="mb-2" />
              <h2 class="text-h5 font-weight-bold text-primary">CLINIC BOOKING SYSTEM</h2>
              <div class="text-caption text-grey-lighten-1">Hệ thống đặt lịch & quản lý phòng khám liên thông</div>
            </div>

            <!-- Login Form -->
            <v-form @submit.prevent="handleLogin">
              <v-text-field
                v-model="loginForm.username"
                label="Tên đăng nhập"
                prepend-inner-icon="mdi-account"
                variant="outlined"
                density="comfortable"
                class="mb-4"
                required
              />
              <v-text-field
                v-model="loginForm.password"
                label="Mật khẩu"
                prepend-inner-icon="mdi-lock"
                type="password"
                variant="outlined"
                density="comfortable"
                class="mb-6"
                required
              />
              <v-btn type="submit" color="primary" block size="large" class="font-weight-bold mb-4" :loading="loading">
                Đăng Nhập
              </v-btn>
            </v-form>

            <v-divider class="my-4" />

            <!-- Quick Demo Login -->
            <div class="text-subtitle-2 font-weight-bold mb-2 text-center text-grey-lighten-1">Đăng nhập nhanh để Demo (BTL)</div>
            <v-row dense class="mb-4">
              <v-col cols="6">
                <v-btn block variant="tonal" color="primary" size="small" class="font-weight-bold" @click="quickLogin('Admin')">
                  <v-icon icon="mdi-shield-account" class="mr-1" /> Admin
                </v-btn>
              </v-col>
              <v-col cols="6">
                <v-btn block variant="tonal" color="success" size="small" class="font-weight-bold" @click="quickLogin('Doctor')">
                  <v-icon icon="mdi-doctor" class="mr-1" /> Bác Sĩ
                </v-btn>
              </v-col>
              <v-col cols="6">
                <v-btn block variant="tonal" color="secondary" size="small" class="font-weight-bold" @click="quickLogin('Receptionist')">
                  <v-icon icon="mdi-account-tie" class="mr-1" /> Tiếp Tân
                </v-btn>
              </v-col>
              <v-col cols="6">
                <v-btn block variant="tonal" color="info" size="small" class="font-weight-bold" @click="quickLogin('Patient')">
                  <v-icon icon="mdi-account" class="mr-1" /> Bệnh Nhân
                </v-btn>
              </v-col>
            </v-row>

            <div class="text-center">
              <v-btn variant="text" color="warning" size="small" @click="activeTab = 'tv'">
                <v-icon icon="mdi-television-classic" class="mr-1" /> Xem Tivi Hàng Chờ (Công cộng)
              </v-btn>
            </div>
          </v-card>
        </div>

        <!-- GIAO DIỆN CHÍNH SAU KHI ĐĂNG NHẬP HOẶC KHI XEM MÀN HÌNH TV -->
        <div v-else>
          <!-- Nút Đăng xuất & Thông tin người dùng -->
          <div class="d-flex justify-space-between align-center mb-6 bg-surface pa-4 rounded-lg border">
            <div class="d-flex align-center">
              <v-avatar color="primary" size="40" class="mr-3 text-subtitle-1 font-weight-bold text-white">
                {{ (currentUser.username || 'TV').substring(0, 2).toUpperCase() }}
              </v-avatar>
              <div>
                <div class="font-weight-bold text-subtitle-1">
                  {{ currentUser.username || 'Màn hình Tivi công cộng' }}
                </div>
                <div class="text-caption text-grey">
                  Vai trò: <strong class="text-primary">{{ currentUser.role || 'Khách xem TV' }}</strong>
                </div>
              </div>
            </div>
            
            <div class="d-flex align-center">
              <!-- Switch Mock Mode -->
              <v-switch
                v-model="isMockMode"
                color="warning"
                label="Chế độ giả lập (Mock API)"
                hide-details
                density="compact"
                class="mr-6 font-weight-bold"
              />
              <v-btn v-if="currentUser.token" variant="outlined" color="error" size="small" prepend-icon="mdi-logout" @click="handleLogout">
                Đăng xuất
              </v-btn>
              <v-btn v-else variant="flat" color="primary" size="small" prepend-icon="mdi-login" @click="activeTab = 'booking'">
                Đăng nhập hệ thống
              </v-btn>
            </div>
          </div>

          <!-- Tab navigation (Lọc theo vai trò) -->
          <v-tabs v-model="activeTab" bg-color="surface" align-tabs="start" rounded="lg" class="mb-6" border>
            <!-- Bệnh nhân: Thấy đặt lịch khám -->
            <v-tab v-if="currentUser.role === 'Patient' || currentUser.role === 'Admin'" value="booking" class="font-weight-bold">
              <v-icon icon="mdi-calendar-plus" class="mr-2" />
              Bệnh Nhân Đặt Lịch
            </v-tab>
            <!-- Tiếp tân: Thấy tiếp nhận & xếp hàng -->
            <v-tab v-if="currentUser.role === 'Receptionist' || currentUser.role === 'Admin'" value="reception" class="font-weight-bold">
              <v-icon icon="mdi-clipboard-text-play" class="mr-2" />
              Quầy Tiếp Tân
            </v-tab>
            <!-- Bác sĩ: Thấy phòng khám bác sĩ -->
            <v-tab v-if="currentUser.role === 'Doctor' || currentUser.role === 'Admin'" value="doctor" class="font-weight-bold">
              <v-icon icon="mdi-doctor" class="mr-2" />
              Phòng Khám Bác Sĩ
            </v-tab>
            <!-- Dược sĩ & Thu ngân (Tiếp tân/Thu ngân): Thấy Kho thuốc & viện phí -->
            <v-tab v-if="currentUser.role === 'Receptionist' || currentUser.role === 'Admin'" value="pharmacy" class="font-weight-bold">
              <v-icon icon="mdi-pill" class="mr-2" color="success" />
              Kho Thuốc & Viện Phí
            </v-tab>
            <!-- Admin: Thấy Quản lý Admin -->
            <v-tab v-if="currentUser.role === 'Admin'" value="admin" class="font-weight-bold">
              <v-icon icon="mdi-cog" class="mr-2" color="primary" />
              Quản Trị Bác Sĩ
            </v-tab>
            <!-- Mọi người: Xem được Tivi hàng chờ -->
            <v-tab value="tv" class="font-weight-bold">
              <v-icon icon="mdi-television-classic" class="mr-2" color="warning" />
              Màn Hình Tivi Hàng Chờ
            </v-tab>
          </v-tabs>

        <!-- Dynamic Alert Notification -->
        <v-alert
          v-if="alert.show"
          :type="alert.type"
          closable
          class="mb-6"
          variant="tonal"
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
                        :style="bookingForm.doctorId === doc.id ? 'border: 2px solid #38BDF8 !important' : ''"
                        @click="selectDoctor(doc)"
                      >
                        <div class="d-flex justify-between align-center mb-1">
                          <span class="font-weight-bold text-subtitle-1">{{ doc.fullName }}</span>
                          <v-chip size="x-small" color="primary" variant="flat">{{ doc.specialty }}</v-chip>
                        </div>
                        <div class="text-caption text-grey-lighten-1 mb-2">{{ doc.qualifications }}</div>
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
                    <v-text-field
                      v-model="bookingForm.timeSlot"
                      label="Khung giờ muốn khám (Giờ cụ thể, vd: 08:30:00)"
                      variant="outlined"
                      placeholder="hh:mm:ss"
                      class="mb-3"
                      required
                    />

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
                      :disabled="!bookingForm.doctorId || !bookingForm.scheduleId || loading"
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
                        <div class="text-subtitle-2 font-weight-bold text-grey-lighten-1">Thông tin bệnh nhân</div>
                        <div class="text-body-1 py-1">Họ tên: <strong>{{ activeConsultation.patientName }}</strong></div>
                        <div class="text-body-1 py-1">Số thứ tự khám: <strong>{{ activeConsultation.queueNumber }}</strong></div>
                      </v-col>
                      <v-col cols="12" sm="6">
                        <div class="text-subtitle-2 font-weight-bold text-grey-lighten-1">Lịch sử khám bệnh cũ</div>
                        <v-btn size="small" variant="outlined" color="info" class="mt-1" @click="viewPatientHistory(activeConsultation.patientPhone)">
                          Xem lịch sử bệnh án
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
            <v-row>
              <!-- Thu viện phí -->
              <v-col cols="12" md="6">
                <v-card border flat class="bg-surface pa-6 mb-6">
                  <div class="text-h6 font-weight-bold mb-4 text-primary">Danh sách hóa đơn viện phí phòng khám</div>
                  
                  <v-alert v-if="bills.length === 0" type="info" variant="tonal">
                    Không có hóa đơn thanh toán nào hôm nay.
                  </v-alert>

                  <v-list v-else bg-color="transparent" class="pa-0">
                    <v-card v-for="b in bills" :key="b.id" border flat class="mb-3 pa-4">
                      <div class="d-flex justify-space-between align-center mb-2">
                        <span class="font-weight-bold">{{ b.patientName }}</span>
                        <v-chip size="small" :color="b.status === 'DaThanhToan' ? 'success' : 'warning'">
                          {{ b.status === 'DaThanhToan' ? 'Đã Thanh Toán' : 'Chờ Thanh Toán' }}
                        </v-chip>
                      </div>
                      <div class="text-caption text-grey mb-1">Điện thoại: {{ b.patientPhone }}</div>
                      <div class="text-caption text-grey mb-2">Ngày khám: {{ formatDate(b.date) }}</div>
                      
                      <v-divider class="my-2" />
                      
                      <div class="d-flex justify-space-between text-body-2 mb-1">
                        <span>Phí khám bệnh:</span>
                        <span>{{ formatMoney(b.consultationFee) }}đ</span>
                      </div>
                      <div class="d-flex justify-space-between text-body-2 mb-1">
                        <span>Tiền thuốc đơn thuốc:</span>
                        <span>{{ formatMoney(b.medicationFee) }}đ</span>
                      </div>
                      <div class="d-flex justify-space-between font-weight-bold text-subtitle-1 my-2">
                        <span>Tổng tiền:</span>
                        <span class="text-success">{{ formatMoney(b.totalAmount) }}đ</span>
                      </div>

                      <v-btn
                        v-if="b.status === 'ChuaThanhToan'"
                        color="success"
                        block
                        size="small"
                        class="mt-2 font-weight-bold"
                        @click="payBill(b.id)"
                      >
                        Xác nhận thu viện phí
                      </v-btn>
                    </v-card>
                  </v-list>
                </v-card>
              </v-col>

              <!-- Quản lý kho thuốc -->
              <v-col cols="12" md="6">
                <v-card border flat class="bg-surface pa-6 mb-6">
                  <div class="text-h6 font-weight-bold mb-4 text-primary">Quản lý kho thuốc hiện tại</div>
                  
                  <!-- Form nhập thuốc mới -->
                  <div class="bg-slate-900 pa-4 rounded-lg border mb-4">
                    <div class="text-subtitle-2 font-weight-bold mb-2 text-grey-lighten-1">Nhập thêm thuốc mới vào kho</div>
                    <v-form @submit.prevent="addNewDrug">
                      <v-row dense>
                        <v-col cols="12" sm="6">
                          <v-text-field
                            v-model="drugForm.name"
                            label="Tên thuốc"
                            variant="outlined"
                            density="comfortable"
                            required
                          />
                        </v-col>
                        <v-col cols="12" sm="6">
                          <v-text-field
                            v-model="drugForm.activeIngredient"
                            label="Hoạt chất chính"
                            variant="outlined"
                            density="comfortable"
                          />
                        </v-col>
                        <v-col cols="4">
                          <v-text-field
                            v-model="drugForm.unit"
                            label="Đơn vị"
                            variant="outlined"
                            density="comfortable"
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
                            required
                          />
                        </v-col>
                      </v-row>
                      <v-btn type="submit" color="primary" block size="small" class="font-weight-bold mt-2">
                        Nhập kho thuốc
                      </v-btn>
                    </v-form>
                  </div>

                  <!-- Danh sách thuốc tồn kho -->
                  <v-table density="compact" class="bg-surface rounded border">
                    <thead>
                      <tr>
                        <th class="text-left font-weight-bold">Tên thuốc</th>
                        <th class="text-left font-weight-bold">Đơn vị</th>
                        <th class="text-left font-weight-bold">Đơn giá</th>
                        <th class="text-left font-weight-bold">Tồn kho</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="d in drugs" :key="d.id">
                        <td>
                          <div><strong>{{ d.name }}</strong></div>
                          <div class="text-caption text-grey">{{ d.activeIngredient }}</div>
                        </td>
                        <td>{{ d.unit }}</td>
                        <td>{{ formatMoney(d.price) }}đ</td>
                        <td>
                          <v-chip size="small" :color="d.stock > 100 ? 'success' : 'error'">
                            {{ d.stock }}
                          </v-chip>
                        </td>
                      </tr>
                    </tbody>
                  </v-table>
                </v-card>
              </v-col>
            </v-row>
          </v-window-item>

          <!-- 5. ADMIN PORTAL TAB (Quản trị Bác sĩ & Lịch hẹn của Nhóm 1) -->
          <v-window-item value="admin">
            <v-row>
              <!-- Thêm bác sĩ trực ca -->
              <v-col cols="12" md="4">
                <v-card border flat class="bg-surface pa-6 mb-6">
                  <div class="text-h6 font-weight-bold mb-4 text-primary">Thêm Bác sĩ trực ca</div>
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
                      label="Phí khám bệnh"
                      type="number"
                      variant="outlined"
                      density="comfortable"
                      class="mb-4"
                      required
                    />
                    <v-btn type="submit" color="primary" block size="large" class="font-weight-bold" :loading="loading">
                      Lưu thông tin bác sĩ
                    </v-btn>
                  </v-form>
                </v-card>
              </v-col>

              <!-- Danh sách Bác sĩ hiện tại -->
              <v-col cols="12" md="8">
                <v-card border flat class="bg-surface pa-6 mb-6">
                  <div class="text-h6 font-weight-bold mb-4 text-primary">Danh sách bác sĩ phòng khám</div>
                  
                  <v-table density="comfortable" class="bg-surface rounded border">
                    <thead>
                      <tr>
                        <th class="text-left font-weight-bold">Bác sĩ</th>
                        <th class="text-left font-weight-bold">Chuyên khoa</th>
                        <th class="text-left font-weight-bold">Bằng cấp</th>
                        <th class="text-left font-weight-bold">Phí khám</th>
                        <th class="text-left font-weight-bold">Trạng thái</th>
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
                      </tr>
                    </tbody>
                  </v-table>
                </v-card>
              </v-col>
            </v-row>
          </v-window-item>

          <!-- 6. TV QUEUE SCREEN TAB -->
          <v-window-item value="tv">
            <v-card border flat class="bg-surface pa-8">
              <div class="d-flex align-center justify-space-between mb-6">
                <div class="d-flex align-center">
                  <span class="pulse-dot mr-3"></span>
                  <h1 class="text-h4 font-weight-black text-primary">MÀN HÌNH THEO DÕI HÀNG CHỜ PHÒNG KHÁM</h1>
                </div>
                <div class="text-subtitle-1 text-grey-lighten-1">
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
                    <div v-if="group.waitingList.length === 0" class="text-body-2 text-grey-lighten-1 text-center py-2">
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
  </v-app>
</template>

<script>
import { ref, onMounted, onUnmounted, computed, watch } from 'vue'

export default {
  name: 'App',
  setup() {
    // API Configurations
    const menuOpen = ref(false)
    const apiUrl = ref(localStorage.getItem('clinic_api_url') || 'http://localhost:5000/api')
    
    // Mock Mode & Auth Session
    const isMockMode = ref(true)
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
      availableSchedules.value = []
      
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

    const selectSchedule = (sch) => {
      bookingForm.value.scheduleId = sch.id
      bookingForm.value.appointmentDate = sch.date
      bookingForm.value.timeSlot = sch.startTime
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

        const data = await res.json()
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
      localStorage.setItem('clinic_jwt_token', jwtToken.value)
      
      showAlert('Đã áp dụng cấu hình và lưu vào hệ thống!', 'success')
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
          // Gọi API xác thực thực tế tới Nhóm 6 qua Gateway
          const base = apiUrl.value.replace('/appointments-service', '')
          const url = `${base}/pharmacy-billing-service/auth/login`
          const res = await fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(loginForm.value)
          })
          if (!res.ok) throw new Error('Đăng nhập thất bại. Tài khoản hoặc mật khẩu không chính xác.')
          const data = await res.json()
          currentUser.value = {
            username: data.username || loginForm.value.username,
            role: data.role,
            token: data.token
          }
          localStorage.setItem('clinic_user_name', currentUser.value.username)
          localStorage.setItem('clinic_user_role', currentUser.value.role)
          localStorage.setItem('clinic_jwt_token', currentUser.value.token)
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
      showAlert('Đã đăng xuất khỏi hệ thống.', 'info')
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

    const viewPatientHistory = (phone) => {
      const history = medicalRecords.value.filter(mr => mr.patientPhone === phone)
      if (history.length === 0) {
        showAlert('Không tìm thấy lịch sử khám bệnh cũ của bệnh nhân này.', 'info')
      } else {
        let msg = `Lịch sử bệnh án (${history.length} lần khám trước):\n`
        history.forEach((h) => {
          msg += `[${h.date}] Triệu chứng: ${h.symptoms} -> Chẩn đoán: ${h.diagnosis}. Đơn thuốc: ${h.prescription.map(p => p.drugName).join(', ')}\n`
        })
        showAlert(msg, 'success')
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
          // Gọi API Nhóm 2 (Medical Record) qua Gateway
          const base = apiUrl.value.replace('/appointments-service', '')
          const urlRecord = `${base}/medical-records-service/medical-records`
          const resRecord = await fetch(urlRecord, {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${currentUser.value.token}`
            },
            body: JSON.stringify({
              patientName: activeConsultation.value.patientName,
              patientPhone: activeConsultation.value.patientPhone,
              symptoms: activeConsultation.value.symptoms,
              diagnosis: activeConsultation.value.diagnosis,
              notes: activeConsultation.value.notes,
              prescription: prescriptionData
            })
          })

          if (!resRecord.ok) throw new Error('Không thể lưu bệnh án và kê đơn lên máy chủ Nhóm 4')

          // Gọi API Nhóm 1 cập nhật hoàn thành khám
          await updateQueueStatus(activeConsultation.value.queueId, 4)
          showAlert('Khám bệnh và kê đơn thuốc liên thông thành công!', 'success')
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
          // Gọi API Nhóm 3 (Pharmacy & Billing) qua Gateway
          const base = apiUrl.value.replace('/appointments-service', '')
          const url = `${base}/pharmacy-billing-service/bills/${billId}/pay`
          const res = await fetch(url, {
            method: 'POST',
            headers: { 'Authorization': `Bearer ${currentUser.value.token}` }
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

    const addNewDrug = () => {
      const id = 'dr_' + Date.now()
      drugs.value.push({
        id,
        name: drugForm.value.name,
        activeIngredient: drugForm.value.activeIngredient,
        unit: drugForm.value.unit,
        price: drugForm.value.price,
        stock: drugForm.value.stock
      })
      showAlert(`Đã thêm thuốc mới ${drugForm.value.name} vào kho thành công!`, 'success')
      drugForm.value = { name: '', activeIngredient: '', unit: 'Viên', price: 0, stock: 0 }
    }

    const addNewDoctor = async () => {
      try {
        loading.value = true
        const newDoc = {
          id: 'doc_' + Guid.NewGuid(),
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
        }
        doctorForm.value = { fullName: '', specialty: '', qualifications: '', consultationFee: 100000 }
      } catch (err) {
        showAlert(err.message, 'error')
      } finally {
        loading.value = false
      }
    }

    // Watch tab changes to auto-load data for the selected tab
    watch(activeTab, (newTab) => {
      if (newTab === 'reception' && jwtToken.value) {
        fetchPendingAppointments()
      } else if (newTab === 'doctor' && doctorPortal.value.doctorId) {
        fetchDoctorActiveQueue()
      } else if (newTab === 'tv') {
        fetchTvQueue()
      }
    })

    // Auto update TV panel
    onMounted(() => {
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
      }, 5000)
    })

    onUnmounted(() => {
      if (tvInterval) clearInterval(tvInterval)
    })

    return {
      menuOpen,
      apiUrl,
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

      // Functions
      selectDoctor,
      selectSchedule,
      bookAppointment,
      confirmAppointment,
      cancelAppointment,
      searchAppointments,
      clearSearch,
      fetchPendingAppointments,
      fetchDoctorActiveQueue,
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
      addNewDoctor,
      
      // Helpers
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
.hover-card {
  transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
  border: 1px solid #334155 !important;
}

.hover-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 10px 15px -3px rgba(56, 189, 248, 0.1), 0 4px 6px -4px rgba(56, 189, 248, 0.1);
  border-color: #475569 !important;
}

/* Custom styles for background container */
.v-application {
  background: #0F172A !important;
}

.bg-background {
  background-color: #0F172A !important;
}

.bg-surface {
  background-color: #1E293B !important;
}

.bg-slate-900 {
  background-color: #0B0F19 !important;
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
</style>
