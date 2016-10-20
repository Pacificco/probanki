-- Создание нового пользователя в базе данных
if exists(select 1 from sysobjects where name = N'CreateNewUser' and xtype='P') drop proc CreateNewUser
go
create proc CreateNewUser (
	@NicName nvarchar(20), 
	@Name nvarchar(20), 
	@Sex int,
	@Email nvarchar(50), 
	@Password nvarchar(32), 
	@Subscribed nvarchar(32),
	@Role nvarchar(255),
	@Token nvarchar(255)
	) as
begin
	
	SET NOCOUNT ON;

	begin try 
	
		insert Users(Nic, Name, Sex, Email, Password, IsSubscribed, IsActive, Rols, Token)
		values (@NicName, @Name, @Sex, @Email, @Password, @Subscribed, 0, @Role, @Token)
		select SCOPE_IDENTITY()
	end try 
	begin catch 
		select 0 as returnResult
	end catch
    
end
go
