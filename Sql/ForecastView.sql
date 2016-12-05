-- Возвращает прогноз
if exists(select 1 from sysobjects where name = N'ForecastView' and xtype='P') drop proc ForecastView
go
create proc ForecastView (@Id int) as	
begin	
	if @Id is null return
	select * from Forecasts f where f.Id = @Id
end
go