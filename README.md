# ☕ HỆ THỐNG QUẢN LÝ QUÁN CAFÉ
**Môn học: Phát triển ứng dụng Desktop - Trường Đại học Kinh tế TP. Hồ Chí Minh (UEH)**

[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](#)
[![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](#)
[![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)](#)
[![Windows Forms](https://img.shields.io/badge/Windows_Forms-0078D6?style=for-the-badge&logo=windows&logoColor=white)](#)

---

## 📖 Bối cảnh dự án

Trong những năm gần đây, ngành F&B tại Việt Nam, đặc biệt là mô hình quán cà phê, tăng trưởng mạnh mẽ. Theo báo cáo của Statista (2023), quy mô thị trường cà phê Việt Nam dự kiến đạt trên 1,2 tỷ USD và tiếp tục tăng trưởng ổn định hằng năm. 

Bên cạnh đó, xu hướng số hóa trong kinh doanh nhỏ lẻ (SME) ngày càng phổ biến: hơn 60% cửa hàng F&B được khảo sát tại các thành phố lớn đã chuyển sang sử dụng phần mềm quản lý bán hàng nhằm:
* Giảm thiểu sai sót trong khâu vận hành.
* Tăng tốc độ phục vụ khách hàng.
* Tối ưu hóa chi phí quản lý.

Nắm bắt xu hướng này, nhóm chúng em đã tiến hành xây dựng và phát triển **Hệ thống Quản lý quán café** trên nền tảng Desktop.

---

## 🎯 Mô tả và Mục tiêu

### Mô tả chức năng chính
Hệ thống được thiết kế để phục vụ toàn diện cho quy trình hoạt động của một quán café, bao gồm:
* **Quản lý danh mục:** Thêm, xóa, sửa thông tin đồ uống và phân loại đồ uống.
* **Quản lý nhân sự & Khách hàng:** Quản lý thông tin (thêm, sửa, xóa) của nhân viên và hồ sơ khách hàng.
* **Nghiệp vụ bán hàng:** Hỗ trợ quy trình Đặt món (Order), Thanh toán, và Xuất/In hóa đơn.
* **Hệ thống quản trị:** Phân quyền đăng nhập chặt chẽ và cung cấp các báo cáo thống kê kinh doanh trực quan cho cấp quản lý.

### Mục tiêu dự án
* Hỗ trợ và tự động hóa quy trình bán hàng, từ khâu order đến thống kê doanh thu.
* Tối ưu hiệu suất làm việc, giúp quá trình vận hành giữa các khâu diễn ra mượt mà.
* Giúp ban quản lý nắm bắt tình hình kinh doanh nhanh chóng, tạo cơ sở cho các quyết định chiến lược.
* Xây dựng giao diện phần mềm đơn giản, thân thiện, mang lại trải nghiệm UI/UX tối ưu nhất cho người dùng cuối.

---

## 💻 Kiến trúc và Công nghệ triển khai

Dự án được triển khai trên nền tảng **.NET Framework**, sử dụng ngôn ngữ **C#** kết hợp với cơ sở dữ liệu **SQLite**.

### 1. Nền tảng Windows Forms
* Sử dụng `System.Windows.Forms` và `System.Drawing` để xây dựng giao diện đồ họa (GUI) truyền thống.
* Ứng dụng mô hình phát triển nhanh **RAD** (Rapid Application Development) thông qua thao tác kéo thả Controls trên Toolbox của Visual Studio.
* Cấu trúc mã nguồn được phân tách rõ ràng giữa phần thiết kế giao diện (`.Designer.cs`) và phần mã logic nghiệp vụ (`Code-behind .cs`).
* Khả năng mở rộng theo mô hình 3 lớp (3-tier) hoặc thiết kế kiến trúc MVC/MVP.

### 2. Cơ sở dữ liệu SQLite
* Sử dụng hệ quản trị CSDL quan hệ **SQLite** (thông qua thư viện `System.Data.SQLite`) - không cần máy chủ (serverless) và cấu hình "Zero-configuration".
* Lưu trữ và truy xuất dữ liệu cục bộ cực nhanh, an toàn và tin cậy ngay trên thiết bị mà không phụ thuộc vào kết nối Internet.
* Tuân thủ chuẩn ACID và sử dụng ngôn ngữ truy vấn cấu trúc SQL quen thuộc.

### 3. Chuẩn lập trình (Coding Conventions)
* Áp dụng nghiêm ngặt các quy ước coding của ngôn ngữ C# trong quá trình làm việc nhóm.
* Giúp tối ưu hóa thời gian phát triển, thống nhất phong cách viết code và hỗ trợ tốt cho tiến trình bảo trì sản phẩm sau này.

---

## 🛠 Kiến thức áp dụng

Trong suốt quá trình thực hiện dự án, nhóm đã nghiên cứu và áp dụng các kiến thức chuyên môn:
* Sử dụng các control cơ bản và nâng cao trong WinForms.
* Phân tích và thiết kế hệ thống cơ sở dữ liệu.
* Phân tích và thiết kế giao diện người dùng (UI/UX).
* Lập trình CSDL: Trigger, Procedure, Function SQL.
* Kỹ thuật truyền và xử lý dữ liệu giữa các Form.
* Triển khai hệ thống phân quyền người dùng (Role-based access control).
* Kỹ thuật DataBinding cơ bản và nâng cao.
* Xử lý luồng nghiệp vụ thao tác dữ liệu (CRUD) từ trang quản trị.

---

## 👥 Đội ngũ tác giả

Dự án được thực hiện bởi nhóm sinh viên Trường Đại học Kinh tế TP. Hồ Chí Minh:

| STT | Họ và Tên | Khám phá GitHub |
| :---: | :--- | :--- |
| 1 | **Lê Viết Bảo** | [@BaoVietLe](https://github.com/BaoVietLe) |
| 2 | **Trần Thế Hào** | [@haotran31221026239-ui](https://github.com/haotran31221026239-ui) |
| 3 | **Trịnh Khánh Huyền** | [@KHuyenT](https://github.com/KHuyenT) |
| 4 | **Hứa Đức Nghĩa** | [@nghia2122005-cmyk](https://github.com/nghia2122005-cmyk) |
| 5 | **Nguyễn Kiều Minh Trí** | [@MinhTri1701](https://github.com/MinhTri1701) |
| 6 | **Lê Hoàng Việt** | [@LeHoangViet905](https://github.com/LeHoangViet905) |

---

## 📂 Tài liệu thiết kế
*(Cập nhật link tài liệu phân tích thiết kế, diagram, mockup tại đây)*

## 💡 Đóng góp ý kiến
*(Cập nhật link form đóng góp ý kiến hoặc hướng dẫn tạo Pull Request/Issues tại đây)*

---

## 🙏 Lời cảm ơn

Nhóm chúng em xin được gửi lời cảm ơn chân thành đến **Thầy Nguyễn Mạnh Tuấn** vì sự tâm huyết và những bài giảng giá trị trong suốt học phần. Những định hướng, tinh thần học tập nghiêm túc cùng các bài tập củng cố của thầy đã đem đến cho chúng em những hiểu biết vô cùng bổ ích. 

Chúng em tin rằng những kiến thức thực tiễn từ môn học này chính là hành trang quý báu trên bước đường phát triển sự nghiệp của chúng em sau này. Xin chân thành cảm ơn Thầy!
