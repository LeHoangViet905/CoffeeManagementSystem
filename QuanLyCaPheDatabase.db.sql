BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Chitietdonhang" (
	"Madonhang"	TEXT,
	"Madouong"	TEXT,
	"Soluong"	INTEGER,
	"Dongia"	REAL,
	"Thanhtien"	REAL,
	"Ghichu"	TEXT,
	PRIMARY KEY("Madonhang","Madouong"),
	FOREIGN KEY("Madonhang") REFERENCES "Donhang"("Madonhang") ON DELETE CASCADE,
	FOREIGN KEY("Madouong") REFERENCES "Douong"("Madouong")
);
CREATE TABLE IF NOT EXISTS "Donhang" (
	"Madonhang"	TEXT,
	"Manhanvien"	TEXT,
	"Makhachhang"	TEXT,
	"Thoigiandat"	TEXT,
	"Trangthaidon"	TEXT,
	"Tongtien"	REAL,
	PRIMARY KEY("Madonhang"),
	FOREIGN KEY("Makhachhang") REFERENCES "Khachhang"("Makhachhang"),
	FOREIGN KEY("Manhanvien") REFERENCES "Nhanvien"("Manhanvien")
);
CREATE TABLE IF NOT EXISTS "Douong" (
	"Madouong"	TEXT,
	"Tendouong"	TEXT,
	"Maloai"	TEXT,
	"Mota"	TEXT,
	"Hinhanh"	TEXT,
	PRIMARY KEY("Madouong"),
	FOREIGN KEY("Maloai") REFERENCES "Loaidouong"("Maloai") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "Giadouong" (
	"Magia"	TEXT,
	"Madouong"	TEXT,
	"Giaban"	REAL,
	"Thoigianapdung"	TEXT,
	PRIMARY KEY("Magia"),
	FOREIGN KEY("Madouong") REFERENCES "Douong"("Madouong") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "Khachhang" (
	"Makhachhang"	TEXT,
	"Hoten"	TEXT,
	"Sodienthoai"	TEXT UNIQUE,
	"Email"	TEXT,
	"Ngaydangky"	TEXT,
	"Diemtichluy"	INTEGER DEFAULT 0,
	PRIMARY KEY("Makhachhang")
);
CREATE TABLE IF NOT EXISTS "Loaidouong" (
	"Maloai"	TEXT,
	"Tenloai"	TEXT,
	PRIMARY KEY("Maloai")
);
CREATE TABLE IF NOT EXISTS "Nhanvien" (
	"Manhanvien"	TEXT,
	"Hoten"	TEXT,
	"Ngaysinh"	TEXT,
	"Gioitinh"	TEXT,
	"Diachi"	TEXT,
	"Sodienthoai"	TEXT,
	"Email"	TEXT,
	"Ngayvaolam"	TEXT,
	PRIMARY KEY("Manhanvien")
);
CREATE TABLE IF NOT EXISTS "Taikhoan" (
	"Mataikhoan"	TEXT,
	"Tendangnhap"	TEXT UNIQUE,
	"Matkhau"	TEXT,
	"Vaitro"	TEXT,
	"Manhanvien"	TEXT UNIQUE,
	PRIMARY KEY("Mataikhoan"),
	FOREIGN KEY("Manhanvien") REFERENCES "Nhanvien"("Manhanvien") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "Thanhtoan" (
	"Mathanhtoan"	TEXT,
	"Madonhang"	TEXT UNIQUE,
	"Thoigianthanhtoan"	TEXT,
	"Hinhthucthanhtoan"	TEXT,
	"Sotienthanhtoan"	REAL,
	"Manhanvienthu"	TEXT,
	"Ghichu"	TEXT,
	PRIMARY KEY("Mathanhtoan"),
	FOREIGN KEY("Madonhang") REFERENCES "Donhang"("Madonhang") ON DELETE CASCADE,
	FOREIGN KEY("Manhanvienthu") REFERENCES "Nhanvien"("Manhanvien")
);
CREATE TABLE IF NOT EXISTS NhomTuyChon (
    MaNhom INTEGER PRIMARY KEY AUTOINCREMENT, 
    TenNhom TEXT NOT NULL,
    ChonNhieu INTEGER DEFAULT 0 
);


CREATE TABLE IF NOT EXISTS ChiTietTuyChon (
    MaChiTiet INTEGER PRIMARY KEY AUTOINCREMENT,
    MaNhom INTEGER,
    TenChiTiet TEXT,
    GiaThem REAL DEFAULT 0, 
    FOREIGN KEY(MaNhom) REFERENCES NhomTuyChon(MaNhom) ON DELETE CASCADE
);


CREATE TABLE CauHinhMon (
    Madouong TEXT,
    MaNhom INTEGER,
    PRIMARY KEY (Madouong, MaNhom), 
    FOREIGN KEY(Madouong) REFERENCES Douong(Madouong) ON DELETE CASCADE,
    FOREIGN KEY(MaNhom) REFERENCES NhomTuyChon(MaNhom) ON DELETE CASCADE
);
INSERT INTO "Chitietdonhang" VALUES ('DH001','DU001',1,32000.0,32000.0,NULL);
INSERT INTO "Chitietdonhang" VALUES ('DH001','DU002',1,45000.0,45000.0,NULL);
INSERT INTO "Chitietdonhang" VALUES ('DH002','DU003',1,40000.0,40000.0,'Ít đường');
INSERT INTO "Chitietdonhang" VALUES ('DH003','DU002',2,45000.0,90000.0,NULL);
INSERT INTO "Chitietdonhang" VALUES ('DH004','DU004',1,35000.0,35000.0,NULL);
INSERT INTO "Chitietdonhang" VALUES ('DH005','DU005',1,55000.0,55000.0,NULL);
INSERT INTO "Chitietdonhang" VALUES ('DH006','DU004',2,35000.0,70000.0,'Thêm đá');
INSERT INTO "Chitietdonhang" VALUES ('DH007','DU007',1,52000.0,52000.0,NULL);
INSERT INTO "Chitietdonhang" VALUES ('DH008','DU001',1,32000.0,32000.0,NULL);
INSERT INTO "Chitietdonhang" VALUES ('DH008','DU005',1,55000.0,55000.0,NULL);
INSERT INTO "Chitietdonhang" VALUES ('DH008','DU009',1,28000.0,28000.0,NULL);
INSERT INTO "Chitietdonhang" VALUES ('DH009','DU006',1,42000.0,42000.0,NULL);
INSERT INTO "Chitietdonhang" VALUES ('DH009','DU008',1,38000.0,38000.0,NULL);
INSERT INTO "Chitietdonhang" VALUES ('DH010','DU010',1,30000.0,30000.0,'Không kem');
INSERT INTO "Donhang" VALUES ('DH001','NV001','KH001','2024-05-27 10:30:00.000','Đã thanh toán',77000.0);
INSERT INTO "Donhang" VALUES ('DH002','NV002',NULL,'2024-05-27 14:00:00.000','Đã đặt',40000.0);
INSERT INTO "Donhang" VALUES ('DH003','NV003','KH003','2024-05-28 09:15:00.000','Đã thanh toán',90000.0);
INSERT INTO "Donhang" VALUES ('DH004','NV004','KH002','2024-05-28 11:45:00.000','Đã đặt',35000.0);
INSERT INTO "Donhang" VALUES ('DH005','NV005','KH005','2024-05-28 15:00:00.000','Đã thanh toán',55000.0);
INSERT INTO "Donhang" VALUES ('DH006','NV001','KH004','2024-05-29 08:00:00.000','Đã thanh toán',70000.0);
INSERT INTO "Donhang" VALUES ('DH007','NV002',NULL,'2024-05-29 11:00:00.000','Đã đặt',52000.0);
INSERT INTO "Donhang" VALUES ('DH008','NV006','KH006','2024-05-29 16:30:00.000','Đã thanh toán',115000.0);
INSERT INTO "Donhang" VALUES ('DH009','NV007','KH007','2024-05-30 09:45:00.000','Đã thanh toán',80000.0);
INSERT INTO "Donhang" VALUES ('DH010','NV008',NULL,'2024-05-30 13:10:00.000','Đã đặt',30000.0);
INSERT INTO "Douong" VALUES ('DU001','Espresso Đậm Đà','LD001','Cà phê nguyên chất, hương vị mạnh mẽ.','espresso.jpg');
INSERT INTO "Douong" VALUES ('DU002','Latte Hạnh Nhân','LD001','Cà phê sữa béo ngậy với sữa hạnh nhân.','latte_hanh_nhan.jpg');
INSERT INTO "Douong" VALUES ('DU003','Trà Đào Cam Sả','LD002','Trà thanh mát với đào, cam tươi, và sả thơm.','tra_dao_cam_sa.jpg');
INSERT INTO "Douong" VALUES ('DU004','Nước Cam Ép Tươi','LD003','100% cam tươi, không đường.','nuoc_cam_ep.jpg');
INSERT INTO "Douong" VALUES ('DU005','Bánh Tiramisu Ý','LD005','Bánh tiramisu chuẩn vị, mềm mịn.','tiramisu.jpg');
INSERT INTO "Douong" VALUES ('DU006','Sinh Tố Bơ Xanh','LD006','Sinh tố bơ tươi mát, giàu dinh dưỡng.','sinh_to_bo.jpg');
INSERT INTO "Douong" VALUES ('DU007','Matcha Đá Xay','LD004','Matcha đậm đà xay cùng đá và kem.','matcha_da_xay.jpg');
INSERT INTO "Douong" VALUES ('DU008','Sữa Chua Dâu Tây','LD007','Sữa chua sánh mịn với dâu tây tươi.','sua_chua_dau.jpg');
INSERT INTO "Douong" VALUES ('DU009','Americano Nóng','LD001','Cà phê đen pha loãng với nước nóng.','americano.jpg');
INSERT INTO "Douong" VALUES ('DU010','Bánh Mì Phô Mai','LD010','Bánh mì nướng với lớp phô mai béo ngậy.','banh_mi_pho_mai.jpg');
INSERT INTO "Giadouong" VALUES ('GIA001','DU001',30000.0,'2023-01-01 08:00:00.000');
INSERT INTO "Giadouong" VALUES ('GIA002','DU002',45000.0,'2023-01-01 08:00:00.000');
INSERT INTO "Giadouong" VALUES ('GIA003','DU003',40000.0,'2023-01-01 08:00:00.000');
INSERT INTO "Giadouong" VALUES ('GIA004','DU004',35000.0,'2023-01-01 08:00:00.000');
INSERT INTO "Giadouong" VALUES ('GIA005','DU005',55000.0,'2023-01-01 08:00:00.000');
INSERT INTO "Giadouong" VALUES ('GIA006','DU006',42000.0,'2023-02-10 10:00:00.000');
INSERT INTO "Giadouong" VALUES ('GIA007','DU007',52000.0,'2023-03-15 11:30:00.000');
INSERT INTO "Giadouong" VALUES ('GIA008','DU008',38000.0,'2023-04-20 13:00:00.000');
INSERT INTO "Giadouong" VALUES ('GIA009','DU009',28000.0,'2023-05-25 09:00:00.000');
INSERT INTO "Giadouong" VALUES ('GIA010','DU010',30000.0,'2023-06-01 08:00:00.000');
INSERT INTO "Giadouong" VALUES ('GIA011','DU001',32000.0,'2024-05-01 09:00:00.000');
INSERT INTO "Khachhang" VALUES ('KH001','Lê Thanh Ngân','0771112223','ngan.le@mail.com','2023-02-01',150);
INSERT INTO "Khachhang" VALUES ('KH002','Đỗ Mạnh Cường','0773334445','cuong.do@mail.com','2023-03-10',80);
INSERT INTO "Khachhang" VALUES ('KH003','Bùi Ngọc Lan','0775556667','lan.bui@mail.com','2023-04-05',220);
INSERT INTO "Khachhang" VALUES ('KH004','Trịnh Bá Quyền','0777778889','quyen.trinh@mail.com','2023-05-12',50);
INSERT INTO "Khachhang" VALUES ('KH005','Võ Thị Mai','0779990001','mai.vo@mail.com','2023-06-20',120);
INSERT INTO "Khachhang" VALUES ('KH006','Phan Văn Minh','0701020304','minh.phan@mail.com','2023-07-01',300);
INSERT INTO "Khachhang" VALUES ('KH007','Đặng Thị Thu','0705060708','thu.dang@mail.com','2023-08-15',90);
INSERT INTO "Khachhang" VALUES ('KH008','Hoàng Duy Anh','0709101112','duyanh.hoang@mail.com','2023-09-22',180);
INSERT INTO "Khachhang" VALUES ('KH009','Nguyễn Thị Hoa','0701314151','hoa.nguyen@mail.com','2023-10-01',250);
INSERT INTO "Khachhang" VALUES ('KH010','Trần Văn Tùng','0701617181','tung.tran@mail.com','2023-11-11',70);
INSERT INTO "Loaidouong" VALUES ('LD001','Cà phê truyền thống');
INSERT INTO "Loaidouong" VALUES ('LD002','Trà hiện đại');
INSERT INTO "Loaidouong" VALUES ('LD003','Nước ép trái cây');
INSERT INTO "Loaidouong" VALUES ('LD004','Đồ uống đá xay');
INSERT INTO "Loaidouong" VALUES ('LD005','Bánh ngọt & Tráng miệng');
INSERT INTO "Loaidouong" VALUES ('LD006','Sinh tố');
INSERT INTO "Loaidouong" VALUES ('LD007','Sữa chua & Smoothies');
INSERT INTO "Loaidouong" VALUES ('LD008','Đồ uống đặc biệt');
INSERT INTO "Loaidouong" VALUES ('LD009','Nước giải khát');
INSERT INTO "Loaidouong" VALUES ('LD010','Thực phẩm nhẹ');
INSERT INTO "Nhanvien" VALUES ('NV001','Nguyễn Văn A','1990-05-15','Nam','123 Đường ABC, Q1, HCM','0901234567','vana@cafe.com','2022-01-10');
INSERT INTO "Nhanvien" VALUES ('NV002','Trần Thị B','1995-11-20','Nữ','456 Đường XYZ, Q3, HCM','0912345678','thib@cafe.com','2022-03-01');
INSERT INTO "Nhanvien" VALUES ('NV003','Lê Văn C','1992-07-25','Nam','789 Phố KLN, Q5, HCM','0923456789','vanc@cafe.com','2022-06-15');
INSERT INTO "Nhanvien" VALUES ('NV004','Phạm Thị D','1998-02-10','Nữ','101 Hẻm MNO, Q7, HCM','0934567890','thid@cafe.com','2023-01-20');
INSERT INTO "Nhanvien" VALUES ('NV005','Hoàng Văn E','1993-09-01','Nam','202 Ngõ PQR, Q Bình Thạnh, HCM','0945678901','vane@cafe.com','2023-05-05');
INSERT INTO "Nhanvien" VALUES ('NV006','Vũ Thị F','1991-03-03','Nữ','303 Hẻm STU, Q Gò Vấp, HCM','0967890123','thif@cafe.com','2023-07-12');
INSERT INTO "Nhanvien" VALUES ('NV007','Đinh Văn G','1994-12-12','Nam','404 Đường VWX, Q Tân Bình, HCM','0978901234','vang@cafe.com','2024-01-01');
INSERT INTO "Nhanvien" VALUES ('NV008','Bùi Thị H','1996-08-08','Nữ','505 Đường YZA, Q Phú Nhuận, HCM','0989012345','thih@cafe.com','2024-02-14');
INSERT INTO "Nhanvien" VALUES ('NV009','Dương Văn I','1997-01-20','Nam','606 Hẻm BCD, Q Thủ Đức, HCM','0990123456','vani@cafe.com','2024-03-20');
INSERT INTO "Nhanvien" VALUES ('NV010','Ngô Thị K','1999-04-30','Nữ','707 Đường EFG, Q Tân Phú, HCM','0900112233','thik@cafe.com','2024-04-10');
INSERT INTO "Taikhoan" VALUES ('TK001','admin','admin123','Admin','NV001');
INSERT INTO "Taikhoan" VALUES ('TK002','nhanvien1','nv123','NhanVien','NV002');
INSERT INTO "Taikhoan" VALUES ('TK003','nhanvien2','nv123','NhanVien','NV003');
INSERT INTO "Taikhoan" VALUES ('TK004','nhanvien3','nv123','NhanVien','NV004');
INSERT INTO "Taikhoan" VALUES ('TK005','nhanvien4','nv123','NhanVien','NV005');
INSERT INTO "Taikhoan" VALUES ('TK006','nhanvien5','nv123','NhanVien','NV006');
INSERT INTO "Taikhoan" VALUES ('TK007','nhanvien6','nv123','NhanVien','NV007');
INSERT INTO "Taikhoan" VALUES ('TK008','nhanvien7','nv123','NhanVien','NV008');
INSERT INTO "Taikhoan" VALUES ('TK009','nhanvien8','nv123','NhanVien','NV009');
INSERT INTO "Taikhoan" VALUES ('TK010','nhanvien9','nv123','NhanVien','NV010');
INSERT INTO "Thanhtoan" VALUES ('TT001','DH001','2024-05-27 10:32:00.000','Tiền mặt',77000.0,'NV001',NULL);
INSERT INTO "Thanhtoan" VALUES ('TT002','DH003','2024-05-28 09:18:00.000','Chuyển khoản',90000.0,'NV003','Đã chuyển khoản ngân hàng');
INSERT INTO "Thanhtoan" VALUES ('TT003','DH005','2024-05-28 15:02:00.000','Thẻ',55000.0,'NV005','Thanh toán qua thẻ Visa');
INSERT INTO "Thanhtoan" VALUES ('TT004','DH006','2024-05-29 08:05:00.000','Tiền mặt',70000.0,'NV001',NULL);
INSERT INTO "Thanhtoan" VALUES ('TT005','DH008','2024-05-29 16:35:00.000','Chuyển khoản',115000.0,'NV006','Khách trả dư 15k');
INSERT INTO "Thanhtoan" VALUES ('TT006','DH009','2024-05-30 09:48:00.000','Thẻ',80000.0,'NV007',NULL);
INSERT INTO "Thanhtoan" VALUES ('TT007','DH002','2024-05-27 14:05:00.000','Tiền mặt',40000.0,'NV002','Thanh toán ngay');
INSERT INTO "Thanhtoan" VALUES ('TT008','DH004','2024-05-28 11:47:00.000','Tiền mặt',35000.0,'NV004',NULL);
INSERT INTO "Thanhtoan" VALUES ('TT009','DH007','2024-05-29 11:03:00.000','Chuyển khoản',52000.0,'NV002',NULL);
INSERT INTO "Thanhtoan" VALUES ('TT010','DH010','2024-05-30 13:12:00.000','Tiền mặt',30000.0,'NV008','Khách quen');
COMMIT;
