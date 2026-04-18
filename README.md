# ☕ HỆ THỐNG QUẢN LÝ QUÁN CAFÉ
**Môn học: Phát triển ứng dụng Desktop - Trường Đại học Kinh tế TP. Hồ Chí Minh (UEH)**

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](#)
[![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](#)
[![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)](#)
[![Windows Forms](https://img.shields.io/badge/Windows_Forms-0078D6?style=for-the-badge&logo=windows&logoColor=white)](#)

---

## 📑 Mục lục
1. [Bối cảnh dự án](#1-bối-cảnh-dự-án)
2. [Mô tả và Mục tiêu](#2-mô-tả-và-mục-tiêu)
3. [Kiến trúc và Công nghệ triển khai](#3-kiến-trúc-và-công-nghệ-triển-khai)
4. [Hướng dẫn cài đặt](#4-hướng-dẫn-cài-đặt)
5. [Hướng dẫn sử dụng](#5-hướng-dẫn-sử-dụng)
6. [Đội ngũ tác giả](#6-đội-ngũ-tác-giả)

---

## 1. 📖 Bối cảnh dự án

Trong những năm gần đây, ngành F&B tại Việt Nam, đặc biệt là mô hình quán cà phê, tăng trưởng mạnh mẽ. Theo báo cáo của Statista (2023), quy mô thị trường cà phê Việt Nam dự kiến đạt trên 1,2 tỷ USD và tiếp tục tăng trưởng ổn định hằng năm. 

Bên cạnh đó, xu hướng số hóa trong kinh doanh nhỏ lẻ (SME) ngày càng phổ biến: hơn 60% cửa hàng F&B được khảo sát tại các thành phố lớn đã chuyển sang sử dụng phần mềm quản lý bán hàng nhằm giảm thiểu sai sót, tăng tốc độ phục vụ và tối ưu hóa chi phí quản lý.

---

## 2. 🎯 Mô tả và Mục tiêu

### Mô tả chức năng chính
* **Quản lý danh mục:** Thêm, xóa, sửa thông tin đồ uống và phân loại đồ uống.
* **Quản lý nhân sự & Khách hàng:** Quản lý thông tin của nhân viên và hồ sơ khách hàng.
* **Nghiệp vụ bán hàng:** Hỗ trợ quy trình Đặt món (Order), Thanh toán, và Xuất/In hóa đơn.
* **Hệ thống quản trị:** Phân quyền đăng nhập chặt chẽ và cung cấp các báo cáo thống kê kinh doanh trực quan.

### Mục tiêu dự án
* Tự động hóa quy trình bán hàng, từ khâu order đến thống kê doanh thu.
* Giúp ban quản lý nắm bắt tình hình kinh doanh nhanh chóng, tạo cơ sở cho các quyết định chiến lược.
* Xây dựng giao diện phần mềm đơn giản, thân thiện, mang lại trải nghiệm UI/UX tối ưu.

---

## 3. 💻 Kiến trúc và Công nghệ triển khai

Dự án được triển khai trên nền tảng **.NET Framework**, sử dụng ngôn ngữ **C#** kết hợp với cơ sở dữ liệu **SQLite**.

* **Nền tảng Windows Forms:** Sử dụng `System.Windows.Forms` hỗ trợ mô hình phát triển nhanh (RAD).
* **Cơ sở dữ liệu SQLite:** Hệ quản trị CSDL quan hệ dạng nhúng, tốc độ truy xuất nhanh, không cần cấu hình server phức tạp ("Zero-configuration").
* **Kiến thức áp dụng:** Sử dụng Control nâng cao, DataBinding, phân quyền người dùng, Trigger/Procedure/Function SQL.

---

## 4. 🚀 Hướng dẫn cài đặt

Để chạy dự án trên máy tính cá nhân, bạn cần cài đặt môi trường phát triển .NET.

### Yêu cầu hệ thống
* Hệ điều hành: Windows 10/11.
* Công cụ: **Visual Studio 2019** hoặc **Visual Studio 2022** (Đã cài đặt workload *.NET desktop development*).

### Các bước cài đặt
1. **Tải mã nguồn:**
   Clone kho lưu trữ này về máy hoặc tải file `.zip` và giải nén.
   ```bash
   git clone [https://github.com/BaoVietLe/](https://github.com/BaoVietLe/)[ten-repo-cua-ban].git
