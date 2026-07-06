const fs = require('fs');

function getBlock(lines, startKeyword) {
    let start = -1;
    for (let i = 0; i < lines.length; i++) {
        if (lines[i].includes(startKeyword)) {
            start = i;
            break;
        }
    }
    if (start === -1) return [-1, -1];

    let openCount = 0;
    let end = -1;
    for (let i = start; i < lines.length; i++) {
        const line = lines[i];
        openCount += (line.match(/<v-window-item/g) || []).length;
        openCount -= (line.match(/<\/v-window-item>/g) || []).length;
        if (openCount <= 0 && line.includes('</v-window-item>')) {
            end = i;
            break;
        }
    }
    return [start, end];
}

const demoContent = fs.readFileSync('f:\\F3-FullStack\\AppointmentService\\ClinicFrontend\\src\\views\\demo.vue', 'utf8');
const demoLines = demoContent.split('\n');

const dashboardContent = fs.readFileSync('f:\\F3-FullStack\\AppointmentService\\ClinicFrontend\\src\\views\\DashboardPage.vue', 'utf8');
let dashboardLines = dashboardContent.split('\n');

const keywords = ['<v-window-item value="pharmacy">', '<v-window-item value="billing">', '<v-window-item value="financial-reports">'];

const demoBlocks = {};
for (const kw of keywords) {
    const [start, end] = getBlock(demoLines, kw);
    demoBlocks[kw] = { start, end };
    console.log(`Demo: ${kw} => ${start}-${end}`);
}

function replaceBlock(lines, kw, demoBlock) {
    const [dStart, dEnd] = getBlock(lines, kw);
    console.log(`Dashboard: ${kw} => ${dStart}-${dEnd}`);
    if (dStart !== -1 && dEnd !== -1 && demoBlock.start !== -1 && demoBlock.end !== -1) {
        const before = lines.slice(0, dStart);
        const middle = demoLines.slice(demoBlock.start, demoBlock.end + 1);
        const after = lines.slice(dEnd + 1);
        return [...before, ...middle, ...after];
    }
    return lines;
}

dashboardLines = replaceBlock(dashboardLines, '<v-window-item value="financial-reports">', demoBlocks['<v-window-item value="financial-reports">']);
dashboardLines = replaceBlock(dashboardLines, '<v-window-item value="billing">', demoBlocks['<v-window-item value="billing">']);
dashboardLines = replaceBlock(dashboardLines, '<v-window-item value="pharmacy">', demoBlocks['<v-window-item value="pharmacy">']);

fs.writeFileSync('f:\\F3-FullStack\\AppointmentService\\ClinicFrontend\\src\\views\\DashboardPage.vue', dashboardLines.join('\n'), 'utf8');
console.log('Replaced template blocks successfully.');
