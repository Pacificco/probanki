-- Изменяет пароль пользователя в базе данных
if exists(select 1 from sysobjects where name = N'UserPasswordEdit' and xtype='P') drop proc UserPasswordEdit
go
create proc UserPasswordEdit(
	@Id int,
	@Password nvarchar(255)
	) as
begin	
	update Users set Token = '', Password = @Password where Id = @Id
end