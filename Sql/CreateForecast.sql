-- Создает новый прогноз
if exists(select 1 from sysobjects where name = N'CreateForecast' and xtype='P') drop proc CreateForecast
go
create proc CreateForecast (@ForecastDate datetime, @UserId int, @SubjectId tinyint, @WinAmount float) as	
begin	
	insert into Forecasts(IsClosed,CreateDate,ForecastDate,ReportDate,ReportUserId,SubjectId,WinAmount,WinValue,Winner)
	values (
		0,
		GETDATE(),
		@ForecastDate,
		GETDATE(),
		@UserId,
		@SubjectId,
		@WinAmount,
		NULL,
		NULL
	)
end
go