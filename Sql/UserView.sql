-- Возвращает пользователя из базы данных по Email
if exists(select 1 from sysobjects where name = N'ActiveUserByEmailView' and xtype='P') drop proc ActiveUserByEmailView
go
create proc ActiveUserByEmailView (
	@Email nvarchar(50)
	) as
begin	
	select * from Users where Email = @Email and IsActive = 1 and IsBan = 0 and EmailConfirmed = 1;  
end


-- Возвращает пользователя из базы данных по идентификатору 
if exists(select 1 from sysobjects where name = N'ActiveUserView' and xtype='P') drop proc ActiveUserView
go
create proc ActiveUserView (
	@Id int
	) as
begin	
	select * from Users where Id = @Id and IsActive = 1 and IsBan = 0 and EmailConfirmed = 1;  
end


-- Возвращает пользователя из базы данных по идентификатору
if exists(select 1 from sysobjects where name = N'UserView' and xtype='P') drop proc UserView
go
create proc UserView (
	@Id int
	) as
begin	
	select * from Users where Id = @Id    
end