-- Пополнить баланс пользователя
if exists(select 1 from sysobjects where name = N'AddUserToForecast' and xtype='P') drop proc AddUserToForecast
go
create proc AddUserToForecast (
	@ForecastId int,
	@UserId int,		
	@ForecastValue float
	) as
begin	
	
	begin try 

		-- Проверяем, допущен ли пользователь к участию в прогнозах
		if not exists (select 1 from UsersForecastInfo where UserId = @UserId and IsConfirmed = 1 and ForecastEndDate > GETDATE()) begin			
			select cast(0 as bit), 2
			return 0
		end

		-- Проверяем, участвует ли пользователь уже в текущем прогнозе
		if exists (select 1 from ForecastsUsers where UserId = @UserId and ForecastId = @ForecastId) begin			
			select cast(0 as bit), 4
			return 0
		end
		
		-- Проверяем, участвует ли пользователь уже в текущем прогнозе
		if not exists (select 1 from Forecasts where Id = @ForecastId and GETDATE() < (dateadd(dd, -2, ForecastDate)) and IsClosed = 0) begin			
			select cast(0 as bit), 5
			return 0
		end
		
		insert into ForecastsUsers(UserId,ForecastId,Value,ReportDate)
		values(@UserId, @ForecastId, @ForecastValue, GETDATE())
				
		select cast(1 as bit), 0
				
		return 0
	
	end try 
	begin catch 
		select cast(0 as bit), 100
		return 1
	end catch
	
end
go