-- Возвращает пользователей, которые приняли участие в прогнозе
if exists(select 1 from sysobjects where name = N'ForecastUsers' and xtype='P') drop proc ForecastUsers
go
create proc ForecastUsers (@Id int) as	
begin	
	if @Id is null return
	
	select fu.Value, fu.ReportDate, u.Nic, u.Avatar, u.Rang
	from ForecastsUsers fu join Users u on u.Id = fu.UserId
	where fu.ForecastId = @Id
end
go