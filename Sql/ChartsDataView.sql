-- ��������� ������ �� ������� ��������
if exists(select 1 from sysobjects where name = N'ChartsDataView' and xtype='P') drop proc ChartsDataView
go
create proc ChartsDataView (@SubjectId int) as	
begin	

	select * from ChartsData where SubjectId = @SubjectId and ChartDate > DATEADD(MONTH, -3, GETDATE()) order by ChartDate
	
end
go