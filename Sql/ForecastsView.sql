-- Возвращает прогнозы
if exists(select 1 from sysobjects where name = N'ForecastsView' and xtype='P') drop proc ForecastsView
go
create proc ForecastsView (
	@IsClosed bit, 
	@SubjectId tinyint,
	@RowBegin int,
	@RowEnd int
) as	
begin	
	if @IsClosed = 1 begin
		if @SubjectId is null
			select * from Forecasts f where f.IsClosed = 1 order by f.SubjectId
		else
			select * from Forecasts f where f.IsClosed = 1 and f.SubjectId = @SubjectId
	end else begin
		if @SubjectId is null
			select * from Forecasts f where f.IsClosed = 0 order by f.CreateDate desc, f.ForecastDate
		else
			select * from Forecasts f where f.IsClosed = 0 and f.SubjectId = @SubjectId order by f.CreateDate desc, f.ForecastDate
	end
end
go