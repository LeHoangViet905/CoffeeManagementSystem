MÔN PHÁT TRIỂN ỨNG DỤNG DESKTOP - TRƯỜNG ĐẠI HỌC KINH TẾ THÀNH PHỐ HỒ CHÍ MINH

Tên đề tài: Phát triển Hệ thống Quản lý quán café

Trong những năm gần đây, ngành F&B tại Việt Nam, đặc biệt là mô hình quán cà phê, tăng trưởng mạnh mẽ. Theo báo cáo của Statista (2023), quy mô thị trường cà phê Việt Nam dự kiến đạt trên 1,2 tỷ USD và tiếp tục tăng trưởng ổn định hằng năm. Bên cạnh đó, xu hướng số hóa trong kinh doanh nhỏ lẻ (SME) cũng ngày càng phổ biến: hơn 60% cửa hàng F&B được khảo sát tại các thành phố lớn đã chuyển sang sử dụng phần mềm quản lý bán hàng nhằm giảm sai sót, tăng tốc độ phục vụ và tối ưu chi phí vận hành.

MÔ TẢ VÀ MỤC TIÊU DỰ ÁN

MÔ TẢ: Hệ thống quản lý quán café có các chức năng cơ bản như sau: Quản lý đồ uống và loại đồ uống (thêm, xóa, sửa). Quản lý nhân viên và khách hàng (thêm, sửa, xóa thông tin nhân viên và khách hàng), phân quyền đăng nhập, phục vụ cho quy trình hoạt động từ Đặt món, thanh toán, xuất/in hóa đơn và cung cấp báo cáo kinh doanh cho quản lý.

MỤC TIÊU: Hệ thống hỗ trợ quy trình bán hàng từ khâu order, thanh toán, quản lý khách hàng - nhân viên đến thống kê doanh thu, giúp tối ưu hiệu suất và vận hành mượt mà giữa các khâu, giúp quản lý nắm được tình hình kinh doanh một cách nhanh chóng và hỗ trợ đưa ra quyết định. Xây dựng giao diện đơn giản, thân thiện và dễ sử dụng cho người dùng, đảm bảo mang đến những trải nghiệm tối ưu nhất.

TÁC GIẢ:

•	Lê Viết Bảo: https://github.com/BaoVietLe 

•	Trần Thế Hào: https://github.com/haotran31221026239-ui 

•	Trịnh Khánh Huyền: https://github.com/KHuyenT 

•	Hứa Đức Nghĩa: https://github.com/nghia2122005-cmyk 

•	Nguyễn Kiều Minh Trí: https://github.com/MinhTri1701

•	Lê Hoàng Việt: https://github.com/LeHoangViet905 

KIẾN THỨC ÁP DỤNG:

•	Sử dụng các control cơ bản và nâng cao trong Winform.

•	Phân tích thiết kế hệ thống cơ sở dữ liệu.

•	Phân tích thiết kế giao diện.

•	Trigger SQL.

•	Procedure, function SQL.

•	Chuyển data giữa các form.

•	Phân quyền người dùng trên ứng dụng.

•	DataBiding cơ bản và nâng cao.

•	Thêm, xóa, sửa dữ liệu từ trang quản trị.


TRIỂN KHAI

Nền tảng xây dựng:
Nền tảng hệ thống phần mềm Công nghệ .NET (hoặc .NET Framework) được dùng để xây dựng các ứng dụng Desktop truyền thống là Windows Forms, nó cung cấp các vùng tên (namespace) khá mạnh và đầy đủ phục vụ cho việc tạo giao diện đồ họa là System.Windows.Forms và System.Drawing. Trong phần này chúng ta sẽ tập trung chủ yếu vào việc dùng ngôn ngữ C# để lập trình với Windows Forms trên môi trường Visual Studio.

Bộ công cụ Windows Forms cũng được thiết kế để hỗ trợ mô hình phát triển nhanh (RAD). Với Windows Forms, ta có thể kéo thả các điều khiển (Controls) từ thanh công cụ (Toolbox) lên Form thiết kế cũng như có thể viết mã trực tiếp trong tập tin .cs. Ứng dụng Desktop sẽ được triển khai và chạy trực tiếp trên máy tính của người dùng (Client-side), tận dụng tối đa tài nguyên phần cứng của máy. .NET còn hỗ trợ ta bộ công cụ để tạo ra các ứng dụng tuân theo mô hình 3 lớp (3-tier) hoặc các mẫu thiết kế như MVC/MVP, giúp ta có thể quản lý được ứng dụng dễ dàng hơn và nhờ thế nâng cao hiệu suất phát triển cũng như bảo trì phần mềm.

Windows Forms là bộ công cụ cho phép thực thi các ứng dụng cửa sổ (Window-based applications) với giao diện người dùng phong phú, có khả năng truy cập sâu vào hệ thống tập tin và thiết bị ngoại vi của máy tính.

Với Windows Forms, ta tạo ra các Form giao diện và dùng mã C# chạy trên máy Client để xử lý các sự kiện người dùng (như click chuột, nhập liệu) rồi phản hồi lại ngay lập tức trên giao diện. Ứng dụng sau khi biên dịch sẽ trở thành các tập tin thực thi (.exe) chạy trên nền tảng hệ điều hành Windows. Ngoài dùng C#, ta cũng có thể dùng ngôn ngữ VB.NET để tạo ra các ứng dụng Desktop tương tự.

Windows Forms chia cấu trúc mã nguồn thành hai phần: phần thiết kế giao diện (Designer) và phần mã logic phía sau (Code-behind). Phần giao diện (các thuộc tính của nút bấm, ô nhập liệu...) được lưu trữ trong tập tin có phần mở rộng là .Designer.cs (thường do Visual Studio tự sinh mã), còn mã xử lý nghiệp vụ được lưu trữ trong tập tin có phần mở rộng là .cs.

SQLite là một hệ quản trị cơ sở dữ liệu quan hệ nhỏ gọn, khép kín và không cần máy chủ (serverless). Khác với các hệ thống cơ sở dữ liệu client-server lớn, SQLite là một thư viện phần mềm được nhúng trực tiếp vào ứng dụng, có chức năng lưu trữ và truy xuất dữ liệu nằm ngay trong một tập tin duy nhất trên đĩa cứng của máy tính đang chạy ứng dụng đó.

SQLite là hệ quản trị cơ sở dữ liệu mã nguồn mở phổ biến nhất thế giới cho các ứng dụng cục bộ và di động. Vì SQLite là cơ sở dữ liệu dạng nhúng, nó có tốc độ truy xuất cực nhanh, ổn định, không yêu cầu cấu hình cài đặt phức tạp và có tính khả chuyển cao (chỉ cần copy tập tin .db là có thể di chuyển dữ liệu). Với đặc tính "Zero-configuration" (Không cần cấu hình), SQLite rất thích hợp cho các ứng dụng Desktop vừa và nhỏ, các phần mềm quản lý kho, bán hàng offline viết bằng C#. SQLite miễn phí hoàn toàn và thư viện của nó hỗ trợ đa nền tảng từ Windows, Linux, Mac OS cho đến Android, iOS.

SQLite là một trong những ví dụ điển hình về Hệ Quản trị Cơ sở dữ liệu quan hệ tuân thủ chuẩn ACID và sử dụng Ngôn ngữ truy vấn có cấu trúc (SQL).

SQLite được sử dụng rộng rãi để bổ trợ cho C# (thông qua thư viện System.Data.SQLite), Python, Java và nhiều ngôn ngữ khác, nó làm nơi lưu trữ dữ liệu an toàn, tin cậy ngay trên thiết bị của người dùng mà không cần phụ thuộc vào kết nối Internet liên tục.

Chuẩn lập trình:
Đưa ra các quy ước khi coding với ngôn ngữ lập trình C#, với các quy tắc này giúp tiết kiệm thời gian rất lớn trong tiến trình phát triển phần mềm và cả trong quá trình bảo trì sản phẩm. Giúp sinh viên quen với làm việc theo nhóm. Tài liệu này chủ yếu hướng dẫn sinh viên với ngôn ngữ lập trình C#, nhưng có rất nhiều quy tắc được sử dụng trong nhiều ngôn ngữ lập trình khác tích hợp trong bộ công cụ Visual Studio .NET.

TÀI LIỆU THIẾT KẾ

ĐÓNG GÓP Ý KIẾN

LỜI CẢM ƠN

Nhóm chúng em cũng xin được gửi lời cảm ơn đến thầy Nguyễn Mạnh Tuấn vì sự cố gắng, tâm huyết đem đến cho chúng em những bài học quý giá, tinh thần học tập hiệu quả nghiêm túc, những bài tập củng cố và mở rộng hiểu biết vô cùng bổ ích. Chúng em tin rằng những kiến thức này chính là hành trang quý báu trên bước đường công việc sau này.




