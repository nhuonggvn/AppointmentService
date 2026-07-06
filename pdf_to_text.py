import os
import sys

def extract_pdf_text(pdf_path, txt_path):
    # Thử import pypdf, nếu chưa có thì tự động cài đặt
    try:
        import pypdf
    except ImportError:
        print("pypdf chưa được cài đặt. Đang tiến hành cài đặt...")
        import subprocess
        subprocess.check_call([sys.executable, "-m", "pip", "install", "pypdf"])
        import pypdf
    
    print(f"Đang đọc tập tin PDF từ: {pdf_path}")
    reader = pypdf.PdfReader(pdf_path)
    text = ""
    for idx, page in enumerate(reader.pages):
        text += f"--- TRANG {idx + 1} ---\n"
        page_text = page.extract_text()
        if page_text:
            text += page_text + "\n"
            
    with open(txt_path, "w", encoding="utf-8") as f:
        f.write(text)
    print(f"Đã trích xuất văn bản thành công ra tập tin: {txt_path}")

if __name__ == "__main__":
    pdf_file = "/Users/mac/Desktop/AppointmentService/Hướng dẫn BTL Fullstack.pdf"
    txt_file = "/Users/mac/Desktop/AppointmentService/pdf_content.txt"
    extract_pdf_text(pdf_file, txt_file)
