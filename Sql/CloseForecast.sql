-- Закрывает прогноз
if exists(select 1 from sysobjects where name = N'CloseForecast' and xtype='P') drop proc CloseForecast
go
create proc CloseForecast (
	@ForecastId int,
	@FactValue float
	) as
begin
	
	SET NOCOUNT ON;

	declare @trancount int
	select @trancount = @@TRANCOUNT

	begin try 
	
		if @trancount = 0
			begin transaction
	
		if @FactValue is null 
			return 1
			
		update Forecasts set IsClosed = 1, FactValue = @FactValue where Id = @ForecastId
		exec DefineForecastWinner @ForecastId
		
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

-- Определяет победителя прогноза
if exists(select 1 from sysobjects where name = N'DefineForecastWinner' and xtype='P') drop proc DefineForecastWinner
go
create proc DefineForecastWinner (
	@ForecastId int	
	) as
begin
	
	SET NOCOUNT ON;

	declare @trancount int
	select @trancount = @@TRANCOUNT

	begin try 
	
		if @trancount = 0
			begin transaction
			
		declare @factValue float = (select FactValue from Forecasts where Id = @ForecastId)
		
		if @factValue is null
			return 1
		
		declare @WinUser int = -1
		declare @ExecValue float = (select top 1 fu.Value from ForecastsUsers fu 
									where fu.ForecastId = @ForecastId and fu.Value = @factValue)
		
		if @ExecValue is not null begin
		
			set @WinUser = (select top 1 fu.Value from ForecastsUsers fu 
									where fu.ForecastId = @ForecastId and fu.Value = @ExecValue
									order by fu.ReportDate)
			if @WinUser is null
				return 1
			
			update Forecasts set WinValue = @ExecValue, Winner = @WinUser where Id = @ForecastId
		
		end else begin
				
			declare @LowValue float = (select MAX(fu.Value) from ForecastsUsers fu 
										where fu.ForecastId = @ForecastId and fu.Value < @factValue)
										
			declare @HighValue float = (select MIN(fu.Value) from ForecastsUsers fu 
										where fu.ForecastId = @ForecastId and fu.Value > @factValue)
					
			declare @DifLowValue float = 9999;
			if @LowValue is not null begin
				set @DifLowValue =  @factValue - @LowValue
			end
			
			declare @DifHighValue float = 9999;
			if @HighValue is not null begin
				set @DifHighValue =  @HighValue - @factValue
			end
			
			if @DifLowValue < @DifHighValue begin
			
				set @WinUser = (select top 1 fu.Value from ForecastsUsers fu 
									where fu.ForecastId = @ForecastId and fu.Value = @LowValue
									order by fu.ReportDate)
				if @WinUser is null
					return 1	
					
				update Forecasts set WinValue = @LowValue, Winner = @WinUser where Id = @ForecastId
						
			end else begin
				set @WinUser = (select top 1 fu.Value from ForecastsUsers fu 
									where fu.ForecastId = @ForecastId and fu.Value = @HighValue
									order by fu.ReportDate)
				if @WinUser is null
					return 1
					
				update Forecasts set WinValue = @HighValue, Winner = @WinUser where Id = @ForecastId
			end

		end
		
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
