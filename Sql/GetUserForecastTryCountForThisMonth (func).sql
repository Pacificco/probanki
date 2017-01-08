if exists(select 1 from sysobjects where name = N'GetUserForecastTryCountForThisMonth' and xtype='FN') drop function GetUserForecastTryCountForThisMonth
go
create function GetUserForecastTryCountForThisMonth (@UserId int) returns int		-- Количестов прогнозов, в которых пользователь может принять участие в текущем месяце
begin

	declare @result int
	select @result=0

	if not exists(select 1 from UsersForecastInfo where UserId = @UserId)
		select @result=0
	else begin
		-- Определяем период для определения количества участий в прогнозах в текущем месяце
		declare @m nvarchar(2) = CAST(MONTH(GETDATE()) as varchar(2))
		if LEN(@m) = 1 begin set @m = '0' + @m end
		
		set @result = (
			select COUNT(1) 
			from ForecastsUsers 
			where UserId = @UserId 
				and ReportDate >= '2017-' + @m + '-01 00:00:00' 
			and ReportDate <= GETDATE()
			)
	end
			
	return @result

end
go