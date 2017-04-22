-- Возвращает пользователей текущих прогнозов
if exists(select 1 from sysobjects where name = N'CurrentForecastsUsersView' and xtype='P') drop proc CurrentForecastsUsersView
go
create proc CurrentForecastsUsersView (@Id1 int, @Id2 int, @Id3 int, @Id4 int) as	
begin	
	select top 3 u.Id as UserId, u.Nic, u.Avatar, fu.ForecastId as ForecastId, fu.ReportDate, fu.Value
	from Users u join ForecastsUsers fu on fu.UserId = u.Id 
	where fu.ForecastId = @Id1 and u.IsActive = 1 and u.IsBan = 0	
	union
	select top 3 u.Id as UserId, u.Nic, u.Avatar, fu.ForecastId as ForecastId, fu.ReportDate, fu.Value
	from Users u join ForecastsUsers fu on fu.UserId = u.Id 
	where fu.ForecastId = @Id2 and u.IsActive = 1 and u.IsBan = 0	
	union
	select top 3 u.Id as UserId, u.Nic, u.Avatar, fu.ForecastId as ForecastId, fu.ReportDate, fu.Value
	from Users u join ForecastsUsers fu on fu.UserId = u.Id 
	where fu.ForecastId = @Id3 and u.IsActive = 1 and u.IsBan = 0	
	union
	select top 3 u.Id as UserId, u.Nic, u.Avatar, fu.ForecastId as ForecastId, fu.ReportDate, fu.Value
	from Users u join ForecastsUsers fu on fu.UserId = u.Id 
	where fu.ForecastId = @Id4 and u.IsActive = 1 and u.IsBan = 0
	order by fu.ReportDate desc
end
go