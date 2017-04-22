-- Cписок прогнозов, в которых пользователь принял или может принять участие в текущем месяце
if exists(select 1 from sysobjects where name = N'UserForecastsForMonthView' and xtype='P') drop proc UserForecastsForMonthView
go
create proc UserForecastsForMonthView (
	@UserId int
	) as
begin
		-- Определяем период для определения прогнозов в текущем месяце
		declare @m nvarchar(2) = CAST(MONTH(GETDATE()) as varchar(2))
		if LEN(@m) = 1 begin set @m = '0' + @m end
		declare @date datetime = getdate()
		declare @dayCount varchar(2)
		set @dayCount = cast(day(dateadd(dd, -day(dateadd(mm, 1, @date)), dateadd(mm, 1, @date))) as varchar(2))
		declare @curYear varchar(4) 
		set @curYear = cast(year(getdate()) as varchar(4))
		declare @dateFrom varchar(25) = @curYear + '-' + @m + '-01 00:00:00'
		declare @dateTo varchar(25) = @curYear + '-' + @m + '-' + @dayCount + ' 23:59:59'
		
		--select Convert(datetime, @dateFrom, 120)
		--select Convert(datetime, @dateTo, 120)
		
		-- Прогнозы за текущий месяц
		declare @ForecastsForMonth table (	Id				int			not null,
											ForecastDate	datetime	not null,											
											SubjectId		tinyint		not null,
											IsClosed		bit			not null
											primary key (Id)
										)
		insert @ForecastsForMonth
		select f.Id, f.ForecastDate, f.SubjectId, f.IsClosed 
		from Forecasts f			
		where f.ForecastDate >= Convert(datetime, @dateFrom, 120) 
			and f.ForecastDate <= Convert(datetime, @dateTo, 120)
			
		if @UserId is null begin
						
			select null, null, null, null, f.* 
			from @ForecastsForMonth f			
				--left join ForecastsUsers fu on f.Id = fu.ForecastId --and fu.UserId = 25
				--left join Users u on u.Id = fu.UserId			
			--where fu.UserId = @UserId --or fu.UserId is null
			order by f.ForecastDate
		
		end else begin
		
			select u.Id, u.Nic, fu.Value, fu.ReportDate, f.* 
			from @ForecastsForMonth f			
				left join ForecastsUsers fu on f.Id = fu.ForecastId and fu.UserId = @UserId
				left join Users u on u.Id = fu.UserId
			--where fu.UserId = @UserId --or fu.UserId is null
			order by f.ForecastDate
		
		end
c
end
go