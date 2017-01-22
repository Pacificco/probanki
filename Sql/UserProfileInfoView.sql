-- Возвращает информацию о прогнозах пользователя из базы данных по идентификатору
if exists(select 1 from sysobjects where name = N'UserProfileInfoView' and xtype='P') drop proc UserProfileInfoView
go
create proc UserProfileInfoView (
	@Id int
	) as
begin	
	select u.*, dbo.GetUserForecastTryCountForThisMonth(@Id) as forecastTryCount 
	from UsersForecastInfo u 
	where u.UserId = @Id
end