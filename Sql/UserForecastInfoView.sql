-- Возвращает информацию о прогнозах пользователя из базы данных по идентификатору
if exists(select 1 from sysobjects where name = N'UserForecastInfoView' and xtype='P') drop proc UserForecastInfoView
go
create proc UserForecastInfoView (
	@Id int
	) as
begin	
	select * from UsersForecastInfo where UserId = @Id    
end