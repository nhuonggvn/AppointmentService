import re

def get_block(lines, start_keyword, end_keyword, start_idx=0):
    start = -1
    for i in range(start_idx, len(lines)):
        if start_keyword in lines[i]:
            start = i
            break
    if start == -1: return -1, -1

    # To handle nested elements, we count <v-window-item and </v-window-item>
    open_count = 0
    end = -1
    for i in range(start, len(lines)):
        open_count += lines[i].count('<v-window-item')
        open_count -= lines[i].count('</v-window-item>')
        if open_count <= 0 and '</v-window-item>' in lines[i]:
            end = i
            break
    return start, end

with open(r'f:\F3-FullStack\AppointmentService\ClinicFrontend\src\views\demo.vue', 'r', encoding='utf-8') as f:
    demo_lines = f.readlines()

with open(r'f:\F3-FullStack\AppointmentService\ClinicFrontend\src\views\DashboardPage.vue', 'r', encoding='utf-8') as f:
    dashboard_lines = f.readlines()

# Extract from demo
p_start, p_end = get_block(demo_lines, 'value="pharmacy"')
b_start, b_end = get_block(demo_lines, 'value="billing"')
f_start, f_end = get_block(demo_lines, 'value="financial-reports"')

print(f"Demo: Pharmacy ({p_start}-{p_end}), Billing ({b_start}-{b_end}), Financial ({f_start}-{f_end})")

# Extract from dashboard
dp_start, dp_end = get_block(dashboard_lines, 'value="pharmacy"')
db_start, db_end = get_block(dashboard_lines, 'value="billing"')
df_start, df_end = get_block(dashboard_lines, 'value="financial-reports"')

print(f"Dashboard: Pharmacy ({dp_start}-{dp_end}), Billing ({db_start}-{db_end}), Financial ({df_start}-{df_end})")

# Replacing logic: we need to replace backwards to avoid messing up the line numbers
new_lines = dashboard_lines[:]

if df_start != -1 and df_end != -1 and f_start != -1 and f_end != -1:
    new_lines[df_start:df_end+1] = demo_lines[f_start:f_end+1]

# Re-calculate indices after first replace, but it's easier to do it if we write an AST or just rely on replacing backwards.
# Wait, if we replace financial first (which is at the bottom), it won't affect pharmacy and billing which are above it.
# Let's find db_start again in the new lines just to be safe.
db_start, db_end = get_block(new_lines, 'value="billing"')
if db_start != -1 and db_end != -1 and b_start != -1 and b_end != -1:
    new_lines[db_start:db_end+1] = demo_lines[b_start:b_end+1]

dp_start, dp_end = get_block(new_lines, 'value="pharmacy"')
if dp_start != -1 and dp_end != -1 and p_start != -1 and p_end != -1:
    new_lines[dp_start:dp_end+1] = demo_lines[p_start:p_end+1]

with open(r'f:\F3-FullStack\AppointmentService\ClinicFrontend\src\views\DashboardPage.vue', 'w', encoding='utf-8') as f:
    f.writelines(new_lines)

print("Replaced template successfully.")
