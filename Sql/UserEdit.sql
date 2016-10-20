-- Обновляет EmailConfirmed по Id
if exists(select 1 from sysobjects where name = N'UserEmailConfirmedEdit' and xtype='P') drop proc UserEmailConfirmedEdit
go
create proc UserEmailConfirmedEdit (
	@Id int,
	@Confirmed int
	) as
begin	
	update Users set EmailConfirmed = @Confirmed, IsActive = @Confirmed where Id = @Id
end
go


-- Обновляет Token по Id
if exists(select 1 from sysobjects where name = N'UserTokenEdit' and xtype='P') drop proc UserTokenEdit
go
create proc UserTokenEdit (
	@Id int,
	@Token nvarchar(255)
	) as
begin	
	update Users set Token = @Token where Id = @Id
end
go


-- Изменяет пароль по Id
if exists(select 1 from sysobjects where name = N'UserPasswordEdit' and xtype='P') drop proc UserPasswordEdit
go
create proc UserPasswordEdit (
	@Id int,
	@Password nvarchar(255)
	) as
begin	
	update Users set Token = '', Password = @Password where Id = @Id
end
go