-- Удаляет прогноз
if exists(select 1 from sysobjects where name = N'DeleteForecast' and xtype='P') drop proc DeleteForecast
go
create proc DeleteForecast (
	@Id int
) as	
begin	

	set nocount on

	declare @trancount int
	select @trancount = @@TRANCOUNT

	begin try 

	delete from ForecastsUsers where ForecastId = @Id
	delete from Forecasts where Id = @Id
	
	if @trancount = 0
			commit transaction
		return 0
	end try 
	begin catch 
		if @trancount = 0
			rollback transaction
		return 1
	end catch 
	
end
go