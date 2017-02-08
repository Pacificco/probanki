-- Возвращает прогноз
if exists(select 1 from sysobjects where name = N'ForecastView' and xtype='P') drop proc ForecastView
go
create proc ForecastView (@Id int) as	
begin	
	if @Id is null return
	select f.*, s.Alias as SubjectAlias, s.Name as SubjectName
	from Forecasts f join ForecastSubjects s on s.Id = f.SubjectId where f.Id = @Id
end
go