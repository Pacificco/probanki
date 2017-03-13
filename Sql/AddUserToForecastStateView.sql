-- Список прогнозов, в которых пользователь принял или может принять участие в текущем месяце
if exists(select 1 from sysobjects where name = N'AddUserToForecastStateView' and xtype='P') drop proc AddUserToForecastStateView
go
create proc AddUserToForecastStateView (
	@UserId int, @ForecastId int
	) as
begin
		
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
		-- Если подписка уже закончилась
		if @tariff = 0 begin
			select cast(0 as bit), 3;
			return 0;
		end 
			
		-- Проверяем число прогнозов за текущий месяц
		declare @tryCount int = dbo.GetUserForecastTryCountForThisMonth(@UserId)						
		-- Если подписка уже закончилась
		if @tryCount = 0 begin
			select cast(0 as bit), 2;
			return 0;
		end 					

		-- Проверяем на участие в указанном прогнозе
		if exists(select 1 from ForecastsUsers fu where fu.UserId = @UserId and fu.ForecastId = @ForecastId) begin
			select cast(0 as bit), 4;
			return 0;
		end
		
		-- Возможно принять участие в указанном прогнозе
		select cast(1 as bit), 0;			
		return 0
end
go