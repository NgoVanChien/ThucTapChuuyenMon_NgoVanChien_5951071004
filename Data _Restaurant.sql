CREATE DATABASE Restaurant
GO

USE  Restaurant
GO

create table TableFood
(
id  int identity primary key,
name nvarchar(100) Not Null default N'Bàn chưa có tên',
status nvarchar(100) Not Null default N'Trống'          -- 1 : Có người  || -- 0: Trống
)
GO

create table Account
(
UserName nvarchar(100) primary key, 
DisplayName nvarchar(100) NOT NULL default N'ChienN',
PassWord nvarchar(1000)NOT NULL default 0,
Type int NOT NULL default 0                               --1: Admin ||    -- 0 : Nhân viên
)
GO

create table FoodCategory
(
id  int identity primary key,
name nvarchar(100) Not Null default N'Chưa đặt tên'
)
GO

create table Food
(
id  int identity primary key,
name nvarchar(100) Not Null default N'Chưa đặt tên',
idCategory int Not Null,
price float Not Null default 0

foreign key (idCategory) references dbo.FoodCategory(id)
)
GO

create table Bill
(
id int identity primary key,
DateCheckIn date Not Null default getdate(),
DateCheckOut date,
idTable int Not Null,
status int Not Null default 0                                -- 1 : Đã thanh toán || 0 : Chưa thanh toán

foreign key(idTable) references dbo.TableFood(id)
)
GO

create table BillInfo
(
id int identity primary key,
idBill int Not Null,
idFood int Not Null,
count int Not Null default 0

foreign key (idBill) references dbo.Bill(id),
foreign key (idFood) references dbo.Food(id)
)
GO
-- Thêm dữ liệu Account
 insert into Account (UserName,DisplayName,PassWord,Type)
 values
 (N'Chien Ngo',N'Ngo Van Chien',N'ad',1)

 insert into Account (UserName,DisplayName,PassWord,Type)
 values
 (N'San',N'Khoai Thi San',N'st',0)

  GO

  create proc USP_GetAccountByUserName
  @userName nvarchar (100)
  AS
  BEGIN
  select *from Account where UserName=@userName
  END

   GO
   
Insert into Account (UserName,DisplayName,PassWord,Type) values(N'chien',N'nvchien',N'123',0)

  EXEC USP_GetAccountByUserName @userName=N'Bap'
  
  delete from Account where PassWord=N'123' 
 
  SELECT * FROM Account WHERE UserName = N''OR 1=1--
  GO

  CREATE PROC USP_Login
  @userName nvarchar(100), @passWord nvarchar(100)
  AS
  BEGIN
  SELECT * FROM Account WHERE UserName=@userName and PassWord=@passWord
  END
  GO
  --Thêm bàn ăn
  DECLARE @i int = 1
  while @i<=20
  Begin
  Insert TableFood(name) values (N'Bàn' + CAST(@i as nvarchar(100)))
  set @i=@i+1
  END
 GO 


create proc USP_GetTableList
AS SELECT * FROM TableFood
GO

UPdate TableFood set status=N'Có người' where id=5

EXEC USP_GetTableList

Go
-- Thêm Danh mục 
Insert FoodCategory (name) values(N'Điểm tâm sáng')
Insert FoodCategory (name) values(N'Hải sản')
Insert FoodCategory (name) values(N'Lẩu')
Insert FoodCategory (name) values(N'Điểm tâm trưa')
Insert FoodCategory (name) values(N'Nước giải khát')
Insert FoodCategory (name) values(N'Trà')
Insert FoodCategory (name) values(N'Chiên xào')
Insert FoodCategory (name) values(N'Điểm tâm tối')

-- Thêm món 
Insert Food(name,idCategory,price) values(N'Bánh mì bò kho',1,30000)
Insert Food(name,idCategory,price) values(N'Ghẹ',2,200000)
Insert Food(name,idCategory,price) values(N'Lẩu cá ',3,350000)
Insert Food(name,idCategory,price) values(N'Lẩu Thái',3,150000)
Insert Food(name,idCategory,price) values(N'Bún xào',4,25000)
Insert Food(name,idCategory,price) values(N'Banh canh',4,25000)
Insert Food(name,idCategory,price) values(N'Sting',5,15000)
Insert Food(name,idCategory,price) values(N'C2',5,12000)
Insert Food(name,idCategory,price) values(N'Trà gừng',6,22000)
Insert Food(name,idCategory,price) values(N'Trà Thái',6,20000)
Insert Food(name,idCategory,price) values(N'Khoai tây chiên',7,15000)
Insert Food(name,idCategory,price) values(N'Nuôi xào',7,25000)
Insert Food(name,idCategory,price) values(N'Hủ tếu',8,25000)
Insert Food(name,idCategory,price) values(N'Phở',8,55000)

-- thêm hóa đơn
Insert Bill(DateCheckIn,DateCheckOut,idTable,status) values(GETDATE(),null,3,0)
Insert Bill(DateCheckIn,DateCheckOut,idTable,status) values(GETDATE(),null,4,0)
Insert Bill(DateCheckIn,DateCheckOut,idTable,status) values(GETDATE(),GETDATE(),5,1)
Insert Bill(DateCheckIn,DateCheckOut,idTable,status) values(GETDATE(),null,1,0)
Insert Bill(DateCheckIn,DateCheckOut,idTable,status) values(GETDATE(),null,2,0)
Insert Bill(DateCheckIn,DateCheckOut,idTable,status) values(GETDATE(),GETDATE(),6,1)

--thêm thông tin hóa đơn
Insert BillInfo(idBill,idFood,count) values(1,1,2)
Insert BillInfo(idBill,idFood,count) values(1,3,4)
Insert BillInfo(idBill,idFood,count) values(2,5,1)
Insert BillInfo(idBill,idFood,count) values(2,1,2)
Insert BillInfo(idBill,idFood,count) values(3,6,2)
Insert BillInfo(idBill,idFood,count) values(3,5,2)
Insert BillInfo(idBill,idFood,count) values(6,1,2)
Insert BillInfo(idBill,idFood,count) values(5,6,2)
Insert BillInfo(idBill,idFood,count) values(4,5,2)

select* from Food
select* from FoodCategory
select* from Bill
select* from BillInfo
select* from TableFood

SELECT * FROM Bill WHERE idTable=3 AND status = 0
SELECT * FROM BillInfo WHERE idBill=5 
--SELECT * FROM Food WHERE idCategory =  + id


SELECT f.name,bi.count,f.price, f.price*bi.count AS totalPrice FROM BillInfo AS bi, Bill AS b, Food AS f 
		WHERE bi.idBill= b.id AND bi.idFood= f.id AND b.status=1 AND b.idTable=5

		DELETE FROM Food WHERE id>6
		go
		SELECT MAX(id) FROM Bill
		go

		CREATE PROC USP_InsertBill
		@idTable INT
		AS
		BEGIN
		INSERT Bill(DateCheckIn,DateCheckOut,idTable,status,discount) values(GETDATE(),NULL,@idTable,0,0)
		END 
		GO

		CREATE PROC USP_InsertBillInfo

		@idBill INT, @idFood INT, @count INT

		AS

		BEGIN

		DECLARE @isExitsBillInfo INT;

		DECLARE @foodCount INT = 1

		SELECT @isExitsBillInfo = id, @foodCount = b.count 
		FROM BillInfo AS b 
		WHERE idBill=@idBill AND idFood=@idFood

		IF(@isExitsBillInfo > 0)

		BEGIN
		DECLARE @newCount INT = @foodCount + @count

		IF(@newCount > 0)

		UPDATE BillInfo SET count = @foodCount + @count WHERE idFood=@idFood
		ELSE
		DELETE BillInfo WHERE idBill = @idBill AND idFood = @idFood

		

		END

		ELSE

		BEGIN

		 Insert BillInfo(idBill,idFood,count) values(@idBill,@idFood,@count)

		END
		END 
		GO
		
		
	    
		CREATE TRIGGER UTG_UpdateBillInfo
		ON BillInfo FOR INSERT, UPDATE
		AS
		BEGIN
		    DECLARE @idBill INT

		    SELECT @idBill = idBill FROM Inserted

		    DECLARE @idTable INT

		   SELECT @idTable = idTable FROM Bill WHERE id=@idBill AND status = 0
		   UPDATE TableFood SET status = N'Có người' WHERE id =@idTable
		END 
		GO

		create TRIGGER UTG_UpdateBill ON Bill FOR UPDATE
		AS
		BEGIN

		DECLARE @idBill INT

		SELECT @idBill = id  FROM Inserted 

		DECLARE @idTable INT

		SELECT @idTable = idTable FROM Bill WHERE id = @idBill

		DECLARE @count int = 0

		SELECT @count = COUNT(*) FROM Bill WHERE idTable = @idTable AND status = 0

		IF (@count = 0)

		UPDATE TableFood SET status = N'Trống' WHERE id =@idTable
		

		END
		GO

		-- Thêm cột Discount và totalPrice vào bảng Bill

		ALTER TABLE Bill ADD Discount INT
		
		UPDATE Bill SET discount = 0
		ALTER TABLE Bill ADD totalPrice FLOAT

		GO

		CREATE TRIGGER UTG_UpdateBillInfo ON BillInfo FOR INSERT,UPDATE
		AS
		BEGIN

		DECLARE @idBill INT

		SELECT @idBill = idBill FROM Inserted

		DECLARE @idTable INT

		SELECT @idTable = idTable FROM Bill WHERE id=@idBill AND status = 0
		DECLARE @count INT
		SELECT @count = COUNT (*) FROM BillInfo WHERE idBill= @idBill

		IF(@count > 0)
		UPDATE TableFood SET status = N'Có người' WHERE id =@idTable
		
		ELSE
		UPDATE TableFood SET status = N'Trống' WHERE id =@idTable
END		
GO


	

EXEC USP_SwitchTable @idTable1 = 11,
@idtable2 =12
go

CREATE PROC USP_SwitchTable
		@idTable1 Int, @idTable2 INT
		
		AS
		BEGIN
		DECLARE @idFirstBill INT
		DECLARE @idSecondBill INT
		DECLARE @isFirstTableEmty INT = 1
		DECLARE @isSecondTableEmty INT = 1

		SELECT @idSecondBill = id FROM Bill WHERE idTable = @idTable2 AND status = 0
			SELECT @idFirstBill = id FROM Bill WHERE idTable = @idTable1 AND status = 0

			PRINT @idFirstBill
			PRINT @idSecondBill
			PRINT '--------'

			IF(@idFirstBill IS NULL)
			BEGIN


			INSERT INTO Bill(DateCheckIn,DateCheckOut,idTable,status) VALUES (GETDATE(),NULL,@idTable1,0)

			SELECT @idFirstBill = MAX(id) FROM Bill WHERE idTable = @idTable1 AND status = 0

			

			 END

			 SELECT @isFirstTableEmty = COUNT (*) FROM BillInfo WHERE idBill =@idFirstBill

			PRINT @idFirstBill
			PRINT @idSecondBill
			PRINT '--------'


			 IF(@idSecondBill IS NULL)
			BEGIN
			PRINT '000000002'
			INSERT INTO Bill(DateCheckIn,DateCheckOut,idTable,status) VALUES (GETDATE(),NULL,@idTable2,0)

			SELECT @idSecondBill = MAX(id) FROM Bill WHERE idTable = @idTable2 AND status = 0

			

			 END

			 SELECT @isSecondTableEmty = COUNT (*) FROM BillInfo WHERE idBill =@idSecondBill

		SELECT id INTO IDBillInfoTable FROM BillInfo WHERE idBill=@idSecondBill
		UPDATE BillInfo SET idBill= @idSecondBill WHERE idBill=@idFirstBill
		UPDATE BillInfo SET idBill= @idFirstBill WHERE id IN (SELECT * FROM IDBillInfoTable)

		DROP Table IDBillInfoTable

		IF(@isFirstTableEmty = 0)
		UPDATE TableFood SET status = N'Trống' WHERE id = @idTable2

		IF(@isSecondTableEmty = 0)
		UPDATE TableFood SET status = N'Trống' WHERE id = @idTable1


		END
		GO
		
		EXEC USP_SwitchTable @idTable1= 5, @idTable2 = 6


select t.name, b.totalPrice , datecheckin, datecheckout, discount
from bill as b, tablefood as t
where datecheckin >='20210601' and datecheckout <= '20210607' and b.status = 1
and t.id = b.idtable
go

		CREATE PROC USP_GetListBillByDate
		@checkIN date, @checkout date
		AS
		BEGIN
		SELECT   t.name AS [Tên bàn], b.totalPrice AS [Tổng tiền ( nghìn vnd )], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá ( % )]
		FROM Bill AS b, TableFood AS t
		WHERE DateCheckIn >= @checkIN AND DateCheckOut <= @checkout AND b.status = 1 AND t.id = b.idTable 
		END 
		GO



		CREATE PROC USP_UpdateAccount
		@userName NVARCHAR(100), @displayName NVARCHAR(100) , @password NVARCHAR(100) , @newPassword NVARCHAR(100)
		AS
		BEGIN
		DECLARE @isRightPass INT =0
		
		SELECT @isRightPass = COUNT (*) FROM Account WHERE UserName = @userName AND PassWord = @password

		IF(@isRightPass = 1)
		BEGIN

		IF(@newPassword = NULL OR @newPassword = '')
		BEGIN
		UPDATE Account SET DisplayName = @displayName WHERE UserName= @userName
		END
		ELSE
		UPDATE Account SET DisplayName =@displayName, PassWord = @newPassword WHERE UserName = @userName
		END
        END
		GO


	



		
		CREATE TRIGGER UTG_DeleteBillInfo ON BillInfo FOR DELETE
		AS
		BEGIN
		DECLARE @idBillInfo INT
		DECLARE @idBill INT

		SELECT @idBillInfo = id, @idBill = Deleted.idBill FROM DELETED

		DECLARE @idTable INT
		SELECT idTable = @idTable FROM Bill WHERE id = @idBill

		DECLARE @count INT = 0
		
		SELECT @count = COUNT (*) FROM BillInfo AS bi, Bill AS b WHERE b.id= bi.idBill AND b.id = @idBill AND status = 0

		IF(@count = 0)
		UPDATE TableFood SET status = N'Trống' WHERE id = @idTable
		END
		GO
		
		
CREATE FUNCTION [dbo].[GetUnsignString](@strInput NVARCHAR(4000)) 
RETURNS NVARCHAR(4000)
AS
BEGIN     
    IF @strInput IS NULL RETURN @strInput
    IF @strInput = '' RETURN @strInput
    DECLARE @RT NVARCHAR(4000)
    DECLARE @SIGN_CHARS NCHAR(136)
    DECLARE @UNSIGN_CHARS NCHAR (136)

    SET @SIGN_CHARS       = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵýĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ'+NCHAR(272)+ NCHAR(208)
    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyyAADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'

    DECLARE @COUNTER int
    DECLARE @COUNTER1 int
    SET @COUNTER = 1
 
    WHILE (@COUNTER <=LEN(@strInput))
    BEGIN   
      SET @COUNTER1 = 1
      --Tim trong chuoi mau
       WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1)
       BEGIN
     IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) )
     BEGIN           
          IF @COUNTER=1
              SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1)                   
          ELSE
              SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER)    
              BREAK         
               END
             SET @COUNTER1 = @COUNTER1 +1
       END
      --Tim tiep
       SET @COUNTER = @COUNTER +1
    END
    RETURN @strInput
END

select sum(totalPrice) from Bill

select * from food where dbo.GetUnsignString(name) like N'%' + dbo.GetUnsignString(N'bun') + '%'
go

        ALTER PROC USP_GetListBillByDateAndPage
		@checkIN date, @checkout date, @page INT
		AS
		BEGIN
		DECLARE @pageRows INT = 10
		DECLARE @selectRows INT = @pageRows 
		DECLARE @exceptRows INT = (@page-1) * @pageRows

		;WITH BillShow AS (SELECT b.id, t.name AS [Tên bàn], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra],b.totalPrice AS [Tổng tiền ( nghìn vnd )] ,discount AS [Giảm giá (%)]
		FROM Bill AS b, TableFood AS t
		WHERE DateCheckIn >= @checkIN AND DateCheckOut <= @checkout AND b.status = 1 AND t.id = b.idTable)

		SELECT TOP (@selectRows) * FROM BillShow WHERE id NOT IN (SELECT TOP (@exceptRows) id FROM BillShow)
		
		END 
		GO


		
	  
		CREATE PROC USP_GetNumBillByDate
		@checkIN date, @checkout date
		AS
		BEGIN
		SELECT  COUNT(*)
		FROM Bill AS b, TableFood AS t
		WHERE DateCheckIn >= @checkIN AND DateCheckOut <= @checkout AND b.status = 1 AND t.id = b.idTable 
		END 
		GO
		
		
		select * from tablefood

		delete from tablefood
		


		