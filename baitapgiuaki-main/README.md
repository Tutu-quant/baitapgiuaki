# RoomHub BCS240032 - Quản lý phòng trọ

Ứng dụng ASP.NET Core MVC dùng Entity Framework Core + SQL Server cho đề giữa kỳ “Quản lý phòng trọ”. Dự án giữ mã sinh viên `BCS240032` trong tên model, DbContext, controller, repository và tên bảng.

## Chạy nhanh bằng một cú nhấp

1. Mở thư mục dự án `baitapgiuaki-main\baitapgiuaki-main`.
2. Nhấp đúp file `start-roomhub.bat`.
3. Đợi cửa sổ console chạy `dotnet restore` và `dotnet run`.
4. Trình duyệt sẽ tự mở tại `http://localhost:5036`.
5. Khi muốn dừng server, bấm `Ctrl+C` trong cửa sổ console.

Lần chạy đầu tiên ứng dụng sẽ tự tạo database `MID_BCS240032`, tạo bảng và thêm dữ liệu mẫu.

## Yêu cầu máy

- .NET 10 SDK.
- SQL Server đang chạy trên máy local, mặc định là `localhost` / service `MSSQLSERVER`.
- Windows Authentication có quyền tạo database.

Nếu máy dùng SQL Server instance khác, sửa `appsettings.json`:

```json
"MID_BCS240032": "Server=localhost;Database=MID_BCS240032;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;Connect Timeout=5;MultipleActiveResultSets=True;"
```

Ví dụ nếu dùng instance `SQLEXPRESS`, đổi `Server=localhost` thành `Server=localhost\\SQLEXPRESS`.

## Chạy thủ công

```powershell
dotnet restore .\Lesson3-CNLTWeb.csproj
dotnet run --project .\Lesson3-CNLTWeb.csproj --no-launch-profile --urls http://localhost:5036
```

Sau đó mở:

```text
http://localhost:5036
```

## Database và bảng đúng theo đề

Database:

```text
MID_BCS240032
```

Bảng:

```text
RoomTypes_BCS240032
Rooms_BCS240032
RoomImages_BCS240032
```

Dữ liệu mẫu tự tạo gồm:

- 3 loại phòng.
- 5 phòng trọ.
- Mỗi phòng thuộc một loại phòng.
- Có ảnh mẫu và ảnh đại diện để demo chức năng quản lý ảnh.

## Checklist demo theo đề

- Mở SQL Server và chứng minh database `MID_BCS240032`.
- Chứng minh 3 bảng có hậu tố `BCS240032`.
- Mở danh sách phòng, thấy tên phòng và tên loại phòng.
- Tìm kiếm theo tên phòng.
- Lọc theo loại phòng.
- Lọc trạng thái còn phòng.
- Nhập giá tối đa.
- Sắp xếp theo giá tăng, giá giảm, diện tích giảm.
- Kiểm tra cột `Giá/m² = Price / Area`; cột này chỉ tính khi hiển thị.
- Tạo hoặc sửa phòng, loại phòng lấy từ database.
- Vào chi tiết phòng, thêm ảnh bằng đường dẫn.
- Đổi ảnh đại diện; mỗi phòng chỉ có một ảnh đại diện.
- Thử xóa loại phòng đang có phòng sử dụng để thấy thông báo lỗi thân thiện.
- Thử truy cập ID không tồn tại để thấy xử lý lỗi.

## Ghi chú kỹ thuật

- App dùng `DatabaseSeederBCS240032` để tự tạo dữ liệu demo khi database chưa có phòng.
- Lọc và sắp xếp được xử lý bằng `IQueryable`/EF Core trong repository, không lấy toàn bộ dữ liệu rồi lọc bằng vòng lặp.
- Ràng buộc tên phòng không trùng trong cùng loại phòng được cấu hình bằng unique index.
- Ràng buộc mỗi phòng chỉ có tối đa một ảnh đại diện được cấu hình bằng filtered unique index và logic repository.
- Tính năng quản lý sách cũ vẫn được giữ trong giao diện như một module phụ; khi demo điểm chính, ưu tiên các màn hình phòng trọ `BCS240032`.
