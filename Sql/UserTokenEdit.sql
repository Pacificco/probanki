-- Изменяет token пользователя в базе данных
if exists(select 1 from sysobjects where name = N'UserTokenEdit' and xtype='P') drop proc UserTokenEdit
go
create proc UserTokenEdit (
	@Id int,
	@Token nvarchar(255)
	) as
begin	
	update Users set Token = @Token where Id = @Id
end