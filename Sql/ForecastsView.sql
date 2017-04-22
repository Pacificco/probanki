-- Возвращает прогнозы
if exists(select 1 from sysobjects where name = N'ForecastsView' and xtype='P') drop proc ForecastsView
go
create proc ForecastsView (
	@IsClosed bit, 
	@SubjectId tinyint
) as	
begin	
	if @IsClosed = 1 begin -- архивные прогнозы
		if @SubjectId is null
			select * from Forecasts f where f.IsClosed = 1 order by f.ForecastDate desc, f.SubjectId	-- все
		else
			select * from Forecasts f where f.IsClosed = 1 and f.SubjectId = @SubjectId order by f.ForecastDate desc
	end else begin -- не архивные прогнозы
		if @SubjectId is null
			select * from Forecasts f where f.IsClosed = 0 and f.ForecastDate > GETDATE() order by f.SubjectId		-- все
		else
			select * from Forecasts f where f.IsClosed = 0 and f.ForecastDate > GETDATE() 
				and f.SubjectId = @SubjectId order by f.ForecastDate desc
	end
end
go