-- Список прогнозов, в которых пользователь принял или может принять участие в текущем месяце
if exists(select 1 from sysobjects where name = N'AddUserToForecastStateView' and xtype='P') drop proc AddUserToForecastStateView
go
create proc AddUserToForecastStateView (
	@UserId int, @ForecastId int
	) as
begin
	
	begin try
		
		-- Проверяем баланс пользователя
		declare @tariff_info table (ForecastEndDate		datetime	not null,											
									TariffId			tinyint		not null,
									IsConfirmed			bit			null
									)		
		insert @tariff_info
		select uf.ForecastEndDate, uf.TariffId, uf.IsConfirmed
			from UsersForecastInfo uf 
			where uf.UserId = @UserId				
		declare @tariff bit = (select case when ForecastEndDate > getdate() and IsConfirmed = 1 then cast(1 as bit) else cast(0 as bit) end from @tariff_info)								
		-- Если тариф не определен / нет подписки
		if @tariff = 0 begin
			select cast(0 as bit), 3;
			return 0;
		end 
			
		-- Проверяем число прогнозов за текущий месяц
		declare @tariffId tinyint = (select TariffId from @tariff_info)
		--declare @tryCount int = dbo.GetUserForecastTryCountForThisMonth(@UserId, @tariffId)
		declare @tryCount int = dbo.GetUserForecastTryCountForThisForecasts(@UserId, @tariffId)	-- u0201459_probanki_user
		-- Если количество прогнозов исчерпано
		if @tryCount = 0 begin
			select cast(0 as bit), 2;
			return 0;
		end 					

		-- Проверяем на участие в указанном прогнозе
		if exists(select 1 from ForecastsUsers fu where fu.UserId = @UserId and fu.ForecastId = @ForecastId) begin
			select cast(0 as bit), 4;
			return 0;
		end
		
		-- В прогнозе уже нельзя принять участие
		declare @fDate datetime = (select ForecastDate from Forecasts where Id = @ForecastId)		
		if getdate() > (dateadd(dd, -2, @fDate)) begin
			select cast(0 as bit), 5;
			return 0;
		end
		
		-- Возможно принять участие в указанном прогнозе
		select cast(1 as bit), 0;			
		return 0

	end try 
	begin catch 
		select cast(0 as bit), 3;
			return 0;
		return 1
	end catch
	
end
go