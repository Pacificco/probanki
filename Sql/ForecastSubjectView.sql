-- Возвращает предметы прогнозов
if exists(select 1 from sysobjects where name = N'ForecastSubjectView' and xtype='P') drop proc ForecastSubjectView
go
create proc ForecastSubjectView (
	@SubjectAlias nvarchar(50)
	) as
begin	
	select * from ForecastSubjects where Alias = @SubjectAlias;	
end
go