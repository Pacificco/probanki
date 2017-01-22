-- Список прогнозов, в которых пользователь принял или может принять участие в текущем месяце
if exists(select 1 from sysobjects where name = N'UserForecastsForMonthView' and xtype='P') drop proc UserForecastsForMonthView
go
create proc UserForecastsForMonthView (
	@UserId int
	) as
begin
	if not exists(select 1 from UsersForecastInfo where UserId = @UserId)
		return 1
	else begin
		-- Определяем период для определения прогнозов в текущем месяце
		declare @m nvarchar(2) = CAST(MONTH(GETDATE()) as varchar(2))
		if LEN(@m) = 1 begin set @m = '0' + @m end
				
		select u.Id, u.Nic, fu.Value, fu.ReportDate, f.* 
		from Forecasts f
			left join ForecastsUsers fu on f.Id = fu.ForecastId
			left join Users u on u.Id = fu.UserId
		where f.ForecastDate >= '2017-' + @m + '-01 00:00:00' 
			and f.ForecastDate <= GETDATE()
			and u.Id = @UserId
	end
			
	return 0

end
go