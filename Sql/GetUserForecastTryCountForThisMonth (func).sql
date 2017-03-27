if exists(select 1 from sysobjects where name = N'GetUserForecastTryCountForThisMonth' and xtype='FN') drop function GetUserForecastTryCountForThisMonth
go
create function GetUserForecastTryCountForThisMonth (
	@UserId int,
	@tariff tinyint
) returns int		-- Количестов прогнозов, в которых пользователь может принять участие в текущем месяце
begin

	declare @result int
	select @result=0


	if not exists(select 1 from UsersForecastInfo where UserId = @UserId and IsConfirmed = 1 and ForecastEndDate > GETDATE())
		select @result=0
		
	else begin
	
		declare @countByTariff int
		set @countByTariff =
		case @tariff
			when 1 then 4
			when 2 then 3
			when 3 then 2
			when 4 then 1 
			else 0
		end * 2
	
		declare @curDate datetime = GETDATE()
	
		-- Определяем период для определения количества участий в прогнозах в текущем месяце
		declare @m nvarchar(2) = CAST(MONTH(@curDate) as varchar(2))
		if LEN(@m) = 1 begin set @m = '0' + @m end
		
		declare @dateFrom datetime = cast(year(@curDate) as varchar(4)) + @m + '01 00:00:00'
		declare @dayCount int = day(dateadd(dd, -day(dateadd(mm, 1, @curDate)), dateadd(mm, 1, @curDate)))
		declare @d nvarchar(2) = CAST(@dayCount as varchar(2))
		declare @dateTo datetime = cast(year(@curDate) as varchar(4)) + @m + @d + ' 23:59:59'
		
		declare @existsTryCount int =
		(
			select COUNT(1) 
			from ForecastsUsers fu join Forecasts f on f.Id = fu.ForecastId
			where fu.UserId = @UserId 
				and f.ForecastDate >= @dateFrom
				and f.ForecastDate <= @dateTo
		)
		
		select @result = (@countByTariff - @existsTryCount)
	end
			
	return @result

end
go