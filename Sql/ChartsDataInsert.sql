-- Создает запись в таблице графиков
if exists(select 1 from sysobjects where name = N'ChartsDataInsert' and xtype='P') drop proc ChartsDataInsert
go
create proc ChartsDataInsert (@SubjectId int, @ChartValue numeric(18,4), @ChartDate datetime) as	
begin	

	set nocount on

	begin try 

		insert into ChartsData(SubjectId, ChartValue, ChartDate)
			values (@SubjectId, @ChartValue, @ChartDate)	
		
		return 0
	end try 
	begin catch 		
		return 1
	end catch 
	
end
go