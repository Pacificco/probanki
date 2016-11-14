-- Возвращает предметы прогнозов
if exists(select 1 from sysobjects where name = N'ForecastSubjectsView' and xtype='P') drop proc ForecastSubjectsView
go
create proc ForecastSubjectsView (
	@SubjectAlias nvarchar(50)
	) as
begin	
	select * from ForecastSubjects order by Id
end
go